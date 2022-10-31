using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShootingGame
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] private Transform gunHole;

        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float range;

        [SerializeField] private Text stateText;

        private int layer;
        private void Start()
        {
            layer = LayerMask.NameToLayer("Enemy") | LayerMask.NameToLayer("Floor");
        }

        void Update()
        {
            SetLineRenderer();
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("쐈다");
                SetInfo();
            }
        }

        private void SetLineRenderer()
        {
            lineRenderer.SetPosition(0, gunHole.position);

            RaycastHit hit;
            if (Physics.Raycast(gunHole.position, gunHole.forward, out hit, range))
            {
                lineRenderer.SetPosition(1, hit.point);
                stateText.text = hit.transform.name;
            }
            else
            {
                lineRenderer.SetPosition(1, gunHole.position + gunHole.forward * range);
                stateText.text = "";
            }
        }

        private void SetInfo()
        {
            RaycastHit hit;
            if(Physics.Raycast(gunHole.position, gunHole.forward, out hit, range))
            {

                Debug.Log("SetInfo 실행?");
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    myZombie.Zombie zombie = hit.transform.GetComponent<myZombie.Zombie>();
                    zombie.Hit();
                }
                else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
                {
                    // 이동하기
                    Debug.Log(hit.point + "로 이동합니다.");
                }
            }
        }
    }
}

