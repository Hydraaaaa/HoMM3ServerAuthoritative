using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.ResourceManagement.ResourceLocations;

public class Map : MonoBehaviour
{
    const int WATER_TILE_INDEX = 8;
    const int CLEAR_RIVER_INDEX = 1;
    const int LAVA_RIVER_INDEX = 4;

    [SerializeField] GameSettings m_GameSettings = null;
    [SerializeField] TerrainTileObject m_TileObjectPrefab = null;
    [SerializeField] SpriteRenderer m_TerrainFrame = null;
    [SerializeField] Transform m_TerrainMask = null;
    [SerializeField] MapObject m_MapObjectPrefab = null;
    [SerializeField] MapShadowObject m_ShadowObjectPrefab = null;
    [SerializeField] Pathfinding m_Pathfinding = null;

    [Space]

    [SerializeField] Transform m_TerrainRoot = null;
    [SerializeField] Transform m_UndergroundTerrainRoot = null;

    [Space]

    [SerializeField] Transform m_TerrainTileObjectParent = null;
    [SerializeField] Transform m_RiverTileObjectParent = null;
    [SerializeField] Transform m_RoadTileObjectParent = null;
    [SerializeField] Transform m_MapObjectParent = null;
    [SerializeField] Transform m_ShadowObjectParent = null;
    
    [Space]

    [SerializeField] Transform m_UndergroundTerrainTileObjectParent = null;
    [SerializeField] Transform m_UndergroundRiverTileObjectParent = null;
    [SerializeField] Transform m_UndergroundRoadTileObjectParent = null;
    [SerializeField] Transform m_UndergroundMapObjectParent = null;
    [SerializeField] Transform m_UndergroundShadowObjectParent = null;


    [Space]

    [SerializeField] List<Sprite> m_DirtSprites = null;
    [SerializeField] List<Sprite> m_SandSprites = null;
    [SerializeField] List<Sprite> m_GrassSprites = null;
    [SerializeField] List<Sprite> m_SnowSprites = null;
    [SerializeField] List<Sprite> m_SwampSprites = null;
    [SerializeField] List<Sprite> m_RoughSprites = null;
    [SerializeField] List<Sprite> m_SubterraneanSprites = null;
    [SerializeField] List<Sprite> m_LavaSprites = null;
    [SerializeField] List<Sprite> m_WaterSprites = null;
    [SerializeField] List<Sprite> m_RockSprites = null;

    [Space]

    [SerializeField] List<AnimationClip> m_WaterAnimations = null;

    [Space]

    [SerializeField] List<Sprite> m_ClearRiverSprites = null;
    [SerializeField] List<Sprite> m_IcyRiverSprites = null;
    [SerializeField] List<Sprite> m_MuddyRiverSprites = null;
    [SerializeField] List<Sprite> m_LavaRiverSprites = null;
    
    [Space]

    [SerializeField] List<AnimationClip> m_ClearRiverAnimations = null;
    [SerializeField] List<AnimationClip> m_LavaRiverAnimations = null;

    [Space]

    [SerializeField] List<Sprite> m_DirtRoadSprites = null;
    [SerializeField] List<Sprite> m_GravelRoadSprites = null;
    [SerializeField] List<Sprite> m_CobbleRoadSprites = null;

    List<TerrainTileObject> m_TerrainTileObjects;
    List<TerrainTileObject> m_RiverTileObjects;
    List<TerrainTileObject> m_RoadTileObjects;
    List<MapObject> m_Objects;

    List<TerrainTileObject> m_UndergroundTerrainTileObjects;
    List<TerrainTileObject> m_UndergroundRiverTileObjects;
    List<TerrainTileObject> m_UndergroundRoadTileObjects;
    List<MapObject> m_UndergroundObjects;

    List<List<Sprite>> m_TerrainSprites;
    List<List<Sprite>> m_RiverSprites;
    List<List<Sprite>> m_RoadSprites;

    void Awake()
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
        m_TerrainSprites.Add(m_WaterSprites);
        m_TerrainSprites.Add(m_RockSprites);

