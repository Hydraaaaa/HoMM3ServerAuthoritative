using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MapDwelling : MapObjectBase
{
    [SerializeField] MapObjectRenderer m_Renderer;
    [SerializeField] MapObjectRenderer m_ShadowRenderer;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] PlayerColors m_PlayerColors;

    public void Initialize(ScenarioObject a_ScenarioObject, GameReferences a_GameReferences)
    {
        m_GameReferences = a_GameReferences;

        gameObject.name = a_ScenarioObject.Template.Name;

        m_SpriteRenderer.sortingOrder = -32767 + a_ScenarioObject.SortOrder;

        uint _ColorIndex = a_ScenarioObject.DwellingOwner;

        // Seems neutral objects can represent multiple different values
        if (_ColorIndex > 8)
        {
            _ColorIndex = 8;
        }

        m_SpriteRenderer.material.SetColor("_PlayerColor", m_PlayerColors.Colors[_ColorIndex]);

        var _Operation = Addressables.LoadAssetAsync<MapObjectVisualData>($"MapObjects/{gameObject.name}.asset");

        // Synchonously, because underground objects are initially disabled, which break their coroutines
        MapObjectVisualData _Data = _Operation.WaitForCompletion();

        if (_Operation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
        {
            Debug.Log(gameObject.name);
        }
        else
        {
            m_Renderer.SetSprites(_Operation.Result.Sprites);
            m_ShadowRenderer.SetSprites(_Operation.Result.ShadowSprites);
        }
    }
}
