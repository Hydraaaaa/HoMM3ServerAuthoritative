using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScenarioDetailsButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform m_PressTransform;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        m_PressTransform.anchoredPosition = new Vector2(1.0f, -1.0f);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        m_PressTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
    }
}
