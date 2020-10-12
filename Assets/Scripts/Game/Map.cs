using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.ResourceManagement.ResourceLocations;

public class Map : MonoBehaviour
{
    [SerializeField] GameSettings m_GameSettings = null;
    [SerializeField] SpriteRenderer m_TerrainSpritePrefab = null;
    [SerializeField] SpriteRenderer m_TerrainFrame = null;
    [SerializeField] Transform m_TerrainMask = null;
    [SerializeField] MapObject m_MapObjectPrefab = null;
    [SerializeField] MapShadowObject m_ShadowObjectPrefab = null;

    [Space]

    [SerializeField] Transform m_TerrainRoot = null;
    [SerializeField] Transform m_UndergroundTerrainRoot = null;

    [Space]

    [SerializeField] Transform m_TerrainSpriteParent = null;
    [SerializeField] Transform m_RiverSpriteParent = null;
    [SerializeField] Transform m_RoadSpriteParent = null;
    [SerializeField] Transform m_ObjectSpriteParent = null;
    [SerializeField] Transform m_ShadowSpriteParent = null;
    
    [Space]

    [SerializeField] Transform m_UndergroundTerrainSpriteParent = null;
    [SerializeField] Transform m_UndergroundRiverSpriteParent = null;
    [SerializeField] Transform m_UndergroundRoadSpriteParent = null;
    [SerializeField] Transform m_UndergroundObjectSpriteParent = null;
    [SerializeField] Transform m_UndergroundShadowSpriteParent = null;


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

    [SerializeField] List<Sprite> m_ClearRiverSprites = null;
    [SerializeField] List<Sprite> m_IcyRiverSprites = null;
    [SerializeField] List<Sprite> m_MuddyRiverSprites = null;
    [SerializeField] List<Sprite> m_LavaRiverSprites = null;

    [Space]

    [SerializeField] List<Sprite> m_DirtRoadSprites = null;
    [SerializeField] List<Sprite> m_GravelRoadSprites = null;
    [SerializeField] List<Sprite> m_CobbleRoadSprites = null;

    List<SpriteRenderer> m_TerrainSpriteRenderers;
    List<SpriteRenderer> m_RiverSpriteRenderers;
    List<SpriteRenderer> m_RoadSpriteRenderers;
    List<SpriteRenderer> m_ObjectSpriteRenderers;

    List<SpriteRenderer> m_UndergroundTerrainSpriteRenderers;
    List<SpriteRenderer> m_UndergroundRiverSpriteRenderers;
    List<SpriteRenderer> m_UndergroundRoadSpriteRenderers;
    List<SpriteRenderer> m_UndergroundObjectSpriteRenderers;

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

        // <><><><><> Above ground terrain

        List<TerrainTile> _Terrain = m_GameSettings.Scenario.Terrain;

        m_TerrainSpriteRenderers = new List<SpriteRenderer>(_Terrain.Capacity);
        m_RiverSpriteRenderers = new List<SpriteRenderer>();
        m_RoadSpriteRenderers = new List<SpriteRenderer>();

