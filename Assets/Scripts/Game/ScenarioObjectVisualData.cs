using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScenarioObjectVisualData : ScriptableObject
{
    public Sprite Sprite => m_Sprite;
    public Sprite ShadowSprite => m_ShadowSprite;
    public AnimationClip Animation => m_Animation;
    public AnimationClip ShadowAnimation => m_ShadowAnimation;
    public bool IsLowerSortOrderPriority => m_IsLowerSortOrderPriority;

    [SerializeField] Sprite m_Sprite = null;
    [SerializeField] Sprite m_ShadowSprite = null;
    [SerializeField] AnimationClip m_Animation = null;
    [SerializeField] AnimationClip m_ShadowAnimation = null;
    [SerializeField] bool m_IsLowerSortOrderPriority = false;
}
