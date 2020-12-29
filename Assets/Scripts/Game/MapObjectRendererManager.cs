using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectRendererManager : MonoBehaviour
{
    static MapObjectRendererManager s_Instance;

    static List<MapObjectRenderer> s_Renderers = new List<MapObjectRenderer>();

    public static void AddObject(MapObjectRenderer a_Renderer)
    {
        s_Renderers.Add(a_Renderer);
    }

    public static void RemoveObject(MapObjectRenderer a_Renderer)
    {
        s_Renderers.Remove(a_Renderer);
    }

    [SerializeField] Transform m_Camera;

    void Awake()
    {
        s_Instance = this;
    }

    void Update()
    {
        int _ScreenWidth = Screen.width - GameScreenScaler.VIEWPORT_PADDING_LEFT - GameScreenScaler.VIEWPORT_PADDING_RIGHT;
        int _ScreenHeight = Screen.height - GameScreenScaler.VIEWPORT_PADDING_TOP - GameScreenScaler.VIEWPORT_PADDING_BOTTOM;

        Vector2Int _MinBounds = new Vector2Int((int)m_Camera.transform.position.x - Mathf.CeilToInt(_ScreenWidth / 64), (int)m_Camera.transform.position.y - Mathf.CeilToInt(_ScreenHeight / 64) - 1);
        Vector2Int _MaxBounds = new Vector2Int((int)m_Camera.transform.position.x + Mathf.CeilToInt(_ScreenWidth / 64) + 3, (int)m_Camera.transform.position.y + Mathf.CeilToInt(_ScreenHeight / 64));

        int _Frame = Mathf.FloorToInt(Time.time * 5.5f);

        for (int i = 0; i < s_Renderers.Count; i++)
        {
            MapObjectRenderer _Animation = s_Renderers[i];
            Transform _Transform = _Animation.transform;
            
            if (_Transform.position.x >= _MinBounds.x &&
                _Transform.position.y >= _MinBounds.y &&
                _Transform.position.x <= _MaxBounds.x &&
                _Transform.position.y <= _MaxBounds.y)
            {
                _Animation.Animate(_Frame);
            }
        }
    }
}
