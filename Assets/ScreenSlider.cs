using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenSlider : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 dir;

    private Vector2 origin;

    public void OnBeginDrag(PointerEventData eventData)
    {
        origin = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dir = (eventData.position - origin) / 10;

        origin = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dir = Vector2.zero;
    }

}
