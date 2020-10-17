using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;
    public SimpleAnimation Animation => m_Animation;

    public PlayerColors PlayerColors => m_PlayerColors;

    [SerializeField] protected SpriteRenderer m_SpriteRenderer = null;
    [SerializeField] protected SimpleAnimation m_Animation = null;

    [Space]

    [SerializeField] protected PlayerColors m_PlayerColors = null;

    public ScenarioObject ScenarioObject => m_ScenarioObject;
    public MapShadowObject Shadow => m_Shadow;

    protected ScenarioObject m_ScenarioObject;
    protected MapShadowObject m_Shadow;

    public byte[] Collision;

    public void Initialize(ScenarioObject a_ScenarioObject, MapShadowObject a_Shadow)
    {
        m_ScenarioObject = a_ScenarioObject;
        m_Shadow = a_Shadow;

        gameObject.name = a_ScenarioObject.Template.Name;

        transform.position = new Vector3(a_ScenarioObject.XPos + 0.5f, -a_ScenarioObject.YPos - 0.5f, 0);
        m_SpriteRenderer.sortingOrder = -32767 + a_ScenarioObject.SortOrder;
        m_SpriteRenderer.sortingLayerName = "MapObjects";

        Collision = a_ScenarioObject.Template.Passability;

        switch (a_ScenarioObject.Template.Type)
        {
            case ScenarioObjectType.Town:
                Town _Town = gameObject.AddComponent<Town>();

                _Town.Initialize(this);
                break;

            case ScenarioObjectType.Dwelling:
                Dwelling _Dwelling = gameObject.AddComponent<Dwelling>();

                _Dwelling.Initialize(this);
                break;
        }
    }
}
