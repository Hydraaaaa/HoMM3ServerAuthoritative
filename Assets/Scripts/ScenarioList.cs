using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioList : MonoBehaviour
{
    [SerializeField] Map[] m_Maps;
    [SerializeField] ScenarioEntry[] m_ScenarioEntries;
    [SerializeField] Text m_Description;

    Map m_SelectedMap;

    void Awake()
    {
        for (int i = 0; i < m_ScenarioEntries.Length; i++)
        {
            m_ScenarioEntries[i].ScenarioList = this;
        }

        PopulateScenarioEntries(0);
    }
    
    void PopulateScenarioEntries(int a_Offset)
    {
        int _EntriesToPopulate = Mathf.Min(m_ScenarioEntries.Length, m_Maps.Length - a_Offset);

        for (int i = 0; i < _EntriesToPopulate; i++)
        {
            m_ScenarioEntries[i].SetMap(m_Maps[i + a_Offset]);
            m_ScenarioEntries[i].SetSelected(m_SelectedMap);
        }

        for (int i = _EntriesToPopulate; i < m_ScenarioEntries.Length; i++)
        {
            m_ScenarioEntries[i].SetMap(null);
        }
    }

    public void EntryClicked(Map a_Map)
    {
        m_SelectedMap = a_Map;

        m_Description.text = a_Map.Description;

        for (int i = 0; i < m_ScenarioEntries.Length; i++)
        {
            m_ScenarioEntries[i].SetSelected(m_SelectedMap);
        }
    }
}
