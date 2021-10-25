using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingHall : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] TownScreen m_TownScreen;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            m_TownScreen.HallPressed();
        }
    }
}
