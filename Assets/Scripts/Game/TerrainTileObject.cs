using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTileObject : MonoBehaviour
{
    public SpriteRenderer Renderer => m_Renderer;
    public SimpleAnimation Animation => m_Animation;

    [SerializeField] SpriteRenderer m_Renderer = null;
    [SerializeField] SimpleAnimation m_Animation = null;
}
