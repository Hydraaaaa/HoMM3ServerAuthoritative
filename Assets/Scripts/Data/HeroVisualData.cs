using System;
using UnityEngine;

[CreateAssetMenu]
public class HeroVisualData : ScriptableObject
{
    public static readonly bool[] SPRITES_FLIPPED = { false, false, false, true, true, true, false, false };

    public Sprite[] IdleSprites => m_IdleSprites;
    public SpriteArrayContainer[] MovingSprites => m_MovingSprites;

    public Sprite[] ShadowIdleSprites => m_ShadowIdleSprites;
    public SpriteArrayContainer[] ShadowMovingSprites => m_ShadowMovingSprites;

    [SerializeField] Sprite[] m_IdleSprites;
    [SerializeField] Sprite[] m_ShadowIdleSprites;
    [SerializeField] SpriteArrayContainer[] m_MovingSprites;
    [SerializeField] SpriteArrayContainer[] m_ShadowMovingSprites;
}
