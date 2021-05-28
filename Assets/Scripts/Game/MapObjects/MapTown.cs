using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapTown : MapObjectBase
{
    public Faction Faction { get; private set; }
    public int PlayerIndex { get; private set; }
    public bool IsUnderground { get; private set; }
    public BuiltBuildings Buildings { get; private set; }

    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] MapObjectRenderer m_Renderer;
    [SerializeField] MapObjectRenderer m_ShadowRenderer;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] PlayerColors m_PlayerColors;
    [SerializeField] FactionList m_Factions;

    public void Initialize(ScenarioObject a_ScenarioObject, GameReferences a_GameReferences)
    {
        m_GameReferences = a_GameReferences;

        m_SpriteRenderer.sortingOrder = -32767 + a_ScenarioObject.SortOrder;

        PlayerIndex = a_ScenarioObject.Town.Owner;
        IsUnderground = a_ScenarioObject.IsUnderground;

        // Neutral objects are 255
        if (PlayerIndex == 255)
        {
            PlayerIndex = 8;
        }

        switch (a_ScenarioObject.Template.Name)
        {
            case "avcranx0":
            case "avcranz0":
            case "avcrand0":
                if (PlayerIndex != 8)
                {
                    Faction = m_GameSettings.Players.First(a_Player => a_Player.Index == PlayerIndex).Faction;
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

        m_SpriteRenderer.material.SetColor("_PlayerColor", m_PlayerColors.Colors[PlayerIndex]);

        if (a_ScenarioObject.Town.HasCustomBuildings)
        {
            Buildings = a_ScenarioObject.Town.CustomBuildings;
        }
        else
        {
            Buildings = new BuiltBuildings();
            Buildings.Tavern = true;

            if (a_ScenarioObject.Town.HasFort)
            {
                Buildings.Fort = true;
                Buildings.Dwelling1 = true;

                int _Random = Random.Range(0, 3);

                if (_Random == 0)
                {
                    Buildings.Dwelling2 = true;
                }
            }
        }

        if (Buildings.Capitol)
        {
            m_Renderer.SetSprites(Faction.MapVisualDataCapitol.m_Sprites);
            m_ShadowRenderer.SetSprites(Faction.MapVisualDataCapitol.m_ShadowSprites);
        }
        else if (Buildings.Fort)
        {
            m_Renderer.SetSprites(Faction.MapVisualData.m_Sprites);
            m_ShadowRenderer.SetSprites(Faction.MapVisualData.m_ShadowSprites);
        }
        else
        {
            m_Renderer.SetSprites(Faction.MapVisualDataNoFort.m_Sprites);
            m_ShadowRenderer.SetSprites(Faction.MapVisualDataNoFort.m_ShadowSprites);
        }

        if (a_ScenarioObject.Town.Name != "")
        {
            gameObject.name = a_ScenarioObject.Town.Name;
        }
        else
        {
            gameObject.name = Faction.name;
        }

        if (PlayerIndex == m_GameSettings.LocalPlayerIndex)
        {
            m_GameReferences.LocalOwnership.AddTown(this);
        }
    }
}
