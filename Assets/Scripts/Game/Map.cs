using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    const int WATER_TILE_INDEX = 8;
    const int CLEAR_RIVER_INDEX = 1;
    const int LAVA_RIVER_INDEX = 4;

    public GameSettings GameSettings => m_GameSettings;

    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] TerrainTileObject m_TileObjectPrefab;
    [SerializeField] SpriteRenderer m_TerrainFrame;
    [SerializeField] Transform m_TerrainMask;
    [SerializeField] MapObject m_MapObjectPrefab;
    [SerializeField] MapHero m_MapHeroPrefab;
    [SerializeField] MapDwelling m_MapDwellingPrefab;
    [SerializeField] MapGarrison m_MapGarrisonPrefab;
    [SerializeField] MapTown m_MapTownPrefab;
    [SerializeField] MapResource m_MapResourcePrefab;
    [SerializeField] MapMonster m_MapMonsterPrefab;
    [SerializeField] Pathfinding m_Pathfinding;
    [SerializeField] LocalOwnership m_LocalOwnership;
    [SerializeField] SidebarButtons m_SidebarButtons;
    [SerializeField] GameReferences m_GameReferences;

    [Space]

    [SerializeField] Transform m_OverworldTerrainRoot;
    [SerializeField] Transform m_UndergroundTerrainRoot;

    [Space]

    [SerializeField] Transform m_TerrainTileObjectParent;
    [SerializeField] Transform m_RiverTileObjectParent;
    [SerializeField] Transform m_RoadTileObjectParent;
    [SerializeField] Transform m_MapObjectParent;

    [Space]

    [SerializeField] Transform m_UndergroundTerrainTileObjectParent;
    [SerializeField] Transform m_UndergroundRiverTileObjectParent;
    [SerializeField] Transform m_UndergroundRoadTileObjectParent;
    [SerializeField] Transform m_UndergroundMapObjectParent;


    [Space]

    [SerializeField] List<Sprite> m_DirtSprites;
    [SerializeField] List<Sprite> m_SandSprites;
    [SerializeField] List<Sprite> m_GrassSprites;
    [SerializeField] List<Sprite> m_SnowSprites;
    [SerializeField] List<Sprite> m_SwampSprites;
    [SerializeField] List<Sprite> m_RoughSprites;
    [SerializeField] List<Sprite> m_SubterraneanSprites;
    [SerializeField] List<Sprite> m_LavaSprites;
    [SerializeField] List<Sprite> m_RockSprites;

    [Space]

    [NonReorderable]
    [SerializeField] List<SpriteArrayContainer> m_WaterAnimations;

    [Space]

    [SerializeField] List<Sprite> m_IcyRiverSprites;
    [SerializeField] List<Sprite> m_MuddyRiverSprites;

    [Space]

    [NonReorderable]
    [SerializeField] List<SpriteArrayContainer> m_ClearRiverAnimations;
    [NonReorderable]
    [SerializeField] List<SpriteArrayContainer> m_LavaRiverAnimations;

    [Space]

    [SerializeField] List<Sprite> m_DirtRoadSprites;
    [SerializeField] List<Sprite> m_GravelRoadSprites;
    [SerializeField] List<Sprite> m_CobbleRoadSprites;

    List<TerrainTileObject> m_TerrainTileObjects;
    List<TerrainTileObject> m_RiverTileObjects;
    List<TerrainTileObject> m_RoadTileObjects;
    List<GameObject> m_OverworldObjects;

    List<TerrainTileObject> m_UndergroundTerrainTileObjects;
    List<TerrainTileObject> m_UndergroundRiverTileObjects;
    List<TerrainTileObject> m_UndergroundRoadTileObjects;
    List<GameObject> m_UndergroundObjects;

    List<List<Sprite>> m_TerrainSprites;
    List<List<Sprite>> m_RiverSprites;
    List<List<Sprite>> m_RoadSprites;

    void Start()
    {
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

                _TileObject.transform.parent = m_TerrainTileObjectParent;

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

                    _TileObject.transform.parent = m_RiverTileObjectParent;

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

                    _TileObject.transform.parent = m_RoadTileObjectParent;

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

                    _TileObject.transform.parent = m_UndergroundTerrainTileObjectParent;

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

                        _TileObject.transform.parent = m_UndergroundRiverTileObjectParent;

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

                        _TileObject.transform.parent = m_UndergroundRoadTileObjectParent;

                        m_UndergroundRoadTileObjects.Add(_TileObject);
                    }
                }
            }
        }

        // <><><><><> Objects

        m_OverworldObjects = new List<GameObject>();
        m_UndergroundObjects = new List<GameObject>();

        List<ScenarioObject> _Objects = m_GameSettings.Scenario.Objects;
        List<MapObjectBase> _MapObjects = new List<MapObjectBase>(_Objects.Count);
        Dictionary<ScenarioObject, DynamicMapObstacle> _DynamicObstacles = new Dictionary<ScenarioObject, DynamicMapObstacle>();

        // Load map objects
        for (int i = 0; i < _Objects.Count; i++)
        {
            ScenarioObject _Object = _Objects[i];
            MapObjectBase _MapObject;

            switch (_Object.Template.Type)
            {
                case ScenarioObjectType.Hero:
                    MapHero _Hero = Instantiate(m_MapHeroPrefab, m_MapObjectParent);
                    _Hero.Initialize(_Object, m_GameReferences);
                    _MapObject = _Hero;
                    _DynamicObstacles.Add(_Object, _Hero.DynamicObstacle);
                    break;

                case ScenarioObjectType.Resource:
                    MapResource _Resource = Instantiate(m_MapResourcePrefab, m_MapObjectParent);
                    _Resource.Initialize(_Object, m_GameReferences);
                    _MapObject = _Resource;
                    _DynamicObstacles.Add(_Object, _Resource.DynamicObstacle);
                    break;

                case ScenarioObjectType.Dwelling:
                    MapDwelling _Dwelling = Instantiate(m_MapDwellingPrefab, m_MapObjectParent);
                    _Dwelling.Initialize(_Object, m_GameReferences);
                    _MapObject = _Dwelling;
                    break;

                case ScenarioObjectType.Town:
                    MapTown _Town = Instantiate(m_MapTownPrefab, m_MapObjectParent);
                    _Town.Initialize(_Object, m_GameReferences);
                    _MapObject = _Town;
                    break;

                case ScenarioObjectType.Monster:
                    MapMonster _Monster = Instantiate(m_MapMonsterPrefab, m_MapObjectParent);
                    _Monster.Initialize(_Object, m_GameReferences);
                    _MapObject = _Monster;
                    _DynamicObstacles.Add(_Object, _Monster.DynamicObstacle);
                    break;

                case ScenarioObjectType.Garrison:
                    MapGarrison _Garrison = Instantiate(m_MapGarrisonPrefab, m_MapObjectParent);
                    _Garrison.Initialize(_Object, m_GameReferences);
                    _MapObject = _Garrison;
                    break;

                default:
                    MapObject _Obj = Instantiate(m_MapObjectPrefab, m_MapObjectParent);
                    _Obj.Initialize(_Object, m_GameReferences);
                    _MapObject = _Obj;
                    break;

            }

            _MapObject.transform.position = new Vector3(_Object.PosX + 0.5f, -_Object.PosY - 0.5f, 0);
            _MapObject.MouseCollision = new byte[6];
            _MapObject.MouseCollision[0] = (byte)(~_Object.Template.Passability[0] | _Object.Template.Interactability[0]);
            _MapObject.MouseCollision[1] = (byte)(~_Object.Template.Passability[1] | _Object.Template.Interactability[1]);
            _MapObject.MouseCollision[2] = (byte)(~_Object.Template.Passability[2] | _Object.Template.Interactability[2]);
            _MapObject.MouseCollision[3] = (byte)(~_Object.Template.Passability[3] | _Object.Template.Interactability[3]);
            _MapObject.MouseCollision[4] = (byte)(~_Object.Template.Passability[4] | _Object.Template.Interactability[4]);
            _MapObject.MouseCollision[5] = (byte)(~_Object.Template.Passability[5] | _Object.Template.Interactability[5]);

            _MapObject.InteractionCollision = _Object.Template.Interactability;


            if (_Object.IsUnderground)
            {
                m_UndergroundObjects.Add(_MapObject.gameObject);
                _MapObject.transform.parent = m_UndergroundMapObjectParent;
            }
            else
            {
                m_OverworldObjects.Add(_MapObject.gameObject);
            }

            _MapObjects.Add(_MapObject);
        }

        m_Pathfinding.Generate(m_GameSettings.Scenario, _MapObjects, _DynamicObstacles);

        // Spawn starting heroes
        for (int i = 0; i < m_GameSettings.Players.Count; i++)
        {
            PlayerInfo _PlayerInfo = m_GameSettings.Scenario.PlayerInfo[i];
            if (_PlayerInfo.GenerateHeroAtMainTown)
            {
                MapHero _MapHero = Instantiate(m_MapHeroPrefab, m_MapObjectParent);

                // Spawn map object at main town coordinates
                if (_PlayerInfo.IsMainTownUnderground)
                {
                    _MapHero.transform.parent = m_UndergroundMapObjectParent;
                }

                Hero _Hero;

                if (_PlayerInfo.IsMainHeroRandom)
                {
                    _Hero = HeroPool.GetRandomHero(m_GameSettings.Players[i].Index, m_GameSettings.Players[i].Faction, true);
                    HeroPool.ClaimHero(_Hero);
                }
                else
                {
                    _Hero = m_GameSettings.Players[i].Hero;
                }

                _MapHero.Initialize
                (
                    _Hero,
                    m_GameSettings.Players[i].Index,
                    m_GameSettings.Scenario.PlayerInfo[i].MainTownXCoord + 1,
                    m_GameSettings.Scenario.PlayerInfo[i].MainTownYCoord,
                    m_GameSettings.Scenario.PlayerInfo[i].IsMainTownUnderground,
                    m_GameReferences
                );
            }
        }

        List<MapHero> _LocalOwnership = m_LocalOwnership.GetHeroes();

        if (_LocalOwnership.Count > 0)
        {
            m_LocalOwnership.SelectHero(_LocalOwnership[0]);
        }
    }

    public void ShowUnderground(bool a_Show)
    {
        m_OverworldTerrainRoot.gameObject.SetActive(!a_Show);
        m_UndergroundTerrainRoot.gameObject.SetActive(a_Show);

        m_SidebarButtons.SetUndergroundButton(a_Show);
    }
}
