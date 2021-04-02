using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [Serializable]
    public class Player
    {
        public bool IsPlayer;
        public bool IsLocalPlayer;
        public int Index;

        public Faction Faction;
        [SerializeReference] public Hero Hero;
        public StartingBonus StartingBonus;
    }

    public Scenario Scenario;
    public int Rating;
    public int LocalPlayerIndex = 0;

    public List<Player> Players = new List<Player>();
}
