using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    public MapObject MapObject { get; private set; }

    public void Initialize(MapObject a_Object)
    {
        MapObject = a_Object;

        int _ColorIndex = a_Object.ScenarioObject.Town.Owner;

        if (_ColorIndex == 255)
        {
            _ColorIndex = 8;
        }

        MapObject.SpriteRenderer.material.SetColor("_PlayerColor", a_Object.PlayerColors.Colors[_ColorIndex]);
    }
}
