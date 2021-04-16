using UnityEngine;
using UnityEngine.UI;

public class SidebarButtons : MonoBehaviour
{
    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] PlayerColors m_PlayerColors;

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

        m_KingdomOverviewLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].KingdomOverview;
        m_KingdomOverviewLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].UndergroundPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].UndergroundDisabled;

        m_UndergroundLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].Underground;
        m_UndergroundLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].OverworldPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].OverworldDisabled;

        m_OverworldLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].Overworld;
        m_OverworldLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].QuestLogPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].QuestLogDisabled;

        m_QuestLogLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].QuestLog;
        m_QuestLogLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].SleepHeroPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].SleepHeroDisabled;

        m_SleepHeroLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].SleepHero;
        m_SleepHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].WakeHeroPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].WakeHeroDisabled;

        m_WakeHeroLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].WakeHero;
        m_WakeHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].MoveHeroPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].MoveHeroDisabled;

        m_MoveHeroLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].MoveHero;
        m_MoveHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].SpellbookPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].SpellbookDisabled;

        m_SpellbookLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].Spellbook;
        m_SpellbookLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].AdventureOptionsPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].AdventureOptionsDisabled;

        m_AdventureOptionsLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].AdventureOptions;
        m_AdventureOptionsLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].SystemOptionsPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].SystemOptionsDisabled;

        m_SystemOptionsLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].SystemOptions;
        m_SystemOptionsLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].NextHeroPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].NextHeroDisabled;

        m_NextHeroLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].NextHero;
        m_NextHeroLowRes.spriteState = _SpriteState;

        _SpriteState = new SpriteState();
        _SpriteState.pressedSprite = m_PlayerColors.Elements[PlayerIndex].EndTurnPressed;
        _SpriteState.disabledSprite = m_PlayerColors.Elements[PlayerIndex].EndTurnDisabled;

        m_EndTurnLowRes.image.sprite = m_PlayerColors.Elements[PlayerIndex].EndTurn;
        m_EndTurnLowRes.spriteState = _SpriteState;
    }
}
