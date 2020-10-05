using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColor : MonoBehaviour
{
    [SerializeField] GameSettings m_Settings = null;
    [SerializeField] PlayerColors m_Colors = null;

    [Space]

    [SerializeField] Image m_Sidebar = null;
    [SerializeField] Image m_SidebarSmall = null;
    [SerializeField] Image m_BorderTopLeft = null;
    [SerializeField] Image m_BorderTop = null;
    [SerializeField] Image m_BorderLeft = null;
    [SerializeField] Image m_BorderRight = null;
    [SerializeField] Image m_BorderBottomLeft = null;
    [SerializeField] Image m_BorderBottomRight = null;
    [SerializeField] Image m_Resources = null;
    [SerializeField] Image m_BottomBarFill = null;
    [SerializeField] Image m_Date = null;

    void Awake()
    {
        PlayerColors.PlayerElements _Elements = m_Colors.Elements[m_Settings.LocalPlayerIndex];

        m_Sidebar.sprite = _Elements.Sidebar;
        m_SidebarSmall.sprite = _Elements.SidebarSmall;
        m_BorderTopLeft.sprite = _Elements.BorderTopLeft;
        m_BorderTop.sprite = _Elements.BorderTop;
        m_BorderRight.sprite = _Elements.BorderRight;
        m_BorderLeft.sprite = _Elements.BorderLeft;
        m_BorderBottomLeft.sprite = _Elements.BorderBottomLeft;
        m_BorderBottomRight.sprite = _Elements.BorderBottomRight;
        m_Resources.sprite = _Elements.Resources;
        m_BottomBarFill.sprite = _Elements.BottomBarFill;
        m_Date.sprite = _Elements.Date;
    }
}
