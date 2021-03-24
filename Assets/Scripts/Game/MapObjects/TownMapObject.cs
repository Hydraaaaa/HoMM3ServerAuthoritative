using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMapObject : MonoBehaviour
{
    public MapObject MapObject { get; private set; }
    public Faction Faction { get; private set; }

    public void Initialize(MapObject a_Object)
    {
        MapObject = a_Object;

        int _ColorIndex = a_Object.ScenarioObject.Town.Owner;

        if (_ColorIndex == 255)
        {
            _ColorIndex = 8;
        }

        MapObject.SpriteRenderer.material.SetColor("_PlayerColor", a_Object.PlayerColors.Colors[_ColorIndex]);

        switch (gameObject.name)
        {
            case "avcranx0":
            case "avcranz0":
            case "avcrand0":
                if (_ColorIndex != 8)
                {
                    Faction = MapObject.Map.GameSettings.Players[_ColorIndex].Faction;
                }
                else
                {
                    Faction = MapObject.Factions.Factions[Random.Range(0, MapObject.Factions.Factions.Count)];
                }
                break;

            case "avccasx0":
            case "avccasz0":
            case "avccast0":
                Faction = MapObject.Factions.Factions[0];
                break;

            case "avcramx0":
            case "avcramz0":
            case "avcramp0":
                Faction = MapObject.Factions.Factions[1];
                break;

            case "avctowx0":
            case "avctowz0":
            case "avctowr0":
                Faction = MapObject.Factions.Factions[2];
                break;

            case "avcinfx0":
            case "avcinfz0":
            case "avcinft0":
                Faction = MapObject.Factions.Factions[3];
                break;

            case "avcnecx0":
            case "avcnecz0":
            case "avcnecr0":
                Faction = MapObject.Factions.Factions[4];
                break;

            case "avcdunx0":
            case "avcdunz0":
            case "avcdung0":
                Faction = MapObject.Factions.Factions[5];
                break;

            case "avcstrx0":
            case "avcstrz0":
            case "avcstro0":
                Faction = MapObject.Factions.Factions[6];
                break;

            case "avcftrx0":
            case "avcforz0":
            case "avcftrt0":
                Faction = MapObject.Factions.Factions[7];
                break;

            case "avchforx":
            case "avchforz":
            case "avchfor0":
                Faction = MapObject.Factions.Factions[8];
                break;
        }

        // TODO: Check buildings to determine if there's a fort or capitol

        MapObject.Renderer.SetSprites(Faction.MapVisualData.m_Sprites);
        MapObject.Shadow.Renderer.SetSprites(Faction.MapVisualData.m_ShadowSprites);
    }
}
