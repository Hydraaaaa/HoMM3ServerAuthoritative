using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSettings : MonoBehaviour
{
    Map m_Map;
    int m_PlayerIndex;

    [SerializeField] GameObject[] m_Players;

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

        for (int i = 0; i < a_Map.PlayerInfo.Count; i++)
        {
            m_Players[i].SetActive(a_Map.PlayerInfo[i].ComputerPlayable);
        }
    }
}
