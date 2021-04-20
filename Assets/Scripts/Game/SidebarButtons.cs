using UnityEngine;
using UnityEngine.UI;

public class SidebarButtons : MonoBehaviour
{
    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] PlayerColors m_PlayerColors;
    [SerializeField] Map m_Map;
    [SerializeField] LocalOwnership m_LocalOwnership;

    [Header("High Res")]
    [SerializeField] Button m_KingdomOverviewHighRes;
    [SerializeField] Button m_UndergroundHighRes;
    [SerializeField] Button m_OverworldHighRes;
    [SerializeField] Button m_ViewWorldHighRes;
    [SerializeField] Button m_ReplayOpponentTurnHighRes;
    [SerializeField] Button m_ViewPuzzleHighRes;
    [SerializeField] Button m_DigHighRes;
    [SerializeField] Button m_QuestLogHighRes;
    [SerializeField] Button m_SleepHeroHighRes;
    [SerializeField] Button m_WakeHeroHighRes;
    [SerializeField] Button m_MoveHeroHighRes;
    [SerializeField] Button m_SpellbookHighRes;
    [SerializeField] Button m_ScenarioInfoHighRes;
    [SerializeField] Button m_SystemOptionsHighRes;
    [SerializeField] Button m_NextHeroHighRes;
    [SerializeField] Button m_EndTurnHighRes;

    [Header("Low Res")]
    [SerializeField] Button m_KingdomOverviewLowRes;
    [SerializeField] Button m_UndergroundLowRes;
    [SerializeField] Button m_OverworldLowRes;
    [SerializeField] Button m_QuestLogLowRes;
    [SerializeField] Button m_SleepHeroLowRes;
    [SerializeField] Button m_WakeHeroLowRes;
    [SerializeField] Button m_MoveHeroLowRes;
    [SerializeField] Button m_SpellbookLowRes;
    [SerializeField] Button m_AdventureOptionsLowRes;
    [SerializeField] Button m_SystemOptionsLowRes;
    [SerializeField] Button m_NextHeroLowRes;
    [SerializeField] Button m_EndTurnLowRes;

    // Effectively pointless, but this allows us to check for problems in the hero selection events
    MapHero m_SelectedHero;

    void Awake()
    {
        PlayerColors.PlayerElements _Elements = m_PlayerColors.Elements[m_GameSettings.LocalPlayerIndex];

        SpriteState _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.KingdomOverviewPressed;
        _SpriteState.disabledSprite = _Elements.KingdomOverviewDisabled;

        m_KingdomOverviewHighRes.image.sprite = _Elements.KingdomOverview;
        m_KingdomOverviewHighRes.spriteState = _SpriteState;
        m_KingdomOverviewLowRes.image.sprite = _Elements.KingdomOverview;
        m_KingdomOverviewLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.UndergroundPressed;
        _SpriteState.disabledSprite = _Elements.UndergroundDisabled;

        m_UndergroundHighRes.image.sprite = _Elements.Underground;
        m_UndergroundHighRes.spriteState = _SpriteState;
        m_UndergroundLowRes.image.sprite = _Elements.Underground;
        m_UndergroundLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.OverworldPressed;
        _SpriteState.disabledSprite = _Elements.OverworldDisabled;

        m_OverworldHighRes.image.sprite = _Elements.Overworld;
        m_OverworldHighRes.spriteState = _SpriteState;
        m_OverworldLowRes.image.sprite = _Elements.Overworld;
        m_OverworldLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.ViewWorldPressed;

        m_ViewWorldHighRes.image.sprite = _Elements.ViewWorld;
        m_ViewWorldHighRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.ReplayOpponentTurnPressed;

        m_ReplayOpponentTurnHighRes.image.sprite = _Elements.ReplayOpponentTurn;
        m_ReplayOpponentTurnHighRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.ViewPuzzlePressed;

        m_ViewPuzzleHighRes.image.sprite = _Elements.ViewPuzzle;
        m_ViewPuzzleHighRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.DigPressed;

        m_DigHighRes.image.sprite = _Elements.Dig;
        m_DigHighRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.QuestLogPressed;
        _SpriteState.disabledSprite = _Elements.QuestLogDisabled;

        m_QuestLogHighRes.image.sprite = _Elements.QuestLog;
        m_QuestLogHighRes.spriteState = _SpriteState;
        m_QuestLogLowRes.image.sprite = _Elements.QuestLog;
        m_QuestLogLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.SleepHeroPressed;
        _SpriteState.disabledSprite = _Elements.SleepHeroDisabled;

        m_SleepHeroHighRes.image.sprite = _Elements.SleepHero;
        m_SleepHeroHighRes.spriteState = _SpriteState;
        m_SleepHeroLowRes.image.sprite = _Elements.SleepHero;
        m_SleepHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.WakeHeroPressed;
        _SpriteState.disabledSprite = _Elements.WakeHeroDisabled;

        m_WakeHeroHighRes.image.sprite = _Elements.WakeHero;
        m_WakeHeroHighRes.spriteState = _SpriteState;
        m_WakeHeroLowRes.image.sprite = _Elements.WakeHero;
        m_WakeHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.MoveHeroPressed;
        _SpriteState.disabledSprite = _Elements.MoveHeroDisabled;

        m_MoveHeroHighRes.image.sprite = _Elements.MoveHero;
        m_MoveHeroHighRes.spriteState = _SpriteState;
        m_MoveHeroLowRes.image.sprite = _Elements.MoveHero;
        m_MoveHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.SpellbookPressed;
        _SpriteState.disabledSprite = _Elements.SpellbookDisabled;

        m_SpellbookHighRes.image.sprite = _Elements.Spellbook;
        m_SpellbookHighRes.spriteState = _SpriteState;
        m_SpellbookLowRes.image.sprite = _Elements.Spellbook;
        m_SpellbookLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.AdventureOptionsPressed;
        _SpriteState.disabledSprite = _Elements.AdventureOptionsDisabled;

        m_ScenarioInfoHighRes.image.sprite = _Elements.AdventureOptions;
        m_ScenarioInfoHighRes.spriteState = _SpriteState;
        m_AdventureOptionsLowRes.image.sprite = _Elements.AdventureOptions;
        m_AdventureOptionsLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.SystemOptionsPressed;
        _SpriteState.disabledSprite = _Elements.SystemOptionsDisabled;

        m_SystemOptionsHighRes.image.sprite = _Elements.SystemOptions;
        m_SystemOptionsHighRes.spriteState = _SpriteState;
        m_SystemOptionsLowRes.image.sprite = _Elements.SystemOptions;
        m_SystemOptionsLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.NextHeroPressed;
        _SpriteState.disabledSprite = _Elements.NextHeroDisabled;

        m_NextHeroHighRes.image.sprite = _Elements.NextHero;
        m_NextHeroHighRes.spriteState = _SpriteState;
        m_NextHeroLowRes.image.sprite = _Elements.NextHero;
        m_NextHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = _Elements.EndTurnPressed;
        _SpriteState.disabledSprite = _Elements.EndTurnDisabled;

        m_EndTurnHighRes.image.sprite = _Elements.EndTurn;
        m_EndTurnHighRes.spriteState = _SpriteState;
        m_EndTurnLowRes.image.sprite = _Elements.EndTurn;
        m_EndTurnLowRes.spriteState = _SpriteState;

        m_LocalOwnership.OnHeroSelected += OnHeroSelected;
        m_LocalOwnership.OnHeroDeselected += OnHeroDeselected;
    }

    void OnHeroSelected(MapHero a_Hero, int a_Index)
    {
        if (a_Hero.HasPath)
        {
            m_MoveHeroHighRes.interactable = true;
            m_MoveHeroLowRes.interactable = true;
        }

        a_Hero.OnPathCreated += OnPathCreated;
        a_Hero.OnPathRemoved += OnPathRemoved;

        m_SelectedHero = a_Hero;
    }

    void OnHeroDeselected(MapHero a_Hero)
    {
        if (a_Hero != m_SelectedHero)
        {
            Debug.LogError($"!! EVENTS ARE BROKE");
        }

        a_Hero.OnPathCreated -= OnPathCreated;
        a_Hero.OnPathRemoved -= OnPathRemoved;

        m_MoveHeroHighRes.interactable = false;
        m_MoveHeroLowRes.interactable = false;
    }

    void OnPathCreated()
    {
        m_MoveHeroHighRes.interactable = true;
        m_MoveHeroLowRes.interactable = true;
    }

    void OnPathRemoved()
    {
        m_MoveHeroHighRes.interactable = false;
        m_MoveHeroLowRes.interactable = false;
    }

    public void KingdomOverviewPressed()
    {

    }

    public void UndergroundPressed()
    {
        m_Map.ShowUnderground(true);

        m_OverworldHighRes.gameObject.SetActive(true);
        m_OverworldLowRes.gameObject.SetActive(true);
        m_UndergroundHighRes.gameObject.SetActive(false);
        m_UndergroundLowRes.gameObject.SetActive(false);
    }

    public void OverworldPressed()
    {
        m_Map.ShowUnderground(false);

        m_OverworldHighRes.gameObject.SetActive(false);
        m_OverworldLowRes.gameObject.SetActive(false);
        m_UndergroundHighRes.gameObject.SetActive(true);
        m_UndergroundLowRes.gameObject.SetActive(true);
    }

    public void ViewWorldPressed()
    {

    }

    public void ReplayOpponentTurnPressed()
    {

    }

    public void ViewPuzzlePressed()
    {

    }

    public void DigPressed()
    {

    }

    public void QuestLogPressed()
    {
        // lol, this is never getting used
    }

    public void SleepHeroPressed()
    {

    }

    public void WakeHeroPressed()
    {

    }

    public void MoveHeroPressed()
    {
        MapHero _SelectedHero = m_LocalOwnership.SelectedHero;

        m_Map.ShowUnderground(_SelectedHero.IsUnderground);

        // Not doing null checking, because the button should only be interactable if a hero is selected
        _SelectedHero.MoveToDestination();
    }

    public void SpellbookPressed()
    {

    }

    public void AdventureOptionsPressed()
    {

    }

    public void ScenarioInfoPressed()
    {

    }

    public void SystemOptionsPressed()
    {

    }

    public void NextHeroPressed()
    {

    }

    public void EndTurnPressed()
    {
        // Networking, oooo
    }
}
