using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShadowObject : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;
    public MapObjectRenderer Renderer => m_Renderer;

    [SerializeField] protected SpriteRenderer m_SpriteRenderer = null;
    [SerializeField] protected MapObjectRenderer m_Renderer = null;

    protected MapObject m_Parent;

    public void Initialize(MapObject a_Parent)
    {
        m_Parent = a_Parent;

        gameObject.name = a_Parent.gameObject.name;

        transform.position = new Vector3(a_Parent.ScenarioObject.PosX + 0.5f, -a_Parent.ScenarioObject.PosY - 0.5f, 0);
        m_SpriteRenderer.sortingLayerName = "MapShadows";
    }
}
