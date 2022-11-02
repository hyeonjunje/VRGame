using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform gunHole;
    [SerializeField] private LineRenderer lineRenderer;

    private RaycastHit hit;

    private void Update()
    {
        lineRenderer.SetPosition(0, gunHole.position);
        lineRenderer.SetPosition(1, gunHole.position + gunHole.forward * 100f);
    }


    public void Shoot()
    {
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
}
