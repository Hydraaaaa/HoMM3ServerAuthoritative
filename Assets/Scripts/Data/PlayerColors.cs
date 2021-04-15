using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Player Colors")]
public class PlayerColors : ScriptableObject
{
    [Serializable]
    public class PlayerElements
    {
        public Sprite Sidebar;
        public Sprite SidebarSmall;
        public Sprite BorderTopLeft;
        public Sprite BorderTop;
        public Sprite BorderLeft;
        public Sprite BorderRight;
        public Sprite BorderBottomLeft;
        public Sprite BorderBottomRight;
        public Sprite Resources;
        public Sprite BottomBarFill;
        public Sprite Date;

        // Buttons
        public Sprite NextHero;
        public Sprite NextHeroDisabled;
        public Sprite NextHeroHighlighted;
        public Sprite NextHeroPressed;
        public Sprite EndTurn;
        public Sprite EndTurnDisabled;
        public Sprite EndTurnHighlighted;
        public Sprite EndTurnPressed;
        public Sprite KingdomOverview;
        public Sprite KingdomOverviewDisabled;
        public Sprite KingdomOverviewHighlighted;
        public Sprite KingdomOverviewPressed;
        public Sprite Underground;
        public Sprite UndergroundDisabled;
        public Sprite UndergroundHighlighted;
        public Sprite UndergroundPressed;
        public Sprite Overworld;
        public Sprite OverworldDisabled;
        public Sprite OverworldHighlighted;
        public Sprite OverworldPressed;
        public Sprite QuestLog;
        public Sprite QuestLogDisabled;
        public Sprite QuestLogHighlighted;
        public Sprite QuestLogPressed;
        public Sprite SleepHero;
        public Sprite SleepHeroDisabled;
        public Sprite SleepHeroHighlighted;
        public Sprite SleepHeroPressed;
        public Sprite WakeHero;
        public Sprite WakeHeroDisabled;
        public Sprite WakeHeroHighlighted;
        public Sprite WakeHeroPressed;
        public Sprite MoveHero;
        public Sprite MoveHeroDisabled;
        public Sprite MoveHeroHighlighted;
        public Sprite MoveHeroPressed;
        public Sprite Spellbook;
        public Sprite SpellbookDisabled;
        public Sprite SpellbookHighlighted;
        public Sprite SpellbookPressed;
        public Sprite AdventureOptions;
        public Sprite AdventureOptionsDisabled;
        public Sprite AdventureOptionsHighlighted;
        public Sprite AdventureOptionsPressed;
        public Sprite SystemOptions;
        public Sprite SystemOptionsDisabled;
        public Sprite SystemOptionsHighlighted;
        public Sprite SystemOptionsPressed;
    }

    public PlayerElements[] Elements;
    public HeroFlagVisualData[] Flags;
    public Color[] Colors;
}
