using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public class MapObject : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;
    public MapObjectRenderer Renderer => m_Renderer;
    public MapObjectRenderer ShadowRenderer => m_ShadowRenderer;

    public PlayerColors PlayerColors => m_PlayerColors;
    public FactionList Factions => m_Factions;
    public MonsterList Monsters => m_Monsters;
    public HeroList Heroes => m_Heroes;

    [SerializeField] SpriteRenderer m_SpriteRenderer = null;
    [SerializeField] MapObjectRenderer m_Renderer = null;
    [SerializeField] MapObjectRenderer m_ShadowRenderer = null;

    [Space]

    [SerializeField] PlayerColors m_PlayerColors = null;
    [SerializeField] FactionList m_Factions = null;
    [SerializeField] MonsterList m_Monsters = null;
    [SerializeField] HeroList m_Heroes = null;

    public void Initialize(ScenarioObject a_ScenarioObject)
    {
        gameObject.name = a_ScenarioObject.Template.Name;

        m_SpriteRenderer.sortingOrder = -32767 + a_ScenarioObject.SortOrder;

        if (a_ScenarioObject.Template.IsLowPrioritySortOrder)
        {
            m_SpriteRenderer.sortingLayerName = "MapLowPriorityObjects";
        }

        StartCoroutine(LoadVisualData());
    }

    IEnumerator LoadVisualData()
    {
        var _Operation = Addressables.LoadAssetAsync<MapObjectVisualData>($"MapObjects/{gameObject.name}.asset");

        yield return _Operation;

        if (_Operation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
        {
            Debug.Log(gameObject.name);
            yield break;
        }

        Renderer.SetSprites(_Operation.Result.Sprites);

        if (_Operation.Result.Sprites.Length == 0)
        {
            Debug.Log($"!! FAILED - {_Operation.Result.name}");
        }

        ShadowRenderer.SetSprites(_Operation.Result.ShadowSprites);

        if (_Operation.Result.Sprites.Length == 0)
        {
            Debug.Log($"!! SHADOW - {_Operation.Result.name}");
        }
    }
}
