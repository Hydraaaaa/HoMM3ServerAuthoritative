﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSettings : MonoBehaviour
{
    Scenario m_Scenario;

    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] PlayerResources m_PlayerResources;
    [SerializeField] ScenarioInfo m_ScenarioInfo;
    [SerializeField] ScenarioSettingsPlayer[] m_Players;
    [SerializeField] Image[] m_AlliesFlags;
    [SerializeField] Image[] m_EnemiesFlags;
    [SerializeField] Sprite[] m_FlagSprites;

    void Awake()
    {
        m_ScenarioInfo.OnGameStart += OnGameStart;
    }

    public void UpdateSettings(Scenario a_Scenario)
    {
        m_Scenario = a_Scenario;

        for (int i = 0; i < 8; i++)
        {
            if (a_Scenario.PlayerInfo[i].HumanPlayable)
            {
                m_GameSettings.LocalPlayerIndex = i;
                break;
            }
        }

        for (int i = 0; i < 8; i++)
        {
            m_AlliesFlags[i].gameObject.SetActive(false);
            m_EnemiesFlags[i].gameObject.SetActive(false);
        }

        HeroPool.Initialize(m_Scenario.HeroInfo, m_Scenario.AvailableHeroes);

        int _CurrentPlayer = 0;

        for (int i = 0; i < 8; i++)
        {
            if (a_Scenario.PlayerInfo[i].ComputerPlayable)
            {
                m_Players[_CurrentPlayer].gameObject.SetActive(true);
                m_Players[_CurrentPlayer].Initialize(i, a_Scenario.PlayerInfo[i]);
                m_Players[_CurrentPlayer].SetName("Computer");

                _CurrentPlayer++;
            }
        }

        m_Players[m_GameSettings.LocalPlayerIndex].SetName("Player");

        for (int i = _CurrentPlayer; i < 8; i++)
        {
            m_Players[i].gameObject.SetActive(false);
        }

        UpdateHeroLists();

        UpdateFlags();
    }

    void UpdateFlags()
    {
        byte _Team = m_Scenario.PlayerInfo[m_GameSettings.LocalPlayerIndex].Team;

        int _AlliesIndex = 0;
        int _EnemiesIndex = 0;

        if (m_Scenario.HasTeams)
        {
            for (int i = 0; i < 8; i++)
            {
                if (m_Scenario.PlayerInfo[i].ComputerPlayable)
                {
                    if (m_Scenario.PlayerInfo[i].Team == _Team)
                    {
                        m_AlliesFlags[_AlliesIndex].gameObject.SetActive(true);
                        m_AlliesFlags[_AlliesIndex].sprite = m_FlagSprites[i];
                        _AlliesIndex++;
                    }
                    else
                    {
                        m_EnemiesFlags[_EnemiesIndex].gameObject.SetActive(true);
                        m_EnemiesFlags[_EnemiesIndex].sprite = m_FlagSprites[i];
                        _EnemiesIndex++;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                if (m_Scenario.PlayerInfo[i].ComputerPlayable)
                {
                    if (i == m_GameSettings.LocalPlayerIndex)
                    {
                        m_AlliesFlags[_AlliesIndex].gameObject.SetActive(true);
                        m_AlliesFlags[_AlliesIndex].sprite = m_FlagSprites[i];
                        _AlliesIndex++;
                    }
                    else
                    {
                        m_EnemiesFlags[_EnemiesIndex].gameObject.SetActive(true);
                        m_EnemiesFlags[_EnemiesIndex].sprite = m_FlagSprites[i];
                        _EnemiesIndex++;
                    }
                }
            }
        }
    }

    public void FlagPressed(ScenarioSettingsPlayer a_Player)
    {
        m_Players[m_GameSettings.LocalPlayerIndex].SetName("Computer");

        m_GameSettings.LocalPlayerIndex = a_Player.PlayerIndex;

        m_Players[m_GameSettings.LocalPlayerIndex].SetName("Player");

        UpdateFlags();
    }

    void OnGameStart()
    {
        m_GameSettings.Players.Clear();

        for (int i = 0; i < m_Scenario.ComputerCount; i++)
        {
            m_GameSettings.Players.Add(m_Players[i].GetGameSettings());
        }

        switch (m_GameSettings.Rating)
        {
            case GameSettings.RATING_EASY:
            {
                m_PlayerResources.Wood = 30;
                m_PlayerResources.Ore = 30;

                m_PlayerResources.Mercury = 15;
                m_PlayerResources.Sulfur = 15;
                m_PlayerResources.Crystals = 15;
                m_PlayerResources.Gems = 15;

                m_PlayerResources.Gold = 30000;
                break;
            }

            case GameSettings.RATING_NORMAL:
            {
                m_PlayerResources.Wood = 20;
                m_PlayerResources.Ore = 20;

                m_PlayerResources.Mercury = 10;
                m_PlayerResources.Sulfur = 10;
                m_PlayerResources.Crystals = 10;
                m_PlayerResources.Gems = 10;

                m_PlayerResources.Gold = 20000;
                break;
            }

            case GameSettings.RATING_HARD:
            {
                m_PlayerResources.Wood = 15;
                m_PlayerResources.Ore = 15;

                m_PlayerResources.Mercury = 7;
                m_PlayerResources.Sulfur = 7;
                m_PlayerResources.Crystals = 7;
                m_PlayerResources.Gems = 7;

                m_PlayerResources.Gold = 15000;
                break;
            }

            case GameSettings.RATING_EXPERT:
            {
                m_PlayerResources.Wood = 10;
                m_PlayerResources.Ore = 10;

                m_PlayerResources.Mercury = 4;
                m_PlayerResources.Sulfur = 4;
                m_PlayerResources.Crystals = 4;
                m_PlayerResources.Gems = 4;

                m_PlayerResources.Gold = 10000;
                break;
            }

            case GameSettings.RATING_IMPOSSIBLE:
            {
                m_PlayerResources.Wood = 0;
                m_PlayerResources.Ore = 0;

                m_PlayerResources.Mercury = 0;
                m_PlayerResources.Sulfur = 0;
                m_PlayerResources.Crystals = 0;
                m_PlayerResources.Gems = 0;

                m_PlayerResources.Gold = 0;
                break;
            }
        }
    }

    public void UpdateHeroLists(int a_CallingPlayer = -1)
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            if (i != a_CallingPlayer &&
                m_Players[i].gameObject.activeSelf)
            {
                m_Players[i].UpdateHeroList();
            }
        }
    }
}
