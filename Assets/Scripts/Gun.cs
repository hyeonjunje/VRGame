using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform gunHole;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Animator animator;

    [SerializeField] private Light muzzleLight;
    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private float interactDistance = 5f;

    private AudioSource audioSource;

    private RaycastHit hit;
    private TriggerObject currentTrigger = null;

    private readonly int hashIsShoot = Animator.StringToHash("isShoot");


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        lineRenderer.SetPosition(0, gunHole.position);
        if (Physics.Raycast(gunHole.position, gunHole.forward, out hit, 100f))
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, gunHole.position + gunHole.forward * 100f);
        }

        DetachTrigger();
    }


    public void Shoot()
    {
        // 오브젝트 상호작용
        if (currentTrigger != null)
        {
            if (!currentTrigger.isInteract)
                currentTrigger.Interact();
            else
                currentTrigger.ExitInteract();

            return;
        }

        // 총소리에 적 반응
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, audioSource.maxDistance, 1 << LayerMask.NameToLayer("Zombie"));
        {
            foreach (Collider collider in hitColliders)
            {
                collider.GetComponent<Zombie>().Search();
            }
        }

        // 총쏘기 로직
        audioSource.Play();

        animator.SetTrigger(hashIsShoot);
        if (Physics.Raycast(gunHole.position, gunHole.forward, out hit, 100f))
            Debug.Log(hit.transform.name);

        StartCoroutine(CoMuzzleFlash());

        if (Physics.Raycast(gunHole.position, gunHole.forward, out hit, 100f, 1 << LayerMask.NameToLayer("HitBox")))
        {
            // 적의 히트박스에 총이 맞을경우
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("HitBox"))
            {
                // 머리에 맞을경우
                if(hit.transform.tag == "Head")
                {
                    hit.transform.root.GetComponent<Zombie>().HitHead();
                    Debug.Log(hit.transform.root.GetComponent<Zombie>().name);
                }
                else
                {
                    hit.transform.root.GetComponent<Zombie>().Hit();
                    Debug.Log(hit.transform.root.GetComponent<Zombie>().name);
                }
            }
        }
    }


    private void DetachTrigger()
    {
        if (Physics.Raycast(gunHole.position, gunHole.forward, out hit, interactDistance, 1 << LayerMask.NameToLayer("Trigger")))
        {
            if (currentTrigger == null)
                currentTrigger = hit.transform.gameObject.GetComponent<TriggerObject>();

            currentTrigger.TryInteract();
        }
        else
        {
            if (currentTrigger != null)
            {
                if(currentTrigger.isInteract)
                    currentTrigger.ExitInteract();
                currentTrigger.TryCancelInteract();
                currentTrigger = null;
            }
        }
    }


    IEnumerator CoMuzzleFlash()
    {
        muzzleLight.enabled = true;
        muzzleFlash.Play();

        yield return new WaitForSeconds(0.1f);

        muzzleLight.enabled = false;
        muzzleFlash.Stop();
    }
}
