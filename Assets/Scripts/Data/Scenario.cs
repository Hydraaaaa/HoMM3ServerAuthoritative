using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerInfo
{
    public bool HumanPlayable;
    public bool ComputerPlayable;
    public byte AIBehaviour;
    public bool LimitedTownChoice;
    public ushort AllowedTowns;
    public bool IsTownChoosable;
    public bool HasMainTown;
    public int MainTownXCoord;
    public int MainTownYCoord;
    public bool IsMainTownUnderground;
    public byte MainTownType;
    public bool GenerateHeroAtMainTown;
    public bool IsMainHeroRandom;
    public byte MainHeroType;
    public byte MainHeroPortrait;
    public string MainHeroName;
    public byte Team;

    public List<byte> HeroPortraits;
    public List<string> HeroNames;
}

public enum ScenarioObjectType
{
    Artifact,
    PandorasBox,
    Dwelling,
    Event,
    Garrison,
    Hero,
    Grail,
    Monster,
    Resource,
    Scholar,
    Seer,
    Shrine,
    Sign,
    Spell,
    Town,
    WitchsHut,
    QuestionGuard,
    GeneralDwelling,
    LevelDwelling,
    TownDwelling,
    AbandonedMine,
    Unknown
}

[Serializable]
public class ScenarioObjectTemplate
{
    public string Name;
    public int MineType;

    public ScenarioObjectType Type;
    public int TypeDebug;

    public byte[] Passability;
    public byte[] Interactability;

    public ushort Landscape;
    public bool IsLowPrioritySortOrder;
}

[Serializable]
public class ScenarioObject
{
    public ScenarioObjectTemplate Template;

    public byte PosX;
    public byte PosY;
    public bool IsUnderground;
    public int SortOrder;

    public ScenarioObjectMonster Monster;
    public ScenarioObjectTown Town;
    public uint DwellingOwner;
}

[Serializable]
public class ScenarioObjectMonster
{
    public int Type;
    public int Count;
    public byte Mood;
    public bool IsTreasureOrText;
    public string Text;
    public int Wood;
    public int Mercury;
    public int Ore;
    public int Sulfur;
    public int Crystal;
    public int Gem;
    public int Gold;
    public short ArtifactID;
    public bool NeverRun;
    public bool DontGrow;
}

[Serializable]
public class ScenarioObjectTown
{
    public byte Owner;
    public bool IsNamed;
    public string Name;
    public bool IsGuarded;
    public List<MonsterStack> Guards;
    public bool IsGroupFormation;
    public bool HasCustomBuildings;
    public bool HasFort;
}

[Serializable]
public class MonsterStack
{
    public ushort ID;
    public ushort Amount;
}

[Serializable]
public class TerrainTile
{
    public byte TerrainType;
    public byte TerrainSpriteID;
    public byte RiverType;
    public byte RiverSpriteID;
    public byte RoadType;
    public byte RoadSpriteID;
    public byte Mirrored;
}

[Serializable]
public class HeroInfo
{
    public byte ID;
    public byte Portrait;
    public string Name;

    // Which players can choose this hero
    public byte Players;
}

public class Scenario : ScriptableObject
{
    public const int RESTORATION_OF_ERATHIA = 0x0E;
    public const int ARMAGEDDONS_BLADE = 0x15;
    public const int SHADOW_OF_DEATH = 0x1C;

    public int Version;
    public string Name;
    [TextArea(1, 10)]
    public string Description;

    public int Size;
    public bool HasUnderground;

    public byte Difficulty;

    public List<PlayerInfo> PlayerInfo;

    public int PlayerCount;
    public int ComputerCount;

    public byte WinCondition;
    public byte LossCondition;

    public bool HasTeams;

    // Available Heroes
    // If a hero is already present on the map, they won't be available here
    public byte[] AvailableHeroes;

    public List<HeroInfo> HeroInfo;

    public List<TerrainTile> Terrain;
    public List<TerrainTile> UndergroundTerrain;

    public List<ScenarioObject> Objects;
}
