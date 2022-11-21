using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject potalChild = null;

    private string currentHitName;

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
        // ������Ʈ ��ȣ�ۿ�
        if (currentTrigger != null)
        {
            if (!currentTrigger.isInteract)
                currentTrigger.Interact();
            else
                currentTrigger.ExitInteract();

            return;
        }

        // �ѼҸ��� �� ����
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, audioSource.maxDistance, 1 << LayerMask.NameToLayer("Zombie"));
        {
            foreach (Collider collider in hitColliders)
            {
                collider.GetComponent<Zombie>().Search();
            }
        }

        // �ѽ�� ����
        audioSource.Play();

        animator.SetTrigger(hashIsShoot);
        if (Physics.Raycast(gunHole.position, gunHole.forward, out hit, 100f))
            Debug.Log(hit.transform.name);

        StartCoroutine(CoMuzzleFlash());

        if (Physics.Raycast(gunHole.position, gunHole.forward, out hit, 100f, 1 << LayerMask.NameToLayer("HitBox")))
        {
            // ���� ��Ʈ�ڽ��� ���� �������
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("HitBox"))
            {
                // �Ӹ��� �������
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
        // Ʈ���� ����
        if (Physics.Raycast(gunHole.position, gunHole.forward, out hit, interactDistance, 1 << LayerMask.NameToLayer("Trigger")))
        {
            if(currentHitName != hit.transform.name)
            {
                if(currentTrigger != null)
                {
                    currentTrigger.ExitInteract();
                    currentTrigger.TryCancelInteract();
                }
                currentHitName = hit.transform.name;
                currentTrigger = hit.transform.gameObject.GetComponent<TriggerObject>();
            }
            currentTrigger.TryInteract();
        }
        else if(Physics.Raycast(gunHole.position, gunHole.forward, out hit, 100, 1 << LayerMask.NameToLayer("Trigger")))
        {
            if(hit.transform.tag == "UI")
            {
                if (currentHitName != hit.transform.name)
                {
                    if (currentTrigger != null)
                    {
                        currentTrigger.ExitInteract();
                        currentTrigger.TryCancelInteract();
                    }
                    currentHitName = hit.transform.name;
                    currentTrigger = hit.transform.gameObject.GetComponent<TriggerObject>();
                }
                currentTrigger.TryInteract();
            }
        }
        else
        {
            if (currentTrigger != null)
            {
                currentTrigger.ExitInteract();
                currentTrigger.TryCancelInteract();
            }
            currentTrigger = null;
            currentHitName = null;
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
