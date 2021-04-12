using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Map Object Visual Data")]
public class MapObjectVisualData : ScriptableObject
{
    public Sprite[] Sprites => m_Sprites;
    public Sprite[] ShadowSprites => m_ShadowSprites;

    [SerializeField] public Sprite[] m_Sprites;
    [SerializeField] public Sprite[] m_ShadowSprites;
}
