using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MapResource : MapObjectBase
{
    public MapObjectVisualData Resource { get; private set; }

    public DynamicMapObstacle DynamicObstacle => m_DynamicObstacle;

    [SerializeField] MapObjectRenderer m_Renderer;
    [SerializeField] MapObjectRenderer m_ShadowRenderer;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] DynamicMapObstacle m_DynamicObstacle;

    [SerializeField] ResourceList m_Resources;

    public void Initialize(ScenarioObject a_ScenarioObject, Pathfinding a_Pathfinding)
    {
        gameObject.name = a_ScenarioObject.Template.Name;

        m_SpriteRenderer.sortingOrder = -32767 + a_ScenarioObject.SortOrder;

        switch (a_ScenarioObject.Template.Name)
        {
            case "avtwood0":
                Resource = m_Resources.Resources[0];
                break;

            case "avtmerc0":
                Resource = m_Resources.Resources[1];
                break;

            case "avtore0":
                Resource = m_Resources.Resources[2];
                break;

            case "avtsulf0":
                Resource = m_Resources.Resources[3];
                break;

            case "avtcrys0":
                Resource = m_Resources.Resources[4];
                break;

            case "avtgems0":
                Resource = m_Resources.Resources[5];
                break;

            case "avtgold0":
                Resource = m_Resources.Resources[6];
                break;

            default:
                Resource = m_Resources.Resources[Random.Range(0, m_Resources.Resources.Count)];
                break;
        }

        m_Renderer.SetSprites(Resource.m_Sprites);
        m_ShadowRenderer.SetSprites(Resource.m_ShadowSprites);

        m_DynamicObstacle.Initialize(a_Pathfinding, this);
    }
}
