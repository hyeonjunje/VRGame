using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NonArduinoInput : MonoBehaviour
{
    public static NonArduinoInput instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public Joystick moveJoystick;
    public ScreenSlider screenSlider;

    public EventTrigger fireButton;

    public static bool isFire = false;

    private void Start()
    {
        EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => OnPointerDown((PointerEventData)data));
        fireButton.triggers.Add(entry_PointerDown);

        EventTrigger.Entry entry_PointerUp = new EventTrigger.Entry();
        entry_PointerUp.eventID = EventTriggerType.PointerUp;
        entry_PointerUp.callback.AddListener((data) => OnPointerUp((PointerEventData)data));
        fireButton.triggers.Add(entry_PointerUp);
    }


    private void OnPointerDown(PointerEventData data)
    {
        isFire = true;
    }


    private void OnPointerUp(PointerEventData data)
    {
        isFire = false;
    }
}
