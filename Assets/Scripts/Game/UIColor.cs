using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColor : MonoBehaviour
{
    [SerializeField] GameSettings m_Settings;
    [SerializeField] PlayerColors m_Colors;

    [Space]

    [SerializeField] Image m_Sidebar;
    [SerializeField] Image m_SidebarSmall;
    [SerializeField] Image m_BorderTopLeft;
    [SerializeField] Image m_BorderTop;
    [SerializeField] Image m_BorderLeft;
    [SerializeField] Image m_BorderRight;
    [SerializeField] Image m_BorderBottomLeft;
    [SerializeField] Image m_BorderBottomRight;
    [SerializeField] Image m_Resources;
    [SerializeField] Image m_BottomBarFill;
    [SerializeField] Image m_Date;

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
