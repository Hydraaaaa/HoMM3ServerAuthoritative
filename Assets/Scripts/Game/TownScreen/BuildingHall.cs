using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingHall : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] TownScreen m_TownScreen;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        m_TownScreen.HallPressed();
    }
}
