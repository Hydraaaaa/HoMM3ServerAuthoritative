﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScenarioSettingsPlayer : MonoBehaviour
{
    public int PlayerIndex { get; private set; }

    [SerializeField] ScenarioSettings m_ScenarioSettings = null;
    [SerializeField] FactionList m_Factions = null;
    [SerializeField] HeroList m_Heroes = null;
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
    [SerializeField] Image m_StartingBonusImage = null;
    [SerializeField] Text m_StartingBonusText = null;

    [Space]

    [SerializeField] Sprite[] m_BackgroundSprites = null;
    [SerializeField] Sprite[] m_FlagSprites = null;
    [SerializeField] Sprite[] m_FlagHoverSprites = null;
    [SerializeField] Sprite[] m_FlagPressedSprites = null;
    [SerializeField] Sprite m_RandomTownSprite = null;
    [SerializeField] Sprite m_RandomHeroSprite = null;
    [SerializeField] Sprite m_RandomStartingBonusSprite = null;
    [SerializeField] Sprite m_NoHeroSprite = null;
    [SerializeField] StartingBonusList m_StartingBonuses = null;

    int m_CurrentTownIndex;
    int m_CurrentHeroIndex;
    int m_CurrentStartingBonusIndex;


    bool m_IsTownChoosable;
    bool m_GenerateHeroAtMainTown;
    bool m_IsHeroRandom;

    Hero m_CustomHero;

    bool m_IsPlayer;
    bool m_IsLocalPlayer;

    List<Faction> m_AvailableTowns;
    List<Hero> m_AvailableHeroes;

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

        m_GenerateHeroAtMainTown = a_PlayerInfo.GenerateHeroAtMainTown;
        m_IsHeroRandom = a_PlayerInfo.IsMainHeroRandom;

        if (a_PlayerInfo.IsTownChoosable)
        {
            m_IsTownChoosable = true;
            m_CurrentTownIndex = -1;

            m_AvailableTowns = m_Factions.Factions;
        }
        else
        {
            if (a_PlayerInfo.MainTownType != 255 && a_PlayerInfo.HasMainTown)
            {
                m_IsTownChoosable = false;
                m_CurrentTownIndex = a_PlayerInfo.MainTownType;

                m_AvailableTowns = m_Factions.Factions;
            }
            else
            {
                m_IsTownChoosable = true;

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

                if (m_AvailableTowns.Count > 1)
                {
                    m_CurrentTownIndex = -1;
                }
                else
                {
                    m_CurrentTownIndex = 0;
                }
            }
        }

        m_CustomHero = null;

        if (!a_PlayerInfo.IsMainHeroRandom)
        {
            if (a_PlayerInfo.MainHeroPortrait == 255 || a_PlayerInfo.MainHeroType == 255)
            {
                m_HeroText.text = "None";
                m_HeroImage.sprite = m_NoHeroSprite;
            }
            else
            {
                Hero _BaseHero = m_Heroes.Heroes.First((a_Hero) => a_Hero.Hero.ID == a_PlayerInfo.MainHeroType).Hero;
                Hero _PortraitHero = m_Heroes.Heroes.First((a_Hero) => a_Hero.Hero.ID == a_PlayerInfo.MainHeroPortrait).Hero;

                m_HeroText.text = a_PlayerInfo.MainHeroName;
                m_HeroImage.sprite = _PortraitHero.Portrait;

                m_CustomHero = new Hero();

                if (a_PlayerInfo.MainHeroName != "")
                {
                    m_CustomHero.Name = a_PlayerInfo.MainHeroName;
                }
                else
                {
                    m_CustomHero.Name = _BaseHero.Name;
                }

                m_CustomHero.ID = a_PlayerInfo.MainHeroType;
                m_CustomHero.Faction = _BaseHero.Faction;
                m_CustomHero.Portrait = _PortraitHero.Portrait;

                HeroPool.ClaimHero(_BaseHero);

                // TODO: Custom Hero ID probably needs to pipe through primary/secondary abilities
            }
        }

        m_TownLeft.gameObject.SetActive(m_IsTownChoosable && m_AvailableTowns.Count > 1);
        m_TownRight.gameObject.SetActive(m_IsTownChoosable && m_AvailableTowns.Count > 1);

        m_HeroLeft.gameObject.SetActive(m_IsHeroRandom || m_GenerateHeroAtMainTown);
        m_HeroRight.gameObject.SetActive(m_IsHeroRandom || m_GenerateHeroAtMainTown);

        m_CurrentStartingBonusIndex = -1;
        m_CurrentHeroIndex = -1;

        if (m_CurrentTownIndex != -1)
        {
            m_AvailableHeroes = HeroPool.GetFactionHeroes(PlayerIndex, m_AvailableTowns[m_CurrentTownIndex]);
        }

        UpdateTownSprite();
        UpdateHeroSprite();
        UpdateStartingBonusSprite();
    }

    public void SetName(string a_Name)
    {
        m_NameText.text = a_Name;

        if (m_NameText.text == "Computer")
        {
            m_IsPlayer = false;
            m_IsLocalPlayer = false;
        }
        else
        {
            m_IsPlayer = true;
            m_IsLocalPlayer = true;
        }
    }

    public void FlagPressed()
    {
        m_ScenarioSettings.FlagPressed(this);
        EventSystem.current.SetSelectedGameObject(null);
    }

    void RenewHeroList()
    {
        if (m_CurrentHeroIndex != -1 && m_AvailableHeroes != null)
        {
            HeroPool.FreeHero(m_AvailableHeroes[m_CurrentHeroIndex]);
            m_ScenarioSettings.UpdateHeroLists(PlayerIndex);
        }

        m_CurrentHeroIndex = -1;

        if (m_CurrentTownIndex == -1)
        {
            UpdateHeroSprite();
            return;
        }

        m_AvailableHeroes = HeroPool.GetFactionHeroes(PlayerIndex, m_AvailableTowns[m_CurrentTownIndex]);

        UpdateHeroSprite();
    }

    public void UpdateHeroList()
    {
        if (m_CurrentHeroIndex != -1 && m_AvailableHeroes != null)
        {
            Hero _CurrentHero = m_AvailableHeroes[m_CurrentHeroIndex];

            HeroPool.FreeHero(_CurrentHero);

            m_AvailableHeroes = HeroPool.GetFactionHeroes(PlayerIndex, m_AvailableTowns[m_CurrentTownIndex]);

            HeroPool.ClaimHero(_CurrentHero);

            m_CurrentHeroIndex = m_AvailableHeroes.IndexOf(_CurrentHero);
        }
        else if (m_CurrentTownIndex != -1 && m_AvailableTowns.Count > 0)
        {
            m_AvailableHeroes = HeroPool.GetFactionHeroes(PlayerIndex, m_AvailableTowns[m_CurrentTownIndex]);
        }

        UpdateHeroSprite();
    }

    public void TownLeftPressed()
    {
        m_CurrentTownIndex--;

        if (m_CurrentTownIndex < -1)
        {
            m_CurrentTownIndex = m_AvailableTowns.Count - 1;
        }

        UpdateTownSprite();
        RenewHeroList();
    }

    public void TownRightPressed()
    {
        m_CurrentTownIndex++;

        if (m_CurrentTownIndex > m_AvailableTowns.Count - 1)
        {
            m_CurrentTownIndex = -1;
        }

        UpdateTownSprite();
        RenewHeroList();
    }

    void UpdateTownSprite()
    {
        m_HeroLeft.gameObject.SetActive(m_CurrentTownIndex != -1 && (m_IsHeroRandom || m_GenerateHeroAtMainTown));
        m_HeroRight.gameObject.SetActive(m_CurrentTownIndex != -1 && (m_IsHeroRandom || m_GenerateHeroAtMainTown));

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
        if (m_CurrentHeroIndex != -1)
        {
            HeroPool.FreeHero(m_AvailableHeroes[m_CurrentHeroIndex]);
        }

        m_CurrentHeroIndex--;

        if (m_CurrentHeroIndex < -1)
        {
            m_CurrentHeroIndex = m_AvailableHeroes.Count - 1;
        }

        if (m_CurrentHeroIndex != -1)
        {
            HeroPool.ClaimHero(m_AvailableHeroes[m_CurrentHeroIndex]);
        }

        m_ScenarioSettings.UpdateHeroLists(PlayerIndex);
        UpdateHeroSprite();
    }

    public void HeroRightPressed()
    {
        if (m_CurrentHeroIndex != -1)
        {
            HeroPool.FreeHero(m_AvailableHeroes[m_CurrentHeroIndex]);
        }

        m_CurrentHeroIndex++;

        if (m_CurrentHeroIndex > m_AvailableHeroes.Count - 1)
        {
            m_CurrentHeroIndex = -1;
        }
        else
        {
            HeroPool.ClaimHero(m_AvailableHeroes[m_CurrentHeroIndex]);
        }

        m_ScenarioSettings.UpdateHeroLists(PlayerIndex);
        UpdateHeroSprite();
    }

    void UpdateHeroSprite()
    {
        if (m_GenerateHeroAtMainTown || m_IsHeroRandom)
        {
            if (m_CurrentHeroIndex == -1)
            {
                m_HeroImage.sprite = m_RandomHeroSprite;
                m_HeroText.text = "Random";
            }
            else
            {
                m_HeroImage.sprite = m_AvailableHeroes[m_CurrentHeroIndex].Portrait;
                m_HeroText.text = m_AvailableHeroes[m_CurrentHeroIndex].Name;
            }
        }
        else
        {
            if (m_CustomHero != null)
            {
                m_HeroText.text = m_CustomHero.Name;
                m_HeroImage.sprite = m_CustomHero.Portrait;
            }
            else
            {
                m_HeroText.text = "None";
                m_HeroImage.sprite = m_NoHeroSprite;
            }
        }
    }

    public void StartingBonusLeftPressed()
    {
        m_CurrentStartingBonusIndex--;

        if (m_CurrentStartingBonusIndex < 0)
        {
            m_CurrentStartingBonusIndex = m_StartingBonuses.StartingBonuses.Count - 1;
        }

        UpdateStartingBonusSprite();
    }

    public void StartingBonusRightPressed()
    {
        m_CurrentStartingBonusIndex++;

        if (m_CurrentStartingBonusIndex > m_StartingBonuses.StartingBonuses.Count - 1)
        {
            m_CurrentStartingBonusIndex = -1;
        }

        UpdateStartingBonusSprite();
    }

    void UpdateStartingBonusSprite()
    {
        if (m_CurrentStartingBonusIndex == -1)
        {
            m_StartingBonusImage.sprite = m_RandomStartingBonusSprite;
            m_StartingBonusText.text = "Random";
        }
        else
        {
            m_StartingBonusImage.sprite = m_StartingBonuses.StartingBonuses[m_CurrentStartingBonusIndex].Sprite;
            m_StartingBonusText.text = m_StartingBonuses.StartingBonuses[m_CurrentStartingBonusIndex].name;
        }
    }

    public GameSettings.Player GetGameSettings()
    {
        GameSettings.Player _Player = new GameSettings.Player();

        _Player.IsPlayer = m_IsPlayer;
        _Player.IsLocalPlayer = m_IsLocalPlayer;

        if (m_CurrentTownIndex != -1)
        {
            _Player.Faction = m_AvailableTowns[m_CurrentTownIndex];
        }
        else
        {
            _Player.Faction = m_AvailableTowns[Random.Range(0, m_AvailableTowns.Count)];
        }

        if (m_CurrentHeroIndex != -1)
        {
            _Player.Hero = m_AvailableHeroes[m_CurrentHeroIndex];
        }
        else
        {
            _Player.Hero = null;
        }

        if (m_CurrentStartingBonusIndex != -1)
        {
            _Player.StartingBonus = m_StartingBonuses.StartingBonuses[m_CurrentStartingBonusIndex];
        }

        return _Player;
    }
}
