﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;
    public MapObjectRenderer Renderer => m_Renderer;

    public PlayerColors PlayerColors => m_PlayerColors;
    public FactionList Factions => m_Factions;
    public MonsterList Monsters => m_Monsters;

    [SerializeField] protected SpriteRenderer m_SpriteRenderer = null;
    [SerializeField] protected MapObjectRenderer m_Renderer = null;

    [Space]

    [SerializeField] protected PlayerColors m_PlayerColors = null;
    [SerializeField] protected FactionList m_Factions = null;
    [SerializeField] protected MonsterList m_Monsters = null;
    [SerializeField] protected MapObjectVisualData m_Resources = null;

    [SerializeField] protected int m_X = 0;
    [SerializeField] protected int m_Y = 0;
    [SerializeField] protected byte[] m_Collision;
    [SerializeField] protected byte[] m_Interaction;

    public ScenarioObject ScenarioObject => m_ScenarioObject;
    public MapShadowObject Shadow => m_Shadow;

    protected ScenarioObject m_ScenarioObject;
    protected MapShadowObject m_Shadow;

    public void Initialize(ScenarioObject a_ScenarioObject, MapShadowObject a_Shadow)
    {
        m_X = a_ScenarioObject.PosX;
        m_Y = a_ScenarioObject.PosY;
        m_Collision = a_ScenarioObject.Template.Passability;
        m_Interaction = a_ScenarioObject.Template.Interactability;

        m_ScenarioObject = a_ScenarioObject;
        m_Shadow = a_Shadow;

        gameObject.name = a_ScenarioObject.Template.Name;

        transform.position = new Vector3(a_ScenarioObject.PosX + 0.5f, -a_ScenarioObject.PosY - 0.5f, 0);
        m_SpriteRenderer.sortingOrder = -32767 + a_ScenarioObject.SortOrder;

        if (a_ScenarioObject.Template.IsLowPrioritySortOrder)
        {
            m_SpriteRenderer.sortingLayerName = "MapLowPriorityObjects";
        }
        else
        {
            m_SpriteRenderer.sortingLayerName = "MapObjects";
        }

        switch (a_ScenarioObject.Template.Type)
        {
            case ScenarioObjectType.Town:
                TownMapObject _Town = gameObject.AddComponent<TownMapObject>();

                _Town.Initialize(this);
                break;

            case ScenarioObjectType.Dwelling:
                DwellingMapObject _Dwelling = gameObject.AddComponent<DwellingMapObject>();

                _Dwelling.Initialize(this);
                break;

            case ScenarioObjectType.Monster:
                MonsterMapObject _Monster = gameObject.AddComponent<MonsterMapObject>();

                _Monster.Initialize(this);
                break;
        }
    }
}