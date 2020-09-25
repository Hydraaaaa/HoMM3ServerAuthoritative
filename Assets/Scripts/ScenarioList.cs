using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioList : MonoBehaviour
{
    public List<Map> Maps => m_Maps;
    public List<Map> CurrentMaps => m_CurrentMaps;
    public List<ScenarioEntry> ScenarioEntries => m_ScenarioEntries;
    [SerializeField] List<Map> m_Maps;
    [SerializeField] List<ScenarioEntry> m_ScenarioEntries;
    [SerializeField] GameObject m_DisableToHide;
    [SerializeField] Text m_DetailsName;
    [SerializeField] Text m_DetailsDescription;
    [SerializeField] Image m_DetailsWinConditionImage;
    [SerializeField] Text m_DetailsWinCondition;
    [SerializeField] Image m_DetailsLossConditionImage;
    [SerializeField] Text m_DetailsLossCondition;
    [SerializeField] Text m_DetailsDiff;
    [SerializeField] Image m_DetailsSizeImage;
    [SerializeField] Sprite m_SmallSprite;
    [SerializeField] Sprite m_MedSprite;
    [SerializeField] Sprite m_LargeSprite;
    [SerializeField] Sprite m_XLSprite;
    [SerializeField] ScenarioSettings m_Settings;
    [SerializeField] ScenarioListScrollbar m_Scrollbar;

    public Sprite NormalWinSprite => m_NormalWinSprite;
    public Sprite AcquireArtifactSprite => m_AcquireArtifactSprite;
    public Sprite CreaturesSprite => m_CreaturesSprite;
    public Sprite ResourcesSprite => m_ResourcesSprite;
    public Sprite UpgradeTownSprite => m_UpgradeTownSprite;
    public Sprite BuildGrailSprite => m_BuildGrailSprite;
    public Sprite DefeatHeroSprite => m_DefeatHeroSprite;
    public Sprite CaptureTownSprite => m_CaptureTownSprite;
    public Sprite DefeatMonsterSprite => m_DefeatMonsterSprite;
    public Sprite DwellingsSprite => m_DwellingsSprite;
    public Sprite MinesSprite => m_MinesSprite;
    public Sprite TransportSprite => m_TransportSprite;

    [Space]

    [SerializeField] Sprite m_NormalWinSprite;
    [SerializeField] Sprite m_AcquireArtifactSprite;
    [SerializeField] Sprite m_CreaturesSprite;
    [SerializeField] Sprite m_ResourcesSprite;
    [SerializeField] Sprite m_UpgradeTownSprite;
    [SerializeField] Sprite m_BuildGrailSprite;
    [SerializeField] Sprite m_DefeatHeroSprite;
    [SerializeField] Sprite m_CaptureTownSprite;
    [SerializeField] Sprite m_DefeatMonsterSprite;
    [SerializeField] Sprite m_DwellingsSprite;
    [SerializeField] Sprite m_MinesSprite;
    [SerializeField] Sprite m_TransportSprite;

    public Sprite NormalLossSprite => m_NormalLossSprite;
    public Sprite LoseTownSprite => m_LoseTownSprite;
    public Sprite LoseHeroSprite => m_LoseHeroSprite;
    public Sprite TimeExpiresSprite => m_TimeExpiresSprite;

    [Space]

    [SerializeField] Sprite m_NormalLossSprite;
    [SerializeField] Sprite m_LoseTownSprite;
    [SerializeField] Sprite m_LoseHeroSprite;
    [SerializeField] Sprite m_TimeExpiresSprite;

    List<Map> m_CurrentMaps;
    Map m_SelectedMap;

    public int ListOffset { get; private set; }

    enum SortType
    {
        Unsorted,
        Name,
        Size,
        Version,
        PlayerCount,
        WinCondition,
        LossCondition
    }

    SortType m_SortType = SortType.Unsorted;

    void Awake()
    {
        for (int i = 0; i < m_ScenarioEntries.Count; i++)
        {
            m_ScenarioEntries[i].ScenarioList = this;
        }

        m_CurrentMaps = m_Maps;

        VersionSort();

        PopulateScenarioEntries(0);

        if (m_CurrentMaps.Count > 0)
        {
            EntryClicked(m_CurrentMaps[0]);
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
        a_Offset = Mathf.Clamp(a_Offset, 0, m_CurrentMaps.Count - m_ScenarioEntries.Count);

        ListOffset = a_Offset;

        int _EntriesToPopulate = Mathf.Min(m_ScenarioEntries.Count, m_CurrentMaps.Count - a_Offset);

        for (int i = 0; i < _EntriesToPopulate; i++)
        {
            m_ScenarioEntries[i].SetMap(m_CurrentMaps[i + a_Offset]);
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

        switch (a_Map.WinCondition)
        {
            case 255:
                m_DetailsWinCondition.text = "Defeat All Enemies";
                m_DetailsWinConditionImage.sprite = m_NormalWinSprite;
                break;

            case 0:
                m_DetailsWinCondition.text = "Acquire Artifact or Defeat All Enemies";
                m_DetailsWinConditionImage.sprite = m_AcquireArtifactSprite;
                break;

            case 1:
                m_DetailsWinCondition.text = "Accumulate Creatures";
                m_DetailsWinConditionImage.sprite = m_CreaturesSprite;
                break;

            case 2:
                m_DetailsWinCondition.text = "Accumulate Resources or Defeat All Enemies";
                m_DetailsWinConditionImage.sprite = m_ResourcesSprite;
                break;

            case 3:
                m_DetailsWinCondition.text = "Upgrade Town or Defeat All Enemies";
                m_DetailsWinConditionImage.sprite = m_UpgradeTownSprite;
                break;

            case 4:
                m_DetailsWinCondition.text = "Build a Grail Structure or Defeat All Enemies";
                m_DetailsWinConditionImage.sprite = m_BuildGrailSprite;
                break;

            case 5:
                m_DetailsWinCondition.text = "Defeat Hero or Defeat All Enemies";
                m_DetailsWinConditionImage.sprite = m_DefeatHeroSprite;
                break;

            case 6:
                m_DetailsWinCondition.text = "Capture Town or Defeat All Enemies";
                m_DetailsWinConditionImage.sprite = m_CaptureTownSprite;
                break;

            case 7:
                m_DetailsWinCondition.text = "Defeat Monster";
                m_DetailsWinConditionImage.sprite = m_DefeatMonsterSprite;
                break;

            case 8:
                m_DetailsWinCondition.text = "Flag All Creature Dwellings";
                m_DetailsWinConditionImage.sprite = m_DwellingsSprite;
                break;

            case 9:
                m_DetailsWinCondition.text = "Flag All Mines";
                m_DetailsWinConditionImage.sprite = m_MinesSprite;
                break;

            case 10:
                m_DetailsWinCondition.text = "Transport Artifact or Defeat All Enemies";
                m_DetailsWinConditionImage.sprite = m_TransportSprite;
                break;
        }

        switch (a_Map.LossCondition)
        {
            case 255:
                m_DetailsLossCondition.text = "Lose All Your Towns and Heroes";
                m_DetailsLossConditionImage.sprite = m_NormalLossSprite;
                break;

            case 0:
                m_DetailsLossCondition.text = "Lose Town";
                m_DetailsLossConditionImage.sprite = m_LoseTownSprite;
                break;

            case 1:
                m_DetailsLossCondition.text = "Lose Hero";
                m_DetailsLossConditionImage.sprite = m_LoseHeroSprite;
                break;

            case 2:
                m_DetailsLossCondition.text = "Time Expires";
                m_DetailsLossConditionImage.sprite = m_TimeExpiresSprite;
                break;
        }

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
            m_CurrentMaps = m_CurrentMaps.OrderBy((a_Map) => a_Map.Name).ToList();

            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.Name).ToList();

            m_SortType = SortType.Name;
        }
        else
        {
            m_CurrentMaps.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void SizeSort()
    {
        if (m_SortType != SortType.Size)
        {
            m_CurrentMaps = m_CurrentMaps.OrderBy((a_Map) => a_Map.Size).ToList();

            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.Size).ToList();

            m_SortType = SortType.Size;
        }
        else
        {
            m_CurrentMaps.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void VersionSort()
    {
        if (m_SortType != SortType.Version)
        {
            m_CurrentMaps = m_CurrentMaps.OrderBy((a_Map) => a_Map.Name).ToList();
            m_CurrentMaps = m_CurrentMaps.OrderBy((a_Map) => a_Map.Version).ToList();

            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.Name).ToList();
            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.Version).ToList();

            m_SortType = SortType.Version;
        }
        else
        {
            m_CurrentMaps.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void PlayerCountSort()
    {
        if (m_SortType != SortType.PlayerCount)
        {
            m_CurrentMaps = m_CurrentMaps.OrderBy((a_Map) => a_Map.PlayerCount).ToList();
            m_CurrentMaps = m_CurrentMaps.OrderBy((a_Map) => a_Map.ComputerCount).ToList();

            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.PlayerCount).ToList();
            m_Maps = m_Maps.OrderBy((a_Map) => a_Map.ComputerCount).ToList();

            m_SortType = SortType.PlayerCount;
        }
        else
        {
            m_CurrentMaps.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void WinConditionSort()
    {
        if (m_SortType != SortType.WinCondition)
        {
            m_CurrentMaps = m_CurrentMaps.OrderBy((a_Map) => (byte)(a_Map.WinCondition + 1)).ToList();

            m_Maps = m_Maps.OrderBy((a_Map) => (byte)(a_Map.WinCondition + 1)).ToList();

            m_SortType = SortType.WinCondition;
        }
        else
        {
            m_CurrentMaps.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void LossConditionSort()
    {
        if (m_SortType != SortType.LossCondition)
        {
            m_CurrentMaps = m_CurrentMaps.OrderBy((a_Map) => (byte)(a_Map.LossCondition + 1)).ToList();

            m_Maps = m_Maps.OrderBy((a_Map) => (byte)(a_Map.LossCondition + 1)).ToList();

            m_SortType = SortType.LossCondition;
        }
        else
        {
            m_CurrentMaps.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void SmallFilter()
    {
        m_CurrentMaps = m_Maps.Where((a_Map) => a_Map.Size == 36).ToList();

        m_Scrollbar.UpdateScrollSegments();

        PopulateScenarioEntries(0);
    }

    public void MedFilter()
    {
        m_CurrentMaps = m_Maps.Where((a_Map) => a_Map.Size == 72).ToList();

        m_Scrollbar.UpdateScrollSegments();

        PopulateScenarioEntries(0);
    }

    public void LargeFilter()
    {
        m_CurrentMaps = m_Maps.Where((a_Map) => a_Map.Size == 108).ToList();

        m_Scrollbar.UpdateScrollSegments();

        PopulateScenarioEntries(0);
    }
    
    public void XLFilter()
    {
        m_CurrentMaps = m_Maps.Where((a_Map) => a_Map.Size == 144).ToList();

        m_Scrollbar.UpdateScrollSegments();

        PopulateScenarioEntries(0);
    }

    public void AllFilter()
    {
        m_CurrentMaps = m_Maps;

        m_Scrollbar.UpdateScrollSegments();

        PopulateScenarioEntries(0);
    }
}
