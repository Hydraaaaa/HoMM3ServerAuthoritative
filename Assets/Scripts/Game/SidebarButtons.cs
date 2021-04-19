using UnityEngine;
using UnityEngine.UI;

public class SidebarButtons : MonoBehaviour
{
    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] PlayerColors m_PlayerColors;

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
    [SerializeField] Button m_AdventureOptionsHighRes;
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

    void Awake()
    {
        int PlayerIndex = m_GameSettings.LocalPlayerIndex;

        SpriteState _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].KingdomOverviewPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].KingdomOverviewDisabled;

        m_KingdomOverviewHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].KingdomOverview;
        m_KingdomOverviewHighRes.spriteState = _SpriteState;
        m_KingdomOverviewLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].KingdomOverview;
        m_KingdomOverviewLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].UndergroundPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].UndergroundDisabled;

        m_UndergroundHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].Underground;
        m_UndergroundHighRes.spriteState = _SpriteState;
        m_UndergroundLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].Underground;
        m_UndergroundLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].OverworldPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].OverworldDisabled;

        m_OverworldHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].Overworld;
        m_OverworldHighRes.spriteState = _SpriteState;
        m_OverworldLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].Overworld;
        m_OverworldLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].ViewWorldPressed;

        m_ViewWorldHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].ViewWorld;
        m_ViewWorldHighRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].ReplayOpponentTurnPressed;

        m_ReplayOpponentTurnHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].ReplayOpponentTurn;
        m_ReplayOpponentTurnHighRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].ViewPuzzlePressed;

        m_ViewPuzzleHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].ViewPuzzle;
        m_ViewPuzzleHighRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].DigPressed;

        m_DigHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].Dig;
        m_DigHighRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].QuestLogPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].QuestLogDisabled;

        m_QuestLogHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].QuestLog;
        m_QuestLogHighRes.spriteState = _SpriteState;
        m_QuestLogLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].QuestLog;
        m_QuestLogLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].SleepHeroPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].SleepHeroDisabled;

        m_SleepHeroHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].SleepHero;
        m_SleepHeroHighRes.spriteState = _SpriteState;
        m_SleepHeroLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].SleepHero;
        m_SleepHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].WakeHeroPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].WakeHeroDisabled;

        m_WakeHeroHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].WakeHero;
        m_WakeHeroHighRes.spriteState = _SpriteState;
        m_WakeHeroLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].WakeHero;
        m_WakeHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].MoveHeroPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].MoveHeroDisabled;

        m_MoveHeroHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].MoveHero;
        m_MoveHeroHighRes.spriteState = _SpriteState;
        m_MoveHeroLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].MoveHero;
        m_MoveHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].SpellbookPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].SpellbookDisabled;

        m_SpellbookHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].Spellbook;
        m_SpellbookHighRes.spriteState = _SpriteState;
        m_SpellbookLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].Spellbook;
        m_SpellbookLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].AdventureOptionsPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].AdventureOptionsDisabled;

        m_AdventureOptionsHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].AdventureOptions;
        m_AdventureOptionsHighRes.spriteState = _SpriteState;
        m_AdventureOptionsLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].AdventureOptions;
        m_AdventureOptionsLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].SystemOptionsPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].SystemOptionsDisabled;

        m_SystemOptionsHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].SystemOptions;
        m_SystemOptionsHighRes.spriteState = _SpriteState;
        m_SystemOptionsLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].SystemOptions;
        m_SystemOptionsLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].NextHeroPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].NextHeroDisabled;

        m_NextHeroHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].NextHero;
        m_NextHeroHighRes.spriteState = _SpriteState;
        m_NextHeroLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].NextHero;
        m_NextHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].EndTurnPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].EndTurnDisabled;

        m_EndTurnHighRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].EndTurn;
        m_EndTurnHighRes.spriteState = _SpriteState;
        m_EndTurnLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].EndTurn;
        m_EndTurnLowRes.spriteState = _SpriteState;
    }
}
