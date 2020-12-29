using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTileObject : MonoBehaviour
{
    public SpriteRenderer Renderer => m_Renderer;
    public MapObjectRenderer AnimationRenderer => m_AnimationRenderer;

    [SerializeField] SpriteRenderer m_Renderer = null;
    [SerializeField] MapObjectRenderer m_AnimationRenderer = null;
}
