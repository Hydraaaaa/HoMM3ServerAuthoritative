﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    const int WATER_TILE_INDEX = 8;
    const int CLEAR_RIVER_INDEX = 1;
    const int LAVA_RIVER_INDEX = 4;

    public GameSettings GameSettings => m_GameSettings;

    [SerializeField] GameSettings m_GameSettings = null;
    [SerializeField] TerrainTileObject m_TileObjectPrefab = null;
    [SerializeField] SpriteRenderer m_TerrainFrame = null;
    [SerializeField] Transform m_TerrainMask = null;
    [SerializeField] MapObject m_MapObjectPrefab = null;
    [SerializeField] MapHero m_MapHeroPrefab = null;
    [SerializeField] MapDwelling m_MapDwellingPrefab = null;
    [SerializeField] MapTown m_MapTownPrefab = null;
    [SerializeField] MapResource m_MapResourcePrefab = null;
    [SerializeField] MapMonster m_MapMonsterPrefab = null;
    [SerializeField] Pathfinding m_Pathfinding = null;

    [Space]

    [SerializeField] Transform m_TerrainRoot = null;
    [SerializeField] Transform m_UndergroundTerrainRoot = null;

    [Space]

    [SerializeField] Transform m_TerrainTileObjectParent = null;
    [SerializeField] Transform m_RiverTileObjectParent = null;
    [SerializeField] Transform m_RoadTileObjectParent = null;
    [SerializeField] Transform m_MapObjectParent = null;

    [Space]

    [SerializeField] Transform m_UndergroundTerrainTileObjectParent = null;
    [SerializeField] Transform m_UndergroundRiverTileObjectParent = null;
    [SerializeField] Transform m_UndergroundRoadTileObjectParent = null;
    [SerializeField] Transform m_UndergroundMapObjectParent = null;


    [Space]

    [SerializeField] List<Sprite> m_DirtSprites = null;
    [SerializeField] List<Sprite> m_SandSprites = null;
    [SerializeField] List<Sprite> m_GrassSprites = null;
    [SerializeField] List<Sprite> m_SnowSprites = null;
    [SerializeField] List<Sprite> m_SwampSprites = null;
    [SerializeField] List<Sprite> m_RoughSprites = null;
    [SerializeField] List<Sprite> m_SubterraneanSprites = null;
    [SerializeField] List<Sprite> m_LavaSprites = null;
    [SerializeField] List<Sprite> m_RockSprites = null;

    [Space]

    [NonReorderable]
    [SerializeField] List<SpriteArrayContainer> m_WaterAnimations = null;

    [Space]

    [SerializeField] List<Sprite> m_IcyRiverSprites = null;
    [SerializeField] List<Sprite> m_MuddyRiverSprites = null;

    [Space]

    [NonReorderable]
    [SerializeField] List<SpriteArrayContainer> m_ClearRiverAnimations = null;
    [NonReorderable]
    [SerializeField] List<SpriteArrayContainer> m_LavaRiverAnimations = null;

    [Space]

    [SerializeField] List<Sprite> m_DirtRoadSprites = null;
    [SerializeField] List<Sprite> m_GravelRoadSprites = null;
    [SerializeField] List<Sprite> m_CobbleRoadSprites = null;

    List<TerrainTileObject> m_TerrainTileObjects;
    List<TerrainTileObject> m_RiverTileObjects;
    List<TerrainTileObject> m_RoadTileObjects;
    List<GameObject> m_Objects;

    List<TerrainTileObject> m_UndergroundTerrainTileObjects;
    List<TerrainTileObject> m_UndergroundRiverTileObjects;
    List<TerrainTileObject> m_UndergroundRoadTileObjects;
    List<GameObject> m_UndergroundObjects;

    List<List<Sprite>> m_TerrainSprites;
    List<List<Sprite>> m_RiverSprites;
    List<List<Sprite>> m_RoadSprites;

    void Awake()
    {
        Physics.autoSimulation = false;
        Physics2D.simulationMode = SimulationMode2D.Script;

        m_TerrainSprites = new List<List<Sprite>>();
        m_TerrainSprites.Add(m_DirtSprites);
        m_TerrainSprites.Add(m_SandSprites);
        m_TerrainSprites.Add(m_GrassSprites);
        m_TerrainSprites.Add(m_SnowSprites);
        m_TerrainSprites.Add(m_SwampSprites);
        m_TerrainSprites.Add(m_RoughSprites);
        m_TerrainSprites.Add(m_SubterraneanSprites);
        m_TerrainSprites.Add(m_LavaSprites);
        m_TerrainSprites.Add(new List<Sprite>());
        m_TerrainSprites.Add(m_RockSprites);

        m_RiverSprites = new List<List<Sprite>>();
        m_RiverSprites.Add(new List<Sprite>());
        m_RiverSprites.Add(m_IcyRiverSprites);
        m_RiverSprites.Add(m_MuddyRiverSprites);
        m_RiverSprites.Add(new List<Sprite>());

        m_RoadSprites = new List<List<Sprite>>();
        m_RoadSprites.Add(m_DirtRoadSprites);
        m_RoadSprites.Add(m_GravelRoadSprites);
        m_RoadSprites.Add(m_CobbleRoadSprites);

        if (m_GameSettings.Scenario.Version != Scenario.SHADOW_OF_DEATH)
        {
            Debug.Log($"This map isn't SoD, not supported yet");
            return;
        }

        int _Size = m_GameSettings.Scenario.Size;

        m_TerrainFrame.size = new Vector2(_Size + 2, _Size + 2);
        m_TerrainFrame.transform.localPosition = new Vector3(_Size / 2 - 0.5f, -_Size / 2 + 0.5f, 0);
        m_TerrainMask.localScale = new Vector3(_Size, _Size, 1);
        m_TerrainMask.transform.localPosition = new Vector3(_Size / 2 - 0.5f, -_Size / 2 + 0.5f, 0);

        // <><><><><> Above Ground Terrain

        List<TerrainTile> _Terrain = m_GameSettings.Scenario.Terrain;

        m_TerrainTileObjects = new List<TerrainTileObject>(_Terrain.Capacity);
        m_RiverTileObjects = new List<TerrainTileObject>();
        m_RoadTileObjects = new List<TerrainTileObject>();

        for (int x = 0; x < _Size; x++)
        {
            for (int y = 0; y < _Size; y++)
            {
                int _Index = x + y * _Size;

                // <><><><><> Terrain Tile

                TerrainTileObject _TileObject = Instantiate(m_TileObjectPrefab, new Vector2(x, -y), Quaternion.identity);

                _TileObject.Renderer.sortingOrder = -32768;

                if (_Terrain[_Index].TerrainType < m_TerrainSprites.Count)
                {
                    if (_Terrain[_Index].TerrainType == WATER_TILE_INDEX)
                    {
                        _TileObject.AnimationRenderer.SetSprites(m_WaterAnimations[_Terrain[_Index].TerrainSpriteID].Array);
                    }
                    else if (_Terrain[_Index].TerrainSpriteID < m_TerrainSprites[_Terrain[_Index].TerrainType].Count)
                    {
                        _TileObject.Renderer.sprite = m_TerrainSprites[_Terrain[_Index].TerrainType][_Terrain[_Index].TerrainSpriteID];
                    }
                    else
                    {
                        Debug.Log($"Failed ID {_Terrain[_Index].TerrainSpriteID}");
                    }
                }
                else
                {
                    Debug.Log($"Failed Type {_Terrain[_Index].TerrainType}");
                }

                _TileObject.Renderer.sortingLayerName = "Terrain";
                _TileObject.Renderer.flipX = (_Terrain[_Index].Mirrored & 1) == 1;
                _TileObject.Renderer.flipY = (_Terrain[_Index].Mirrored & 2) == 2;

                _TileObject.name = $"{_TileObject.Renderer.sprite.name}  Pos {_Index}  ID {_Terrain[_Index].TerrainSpriteID}";

                _TileObject.transform.SetParent(m_TerrainTileObjectParent);

                m_TerrainTileObjects.Add(_TileObject);

                // <><><><><> River Tile

                if (_Terrain[_Index].RiverType != 0)
                {
                    _TileObject = Instantiate(m_TileObjectPrefab, new Vector2(x, -y), Quaternion.identity);

                    if (_Terrain[_Index].RiverType - 1 < m_RiverSprites.Count)
                    {
                        if (_Terrain[_Index].RiverType == CLEAR_RIVER_INDEX)
                        {
                            _TileObject.AnimationRenderer.SetSprites(m_ClearRiverAnimations[_Terrain[_Index].RiverSpriteID].Array);
                        }
                        else if (_Terrain[_Index].RiverType == LAVA_RIVER_INDEX)
                        {
                            _TileObject.AnimationRenderer.SetSprites(m_LavaRiverAnimations[_Terrain[_Index].RiverSpriteID].Array);
                        }
                        else if (_Terrain[_Index].RiverSpriteID < m_RiverSprites[_Terrain[_Index].RiverType - 1].Count)
                        {
                            _TileObject.Renderer.sprite = m_RiverSprites[_Terrain[_Index].RiverType - 1][_Terrain[_Index].RiverSpriteID];
                        }
                        else
                        {
                            Debug.Log($"River Failed ID {_Terrain[_Index].RiverSpriteID}");
                        }
                    }
                    else
                    {
                        Debug.Log($"River Failed Type {_Terrain[_Index].RiverSpriteID}");
                    }

                    _TileObject.Renderer.sortingOrder = 1;
                    _TileObject.Renderer.sortingLayerName = "Terrain";
                    _TileObject.Renderer.flipX = (_Terrain[_Index].Mirrored & 4) == 4;
                    _TileObject.Renderer.flipY = (_Terrain[_Index].Mirrored & 8) == 8;

                    _TileObject.name = $"{_TileObject.Renderer.sprite.name}  Pos {_Index}  ID {_Terrain[_Index].RiverSpriteID}";

                    _TileObject.transform.SetParent(m_RiverTileObjectParent);

                    m_RiverTileObjects.Add(_TileObject);
                }

                // <><><><><> Road Tile

                if (_Terrain[_Index].RoadType != 0)
                {
                    _TileObject = Instantiate(m_TileObjectPrefab, new Vector2(x, -y - 0.5f), Quaternion.identity);

                    if (_Terrain[_Index].RoadType - 1 < m_RoadSprites.Count)
                    {
                        if (_Terrain[_Index].RoadSpriteID < m_RoadSprites[_Terrain[_Index].RoadType - 1].Count)
                        {
                            _TileObject.Renderer.sprite = m_RoadSprites[_Terrain[_Index].RoadType - 1][_Terrain[_Index].RoadSpriteID];
                        }
                        else
                        {
                            Debug.Log($"Road Failed ID {_Terrain[_Index].RoadSpriteID}");
                        }
                    }
                    else
                    {
                        Debug.Log($"Road Failed Type {_Terrain[_Index].RoadSpriteID}");
                    }

                    _TileObject.Renderer.sortingOrder = 2;
                    _TileObject.Renderer.sortingLayerName = "Terrain";
                    _TileObject.Renderer.flipX = (_Terrain[_Index].Mirrored & 16) == 16;
                    _TileObject.Renderer.flipY = (_Terrain[_Index].Mirrored & 32) == 32;

                    _TileObject.name = $"{_TileObject.Renderer.sprite.name}  Pos {_Index}  ID {_Terrain[_Index].RoadSpriteID}";

                    _TileObject.transform.SetParent(m_RoadTileObjectParent);

                    m_RoadTileObjects.Add(_TileObject);
                }
            }
        }

        if (m_GameSettings.Scenario.HasUnderground)
        {
            // <><><><><> Underground Terrain

            List<TerrainTile> _UndergroundTerrain = m_GameSettings.Scenario.UndergroundTerrain;

            m_UndergroundTerrainTileObjects = new List<TerrainTileObject>(_Terrain.Capacity);
            m_UndergroundRiverTileObjects = new List<TerrainTileObject>();
            m_UndergroundRoadTileObjects = new List<TerrainTileObject>();

            for (int x = 0; x < _Size; x++)
            {
                for (int y = 0; y < _Size; y++)
                {
                    int _Index = x + y * _Size;

                    // <><><><><> Terrain Tile

                    TerrainTileObject _TileObject = Instantiate(m_TileObjectPrefab, new Vector2(x, -y), Quaternion.identity);

                    _TileObject.Renderer.sortingOrder = -32768;

                    if (_UndergroundTerrain[_Index].TerrainType < m_TerrainSprites.Count)
                    {
                        if (_UndergroundTerrain[_Index].TerrainType == WATER_TILE_INDEX)
                        {
                            if (m_WaterAnimations[_UndergroundTerrain[_Index].TerrainSpriteID] != null)
                            {
                                _TileObject.AnimationRenderer.SetSprites(m_WaterAnimations[_UndergroundTerrain[_Index].TerrainSpriteID].Array);
                            }
                        }
                        else if (_UndergroundTerrain[_Index].TerrainSpriteID < m_TerrainSprites[_UndergroundTerrain[_Index].TerrainType].Count)
                        {
                            _TileObject.Renderer.sprite = m_TerrainSprites[_UndergroundTerrain[_Index].TerrainType][_UndergroundTerrain[_Index].TerrainSpriteID];
                        }
                        else
                        {
                            Debug.Log($"Failed ID {_UndergroundTerrain[_Index].TerrainSpriteID}");
                        }
                    }
                    else
                    {
                        Debug.Log($"Failed Type {_UndergroundTerrain[_Index].TerrainType}");
                    }

                    _TileObject.Renderer.sortingLayerName = "Terrain";
                    _TileObject.Renderer.flipX = (_UndergroundTerrain[_Index].Mirrored & 1) == 1;
                    _TileObject.Renderer.flipY = (_UndergroundTerrain[_Index].Mirrored & 2) == 2;

                    _TileObject.name = $"{_TileObject.Renderer.sprite.name}  Pos {_Index}  ID {_UndergroundTerrain[_Index].TerrainSpriteID}";

                    _TileObject.transform.SetParent(m_UndergroundTerrainTileObjectParent);

                    m_UndergroundTerrainTileObjects.Add(_TileObject);

                    // <><><><><> River Tile

                    if (_UndergroundTerrain[_Index].RiverType != 0)
                    {
                        _TileObject = Instantiate(m_TileObjectPrefab, new Vector2(x, -y), Quaternion.identity);

                        if (_UndergroundTerrain[_Index].RiverType == CLEAR_RIVER_INDEX)
                        {
                            _TileObject.AnimationRenderer.SetSprites(m_ClearRiverAnimations[_UndergroundTerrain[_Index].RiverSpriteID].Array);
                        }
                        else if (_UndergroundTerrain[_Index].RiverType == LAVA_RIVER_INDEX)
                        {
                            _TileObject.AnimationRenderer.SetSprites(m_LavaRiverAnimations[_UndergroundTerrain[_Index].RiverSpriteID].Array);
                        }
                        else if (_UndergroundTerrain[_Index].RiverSpriteID < m_RiverSprites[_UndergroundTerrain[_Index].RiverType - 1].Count)
                        {
                            _TileObject.Renderer.sprite = m_RiverSprites[_UndergroundTerrain[_Index].RiverType - 1][_UndergroundTerrain[_Index].RiverSpriteID];
                        }
                        else
                        {
                            Debug.Log($"River Failed ID {_UndergroundTerrain[_Index].RiverSpriteID}");
                        }

                        _TileObject.Renderer.sortingOrder = 1;
                        _TileObject.Renderer.sortingLayerName = "Terrain";
                        _TileObject.Renderer.flipX = (_UndergroundTerrain[_Index].Mirrored & 4) == 4;
                        _TileObject.Renderer.flipY = (_UndergroundTerrain[_Index].Mirrored & 8) == 8;

                        _TileObject.name = $"{_TileObject.Renderer.sprite.name}  Pos {_Index}  ID {_UndergroundTerrain[_Index].RiverSpriteID}";

                        _TileObject.transform.SetParent(m_UndergroundRiverTileObjectParent);

                        m_UndergroundRiverTileObjects.Add(_TileObject);
                    }

                    // <><><><><> Road Tile

                    if (_UndergroundTerrain[_Index].RoadType != 0)
                    {
                        _TileObject = Instantiate(m_TileObjectPrefab, new Vector2(x, -y - 0.5f), Quaternion.identity);

                        if (_UndergroundTerrain[_Index].RoadType - 1 < m_RoadSprites.Count)
                        {
                            if (_UndergroundTerrain[_Index].RoadSpriteID < m_RoadSprites[_UndergroundTerrain[_Index].RoadType - 1].Count)
                            {
                                _TileObject.Renderer.sprite = m_RoadSprites[_UndergroundTerrain[_Index].RoadType - 1][_UndergroundTerrain[_Index].RoadSpriteID];
                            }
                            else
                            {
                                Debug.Log($"Road Failed ID {_UndergroundTerrain[_Index].RoadSpriteID}");
                            }
                        }
                        else
                        {
                            Debug.Log($"Road Failed Type {_UndergroundTerrain[_Index].RoadSpriteID}");
                        }

                        _TileObject.Renderer.sortingOrder = 2;
                        _TileObject.Renderer.sortingLayerName = "Terrain";
                        _TileObject.Renderer.flipX = (_UndergroundTerrain[_Index].Mirrored & 16) == 16;
                        _TileObject.Renderer.flipY = (_UndergroundTerrain[_Index].Mirrored & 32) == 32;

                        _TileObject.name = $"{_TileObject.Renderer.sprite.name}  Pos {_Index}  ID {_UndergroundTerrain[_Index].RoadSpriteID}";

                        _TileObject.transform.SetParent(m_UndergroundRoadTileObjectParent);

                        m_UndergroundRoadTileObjects.Add(_TileObject);
                    }
                }
            }
        }

        // <><><><><> Objects

        m_Objects = new List<GameObject>();
        m_UndergroundObjects = new List<GameObject>();

        List<ScenarioObject> _Objects = m_GameSettings.Scenario.Objects;

        // Load map objects
        for (int i = 0; i < _Objects.Count; i++)
        {
            ScenarioObject _Object = _Objects[i];

            GameObject _MapObject;

            switch (_Object.Template.Type)
            {
                case ScenarioObjectType.Hero:
                    MapHero _Hero = Instantiate(m_MapHeroPrefab, m_MapObjectParent);
                    _Hero.Initialize(_Object);
                    _MapObject = _Hero.gameObject;
                    break;

                case ScenarioObjectType.Resource:
                    MapResource _Resource = Instantiate(m_MapResourcePrefab, m_MapObjectParent);
                    _Resource.Initialize(_Object);
                    _MapObject = _Resource.gameObject;
                    break;

                case ScenarioObjectType.Dwelling:
                    MapDwelling _Dwelling = Instantiate(m_MapDwellingPrefab, m_MapObjectParent);
                    _Dwelling.Initialize(_Object);
                    _MapObject = _Dwelling.gameObject;
                    break;

                case ScenarioObjectType.Town:
                    MapTown _Town = Instantiate(m_MapTownPrefab, m_MapObjectParent);
                    _Town.Initialize(_Object);
                    _MapObject = _Town.gameObject;
                    break;

                case ScenarioObjectType.Monster:
                    MapMonster _Monster = Instantiate(m_MapMonsterPrefab, m_MapObjectParent);
                    _Monster.Initialize(_Object);
                    _MapObject = _Monster.gameObject;
                    break;

                default:
                    MapObject _Obj = Instantiate(m_MapObjectPrefab, m_MapObjectParent);
                    _Obj.Initialize(_Object);
                    _MapObject = _Obj.gameObject;
                    break;

            }

            _MapObject.transform.position = new Vector3(_Object.PosX + 0.5f, -_Object.PosY - 0.5f, 0);

            if (_Object.IsUnderground)
            {
                m_UndergroundObjects.Add(_MapObject);
                _MapObject.transform.parent = m_UndergroundMapObjectParent;
            }
            else
            {
                m_Objects.Add(_MapObject);
            }
        }

        // Spawn starting heroes
        for (int i = 0; i < m_GameSettings.Players.Count; i++)
        {
            if (m_GameSettings.Scenario.PlayerInfo[i].GenerateHeroAtMainTown)
            {
                // Spawn map object at main town coordinates
                MapHero _Hero = Instantiate(m_MapHeroPrefab, m_MapObjectParent);

                _Hero.Initialize(m_GameSettings.Players[i].Hero, m_GameSettings.Scenario.PlayerInfo[i].MainTownXCoord, m_GameSettings.Scenario.PlayerInfo[i].MainTownYCoord);
            }
        }

        m_Pathfinding.Generate(m_GameSettings.Scenario);
    }
}
