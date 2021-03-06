﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

        public Sprite TownUI;
        public Sprite TownScreenFlag;
        public Sprite TownHallUI1; // Castle
        public Sprite TownHallUI2; // Rampart, Fortress, Conflux
        public Sprite TownHallUI3; // Tower, Necropolis, Dungeon, Stronghold
        [FormerlySerializedAs("BuildWindow")]
        public Sprite BuildPanel;

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
        public Sprite ViewWorld;
        public Sprite ViewWorldPressed;
        public Sprite ReplayOpponentTurn;
        public Sprite ReplayOpponentTurnPressed;
        public Sprite ViewPuzzle;
        public Sprite ViewPuzzlePressed;
        public Sprite Dig;
        public Sprite DigPressed;
    }

    public PlayerElements[] Elements;
    public HeroFlagVisualData[] Flags;
    public Color[] Colors;
}
