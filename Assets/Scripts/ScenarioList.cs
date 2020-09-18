using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioList : MonoBehaviour
{
    public List<Map> Maps => m_Maps;
    public List<ScenarioEntry> ScenarioEntries => m_ScenarioEntries;
    [SerializeField] List<Map> m_Maps;
    [SerializeField] List<ScenarioEntry> m_ScenarioEntries;
    [SerializeField] Text m_DetailsName;
    [SerializeField] Text m_DetailsDescription;

    Map m_SelectedMap;

    public int ListOffset { get; private set; }

    void Awake()
    {
        for (int i = 0; i < m_ScenarioEntries.Count; i++)
        {
            m_ScenarioEntries[i].ScenarioList = this;
        }

        PopulateScenarioEntries(0);

        if (m_Maps.Count > 0)
        {
            EntryClicked(m_Maps[0]);
        }
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            PopulateScenarioEntries(ListOffset - 1);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            PopulateScenarioEntries(ListOffset + 1);
        }
    }
    
    public void PopulateScenarioEntries(int a_Offset)
    {
        a_Offset = Mathf.Clamp(a_Offset, 0, m_Maps.Count - m_ScenarioEntries.Count);

        ListOffset = a_Offset;

        int _EntriesToPopulate = Mathf.Min(m_ScenarioEntries.Count, m_Maps.Count - a_Offset);

        for (int i = 0; i < _EntriesToPopulate; i++)
        {
            m_ScenarioEntries[i].SetMap(m_Maps[i + a_Offset]);
            m_ScenarioEntries[i].SetSelected(m_SelectedMap);
        }

        for (int i = _EntriesToPopulate; i < m_ScenarioEntries.Count; i++)
        {
            m_ScenarioEntries[i].SetMap(null);
        }
    }

    public void EntryClicked(Map a_Map)
    {
        m_SelectedMap = a_Map;

        m_DetailsName.text = a_Map.Name;
        m_DetailsDescription.text = a_Map.Description;

        for (int i = 0; i < m_ScenarioEntries.Count; i++)
        {
            m_ScenarioEntries[i].SetSelected(m_SelectedMap);
        }
    }

    public void ScrollUpPressed()
    {
        PopulateScenarioEntries(ListOffset - 1);
    }

    public void ScrollDownPressed()
    {
        PopulateScenarioEntries(ListOffset + 1);
    }
}
