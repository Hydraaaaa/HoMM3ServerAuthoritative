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

    public void Initialize(MapObject a_Parent)
    {
        m_Parent = a_Parent;

        gameObject.name = a_Parent.ScenarioObject.Template.Name;

        transform.position = new Vector3(a_Parent.ScenarioObject.XPos + 0.5f, -a_Parent.ScenarioObject.YPos - 0.5f, 0);
        m_SpriteRenderer.sortingLayerName = "MapShadows";
    }
}
