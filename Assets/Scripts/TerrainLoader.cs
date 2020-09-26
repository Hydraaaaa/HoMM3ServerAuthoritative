using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoader : MonoBehaviour
{
    [SerializeField] GameSettings m_GameSettings = null;
    [SerializeField] SpriteRenderer m_TerrainSpritePrefab = null;
    [SerializeField] SpriteRenderer m_TerrainFrame = null;

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

    List<SpriteRenderer> m_TerrainSpriteRenderers;
    List<SpriteRenderer> m_TerrainObjectSpriteRenderers;

    List<List<Sprite>> m_TerrainSprites;

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

        if (m_GameSettings.Map.Version != Map.SHADOW_OF_DEATH)
        {
            Debug.Log($"!! This map isn't SoD, not supported yet");
            return;
        }

        List<TerrainTile> _Terrain = m_GameSettings.Map.Terrain;
        int _Size = m_GameSettings.Map.Size;

        m_TerrainFrame.size = new Vector2(_Size + 2, _Size + 2);
        m_TerrainFrame.transform.localPosition = new Vector3(_Size / 2 - 0.5f, -_Size / 2 + 0.5f, 0);

        m_TerrainSpriteRenderers = new List<SpriteRenderer>(_Terrain.Capacity);

        for (int x = 0; x < _Size; x++)
        {
            for (int y = 0; y < _Size; y++)
            {
                int _Index = x + y * _Size;

                SpriteRenderer _Sprite = Instantiate (m_TerrainSpritePrefab, new Vector2(x, -y), Quaternion.identity, transform);

                if (_Terrain[_Index].TerrainType < m_TerrainSprites.Count)
                {
                    if (_Terrain[_Index].TerrainSpriteID < m_TerrainSprites[_Terrain[_Index].TerrainType].Count)
                    {
                        _Sprite.sprite = m_TerrainSprites[_Terrain[_Index].TerrainType][_Terrain[_Index].TerrainSpriteID];
                    }
                    else
                    {
                        Debug.Log($"!! Failed ID {_Terrain[_Index].TerrainSpriteID}");
                    }
                }
                else
                {
                    Debug.Log($"!! Failed Type {_Terrain[_Index].TerrainType}");
                }

                if (_Terrain[_Index].Mirrored % 4 == 1)
                {
                    _Sprite.transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (_Terrain[_Index].Mirrored % 4 == 2)
                {
                    _Sprite.transform.localScale = new Vector3(1, -1, 1);
                }
                else if (_Terrain[_Index].Mirrored % 4 == 3)
                {
                    _Sprite.transform.localScale = new Vector3(-1, -1, 1);
                }

                _Sprite.name = $"{_Sprite.sprite.name}  Pos {_Index}  ID {_Terrain[_Index].TerrainSpriteID} Mirror {_Terrain[_Index].Mirrored}";

                m_TerrainSpriteRenderers.Add(_Sprite);
            }
        }
    }
}
