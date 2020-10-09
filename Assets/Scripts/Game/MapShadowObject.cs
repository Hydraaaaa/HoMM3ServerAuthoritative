using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShadowObject : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;
    public SimpleAnimation Animation => m_Animation;

    [SerializeField] protected SpriteRenderer m_SpriteRenderer = null;
    [SerializeField] protected SimpleAnimation m_Animation = null;

    protected MapObject m_Parent;

    void Initialize(MapObject a_Parent)
    {
        m_Parent = a_Parent;
    }
}
