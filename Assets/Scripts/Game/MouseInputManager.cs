using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] LocalOwnership m_LocalOwnership;
    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] Pathfinding m_Pathfinding;

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
    [SerializeField] Sprite m_AttackCursor;
    [SerializeField] Sprite m_AttackCursor2;
    [SerializeField] Sprite m_AttackCursor3;
    [SerializeField] Sprite m_AttackCursor4;

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

    // Storing object hovered over for reuse in the event of a mouse click
    MapObjectBase m_HoveredObject;
    MapTown m_HoveredTown;
    MapHero m_HoveredHero;

    bool m_UpdateCursor = false;

    void Update()
    {
        // If mouse is in the actual map area
        if (Input.mousePosition.x >= GameScreenScaler.VIEWPORT_PADDING_LEFT &&
            Input.mousePosition.y >= GameScreenScaler.VIEWPORT_PADDING_BOTTOM &&
            Input.mousePosition.x <= Screen.width - GameScreenScaler.VIEWPORT_PADDING_RIGHT &&
            Input.mousePosition.y <= Screen.height - GameScreenScaler.VIEWPORT_PADDING_TOP)
        {
            Vector3 _WorldMousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition - new Vector3(0, 8, 0));
            Vector2Int _WorldMouseCoords = new Vector2Int
            (
                (int)(_WorldMousePos.x + 0.5f),
                (int)(-_WorldMousePos.y + 0.5f)
            );

            // If mouse has moved onto a different tile in the world
            if (m_PreviousWorldMouseCoords != _WorldMouseCoords ||
                m_UpdateCursor)
            {
                // If mouse coords are within the bounds of the map
                if (_WorldMouseCoords.x >= 0 &&
                    _WorldMouseCoords.y >= 0 &&
                    _WorldMouseCoords.x < m_GameSettings.Scenario.Size &&
                    _WorldMouseCoords.y < m_GameSettings.Scenario.Size)
                {
                    m_HoveredTown = null;
                    m_HoveredHero = null;

                    MapHero _SelectedHero = m_LocalOwnership.SelectedHero;

                    m_PreviousWorldMouseCoords = _WorldMouseCoords;
                    m_UpdateCursor = false;

                    Pathfinding.Node _Node = m_Pathfinding.GetNode(_WorldMouseCoords, false);

                    List<MapObjectBase> _Objects = new List<MapObjectBase>(_Node.BlockingObjects.Count + _Node.InteractionObjects.Count);

                    for (int i = 0; i < _Node.BlockingObjects.Count; i++)
                    {
                        _Objects.Add(_Node.BlockingObjects[i]);
                    }

                    for (int i = 0; i < _Node.InteractionObjects.Count; i++)
                    {
                        _Objects.Add(_Node.InteractionObjects[i]);
                    }

                    // Flag used to break out of logic if an earlier bit of logic has already determined what the cursor should be
                    bool _CursorSelected = false;

                    int _TurnCost = 0;

                    if (_SelectedHero != null)
                    {
                        _TurnCost = _SelectedHero.GetPathingTurnCost(_WorldMouseCoords.x, _WorldMouseCoords.y);
                    }

                    // Check if any of the objects are heroes
                    for (int i = 0; i < _Objects.Count; i++)
                    {
                        MapHero _Hero = _Objects[i] as MapHero;

                        if (_Hero != null)
                        {
                            m_HoveredObject = _Hero;

                            if (_SelectedHero != null)
                            {
                                if (_SelectedHero == _Hero)
                                {
                                    CursorManager.SetCursor(m_HeroCursor, new Vector2(-12, 10));
                                }
                                else if (_Hero.IsPrison)
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
                                else if (_Hero.PlayerIndex == m_GameSettings.LocalPlayerIndex)
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
                                else
                                {
                                    switch (_TurnCost)
                                    {
                                        case 0: CursorManager.ResetCursor(); break;
                                        case 1: CursorManager.SetCursor(m_AttackCursor, new Vector2(-13, 13)); break;
                                        case 2: CursorManager.SetCursor(m_AttackCursor2, new Vector2(-13, 13)); break;
                                        case 3: CursorManager.SetCursor(m_AttackCursor3, new Vector2(-13, 13)); break;
                                        default: CursorManager.SetCursor(m_AttackCursor4, new Vector2(-13, 13)); break;
                                    }
                                }

                                _CursorSelected = true;
                            }
                            else if (!_Hero.IsPrison &&
                                     _Hero.PlayerIndex == m_GameSettings.LocalPlayerIndex)
                            {
                                CursorManager.SetCursor(m_HeroCursor, new Vector2(-12, 10));
                                m_HoveredHero = _Hero;

                                _CursorSelected = true;
                            }

                            break;
                        }
                    }

                    // Check if any of the objects are towns
                    if (!_CursorSelected)
                    {
                        for (int i = 0; i < _Objects.Count; i++)
                        {
                            MapTown _Town = _Objects[i] as MapTown;

                            if (_Town != null)
                            {
                                m_HoveredObject = _Objects[i];

                                int _XIndex = 8 - Mathf.Clamp(Mathf.CeilToInt(_Objects[i].transform.position.x - _WorldMousePos.x), 0, 7);
                                int _YIndex = 6 - Mathf.Clamp(Mathf.CeilToInt(_WorldMousePos.y - _Objects[i].transform.position.y), 0, 5);

                                // If the cursor is specifically on the castle's entrance, horse rear cursor
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
                                else if (_Town.PlayerIndex == m_GameSettings.LocalPlayerIndex)
                                {
                                    CursorManager.SetCursor(m_CastleCursor, new Vector2(-12, 12));
                                    m_HoveredTown = _Town;
                                }
                                else
                                {
                                    CursorManager.ResetCursor();
                                }

                                _CursorSelected = true;

                                break;
                            }
                        }
                    }

                    // If a hero is currently selected, set movement cursor
                    if (_SelectedHero != null)
                    {
                        if (!_CursorSelected)
                        {
                            // If the mouse is on an interaction tile, horse rear cursor
                            if (_Objects.Count > 0)
                            {
                                int _XIndex = 8 - Mathf.Clamp(Mathf.CeilToInt(_Objects[0].transform.position.x - _WorldMousePos.x), 0, 7);
                                int _YIndex = 6 - Mathf.Clamp(Mathf.CeilToInt(_WorldMousePos.y - _Objects[0].transform.position.y), 0, 5);

                                if ((_Objects[0].InteractionCollision[_YIndex] & 1 << _XIndex) != 0)
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
                            // If the mouse is on the end point of the current selected destination, horse rear cursor
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

                            _CursorSelected = true;
                        }
                    }

                    if (!_CursorSelected)
                    {
                        CursorManager.ResetCursor();
                    }
                }
                else
                {
                    CursorManager.ResetCursor();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (m_HoveredTown != null)
                {
                    if (m_HoveredTown != m_LocalOwnership.SelectedTown)
                    {
                        m_LocalOwnership.SelectTown(m_HoveredTown);
                    }
                    else
                    {
                        // Enter the town
                    }
                }
                else if (m_LocalOwnership.SelectedHero != null)
                {
                    if (m_LocalOwnership.SelectedHero.IsMoving)
                    {
                        m_LocalOwnership.SelectedHero.CancelMovement();
                    }
                    else
                    {
                        m_LocalOwnership.SelectedHero.OnLeftClick(_WorldMouseCoords.x, _WorldMouseCoords.y, m_HoveredObject);
                    }
                }
                else if (m_HoveredHero != null)
                {
                    m_LocalOwnership.SelectHero(m_HoveredHero);
                }

                m_UpdateCursor = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_LocalOwnership.SelectedHero != null)
                {
                    if (m_LocalOwnership.SelectedHero.IsMoving)
                    {
                        m_LocalOwnership.SelectedHero.CancelMovement();
                    }
                }
            }

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
