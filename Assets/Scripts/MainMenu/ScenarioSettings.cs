using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSettings : MonoBehaviour
{
    Scenario m_Scenario;

    [SerializeField] GameSettings m_GameSettings = null;
    [SerializeField] ScenarioInfo m_ScenarioInfo = null;
    [SerializeField] ScenarioSettingsPlayer[] m_Players = null;
    [SerializeField] Image[] m_AlliesFlags = null;
    [SerializeField] Image[] m_EnemiesFlags = null;
    [SerializeField] Sprite[] m_FlagSprites = null;

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

        HeroPool.Initialize(m_Scenario.HeroInfo);

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

        UpdateHeroLists();

        m_Players[m_GameSettings.LocalPlayerIndex].SetName("Player");

        for (int i = _CurrentPlayer; i < 8; i++)
        {
            m_Players[i].gameObject.SetActive(false);
        }

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

        for (int i = 0; i < m_Scenario.PlayerCount; i++)
        {
            m_GameSettings.Players.Add(m_Players[i].GetGameSettings());
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
