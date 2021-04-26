using System;
using UnityEngine;

[Serializable]
public class Hero
{
    public string Name;
    public int ID;
    public Faction Faction;
    public HeroVisualData HeroVisualData;

    public Sprite SmallPortrait;
    public Sprite LargePortrait;

    public int Attack;
    public int Defense;
    public int Spellpower;
    public int Knowledge;

    public Hero()
    {
        Name = "";
        ID = 0;
        Faction = null;
        SmallPortrait = null;
        LargePortrait = null;
    }

    public Hero(Hero a_Copy)
    {
        Name = a_Copy.Name;
        ID = a_Copy.ID;
        Faction = a_Copy.Faction;
        HeroVisualData = a_Copy.HeroVisualData;

        SmallPortrait = a_Copy.SmallPortrait;
        LargePortrait = a_Copy.LargePortrait;

        Attack = a_Copy.Attack;
        Defense = a_Copy.Defense;
        Spellpower = a_Copy.Spellpower;
        Knowledge = a_Copy.Knowledge;
    }
}