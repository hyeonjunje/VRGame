using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCanvasController : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private float distance;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }


    public void SwitherCanvas(Transform parent)
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main.GetComponent<CinemachineBrain>().OutputCamera;
        canvas.planeDistance = distance;

        canvas.renderMode = RenderMode.WorldSpace;

        transform.SetParent(parent);
    }
}
