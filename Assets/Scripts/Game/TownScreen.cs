using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownScreen : MonoBehaviour
{
    public bool Enabled { get { return m_Root.activeSelf; } }

    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] PlayerColors m_PlayerColors;

    [SerializeField] GameObject m_Root;
    [SerializeField] Image m_UI;
    [SerializeField] Image m_Background;
    [SerializeField] Image m_Portrait;
    [SerializeField] Image m_Flag;

    MapTown m_CurrentTown;

    void Awake()
    {
        PlayerColors.PlayerElements _Elements = m_PlayerColors.Elements[m_GameSettings.LocalPlayerIndex];

        m_UI.sprite = _Elements.TownUI;
        m_Flag.sprite = _Elements.TownScreenFlag;

        m_Root.SetActive(false);
    }

    public void ShowTown(MapTown a_Town)
    {
        m_CurrentTown = a_Town;

        m_Background.sprite = m_CurrentTown.Faction.TownScreenBackground;
        m_Portrait.sprite = m_CurrentTown.Faction.TownSpriteLarge;

        m_Root.SetActive(true);
    }

    public void Hide()
    {
        m_Root.SetActive(false);
    }
}
