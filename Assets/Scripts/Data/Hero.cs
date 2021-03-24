using System;
using UnityEngine;

[Serializable]
public class Hero
{
    public string Name;
    public int ID;
    public Faction Faction;

    public Sprite Portrait;

    public Hero()
    {
        Name = "";
        ID = 0;
        Faction = null;
        Portrait = null;
    }

    public Hero(Hero a_Copy)
    {
        Name = a_Copy.Name;
        ID = a_Copy.ID;
        Faction = a_Copy.Faction;
        Portrait = a_Copy.Portrait;
    }
}