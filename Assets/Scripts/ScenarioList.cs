using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] Text m_DetailsDiff;
    [SerializeField] Image m_DetailsSizeImage;
    [SerializeField] Sprite m_SmallSprite;
    [SerializeField] Sprite m_MedSprite;
    [SerializeField] Sprite m_LargeSprite;
    [SerializeField] Sprite m_XLSprite;
    [SerializeField] ScenarioSettings m_Settings;

    Map m_SelectedMap;

    public int ListOffset { get; private set; }

    enum SortType
    {
        Unsorted,
        Name,
        Size,
        Version,
        PlayerCount
    }

    SortType m_SortType = SortType.Unsorted;

    void Awake()
    {
        for (int i = 0; i < m_ScenarioEntries.Count; i++)
        {
            m_ScenarioEntries[i].ScenarioList = this;
        }

        NameSort();

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

        switch (a_Map.Difficulty)
        {
            case 0: m_DetailsDiff.text = "Easy"; break;
            case 1: m_DetailsDiff.text = "Normal"; break;
            case 2: m_DetailsDiff.text = "Hard"; break;
            case 3: m_DetailsDiff.text = "Expert"; break;
            case 4: m_DetailsDiff.text = "Impossible"; break;
            case 5: m_DetailsDiff.text = "???"; break;
        }

        for (int i = 0; i < m_ScenarioEntries.Count; i++)
        {
            m_ScenarioEntries[i].SetSelected(m_SelectedMap);
        }

        switch (a_Map.Size)
        {
            case 36: m_DetailsSizeImage.sprite = m_SmallSprite; break;
            case 72: m_DetailsSizeImage.sprite = m_MedSprite; break;
            case 108: m_DetailsSizeImage.sprite = m_LargeSprite; break;
            case 144: m_DetailsSizeImage.sprite = m_XLSprite; break;
        }

        m_Settings.UpdateSettings(a_Map);
    }

    public void ScrollUpPressed()
    {
        PopulateScenarioEntries(ListOffset - 1);
    }

    public void ScrollDownPressed()
    {
        PopulateScenarioEntries(ListOffset + 1);
    }

    public void NameSort()
    {
        if (m_SortType != SortType.Name)
        {
            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.Name).ToList();

            m_SortType = SortType.Name;
        }
        else
        {
            m_Maps.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void SizeSort()
    {
        if (m_SortType != SortType.Size)
        {
            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.Size).ToList();

            m_SortType = SortType.Size;
        }
        else
        {
            m_Maps.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void VersionSort()
    {
        if (m_SortType != SortType.Version)
        {
            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.Name).ToList();
            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.Version).ToList();

            m_SortType = SortType.Version;
        }
        else
        {
            m_Maps.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void PlayerCountSort()
    {
        if (m_SortType != SortType.PlayerCount)
        {
            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.PlayerCount).ToList();
            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.ComputerCount).ToList();

            m_SortType = SortType.PlayerCount;
        }
        else
        {
            m_Maps.Reverse();
        }

        PopulateScenarioEntries(0);
    }
}
