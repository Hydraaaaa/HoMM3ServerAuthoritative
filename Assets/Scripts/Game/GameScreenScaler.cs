using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreenScaler : MonoBehaviour
{
    public const int VIEWPORT_PADDING_TOP = 8;
    public const int VIEWPORT_PADDING_BOTTOM = 48;
    public const int VIEWPORT_PADDING_LEFT = 8;
    public const int VIEWPORT_PADDING_RIGHT = 200;

    [SerializeField] Camera m_Camera;
    [SerializeField] GameObject m_Sidebar;
    [SerializeField] GameObject m_SidebarSmall;
    [SerializeField] RectTransform m_InfoCards;

    void Update()
    {
        float _Width = Screen.width - VIEWPORT_PADDING_LEFT - VIEWPORT_PADDING_RIGHT;
        float _Height = Screen.height - VIEWPORT_PADDING_TOP - VIEWPORT_PADDING_BOTTOM;

        if (_Height % 2 == 1)
        {
            _Height += 1;
        }

        m_Camera.pixelRect = new Rect(VIEWPORT_PADDING_LEFT, VIEWPORT_PADDING_BOTTOM, _Width, _Height);

        if (Screen.height >= 664)
        {
            m_Sidebar.SetActive(true);
            m_SidebarSmall.SetActive(false);
            m_InfoCards.anchoredPosition = new Vector2(-10, -464);
        }
        else
        {
            m_Sidebar.SetActive(false);
            m_SidebarSmall.SetActive(true);
            m_InfoCards.anchoredPosition = new Vector2(-10, -400);
        }

        m_Camera.orthographicSize = _Height / 64.0f;
    }
}
