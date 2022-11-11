using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TriggerObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private bool isOneTime;   // 일회성 trigger인지 구별하는 변수

    [SerializeField] private UnityEvent interactEvent, exitInteractEvent;

    public bool isInteract = false;
    public void TryInteract()
    {
        interactionText.gameObject.SetActive(true);
    }


    public void TryCancelInteract()
    {
        interactionText.gameObject.SetActive(false);
    }


    public void Interact()
    {
        interactEvent.Invoke();
        isInteract = true;
    }


    public void ExitInteract()
    {
        exitInteractEvent.Invoke();
        isInteract = false;
    }
}
