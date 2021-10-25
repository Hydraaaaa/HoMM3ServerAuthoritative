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
    [SerializeField] PlayerResources m_Resources;

    [SerializeField] GameObject m_Root;
    [SerializeField] RectTransform m_Body;
    [SerializeField] Image m_UI;
    [SerializeField] Image m_Portrait;
    [SerializeField] Image m_Flag;
    [SerializeField] TownBuildings[] m_TownBuildings;

    [SerializeField] GameObject m_BaseHallScreen;
    [SerializeField] Image[] m_HallScreens;
    [SerializeField] Image m_BuildPanel;

    // Town list
    [SerializeField] List<Image> m_TownList;
    [SerializeField] GameObject m_TownSelectedBorder;
    [SerializeField] Button m_TownUpArrow;
    [SerializeField] Button m_TownDownArrow;

    MapTown m_CurrentTown;
    int m_TownListIndex;
    int m_CurrentTownIndex;
    int m_CurrentFactionIndex;

    void Awake()
    {
        PlayerColors.PlayerElements _Elements = m_PlayerColors.Elements[m_GameSettings.LocalPlayerIndex];

        m_UI.sprite = _Elements.TownUI;
        m_Flag.sprite = _Elements.TownScreenFlag;

        m_HallScreens[0].sprite = _Elements.TownHallUI1;
        m_HallScreens[1].sprite = _Elements.TownHallUI2;
        m_HallScreens[2].sprite = _Elements.TownHallUI3;
        m_HallScreens[3].sprite = _Elements.TownHallUI3;
        m_HallScreens[4].sprite = _Elements.TownHallUI3;
        m_HallScreens[5].sprite = _Elements.TownHallUI3;
        m_HallScreens[6].sprite = _Elements.TownHallUI3;
        m_HallScreens[7].sprite = _Elements.TownHallUI2;
        m_HallScreens[8].sprite = _Elements.TownHallUI2;

        m_BuildPanel.sprite = _Elements.BuildPanel;

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

        m_TownListIndex = _Towns.IndexOf(a_Town) - 1;

        if (m_TownListIndex > _Towns.Count - 3)
        {
            m_TownListIndex = _Towns.Count - 3;
        }

        if (m_TownListIndex < 0)
        {
            m_TownListIndex = 0;
        }

        UpdateTownDisplay();
    }

    void ShowTown(MapTown a_Town)
    {
        m_CurrentTown = a_Town;
        m_CurrentTownIndex = m_LocalOwnership.GetTowns().IndexOf(a_Town);

        int _FactionIndex = m_Factions.Factions.IndexOf(m_CurrentTown.Faction);

        m_TownBuildings[m_CurrentFactionIndex].gameObject.SetActive(false);
        m_TownBuildings[_FactionIndex].gameObject.SetActive(true);
        m_TownBuildings[_FactionIndex].SetBuildings(a_Town.Buildings, m_CurrentTown.CanBuildShipyard);

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
        if (m_TownListIndex > 0)
        {
            m_TownListIndex--;

            UpdateTownDisplay();
        }
    }

    public void TownDownArrowPressed()
    {
        int _TownCount = m_LocalOwnership.GetTownCount();

        if (m_TownListIndex < _TownCount - 3)
        {
            m_TownListIndex++;

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
                if (_Towns[i + m_TownListIndex].Buildings.Fort)
                {
                    m_TownList[i].sprite = _Towns[i + m_TownListIndex].Faction.TownSpriteSmall;
                }
                else
                {
                    m_TownList[i].sprite = _Towns[i + m_TownListIndex].Faction.TownNoFortSpriteSmall;
                }

                m_TownList[i].gameObject.SetActive(true);
            }
            else
            {
                m_TownList[i].gameObject.SetActive(false);
            }
        }

        m_TownUpArrow.interactable = m_TownListIndex != 0;
        m_TownDownArrow.interactable = m_TownListIndex < _Towns.Count - 3;

        if (m_CurrentTownIndex < m_TownListIndex ||
            m_CurrentTownIndex >= m_TownListIndex + 3)
        {
            m_TownSelectedBorder.SetActive(false);
        }
        else
        {
            m_TownSelectedBorder.SetActive(true);
            m_TownSelectedBorder.transform.position = m_TownList[m_CurrentTownIndex - m_TownListIndex].transform.position;
        }
    }

    public void TownPressed(int a_Index)
    {
        int _Index = a_Index + m_TownListIndex;

        if (m_CurrentTownIndex != _Index)
        {
            ShowTown(m_LocalOwnership.GetTowns()[_Index]);

            m_TownSelectedBorder.SetActive(true);
            m_TownSelectedBorder.transform.position = m_TownList[a_Index].transform.position;
        }
    }

    public void HallPressed()
    {
        m_BaseHallScreen.SetActive(true);

        for (int i = 0; i < m_HallScreens.Length; i++)
        {
            m_HallScreens[i].gameObject.SetActive(false);
        }

        m_HallScreens[m_CurrentFactionIndex].gameObject.SetActive(true);
    }

    public void BuildBuilding(BuildingData a_Data)
    {
        m_Resources.Gold -= a_Data.GoldCost;
        m_Resources.Wood -= a_Data.WoodCost;
        m_Resources.Ore -= a_Data.OreCost;
        m_Resources.Mercury -= a_Data.MercuryCost;
        m_Resources.Sulfur -= a_Data.SulfurCost;
        m_Resources.Crystals -= a_Data.CrystalCost;
        m_Resources.Gems -= a_Data.GemCost;

        m_BaseHallScreen.SetActive(false);
        m_TownBuildings[m_CurrentFactionIndex].BuildBuilding(a_Data);
    }

    public TownBuildings GetTownBuildings()
    {
        return m_TownBuildings[m_CurrentFactionIndex];
    }
}
