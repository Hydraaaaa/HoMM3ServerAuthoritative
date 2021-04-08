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

    public void Initialize(ScenarioObject a_ScenarioObject)
    {
        gameObject.name = a_ScenarioObject.Template.Name;

        m_SpriteRenderer.sortingOrder = -32767 + a_ScenarioObject.SortOrder;

        uint _ColorIndex = a_ScenarioObject.DwellingOwner;

        // Seems there are cases where _ColorIndex isn't 255, so doing this sweeping check
        // Not sure if the different values above 7 mean anything
        if (_ColorIndex > 8)
        {
            _ColorIndex = 8;
        }

        m_SpriteRenderer.material.SetColor("_PlayerColor", m_PlayerColors.Colors[_ColorIndex]);

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

        m_Renderer.SetSprites(_Operation.Result.Sprites);

        if (_Operation.Result.Sprites.Length == 0)
        {
            Debug.Log($"!! FAILED - {_Operation.Result.name}");
        }

        m_ShadowRenderer.SetSprites(_Operation.Result.ShadowSprites);

        if (_Operation.Result.Sprites.Length == 0)
        {
            Debug.Log($"!! SHADOW - {_Operation.Result.name}");
        }
    }
}
