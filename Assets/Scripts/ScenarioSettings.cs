﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSettings : MonoBehaviour
{
    Map m_Map;
    int m_PlayerIndex;

    [SerializeField] GameObject[] m_Players;
    [SerializeField] Image[] m_AlliesFlags;
    [SerializeField] Image[] m_EnemiesFlags;
    [SerializeField] Sprite[] m_FlagSprites;

    public void UpdateSettings(Map a_Map)
    {
        m_Map = a_Map;

        for (int i = 0; i < a_Map.PlayerInfo.Count; i++)
        {
            if (a_Map.PlayerInfo[i].HumanPlayable)
            {
                m_PlayerIndex = i;
                break;
            }
        }

        byte _Team = a_Map.PlayerInfo[m_PlayerIndex].Team;

        for (int i = 0; i < 8; i++)
        {
            m_AlliesFlags[i].gameObject.SetActive(false);
            m_EnemiesFlags[i].gameObject.SetActive(false);
        }

        int _AlliesIndex = 0;
        int _EnemiesIndex = 0;

        for (int i = 0; i < a_Map.PlayerInfo.Count; i++)
        {
            m_Players[i].SetActive(a_Map.PlayerInfo[i].ComputerPlayable);
        }

        if (a_Map.HasTeams)
        {
            for (int i = 0; i < a_Map.ComputerCount; i++)
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
        else
        {
            for (int i = 0; i < a_Map.ComputerCount; i++)
            {
                if (i == m_PlayerIndex)
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
