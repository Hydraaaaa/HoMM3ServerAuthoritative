using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HallBuilding : MonoBehaviour, IPointerDownHandler
{
    public BuildingData BuildingData => m_BuildingData;

    public Image Image => m_Image;
    public Image ButtonImage => m_ButtonImage;
    public Image CornerImage => m_CornerImage;

    public bool Buildable { get; set; }

    [SerializeField] BuildPanel m_BuildPanel;

    [SerializeField] BuildingData m_BuildingData;

    [SerializeField] Image m_Image;
    [SerializeField] Image m_ButtonImage;
    [SerializeField] Image m_CornerImage;

    void Reset()
    {
        m_Image = transform.GetChild(0).GetComponent<Image>();
        m_ButtonImage = transform.GetChild(1).GetComponent<Image>();
        m_CornerImage = transform.GetChild(2).GetComponent<Image>();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        m_BuildPanel.Open(this);
    }
}
