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
        public Sprite HeroCard;

        // Buttons
        public Sprite NextHero;
        public Sprite NextHeroPressed;
        public Sprite NextHeroDisabled;
        public Sprite EndTurn;
        public Sprite EndTurnPressed;
        public Sprite EndTurnDisabled;
        public Sprite KingdomOverview;
        public Sprite KingdomOverviewPressed;
        public Sprite KingdomOverviewDisabled;
        public Sprite Underground;
        public Sprite UndergroundPressed;
        public Sprite UndergroundDisabled;
        public Sprite Overworld;
        public Sprite OverworldPressed;
        public Sprite OverworldDisabled;
        public Sprite QuestLog;
        public Sprite QuestLogPressed;
        public Sprite QuestLogDisabled;
        public Sprite SleepHero;
        public Sprite SleepHeroPressed;
        public Sprite SleepHeroDisabled;
        public Sprite WakeHero;
        public Sprite WakeHeroPressed;
        public Sprite WakeHeroDisabled;
        public Sprite MoveHero;
        public Sprite MoveHeroPressed;
        public Sprite MoveHeroDisabled;
        public Sprite Spellbook;
        public Sprite SpellbookPressed;
        public Sprite SpellbookDisabled;
        public Sprite AdventureOptions;
        public Sprite AdventureOptionsPressed;
        public Sprite AdventureOptionsDisabled;
        public Sprite SystemOptions;
        public Sprite SystemOptionsPressed;
        public Sprite SystemOptionsDisabled;
    }

    public PlayerElements[] Elements;
    public HeroFlagVisualData[] Flags;
    public Color[] Colors;
}
