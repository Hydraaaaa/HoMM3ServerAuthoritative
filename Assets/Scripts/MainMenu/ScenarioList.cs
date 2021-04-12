using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioList : MonoBehaviour
{
    public List<Scenario> Scenarios => m_Scenarios;
    public List<Scenario> CurrentScenarios => m_CurrentScenarios;
    public List<ScenarioEntry> ScenarioEntries => m_ScenarioEntries;

    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] List<Scenario> m_Scenarios;
    [SerializeField] List<ScenarioEntry> m_ScenarioEntries;
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
    [SerializeField] ScenarioSettings m_ScenarioSettings;
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

    List<Scenario> m_CurrentScenarios;
    Scenario m_SelectedScenario;

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

        for (int i = m_Scenarios.Count - 1; i >= 0; i--)
        {
            if (m_Scenarios[i].Version != Scenario.SHADOW_OF_DEATH)
            {
                m_Scenarios.RemoveAt(i);
            }
        }

        m_CurrentScenarios = m_Scenarios;

        VersionSort();

        PopulateScenarioEntries(0);

        if (m_CurrentScenarios.Count > 0)
        {
            EntryClicked(m_CurrentScenarios[0]);
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
        if (m_CurrentScenarios.Count >= m_ScenarioEntries.Count)
        {
            a_Offset = Mathf.Clamp(a_Offset, 0, m_CurrentScenarios.Count - m_ScenarioEntries.Count);
        }
        else
        {
            a_Offset = 0;
        }

        ListOffset = a_Offset;

        int _EntriesToPopulate = Mathf.Min(m_ScenarioEntries.Count, m_CurrentScenarios.Count - a_Offset);

        for (int i = 0; i < _EntriesToPopulate; i++)
        {
            m_ScenarioEntries[i].SetScenario(m_CurrentScenarios[i + a_Offset]);
            m_ScenarioEntries[i].SetSelected(m_SelectedScenario);
        }

        for (int i = _EntriesToPopulate; i < m_ScenarioEntries.Count; i++)
        {
            m_ScenarioEntries[i].SetScenario(null);
        }
    }

    public void EntryClicked(Scenario a_Scenario)
    {
        m_SelectedScenario = a_Scenario;

        m_GameSettings.Scenario = a_Scenario;

        if (a_Scenario.Name != "")
        {
            m_DetailsName.text = a_Scenario.Name;
        }
        else
        {
            m_DetailsName.text = "Unnamed";
        }

        m_DetailsDescription.text = a_Scenario.Description;

        switch (a_Scenario.WinCondition)
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

        switch (a_Scenario.LossCondition)
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

        switch (a_Scenario.Difficulty)
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
            m_ScenarioEntries[i].SetSelected(m_SelectedScenario);
        }

        switch (a_Scenario.Size)
        {
            case 36: m_DetailsSizeImage.sprite = m_SmallSprite; break;
            case 72: m_DetailsSizeImage.sprite = m_MedSprite; break;
            case 108: m_DetailsSizeImage.sprite = m_LargeSprite; break;
            case 144: m_DetailsSizeImage.sprite = m_XLSprite; break;
        }

        m_ScenarioSettings.UpdateSettings(a_Scenario);
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
            m_CurrentScenarios = m_CurrentScenarios.OrderBy((a_Scenario) => a_Scenario.Name).ToList();

            m_Scenarios = m_Scenarios.OrderBy((a_Scenario) => a_Scenario.Name).ToList();

            m_SortType = SortType.Name;
        }
        else
        {
            m_CurrentScenarios.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void SizeSort()
    {
        if (m_SortType != SortType.Size)
        {
            m_CurrentScenarios = m_CurrentScenarios.OrderBy((a_Scenario) => a_Scenario.Size).ToList();

            m_Scenarios = m_Scenarios.OrderBy((a_Scenario) => a_Scenario.Size).ToList();

            m_SortType = SortType.Size;
        }
        else
        {
            m_CurrentScenarios.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void VersionSort()
    {
        if (m_SortType != SortType.Version)
        {
            m_CurrentScenarios = m_CurrentScenarios.OrderBy((a_Scenario) => a_Scenario.Name).ToList();
            m_CurrentScenarios = m_CurrentScenarios.OrderBy((a_Scenario) => a_Scenario.Version).ToList();

            m_Scenarios = m_Scenarios.OrderBy((a_Scenario) => a_Scenario.Name).ToList();
            m_Scenarios = m_Scenarios.OrderBy((a_Scenario) => a_Scenario.Version).ToList();

            m_SortType = SortType.Version;
        }
        else
        {
            m_CurrentScenarios.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void PlayerCountSort()
    {
        if (m_SortType != SortType.PlayerCount)
        {
            m_CurrentScenarios = m_CurrentScenarios.OrderBy((a_Scenario) => a_Scenario.PlayerCount).ToList();
            m_CurrentScenarios = m_CurrentScenarios.OrderBy((a_Scenario) => a_Scenario.ComputerCount).ToList();

            m_Scenarios = m_Scenarios.OrderBy((a_Scenario) => a_Scenario.PlayerCount).ToList();
            m_Scenarios = m_Scenarios.OrderBy((a_Scenario) => a_Scenario.ComputerCount).ToList();

            m_SortType = SortType.PlayerCount;
        }
        else
        {
            m_CurrentScenarios.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void WinConditionSort()
    {
        if (m_SortType != SortType.WinCondition)
        {
            m_CurrentScenarios = m_CurrentScenarios.OrderBy((a_Scenario) => (byte)(a_Scenario.WinCondition + 1)).ToList();

            m_Scenarios = m_Scenarios.OrderBy((a_Scenario) => (byte)(a_Scenario.WinCondition + 1)).ToList();

            m_SortType = SortType.WinCondition;
        }
        else
        {
            m_CurrentScenarios.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void LossConditionSort()
    {
        if (m_SortType != SortType.LossCondition)
        {
            m_CurrentScenarios = m_CurrentScenarios.OrderBy((a_Scenario) => (byte)(a_Scenario.LossCondition + 1)).ToList();

            m_Scenarios = m_Scenarios.OrderBy((a_Scenario) => (byte)(a_Scenario.LossCondition + 1)).ToList();

            m_SortType = SortType.LossCondition;
        }
        else
        {
            m_CurrentScenarios.Reverse();
        }

        PopulateScenarioEntries(0);
    }

    public void SmallFilter()
    {
        m_CurrentScenarios = m_Scenarios.Where((a_Scenario) => a_Scenario.Size == 36).ToList();

        m_Scrollbar.UpdateScrollSegments();

        PopulateScenarioEntries(0);
    }

    public void MedFilter()
    {
        m_CurrentScenarios = m_Scenarios.Where((a_Scenario) => a_Scenario.Size == 72).ToList();

        m_Scrollbar.UpdateScrollSegments();

        PopulateScenarioEntries(0);
    }

    public void LargeFilter()
    {
        m_CurrentScenarios = m_Scenarios.Where((a_Scenario) => a_Scenario.Size == 108).ToList();

        m_Scrollbar.UpdateScrollSegments();

        PopulateScenarioEntries(0);
    }

    public void XLFilter()
    {
        m_CurrentScenarios = m_Scenarios.Where((a_Scenario) => a_Scenario.Size == 144).ToList();

        m_Scrollbar.UpdateScrollSegments();

        PopulateScenarioEntries(0);
    }

    public void AllFilter()
    {
        m_CurrentScenarios = m_Scenarios;

        m_Scrollbar.UpdateScrollSegments();

        PopulateScenarioEntries(0);
    }
}
