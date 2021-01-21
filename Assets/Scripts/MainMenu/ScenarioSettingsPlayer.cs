using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScenarioSettingsPlayer : MonoBehaviour
{
    [Serializable]
    class BonusContainer
    {
        public string Name;
        public Sprite Sprite;
    }

    public int PlayerIndex { get; private set; }

    [SerializeField] ScenarioSettings m_ScenarioSettings = null;
    [SerializeField] FactionList m_Factions = null;
    [SerializeField] Image m_BackgroundImage = null;
    [SerializeField] Button m_FlagButton = null;
    [SerializeField] Text m_NameText = null;
    [SerializeField] Text m_HumanOrCPUText = null;

    [SerializeField] Image m_TownImage = null;
    [SerializeField] Text m_TownText = null;
    [SerializeField] Image m_TownLeft = null;
    [SerializeField] Image m_TownRight = null;

    [SerializeField] Image m_HeroImage = null;
    [SerializeField] Text m_HeroText = null;
    [SerializeField] Image m_HeroLeft = null;
    [SerializeField] Image m_HeroRight = null;
    [SerializeField] Image m_BonusImage = null;
    [SerializeField] Text m_BonusText = null;
    [SerializeField] Image m_BonusLeft = null;
    [SerializeField] Image m_BonusRight = null;

    [Space]

    [SerializeField] Sprite[] m_BackgroundSprites = null;
    [SerializeField] Sprite[] m_FlagSprites = null;
    [SerializeField] Sprite[] m_FlagHoverSprites = null;
    [SerializeField] Sprite[] m_FlagPressedSprites = null;
    [SerializeField] Sprite m_RandomTownSprite = null;
    [SerializeField] Sprite m_RandomHeroSprite = null;
    [SerializeField] BonusContainer[] m_BonusSprites = null;

    int m_CurrentTownIndex;
    int m_CurrentHeroIndex;
    int m_CurrentBonusIndex;

    bool m_IsTownChoosable;

    List<Faction> m_AvailableTowns;

    public void Initialize(int a_Index, PlayerInfo a_PlayerInfo)
    {
        PlayerIndex = a_Index;

        m_BackgroundImage.sprite = m_BackgroundSprites[a_Index];

        m_FlagButton.image.sprite = m_FlagSprites[a_Index];

        SpriteState _State = m_FlagButton.spriteState;

        _State.highlightedSprite = m_FlagHoverSprites[a_Index];
        _State.pressedSprite = m_FlagPressedSprites[a_Index];

        m_FlagButton.spriteState = _State;

        m_FlagButton.gameObject.SetActive(a_PlayerInfo.HumanPlayable);

        if (a_PlayerInfo.HumanPlayable)
        {
            m_HumanOrCPUText.text = "Human or CPU";
            m_HumanOrCPUText.rectTransform.anchoredPosition = new Vector2
            (
                m_HumanOrCPUText.rectTransform.anchoredPosition.x,
                -23
            );
        }
        else
        {
            m_HumanOrCPUText.text = "CPU";
            m_HumanOrCPUText.rectTransform.anchoredPosition = new Vector2
            (
                m_HumanOrCPUText.rectTransform.anchoredPosition.x,
                -22
            );
        }

        if (a_PlayerInfo.IsTownChoosable)
        {
            m_IsTownChoosable = true;
            m_CurrentTownIndex = -1;

            m_AvailableTowns = m_Factions.Factions;
        }
        else
        {
            if (a_PlayerInfo.MainTownType != 255)
            {
                m_IsTownChoosable = false;
                m_CurrentTownIndex = a_PlayerInfo.MainTownType;

                m_AvailableTowns = m_Factions.Factions;
            }
            else
            {
                m_IsTownChoosable = true;
                m_CurrentTownIndex = -1;

                m_AvailableTowns = new List<Faction>();

                int _BitwiseIndex = 1;

                for (int i = 0; i < m_Factions.Factions.Count; i++)
                {
                    if ((a_PlayerInfo.AllowedTowns & _BitwiseIndex) == _BitwiseIndex)
                    {
                        m_AvailableTowns.Add(m_Factions.Factions[i]);
                    }

                    _BitwiseIndex *= 2;
                }
            }
        }

        m_TownLeft.gameObject.SetActive(m_IsTownChoosable);
        m_TownRight.gameObject.SetActive(m_IsTownChoosable);

        m_CurrentHeroIndex = -1;
        m_CurrentBonusIndex = 0;

        UpdateTownSprite();
        UpdateHeroSprite();
        UpdateBonusSprite();
    }

    public void SetName(string a_Name)
    {
        m_NameText.text = a_Name;
    }

    public void FlagPressed()
    {
        m_ScenarioSettings.FlagPressed(this);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void TownLeftPressed()
    {
        m_CurrentTownIndex--;

        if (m_CurrentTownIndex < -1)
        {
            m_CurrentTownIndex = m_AvailableTowns.Count - 1;
        }

        m_CurrentHeroIndex = -1;

        UpdateTownSprite();
        UpdateHeroSprite();
    }

    public void TownRightPressed()
    {
        m_CurrentTownIndex++;

        if (m_CurrentTownIndex > m_AvailableTowns.Count - 1)
        {
            m_CurrentTownIndex = -1;
        }

        m_CurrentHeroIndex = -1;

        UpdateTownSprite();
        UpdateHeroSprite();
    }

    void UpdateTownSprite()
    {
        m_HeroLeft.gameObject.SetActive(m_CurrentTownIndex != -1);
        m_HeroRight.gameObject.SetActive(m_CurrentTownIndex != -1);

        if (m_CurrentTownIndex == -1)
        {
            m_TownImage.sprite = m_RandomTownSprite;
            m_TownText.text = "Random";
        }
        else
        {
            m_TownImage.sprite = m_AvailableTowns[m_CurrentTownIndex].TownSprite;
            m_TownText.text = m_AvailableTowns[m_CurrentTownIndex].name;
        }
    }

    public void HeroLeftPressed()
    {
        m_CurrentHeroIndex--;

        if (m_CurrentHeroIndex < -1)
        {
            m_CurrentHeroIndex = m_AvailableTowns[m_CurrentTownIndex].Heroes.Count - 1;
        }

        UpdateHeroSprite();
    }

    public void HeroRightPressed()
    {
        m_CurrentHeroIndex++;

        if (m_CurrentHeroIndex > m_AvailableTowns[m_CurrentTownIndex].Heroes.Count - 1)
        {
            m_CurrentHeroIndex = -1;
        }

        UpdateHeroSprite();
    }

    void UpdateHeroSprite()
    {
        if (m_CurrentHeroIndex == -1)
        {
            m_HeroImage.sprite = m_RandomHeroSprite;
            m_HeroText.text = "Random";
        }
        else
        {
            m_HeroImage.sprite = m_AvailableTowns[m_CurrentTownIndex].Heroes[m_CurrentHeroIndex].Portrait;
            m_HeroText.text = m_AvailableTowns[m_CurrentTownIndex].Heroes[m_CurrentHeroIndex].name;
        }
    }

    public void BonusLeftPressed()
    {
        m_CurrentBonusIndex--;

        if (m_CurrentBonusIndex < 0)
        {
            m_CurrentBonusIndex = m_BonusSprites.Length - 1;
        }

        UpdateBonusSprite();
    }

    public void BonusRightPressed()
    {
        m_CurrentBonusIndex++;

        if (m_CurrentBonusIndex > m_BonusSprites.Length - 1)
        {
            m_CurrentBonusIndex = 0;
        }

        UpdateBonusSprite();
    }

    void UpdateBonusSprite()
    {
        m_BonusImage.sprite = m_BonusSprites[m_CurrentBonusIndex].Sprite;
        m_BonusText.text = m_BonusSprites[m_CurrentBonusIndex].Name;
    }
}
