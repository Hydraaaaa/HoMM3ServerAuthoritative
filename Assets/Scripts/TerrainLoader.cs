using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoader : MonoBehaviour
{
    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] SpriteRenderer m_TerrainSpritePrefab;
    [SerializeField] List<Sprite> m_DirtSprites;
    [SerializeField] List<Sprite> m_SandSprites;
    [SerializeField] List<Sprite> m_GrassSprites;
    [SerializeField] List<Sprite> m_SnowSprites;
    [SerializeField] List<Sprite> m_SwampSprites;
    [SerializeField] List<Sprite> m_RoughSprites;
    [SerializeField] List<Sprite> m_SubterraneanSprites;
    [SerializeField] List<Sprite> m_LavaSprites;
    [SerializeField] List<Sprite> m_WaterSprites;
    [SerializeField] List<Sprite> m_RockSprites;

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

        List<TerrainTile> _Terrain = m_GameSettings.Map.Terrain;
        int _Size = m_GameSettings.Map.Size;

        m_TerrainSpriteRenderers = new List<SpriteRenderer>(_Terrain.Capacity);

        for (int x = 0; x < _Size; x++)
        {
            for (int y = 0; y < _Size; y++)
            {
                int _Index = x + y * _Size;

                SpriteRenderer _Sprite = Instantiate (m_TerrainSpritePrefab, new Vector2(x, -y), Quaternion.identity, transform);

                _Sprite.sprite = m_TerrainSprites[_Terrain[_Index].TerrainType][_Terrain[_Index].TerrainSpriteID];

                if (_Terrain[_Index].Mirrored == 1 ||
                    _Terrain[_Index].Mirrored == 65)
                {
                    _Sprite.transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (_Terrain[_Index].Mirrored == 2 ||
                         _Terrain[_Index].Mirrored == 66)
                {
                    _Sprite.transform.localScale = new Vector3(1, -1, 1);
                }
                else if (_Terrain[_Index].Mirrored == 3 ||
                         _Terrain[_Index].Mirrored == 67)
                {
                    _Sprite.transform.localScale = new Vector3(-1, -1, 1);
                }

                _Sprite.name = _Sprite.sprite.name + " " + _Terrain[_Index].Mirrored;

                m_TerrainSpriteRenderers.Add(_Sprite);
            }
        }
    }
}
