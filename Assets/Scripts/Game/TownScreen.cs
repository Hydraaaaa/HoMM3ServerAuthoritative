using System;
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
    [SerializeField] LocalOwnership m_LocalOwnership;

    [SerializeField] GameObject m_Root;
    [SerializeField] RectTransform m_Body;
    [SerializeField] Image m_UI;
    [SerializeField] Image m_Portrait;
    [SerializeField] Image m_Flag;
    [SerializeField] TownBuildings[] m_TownBuildings;

    // Town list
    [SerializeField] List<Image> m_TownList;
    [SerializeField] GameObject m_TownSelectedBorder;
    [SerializeField] Button m_TownUpArrow;
    [SerializeField] Button m_TownDownArrow;

    MapTown m_CurrentTown;
    int m_CurrentTownIndex;
    int m_CurrentFactionIndex;

    void Awake()
    {
        PlayerColors.PlayerElements _Elements = m_PlayerColors.Elements[m_GameSettings.LocalPlayerIndex];

        m_UI.sprite = _Elements.TownUI;
        m_Flag.sprite = _Elements.TownScreenFlag;

        m_Root.SetActive(false);

        for (int i = 0; i < m_TownBuildings.Length; i++)
        {
            m_TownBuildings[i].gameObject.SetActive(false);
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

    public void OpenTown(MapTown a_Town)
    {
        m_Root.SetActive(true);

        ShowTown(a_Town);

        List<MapTown> _Towns = m_LocalOwnership.GetTowns();

        m_CurrentTownIndex = _Towns.IndexOf(a_Town) - 1;

        if (m_CurrentTownIndex > _Towns.Count - 3)
        {
            m_CurrentTownIndex = _Towns.Count - 3;
        }

        if (m_CurrentTownIndex < 0)
        {
            m_CurrentTownIndex = 0;
        }

        UpdateTownDisplay();
    }

    void ShowTown(MapTown a_Town)
    {
        m_CurrentTown = a_Town;

        int _FactionIndex = m_Factions.Factions.IndexOf(m_CurrentTown.Faction);

        m_TownBuildings[m_CurrentFactionIndex].gameObject.SetActive(false);
        m_TownBuildings[_FactionIndex].gameObject.SetActive(true);
        m_TownBuildings[_FactionIndex].SetBuildings(a_Town.Buildings);

        m_CurrentFactionIndex = _FactionIndex;

        if (m_CurrentTown.Buildings.Fort)
        {
            m_Portrait.sprite = m_CurrentTown.Faction.TownSpriteLarge;
        }
        else
        {
            m_Portrait.sprite = m_CurrentTown.Faction.TownNoFortSpriteLarge;
        }
    }

    public void Hide()
    {
        m_Root.SetActive(false);
    }

    public void TownUpArrowPressed()
    {
        if (m_CurrentTownIndex > 0)
        {
            m_CurrentTownIndex--;

            UpdateTownDisplay();
        }
    }

    public void TownDownArrowPressed()
    {
        int _TownCount = m_LocalOwnership.GetTownCount();

        if (m_CurrentTownIndex < _TownCount - 3)
        {
            m_CurrentTownIndex++;

            UpdateTownDisplay();
        }
    }

    void UpdateTownDisplay()
    {
        List<MapTown> _Towns = m_LocalOwnership.GetTowns();

        for (int i = 0; i < 3; i++)
        {
            if (i < _Towns.Count)
            {
                if (_Towns[i + m_CurrentTownIndex].Buildings.Fort)
                {
                    m_TownList[i].sprite = _Towns[i + m_CurrentTownIndex].Faction.TownSpriteSmall;
                }
                else
                {
                    m_TownList[i].sprite = _Towns[i + m_CurrentTownIndex].Faction.TownNoFortSpriteSmall;
                }

                m_TownList[i].gameObject.SetActive(true);
            }
            else
            {
                m_TownList[i].gameObject.SetActive(false);
            }
        }

        m_TownUpArrow.interactable = m_CurrentTownIndex != 0;
        m_TownDownArrow.interactable = m_CurrentTownIndex < _Towns.Count - 3;

        int _SelectedTownIndex = m_LocalOwnership.GetTowns().IndexOf(m_CurrentTown);

        if (_SelectedTownIndex < m_CurrentTownIndex ||
            _SelectedTownIndex >= m_CurrentTownIndex + 3)
        {
            m_TownSelectedBorder.SetActive(false);
        }
        else
        {
            m_TownSelectedBorder.SetActive(true);
            m_TownSelectedBorder.transform.position = m_TownList[_SelectedTownIndex - m_CurrentTownIndex].transform.position;
        }
    }

    public void TownPressed(int a_Index)
    {
        int _Index = a_Index + m_CurrentTownIndex;

        List<MapTown> _Towns = m_LocalOwnership.GetTowns();

        if (m_LocalOwnership.SelectedTown != _Towns[_Index])
        {
            ShowTown(_Towns[_Index]);

            m_TownSelectedBorder.SetActive(true);
            m_TownSelectedBorder.transform.position = m_TownList[_Index - m_CurrentTownIndex].transform.position;
        }
    }
}
