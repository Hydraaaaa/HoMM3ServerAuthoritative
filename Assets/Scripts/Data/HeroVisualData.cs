using System;
using UnityEngine;

[CreateAssetMenu]
public class HeroVisualData : ScriptableObject
{
    public static readonly bool[] SPRITES_FLIPPED = { false, false, false, true, true, true, false, false };

    // TODO: Make read only again

    public Sprite[] IdleSprites => m_IdleSprites;
    public SpriteArrayContainer[] MovingSprites => m_MovingSprites;

    public Sprite[] ShadowIdleSprites => m_ShadowIdleSprites;
    public SpriteArrayContainer[] ShadowMovingSprites => m_ShadowMovingSprites;

    public SpriteArrayContainer[] FlagIdleSprites => m_FlagIdleSprites;
    public SpriteArrayContainer[] FlagMovingSprites => m_FlagMovingSprites;

    [SerializeField] public Sprite[] m_IdleSprites;
    [SerializeField] public Sprite[] m_ShadowIdleSprites;
    [SerializeField] public SpriteArrayContainer[] m_MovingSprites;
    [SerializeField] public SpriteArrayContainer[] m_ShadowMovingSprites;

    [SerializeField] public SpriteArrayContainer[] m_FlagIdleSprites;
    [SerializeField] public SpriteArrayContainer[] m_FlagMovingSprites;
}
