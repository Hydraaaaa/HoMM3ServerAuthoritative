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
    public string MainHeroName;
    public byte Team;

    public List<string> HeroNames;
}

public class Map : ScriptableObject
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
}
