using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreenScaler : MonoBehaviour
{
    [SerializeField] Camera m_Camera = null;
    [SerializeField] GameObject m_Sidebar = null;
    [SerializeField] GameObject m_SidebarSmall = null;

    [SerializeField] int m_PaddingTop = 0;
    [SerializeField] int m_PaddingBottom = 0;
    [SerializeField] int m_PaddingLeft = 0;
    [SerializeField] int m_PaddingRight = 0;

    void Update()
    {
        float _Width = Screen.width - m_PaddingLeft - m_PaddingRight;
        float _Height = Screen.height - m_PaddingBottom - m_PaddingTop;

        m_Camera.pixelRect = new Rect(m_PaddingLeft, m_PaddingBottom, _Width, _Height);

        if (Screen.height >= 664)
        {
            m_Sidebar.SetActive(true);
            m_SidebarSmall.SetActive(false);
        }
        else
        {
            m_Sidebar.SetActive(false);
            m_SidebarSmall.SetActive(true);
        }

        m_Camera.orthographicSize = _Height / 64.0f;
    }
}
