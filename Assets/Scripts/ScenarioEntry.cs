using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioEntry : MonoBehaviour
{
    public ScenarioList ScenarioList { get; set; }
    public Text ScenarioName => m_ScenarioName;
    public Map Map { get; private set; }
    [SerializeField] Text m_ScenarioName;
    [SerializeField] Color m_SelectedColor;

    public void SetMap(Map a_Map)
    {
        Map = a_Map;

        Debug.Log(a_Map);

        if (a_Map != null)
        {
            m_ScenarioName.text = a_Map.Name;
        }
        else
        {
            m_ScenarioName.text = "";
        }
    }
    
    public void SetSelected(Map a_Map)
    {
        if (a_Map == Map)
        {
            m_ScenarioName.color = m_SelectedColor;
        }
        else
        {
            m_ScenarioName.color = Color.white;
        }
    }

    public void OnClick()
    {
        ScenarioList.EntryClicked(Map);
    }
}
