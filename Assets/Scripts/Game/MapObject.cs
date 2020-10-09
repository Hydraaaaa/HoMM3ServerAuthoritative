using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;
    public SimpleAnimation Animation => m_Animation;

    [SerializeField] protected SpriteRenderer m_SpriteRenderer = null;
    [SerializeField] protected SimpleAnimation m_Animation = null;

    protected ScenarioObject m_ScenarioObject;
    protected MapShadowObject m_Shadow;

    void Initialize(ScenarioObject a_ScenarioObject, MapShadowObject a_Shadow)
    {
        m_ScenarioObject = a_ScenarioObject;
        m_Shadow = a_Shadow;
    }
}
