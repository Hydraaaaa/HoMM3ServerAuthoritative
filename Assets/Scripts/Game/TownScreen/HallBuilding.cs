using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HallBuilding : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BuildingData BuildingData => m_BuildingData;

    public Image Image => m_Image;
    public Image ButtonImage => m_ButtonImage;
    public Image CornerImage => m_CornerImage;

    public bool Buildable { get; set; }
    public bool IsShipyard => m_IsShipyard;

    [SerializeField] BuildPanel m_BuildPanel;

    [SerializeField] BuildingData m_BuildingData;

    [SerializeField] Image m_Image;
    [SerializeField] Image m_ButtonImage;
    [SerializeField] Image m_CornerImage;
    [SerializeField] bool m_IsShipyard;

    void Reset()
    {
        m_Image = transform.GetChild(0).GetComponent<Image>();
        m_ButtonImage = transform.GetChild(1).GetComponent<Image>();
        m_CornerImage = transform.GetChild(2).GetComponent<Image>();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            m_BuildPanel.Open(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            m_BuildPanel.Open(this, true);

            CursorManager.SetCursorVisible(false);
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            m_BuildPanel.Close();

            CursorManager.SetCursorVisible(true);
        }
    }
}
