using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapObjectVisualData : ScriptableObject
{
    public Sprite[] Sprites => m_Sprites;
    public Sprite[] ShadowSprites => m_ShadowSprites;

    [SerializeField] public Sprite[] m_Sprites = null;
    [SerializeField] public Sprite[] m_ShadowSprites = null;
}
