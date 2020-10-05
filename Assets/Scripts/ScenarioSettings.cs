using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSettings : MonoBehaviour
{
    Map m_Map;

    [SerializeField] GameSettings m_GameSettings = null;
    [SerializeField] ScenarioSettingsPlayer[] m_Players = null;
    [SerializeField] Image[] m_AlliesFlags = null;
    [SerializeField] Image[] m_EnemiesFlags = null;
    [SerializeField] Sprite[] m_FlagSprites = null;

    public void UpdateSettings(Map a_Map)
    {
        m_Map = a_Map;

        for (int i = 0; i < 8; i++)
        {
            if (a_Map.PlayerInfo[i].HumanPlayable)
            {
                m_GameSettings.LocalPlayerIndex = i;
                break;
            }
        }

        byte _Team = a_Map.PlayerInfo[m_GameSettings.LocalPlayerIndex].Team;

        for (int i = 0; i < 8; i++)
        {
            m_AlliesFlags[i].gameObject.SetActive(false);
            m_EnemiesFlags[i].gameObject.SetActive(false);
        }

        int _AlliesIndex = 0;
        int _EnemiesIndex = 0;

        int _CurrentPlayer = 0;

        for (int i = 0; i < 8; i++)
        {
            if (a_Map.PlayerInfo[i].ComputerPlayable)
            {
                m_Players[_CurrentPlayer].gameObject.SetActive(true);
                m_Players[_CurrentPlayer].Initialize(i, a_Map.PlayerInfo[i]);

                _CurrentPlayer++;
            }
        }

        for (int i = _CurrentPlayer; i < 8; i++)
        {
            m_Players[i].gameObject.SetActive(false);
        }

        if (a_Map.HasTeams)
        {
            for (int i = 0; i < 8; i++)
            {
                if (a_Map.PlayerInfo[i].ComputerPlayable)
                {
                    if (a_Map.PlayerInfo[i].Team == _Team)
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
                if (a_Map.PlayerInfo[i].ComputerPlayable)
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
        // TODO: Set local player text as Computer

        m_GameSettings.LocalPlayerIndex = a_Player.PlayerIndex;

        // TODO: Set new local player text as Player
    }
}