        m_RiverSprites = new List<List<Sprite>>();
        m_RiverSprites.Add(m_ClearRiverSprites);
        m_RiverSprites.Add(m_IcyRiverSprites);
        m_RiverSprites.Add(m_MuddyRiverSprites);
        m_RiverSprites.Add(m_LavaRiverSprites);

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
                        if (m_WaterAnimations[_Terrain[_Index].TerrainSpriteID] != null)
                        {
                            _TileObject.Animation.AddClip(m_WaterAnimations[_Terrain[_Index].TerrainSpriteID], "Default");
                            _TileObject.Animation.clip = m_WaterAnimations[_Terrain[_Index].TerrainSpriteID];
                            _TileObject.Animation.Play();
                        }
                        else
                        {
                            _TileObject.Renderer.sprite = m_TerrainSprites[_Terrain[_Index].TerrainType][_Terrain[_Index].TerrainSpriteID];
                        }
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
                            if (m_ClearRiverAnimations[_Terrain[_Index].RiverSpriteID] != null)
                            {
                                _TileObject.Animation.AddClip(m_ClearRiverAnimations[_Terrain[_Index].RiverSpriteID], "Default");
                                _TileObject.Animation.clip = m_ClearRiverAnimations[_Terrain[_Index].RiverSpriteID];
                                _TileObject.Animation.Play();
                            }
                            else
                            {
                                _TileObject.Renderer.sprite = m_RiverSprites[_Terrain[_Index].RiverType - 1][_Terrain[_Index].RiverSpriteID];
                            }
                        }
                        else if (_Terrain[_Index].RiverType == LAVA_RIVER_INDEX)
                        {
                            if (m_LavaRiverAnimations[_Terrain[_Index].RiverSpriteID] != null)
                            {
                                _TileObject.Animation.AddClip(m_LavaRiverAnimations[_Terrain[_Index].RiverSpriteID], "Default");
                                _TileObject.Animation.clip = m_LavaRiverAnimations[_Terrain[_Index].RiverSpriteID];
                                _TileObject.Animation.Play();
                            }
                            else
                            {
                                _TileObject.Renderer.sprite = m_RiverSprites[_Terrain[_Index].RiverType - 1][_Terrain[_Index].RiverSpriteID];
                            }
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
                                _TileObject.Animation.AddClip(m_WaterAnimations[_UndergroundTerrain[_Index].TerrainSpriteID], "Default");
                                _TileObject.Animation.clip = m_WaterAnimations[_UndergroundTerrain[_Index].TerrainSpriteID];
                                _TileObject.Animation.Play();
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
                            if (m_ClearRiverAnimations[_UndergroundTerrain[_Index].RiverSpriteID] != null)
                            {
                                _TileObject.Animation.AddClip(m_ClearRiverAnimations[_UndergroundTerrain[_Index].RiverSpriteID], "Default");
                                _TileObject.Animation.clip = m_ClearRiverAnimations[_UndergroundTerrain[_Index].RiverSpriteID];
                                _TileObject.Animation.Play();
                            }
                            else
                            {
                                _TileObject.Renderer.sprite = m_RiverSprites[_UndergroundTerrain[_Index].RiverType - 1][_UndergroundTerrain[_Index].RiverSpriteID];
                            }
                        }
                        else if (_UndergroundTerrain[_Index].RiverType == LAVA_RIVER_INDEX)
                        {
                            if (m_LavaRiverAnimations[_UndergroundTerrain[_Index].RiverSpriteID] != null)
                            {
                                _TileObject.Animation.AddClip(m_LavaRiverAnimations[_UndergroundTerrain[_Index].RiverSpriteID], "Default");
                                _TileObject.Animation.clip = m_LavaRiverAnimations[_UndergroundTerrain[_Index].RiverSpriteID];
                                _TileObject.Animation.Play();
                            }
                            else
                            {
                                _TileObject.Renderer.sprite = m_RiverSprites[_UndergroundTerrain[_Index].RiverType - 1][_UndergroundTerrain[_Index].RiverSpriteID];
                            }
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

        m_Objects = new List<MapObject>();
        m_UndergroundObjects = new List<MapObject>();

        List<ScenarioObject> _Objects = m_GameSettings.Scenario.Objects;

        for (int i = 0; i < _Objects.Count; i++)
        {
            StartCoroutine(LoadAsset(_Objects[i]));
        }

        m_Pathfinding.Generate(m_GameSettings.Scenario);
    }

    IEnumerator LoadAsset(ScenarioObject a_Object)
    {
        MapObject _MapObject = Instantiate(m_MapObjectPrefab, m_MapObjectParent);

        MapShadowObject _ShadowObject = Instantiate(m_ShadowObjectPrefab, m_ShadowObjectParent);

        _MapObject.Initialize(a_Object, _ShadowObject);
        _ShadowObject.Initialize(_MapObject);

        if (a_Object.IsUnderground)
        {
            m_UndergroundObjects.Add(_MapObject);
            _MapObject.transform.parent = m_UndergroundMapObjectParent;
            _ShadowObject.transform.parent = m_UndergroundShadowObjectParent;
        }
        else
        {
            m_Objects.Add(_MapObject);
        }

        var _Operation = Addressables.LoadAssetAsync<ScenarioObjectVisualData>($"MapObjects/{a_Object.Template.Name}.asset");

        yield return _Operation;

        if (_Operation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
        {
            Debug.Log(a_Object.Template.Name);
            yield break;
        }

        if (_Operation.Result.Animation != null)
        {
            // This asset is animated, load animations
            _MapObject.Animation.AddClip(_Operation.Result.Animation, "Default");
            _MapObject.Animation.clip = _Operation.Result.Animation;
            _MapObject.Animation.Play();
        }
        else
        {
            _MapObject.SpriteRenderer.sprite = _Operation.Result.Sprite;
        }

        if (_Operation.Result.ShadowAnimation != null)
        {
            _ShadowObject.Animation.AddClip(_Operation.Result.ShadowAnimation, "Default");
            _ShadowObject.Animation.clip = _Operation.Result.ShadowAnimation;
            _ShadowObject.Animation.Play();
        }
        else
        {
            _ShadowObject.SpriteRenderer.sprite = _Operation.Result.ShadowSprite;
        }
    }
}
