using System;
using UnityEngine;

[CreateAssetMenu]
public class HeroFlagVisualData : ScriptableObject
{
    [Serializable]
    public class SpriteOffsetPair
    {
        public Sprite[] Sprites;
        public Vector2 Offset;
    }

    public Sprite[] IdleSprites => m_IdleSprites;
    public Vector2[] IdleOffsets => m_IdleOffsets;
    public SpriteOffsetPair[] MovingSprites => m_MovingSprites;

    // TODO: Make read only

    [SerializeField] public Sprite[] m_IdleSprites;
    [SerializeField] public Vector2[] m_IdleOffsets;
    [SerializeField] public SpriteOffsetPair[] m_MovingSprites;
}
