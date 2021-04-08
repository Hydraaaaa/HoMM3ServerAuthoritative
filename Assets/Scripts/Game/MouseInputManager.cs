using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] OwnedHeroes m_OwnedHeroes;

    [Header("Cursors")]
    [SerializeField] Sprite m_MoveCursor;
    [SerializeField] Sprite m_MoveCursor2;
    [SerializeField] Sprite m_MoveCursor3;
    [SerializeField] Sprite m_MoveCursor4;
    [SerializeField] Sprite m_InteractCursor;
    [SerializeField] Sprite m_InteractCursor2;
    [SerializeField] Sprite m_InteractCursor3;
    [SerializeField] Sprite m_InteractCursor4;
    [SerializeField] Sprite m_CastleCursor;
    [SerializeField] Sprite m_HeroCursor;
    [SerializeField] Sprite m_TradeCursor;
    [SerializeField] Sprite m_TradeCursor2;
    [SerializeField] Sprite m_TradeCursor3;
    [SerializeField] Sprite m_TradeCursor4;

    [Space]

    [SerializeField] Sprite m_TopLeftCursor;
    [SerializeField] Sprite m_TopCursor;
    [SerializeField] Sprite m_TopRightCursor;
    [SerializeField] Sprite m_LeftCursor;
    [SerializeField] Sprite m_RightCursor;
    [SerializeField] Sprite m_BottomLeftCursor;
    [SerializeField] Sprite m_BottomCursor;
    [SerializeField] Sprite m_BottomRightCursor;

    Vector2Int m_PreviousWorldMouseCoords;
    List<MapObjectBase> m_Objects;
    bool m_UpdateCursor = false;

    void Update()
    {
        if (Input.mousePosition.x >= GameScreenScaler.VIEWPORT_PADDING_LEFT &&
            Input.mousePosition.x <= Screen.width - GameScreenScaler.VIEWPORT_PADDING_RIGHT &&
            Input.mousePosition.y >= GameScreenScaler.VIEWPORT_PADDING_BOTTOM &&
            Input.mousePosition.y <= Screen.height - GameScreenScaler.VIEWPORT_PADDING_TOP)
        {
            Vector3 _WorldMousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition - new Vector3(0, 8, 0));
            Vector2Int _WorldMouseCoords = new Vector2Int
            (
                (int)(_WorldMousePos.x + 0.5f),
                (int)(-_WorldMousePos.y + 0.5f)
            );

            if (m_PreviousWorldMouseCoords != _WorldMouseCoords ||
                m_UpdateCursor)
            {
                MapHero _SelectedHero = m_OwnedHeroes.SelectedHero;

                m_PreviousWorldMouseCoords = _WorldMouseCoords;
                m_UpdateCursor = false;

                Collider2D[] _Colliders = Physics2D.OverlapPointAll(_WorldMousePos);

                m_Objects = new List<MapObjectBase>();

                for (int i = 0; i < _Colliders.Length; i++)
                {
                    MapObjectBase _Object = _Colliders[i].GetComponent<MapObjectBase>();

                    if (_WorldMousePos.x % 1 == 0.5f)
                    {
                        _WorldMousePos.x += 0.01f;
                    }

                    if (_WorldMousePos.y % 1 == -0.5f)
                    {
                        _WorldMousePos.y -= 0.01f;
                    }

                    int _XIndex = 7 - Mathf.FloorToInt(_Object.transform.position.x - _WorldMousePos.x);
                    int _YIndex = 5 - Mathf.FloorToInt(_WorldMousePos.y - _Object.transform.position.y);

                    if (_XIndex < 0 ||
                        _XIndex > 7 ||
                        _YIndex < 0 ||
                        _YIndex > 5)
                    {
                        continue;
                    }

                    if ((_Object.MouseCollision[_YIndex] & 1 << _XIndex) != 0)
                    {
                        m_Objects.Add(_Object);
                    }
                }

                bool _CursorSelected = false;

                int _TurnCost = _SelectedHero.GetPathingTurnCost(_WorldMouseCoords.x, _WorldMouseCoords.y);

                for (int i = 0; i < m_Objects.Count; i++)
                {
                    MapHero _Hero = m_Objects[i] as MapHero;

                    if (_Hero != null)
                    {
                        if (_SelectedHero == _Hero ||
                            _SelectedHero == null)
                        {
                            CursorManager.SetCursor(m_HeroCursor, new Vector2(-12, 10));
                        }
                        else
                        {
                            switch (_TurnCost)
                            {
                                case 0: CursorManager.ResetCursor(); break;
                                case 1: CursorManager.SetCursor(m_TradeCursor, new Vector2(-8, 9)); break;
                                case 2: CursorManager.SetCursor(m_TradeCursor2, new Vector2(-8, 9)); break;
                                case 3: CursorManager.SetCursor(m_TradeCursor3, new Vector2(-8, 9)); break;
                                default: CursorManager.SetCursor(m_TradeCursor4, new Vector2(-8, 9)); break;
                            }
                        }

                        _CursorSelected = true;

                        break;
                    }
                }

                if (!_CursorSelected)
                {
                    for (int i = 0; i < m_Objects.Count; i++)
                    {
                        MapTown _Town = m_Objects[i] as MapTown;

                        if (_Town != null)
                        {
                            int _XIndex = 7 - Mathf.Clamp(Mathf.FloorToInt(m_Objects[i].transform.position.x - _WorldMousePos.x), 0, 7);
                            int _YIndex = 5 - Mathf.Clamp(Mathf.FloorToInt(_WorldMousePos.y - m_Objects[i].transform.position.y), 0, 5);

                            if (_SelectedHero != null &&
                                _XIndex == 5 &&
                                _YIndex == 5)
                            {
                                switch (_TurnCost)
                                {
                                    case 0: CursorManager.ResetCursor(); break;
                                    case 1: CursorManager.SetCursor(m_InteractCursor, new Vector2(-14, 15)); break;
                                    case 2: CursorManager.SetCursor(m_InteractCursor2, new Vector2(-14, 15)); break;
                                    case 3: CursorManager.SetCursor(m_InteractCursor3, new Vector2(-14, 15)); break;
                                    default: CursorManager.SetCursor(m_InteractCursor4, new Vector2(-14, 15)); break;
                                }
                            }
                            else
                            {
                                CursorManager.SetCursor(m_CastleCursor, new Vector2(-12, 12));
                            }

                            _CursorSelected = true;

                            break;
                        }
                    }
                }

                if (!_CursorSelected)
                {
                    if (m_Objects.Count > 0)
                    {
                        int _XIndex = 7 - Mathf.Clamp(Mathf.FloorToInt(m_Objects[0].transform.position.x - _WorldMousePos.x), 0, 7);
                        int _YIndex = 5 - Mathf.Clamp(Mathf.FloorToInt(_WorldMousePos.y - m_Objects[0].transform.position.y), 0, 5);

                        if ((m_Objects[0].InteractionCollision[_YIndex] & 1 << _XIndex) != 0)
                        {
                            switch (_TurnCost)
                            {
                                case 0: CursorManager.ResetCursor(); break;
                                case 1: CursorManager.SetCursor(m_InteractCursor, new Vector2(-14, 15)); break;
                                case 2: CursorManager.SetCursor(m_InteractCursor2, new Vector2(-14, 15)); break;
                                case 3: CursorManager.SetCursor(m_InteractCursor3, new Vector2(-14, 15)); break;
                                default: CursorManager.SetCursor(m_InteractCursor4, new Vector2(-14, 15)); break;
                            }

                            _CursorSelected = true;
                        }
                    }
                }

                if (!_CursorSelected)
                {
                    if (_SelectedHero.GetTargetDestination() == _WorldMouseCoords)
                    {
                        switch (_TurnCost)
                        {
                            case 0: CursorManager.ResetCursor(); break;
                            case 1: CursorManager.SetCursor(m_InteractCursor, new Vector2(-14, 15)); break;
                            case 2: CursorManager.SetCursor(m_InteractCursor2, new Vector2(-14, 15)); break;
                            case 3: CursorManager.SetCursor(m_InteractCursor3, new Vector2(-14, 15)); break;
                            default: CursorManager.SetCursor(m_InteractCursor4, new Vector2(-14, 15)); break;
                        }
                    }
                    else
                    {
                        switch (_TurnCost)
                        {
                            case 0: CursorManager.ResetCursor(); break;
                            case 1: CursorManager.SetCursor(m_MoveCursor, new Vector2(-15, 13)); break;
                            case 2: CursorManager.SetCursor(m_MoveCursor2, new Vector2(-15, 13)); break;
                            case 3: CursorManager.SetCursor(m_MoveCursor3, new Vector2(-15, 13)); break;
                            default: CursorManager.SetCursor(m_MoveCursor4, new Vector2(-15, 13)); break;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                m_OwnedHeroes.SelectedHero?.OnLeftClick(_WorldMouseCoords.x, _WorldMouseCoords.y, m_Objects);
                m_UpdateCursor = true;
            }
        }
        else
        {
            m_UpdateCursor = true;
            CursorManager.ResetCursor();
        }

        if (Input.mousePosition.x <= 2)
        {
            if (Input.mousePosition.y <= 2)
            {
                CursorManager.SetCursor(m_BottomLeftCursor, new Vector2(0, 18));
            }
            else if (Input.mousePosition.y >= Screen.height - 3)
            {
                CursorManager.SetCursor(m_TopLeftCursor, new Vector2(0, 1));
            }
            else
            {
                CursorManager.SetCursor(m_LeftCursor, new Vector2(0, 5));
            }
        }
        else if (Input.mousePosition.x >= Screen.width - 3)
        {
            if (Input.mousePosition.y <= 2)
            {
                CursorManager.SetCursor(m_BottomRightCursor, new Vector2(-18, 18));
            }
            else if (Input.mousePosition.y >= Screen.height - 3)
            {
                CursorManager.SetCursor(m_TopRightCursor, new Vector2(-18, 1));
            }
            else
            {
                CursorManager.SetCursor(m_RightCursor, new Vector2(-23, 6));
            }
        }
        else
        {
            if (Input.mousePosition.y <= 2)
            {
                CursorManager.SetCursor(m_BottomCursor, new Vector2(-6, 23));
            }
            else if (Input.mousePosition.y >= Screen.height - 3)
            {
                CursorManager.SetCursor(m_TopCursor, new Vector2(-6, 0));
            }
        }
    }
}
