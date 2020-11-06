using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwelling : MonoBehaviour
{
    public MapObject MapObject { get; private set; }

    public void Initialize(MapObject a_Object)
    {
        MapObject = a_Object;

        uint _ColorIndex = a_Object.ScenarioObject.DwellingOwner;

        // Seems there are cases where _ColorIndex isn't 255, so doing this sweeping check
        // Not sure if the different values above 7 mean anything
        if (_ColorIndex > 8)
        {
            _ColorIndex = 8;
        }

        MapObject.SpriteRenderer.material.SetColor("_PlayerColor", a_Object.PlayerColors.Colors[_ColorIndex]);
    }
}
