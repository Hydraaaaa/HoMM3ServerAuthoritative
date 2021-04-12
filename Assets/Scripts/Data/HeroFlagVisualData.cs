using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Hero Flag Visual Data")]
public class HeroFlagVisualData : ScriptableObject
{
    public Sprite[] IdleSprites => m_IdleSprites;
    public SpriteArrayContainer[] MovingSprites => m_MovingSprites;

    [SerializeField] Sprite[] m_IdleSprites;
    [SerializeField] SpriteArrayContainer[] m_MovingSprites;
}
