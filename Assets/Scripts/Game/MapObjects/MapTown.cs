using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapTown : MonoBehaviour
{
    public Faction Faction { get; private set; }

    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] MapObjectRenderer m_Renderer;
    [SerializeField] MapObjectRenderer m_ShadowRenderer;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] PlayerColors m_PlayerColors;
    [SerializeField] FactionList m_Factions;

    public void Initialize(ScenarioObject a_ScenarioObject)
    {
        m_SpriteRenderer.sortingOrder = -32767 + a_ScenarioObject.SortOrder;

        int _ColorIndex = a_ScenarioObject.Town.Owner;

        if (_ColorIndex == 255)
        {
            _ColorIndex = 8;
        }

        m_SpriteRenderer.material.SetColor("_PlayerColor", m_PlayerColors.Colors[_ColorIndex]);

        switch (a_ScenarioObject.Template.Name)
        {
            case "avcranx0":
            case "avcranz0":
            case "avcrand0":
                if (_ColorIndex != 8)
                {
                    Faction = m_GameSettings.Players.First(a_Player => a_Player.Index == _ColorIndex).Faction;
                }
                else
                {
                    Faction = m_Factions.Factions[Random.Range(0, m_Factions.Factions.Count)];
                }
                break;

            case "avccasx0":
            case "avccasz0":
            case "avccast0":
                Faction = m_Factions.Factions[0];
                break;

            case "avcramx0":
            case "avcramz0":
            case "avcramp0":
                Faction = m_Factions.Factions[1];
                break;

            case "avctowx0":
            case "avctowz0":
            case "avctowr0":
                Faction = m_Factions.Factions[2];
                break;

            case "avcinfx0":
            case "avcinfz0":
            case "avcinft0":
                Faction = m_Factions.Factions[3];
                break;

            case "avcnecx0":
            case "avcnecz0":
            case "avcnecr0":
                Faction = m_Factions.Factions[4];
                break;

            case "avcdunx0":
            case "avcdunz0":
            case "avcdung0":
                Faction = m_Factions.Factions[5];
                break;

            case "avcstrx0":
            case "avcstrz0":
            case "avcstro0":
                Faction = m_Factions.Factions[6];
                break;

            case "avcftrx0":
            case "avcforz0":
            case "avcftrt0":
                Faction = m_Factions.Factions[7];
                break;

            case "avchforx":
            case "avchforz":
            case "avchfor0":
                Faction = m_Factions.Factions[8];
                break;
        }

        // TODO: Check buildings to determine if there's a fort or capitol

        m_Renderer.SetSprites(Faction.MapVisualData.m_Sprites);
        m_ShadowRenderer.SetSprites(Faction.MapVisualData.m_ShadowSprites);

        if (a_ScenarioObject.Town.Name != "")
        {
            gameObject.name = a_ScenarioObject.Town.Name;
        }
        else
        {
            gameObject.name = Faction.name;
        }
    }
}
