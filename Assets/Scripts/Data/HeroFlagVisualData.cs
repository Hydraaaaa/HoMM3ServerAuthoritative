using System;
using UnityEngine;

[CreateAssetMenu]
public class HeroFlagVisualData : ScriptableObject
{
    public Sprite[] IdleSprites => m_IdleSprites;
    public SpriteArrayContainer[] MovingSprites => m_MovingSprites;

    [SerializeField] Sprite[] m_IdleSprites;
    [SerializeField] SpriteArrayContainer[] m_MovingSprites;
}
