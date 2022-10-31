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
        //Instantiate(bulletPrefabs, gunHole.position, gunHole.rotation);

        Debug.Log("일단 쐈다.");

        if(Physics.Raycast(gunHole.position, gunHole.forward, out hit, 100f, LayerMask.NameToLayer("Enemy")))
        {
            Debug.Log(hit.transform.name + "맞았다.");
        }
    }
}