        for (int x = 0; x < _Size; x++)
        {
            for (int y = 0; y < _Size; y++)
            {
                int _Index = x + y * _Size;

                // <><><><><> Terrain Tile

                SpriteRenderer _Sprite = Instantiate(m_TerrainSpritePrefab, new Vector2(x, -y), Quaternion.identity, m_TerrainSpriteParent);

                _Sprite.sortingOrder = -32768;

                if (_Terrain[_Index].TerrainType < m_TerrainSprites.Count)
                {
                    if (_Terrain[_Index].TerrainSpriteID < m_TerrainSprites[_Terrain[_Index].TerrainType].Count)
                    {
                        _Sprite.sprite = m_TerrainSprites[_Terrain[_Index].TerrainType][_Terrain[_Index].TerrainSpriteID];
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

                if ((int)(_Terrain[_Index].Mirrored & 3) == 3)
                {
                    _Sprite.transform.localScale = new Vector3(-1, -1, 1);
                }
                else if ((int)(_Terrain[_Index].Mirrored & 1) == 1)
                {
                    _Sprite.transform.localScale = new Vector3(-1, 1, 1);
                }
                else if ((int)(_Terrain[_Index].Mirrored & 2) == 2)
                {
                    _Sprite.transform.localScale = new Vector3(1, -1, 1);
                }

                _Sprite.name = $"{_Sprite.sprite.name}  Pos {_Index}  ID {_Terrain[_Index].TerrainSpriteID}";
                _Sprite.sortingLayerName = "Terrain";

                m_TerrainSpriteRenderers.Add(_Sprite);

                // <><><><><> River Tile

                if (_Terrain[_Index].RiverType != 0)
                {
                    _Sprite = Instantiate(m_TerrainSpritePrefab, new Vector2(x, -y), Quaternion.identity, m_RiverSpriteParent);

                    if (_Terrain[_Index].RiverType - 1 < m_RiverSprites.Count)
                    {
                        if (_Terrain[_Index].RiverSpriteID < m_RiverSprites[_Terrain[_Index].RiverType - 1].Count)
                        {
                            _Sprite.sprite = m_RiverSprites[_Terrain[_Index].RiverType - 1][_Terrain[_Index].RiverSpriteID];
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

                    if ((int)(_Terrain[_Index].Mirrored & 12) == 12)
                    {
                        _Sprite.transform.localScale = new Vector3(-1, -1, 1);
                    }
                    else if ((int)(_Terrain[_Index].Mirrored & 4) == 4)
                    {
                        _Sprite.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else if ((int)(_Terrain[_Index].Mirrored & 8) == 8)
                    {
                        _Sprite.transform.localScale = new Vector3(1, -1, 1);
                    }


                    _Sprite.name = $"{_Sprite.sprite.name}  Pos {_Index}  ID {_Terrain[_Index].RiverSpriteID}";
                    _Sprite.sortingOrder = 1;
                    _Sprite.sortingLayerName = "Terrain";

                    m_RiverSpriteRenderers.Add(_Sprite);
                }

                // <><><><><> Road Tile

                if (_Terrain[_Index].RoadType != 0)
                {
                    _Sprite = Instantiate(m_TerrainSpritePrefab, new Vector2(x, -y - 0.5f), Quaternion.identity, m_RoadSpriteParent);

                    if (_Terrain[_Index].RoadType - 1 < m_RoadSprites.Count)
                    {
                        if (_Terrain[_Index].RoadSpriteID < m_RoadSprites[_Terrain[_Index].RoadType - 1].Count)
                        {
                            _Sprite.sprite = m_RoadSprites[_Terrain[_Index].RoadType - 1][_Terrain[_Index].RoadSpriteID];
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

                    if ((int)(_Terrain[_Index].Mirrored & 48) == 48)
                    {
                        _Sprite.transform.localScale = new Vector3(-1, -1, 1);
                    }
                    else if ((int)(_Terrain[_Index].Mirrored & 16) == 16)
                    {
                        _Sprite.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else if ((int)(_Terrain[_Index].Mirrored & 32) == 32)
                    {
                        _Sprite.transform.localScale = new Vector3(1, -1, 1);
                    }


                    _Sprite.name = $"{_Sprite.sprite.name}  Pos {_Index}  ID {_Terrain[_Index].RoadSpriteID}";
                    _Sprite.sortingOrder = 2;
                    _Sprite.sortingLayerName = "Terrain";

                    m_RoadSpriteRenderers.Add(_Sprite);
                }
            }
        }

        if (m_GameSettings.Scenario.HasUnderground)
        {
            // <><><><><> Underground Terrain

            List<TerrainTile> _UndergroundTerrain = m_GameSettings.Scenario.UndergroundTerrain;

            m_UndergroundTerrainSpriteRenderers = new List<SpriteRenderer>(_UndergroundTerrain.Capacity);
            m_UndergroundRiverSpriteRenderers = new List<SpriteRenderer>();
            m_UndergroundRoadSpriteRenderers = new List<SpriteRenderer>();

            for (int x = 0; x < _Size; x++)
            {
                for (int y = 0; y < _Size; y++)
                {
                    int _Index = x + y * _Size;

                    // <><><><><> Terrain Tile

                    SpriteRenderer _Sprite = Instantiate(m_TerrainSpritePrefab, new Vector2(x, -y), Quaternion.identity, m_UndergroundTerrainSpriteParent);

                    if (_UndergroundTerrain[_Index].TerrainType < m_TerrainSprites.Count)
                    {
                        if (_UndergroundTerrain[_Index].TerrainSpriteID < m_TerrainSprites[_UndergroundTerrain[_Index].TerrainType].Count)
                        {
                            _Sprite.sprite = m_TerrainSprites[_UndergroundTerrain[_Index].TerrainType][_UndergroundTerrain[_Index].TerrainSpriteID];
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

                    if ((int)(_UndergroundTerrain[_Index].Mirrored & 3) == 3)
                    {
                        _Sprite.transform.localScale = new Vector3(-1, -1, 1);
                    }
                    else if ((int)(_UndergroundTerrain[_Index].Mirrored & 1) == 1)
                    {
                        _Sprite.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else if ((int)(_UndergroundTerrain[_Index].Mirrored & 2) == 2)
                    {
                        _Sprite.transform.localScale = new Vector3(1, -1, 1);
                    }

                    _Sprite.name = $"{_Sprite.sprite.name}  Pos {_Index}  ID {_UndergroundTerrain[_Index].TerrainSpriteID}";
                    _Sprite.sortingLayerName = "Terrain";

                    m_UndergroundTerrainSpriteRenderers.Add(_Sprite);

                    // <><><><><> River Tile

                    if (_UndergroundTerrain[_Index].RiverType != 0)
                    {
                        _Sprite = Instantiate(m_TerrainSpritePrefab, new Vector2(x, -y), Quaternion.identity, m_UndergroundRiverSpriteParent);

                        if (_UndergroundTerrain[_Index].RiverType - 1 < m_RiverSprites.Count)
                        {
                            if (_UndergroundTerrain[_Index].RiverSpriteID < m_RiverSprites[_UndergroundTerrain[_Index].RiverType - 1].Count)
                            {
                                _Sprite.sprite = m_RiverSprites[_UndergroundTerrain[_Index].RiverType - 1][_UndergroundTerrain[_Index].RiverSpriteID];
                            }
                            else
                            {
                                Debug.Log($"River Failed ID {_UndergroundTerrain[_Index].RiverSpriteID}");
                            }
                        }
                        else
                        {
                            Debug.Log($"River Failed Type {_UndergroundTerrain[_Index].RiverSpriteID}");
                        }

                        if ((int)(_UndergroundTerrain[_Index].Mirrored & 12) == 12)
                        {
                            _Sprite.transform.localScale = new Vector3(-1, -1, 1);
                        }
                        else if ((int)(_UndergroundTerrain[_Index].Mirrored & 4) == 4)
                        {
                            _Sprite.transform.localScale = new Vector3(-1, 1, 1);
                        }
                        else if ((int)(_UndergroundTerrain[_Index].Mirrored & 8) == 8)
                        {
                            _Sprite.transform.localScale = new Vector3(1, -1, 1);
                        }


                        _Sprite.name = $"{_Sprite.sprite.name}  Pos {_Index}  ID {_UndergroundTerrain[_Index].RiverSpriteID}";
                        _Sprite.sortingOrder = 1;
                        _Sprite.sortingLayerName = "Terrain";

                        m_UndergroundRiverSpriteRenderers.Add(_Sprite);
                    }

                    // <><><><><> Road Tile

                    if (_UndergroundTerrain[_Index].RoadType != 0)
                    {
                        _Sprite = Instantiate(m_TerrainSpritePrefab, new Vector2(x, -y - 0.5f), Quaternion.identity, m_UndergroundRoadSpriteParent);

                        if (_UndergroundTerrain[_Index].RoadType - 1 < m_RoadSprites.Count)
                        {
                            if (_UndergroundTerrain[_Index].RoadSpriteID < m_RoadSprites[_UndergroundTerrain[_Index].RoadType - 1].Count)
                            {
                                _Sprite.sprite = m_RoadSprites[_UndergroundTerrain[_Index].RoadType - 1][_UndergroundTerrain[_Index].RoadSpriteID];
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

                        if ((int)(_UndergroundTerrain[_Index].Mirrored & 48) == 48)
                        {
                            _Sprite.transform.localScale = new Vector3(-1, -1, 1);
                        }
                        else if ((int)(_UndergroundTerrain[_Index].Mirrored & 16) == 16)
                        {
                            _Sprite.transform.localScale = new Vector3(-1, 1, 1);
                        }
                        else if ((int)(_UndergroundTerrain[_Index].Mirrored & 32) == 32)
                        {
                            _Sprite.transform.localScale = new Vector3(1, -1, 1);
                        }


                        _Sprite.name = $"{_Sprite.sprite.name}  Pos {_Index}  ID {_UndergroundTerrain[_Index].RoadSpriteID}";
                        _Sprite.sortingOrder = 2;
                        _Sprite.sortingLayerName = "Terrain";

                        m_UndergroundRoadSpriteRenderers.Add(_Sprite);
                    }
                }
            }
        }

        // <><><><><> Objects

        m_ObjectSpriteRenderers = new List<SpriteRenderer>();
        m_UndergroundObjectSpriteRenderers = new List<SpriteRenderer>();

        List<ScenarioObject> _Objects = m_GameSettings.Scenario.Objects;

        for (int i = 0; i < _Objects.Count; i++)
        {
            StartCoroutine(LoadAsset(_Objects[i]));
        }
    }

    IEnumerator LoadAsset(ScenarioObject a_Object)
    {
        MapObject _MapObject = Instantiate(m_MapObjectPrefab, m_ObjectSpriteParent);

        MapShadowObject _ShadowObject = Instantiate(m_ShadowObjectPrefab, m_ShadowSpriteParent);

        _MapObject.Initialize(a_Object, _ShadowObject);
        _ShadowObject.Initialize(_MapObject);

        // Is this asset animated?
        string _Name = $"MapObjectAnimations/{a_Object.Template.Name}.anim";

        var _AnimOperation = Addressables.LoadAssetAsync<AnimationClip>(_Name);

        yield return _AnimOperation;

        if (_AnimOperation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            // This asset is animated, load animations
            _MapObject.SpriteRenderer.GetComponent<SimpleAnimation>().AddClip(_AnimOperation.Result, "Default");
            _MapObject.SpriteRenderer.GetComponent<SimpleAnimation>().clip = _AnimOperation.Result;
            _MapObject.SpriteRenderer.GetComponent<SimpleAnimation>().Play();

            if (a_Object.IsUnderground)
            {
                m_UndergroundObjectSpriteRenderers.Add(_MapObject.SpriteRenderer);
            }
            else
            {
                m_ObjectSpriteRenderers.Add(_MapObject.SpriteRenderer);
            }

            _Name = $"MapObjectShadowAnimations/{a_Object.Template.Name}.anim";

            _AnimOperation = Addressables.LoadAssetAsync<AnimationClip>(_Name);

            yield return _AnimOperation;

            _ShadowObject.SpriteRenderer.GetComponent<SimpleAnimation>().AddClip(_AnimOperation.Result, "Default");
            _ShadowObject.SpriteRenderer.GetComponent<SimpleAnimation>().clip = _AnimOperation.Result;
            _ShadowObject.SpriteRenderer.GetComponent<SimpleAnimation>().Play();

            if (a_Object.IsUnderground)
            {
                m_UndergroundObjectSpriteRenderers.Add(_ShadowObject.SpriteRenderer);
            }
            else
            {
                m_ObjectSpriteRenderers.Add(_ShadowObject.SpriteRenderer);
            }
        }
        else
        {
            // This asset isn't animated
            _Name = $"MapObjects/{a_Object.Template.Name}.bmp";

            var _SpriteOperation = Addressables.LoadAssetAsync<Sprite>(_Name);

            yield return _SpriteOperation;

            if (_SpriteOperation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
            {
                Debug.Log($"!! Missing {a_Object.Template.Name} at ({a_Object.XPos}, {a_Object.YPos})");
            }

            _MapObject.SpriteRenderer.sprite = _SpriteOperation.Result;

            if (a_Object.IsUnderground)
            {
                m_UndergroundObjectSpriteRenderers.Add(_MapObject.SpriteRenderer);
            }
            else
            {
                m_ObjectSpriteRenderers.Add(_MapObject.SpriteRenderer);
            }

            _Name = $"MapObjectShadows/{a_Object.Template.Name}.bmp";

            _SpriteOperation = Addressables.LoadAssetAsync<Sprite>(_Name);

            yield return _SpriteOperation;

            _ShadowObject.SpriteRenderer.sprite = _SpriteOperation.Result;

            if (a_Object.IsUnderground)
            {
                m_UndergroundObjectSpriteRenderers.Add(_ShadowObject.SpriteRenderer);
            }
            else
            {
                m_ObjectSpriteRenderers.Add(_ShadowObject.SpriteRenderer);
            }
        }
    }
}
