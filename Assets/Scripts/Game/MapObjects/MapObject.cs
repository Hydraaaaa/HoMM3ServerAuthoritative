using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public class MapObject : MapObjectBase
{
    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;
    public MapObjectRenderer Renderer => m_Renderer;
    public MapObjectRenderer ShadowRenderer => m_ShadowRenderer;

    public PlayerColors PlayerColors => m_PlayerColors;
    public FactionList Factions => m_Factions;
    public MonsterList Monsters => m_Monsters;
    public HeroList Heroes => m_Heroes;

    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] MapObjectRenderer m_Renderer;
    [SerializeField] MapObjectRenderer m_ShadowRenderer;

    [Space]

    [SerializeField] PlayerColors m_PlayerColors;
    [SerializeField] FactionList m_Factions;
    [SerializeField] MonsterList m_Monsters;
    [SerializeField] HeroList m_Heroes;

    public void Initialize(ScenarioObject a_ScenarioObject, GameReferences a_GameReferences)
    {
        m_GameReferences = a_GameReferences;

        gameObject.name = a_ScenarioObject.Template.Name;

        m_SpriteRenderer.sortingOrder = -32767 + a_ScenarioObject.SortOrder;

        if (a_ScenarioObject.Template.IsLowPrioritySortOrder)
        {
            m_SpriteRenderer.sortingLayerName = "MapLowPriorityObjects";
        }

        var _Operation = Addressables.LoadAssetAsync<MapObjectVisualData>($"MapObjects/{gameObject.name}.asset");

        // Synchonously, because underground objects are initially disabled, which break their coroutines
        MapObjectVisualData _Data = _Operation.WaitForCompletion();

        if (_Operation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
        {
            Debug.Log(gameObject.name);
        }
        else
        {
            Renderer.SetSprites(_Operation.Result.Sprites);
            ShadowRenderer.SetSprites(_Operation.Result.ShadowSprites);
        }
    }
}
