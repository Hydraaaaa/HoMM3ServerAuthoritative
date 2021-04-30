using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownScreen : MonoBehaviour
{
    public bool Enabled { get { return m_Root.activeSelf; } }

    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] PlayerColors m_PlayerColors;
    [SerializeField] FactionList m_Factions;

    [SerializeField] GameObject m_Root;
    [SerializeField] RectTransform m_Body;
    [SerializeField] Image m_UI;
    [SerializeField] Image m_Portrait;
    [SerializeField] Image m_Flag;
    [SerializeField] GameObject[] m_Landscapes;

    MapTown m_CurrentTown;

    int m_CurrentIndex;

    void Awake()
    {
        PlayerColors.PlayerElements _Elements = m_PlayerColors.Elements[m_GameSettings.LocalPlayerIndex];

        m_UI.sprite = _Elements.TownUI;
        m_Flag.sprite = _Elements.TownScreenFlag;

        m_Root.SetActive(false);

        for (int i = 0; i < m_Landscapes.Length; i++)
        {
            m_Landscapes[i].SetActive(false);
        }
    }

    void Update()
    {
        Vector2 _AnchoredPosition = m_Body.anchoredPosition;

        if (Screen.width / 2.0f != Mathf.Round(Screen.width / 2.0f))
        {
            _AnchoredPosition.x = 0.5f;
        }
        else
        {
            _AnchoredPosition.x = 0.0f;
        }

        if (Screen.height / 2.0f != Mathf.Round(Screen.height / 2.0f))
        {
            _AnchoredPosition.y = 0.5f;
        }
        else
        {
            _AnchoredPosition.y = 0.0f;
        }

        m_Body.anchoredPosition = _AnchoredPosition;
    }

    public void ShowTown(MapTown a_Town)
    {
        m_CurrentTown = a_Town;

        int _FactionIndex = m_Factions.Factions.IndexOf(m_CurrentTown.Faction);

        m_Landscapes[m_CurrentIndex].SetActive(false);
        m_Landscapes[_FactionIndex].SetActive(true);

        m_CurrentIndex = _FactionIndex;

        m_Portrait.sprite = m_CurrentTown.Faction.TownSpriteLarge;

        m_Root.SetActive(true);
    }

    public void Hide()
    {
        m_Root.SetActive(false);
    }
}
