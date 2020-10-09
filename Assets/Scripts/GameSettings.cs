using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public Scenario Scenario;
    public int Rating;
    public int LocalPlayerIndex = 0;
}
