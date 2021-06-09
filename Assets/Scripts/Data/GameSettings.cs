using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Game Settings")]
public class GameSettings : ScriptableObject
{
    public const int RATING_EASY = 0;
    public const int RATING_NORMAL = 1;
    public const int RATING_HARD = 2;
    public const int RATING_EXPERT = 3;
    public const int RATING_IMPOSSIBLE = 4;

    [Serializable]
    public class Player
    {
        public bool IsPlayer;
        public bool IsLocalPlayer;
        public int Index;

        // If a hero has been selected, but the hero isn't generated at the main town
        // This flag will tell random heroes to prioritize being set as the player's main hero
        public bool SetMapHero;

        public Faction Faction;
        public Hero Hero;
        public StartingBonus StartingBonus;
    }

    public Scenario Scenario;
    public int Rating;
    public int LocalPlayerIndex = 0;

    public List<Player> Players = new List<Player>();
}
