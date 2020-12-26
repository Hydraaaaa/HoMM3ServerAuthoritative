using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingTest : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] Pathfinding m_Pathfinding;
    [SerializeField] SpriteRenderer m_PathNodePrefab;
    [SerializeField] SpriteRenderer m_PathShadowNodePrefab;
    [SerializeField] GameObject m_StartObject;
    [SerializeField] Sprite[] m_Sprites;

    Vector2Int m_StartPos = Vector2Int.zero;

    List<Pathfinding.Node> m_Path;

    List<SpriteRenderer> m_PathNodes = new List<SpriteRenderer>();
    List<SpriteRenderer> m_PathShadowNodes = new List<SpriteRenderer>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 _MousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);

            m_StartPos = new Vector2Int((int)(_MousePos.x + 0.5f), (int)(_MousePos.y - 0.5f));

            m_StartObject.transform.position = new Vector3(m_StartPos.x, m_StartPos.y, 0);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 _MousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);

            if (m_StartPos != Vector2Int.zero)
            {
                m_Path = m_Pathfinding.GetPath(m_StartPos, new Vector2Int((int)(_MousePos.x + 0.5f), (int)(_MousePos.y - 0.5f)));

                if (m_Path != null)
                {
                    for (int i = 0; i < m_PathNodes.Count; i++)
                    {
                        Destroy(m_PathNodes[i].gameObject);
                        Destroy(m_PathShadowNodes[i].gameObject);
                    }

                    m_PathNodes.Clear();
                    m_PathShadowNodes.Clear();

                    for (int i = 0; i < m_Path.Count; i++)
                    {
                        Vector3 _Position = new Vector3(m_Path[i].PosX, -m_Path[i].PosY, 0);
                        SpriteRenderer _Node = Instantiate(m_PathNodePrefab, _Position, Quaternion.identity);
                        SpriteRenderer _ShadowNode = Instantiate(m_PathShadowNodePrefab, _Position, Quaternion.identity);

                        Vector2Int _PreviousNodePos;
                        Vector2Int _NextNodePos;

                        if (i > 0)
                        {
                            _PreviousNodePos = new Vector2Int(m_Path[i - 1].PosX, m_Path[i - 1].PosY);
                        }
                        else
                        {
                            _PreviousNodePos = m_StartPos;
                            _PreviousNodePos.x = Mathf.Abs(_PreviousNodePos.x);
                            _PreviousNodePos.y = Mathf.Abs(_PreviousNodePos.y);
                        }

                        _PreviousNodePos -= new Vector2Int(m_Path[i].PosX, m_Path[i].PosY);

                        if (i == m_Path.Count - 1)
                        {
                            _Node.sprite = m_Sprites[0];
                            _ShadowNode.sprite = m_Sprites[0];
                        }
                        else
                        {
                            _NextNodePos = new Vector2Int(m_Path[i + 1].PosX, m_Path[i + 1].PosY);
                            _NextNodePos -= new Vector2Int(m_Path[i].PosX, m_Path[i].PosY);

                            if (_NextNodePos.x == -1 &&
                                _NextNodePos.y == -1)
                            {
                                if (_PreviousNodePos.x == 1 &&
                                    _PreviousNodePos.y == 1)
                                {
                                    _Node.sprite = m_Sprites[16];
                                    _ShadowNode.sprite = m_Sprites[16];
                                }
                                else
                                {
                                    if (_PreviousNodePos.x == -1)
                                    {
                                        _Node.sprite = m_Sprites[8];
                                        _ShadowNode.sprite = m_Sprites[8];
                                    }
                                    else if (_PreviousNodePos.y == -1)
                                    {
                                        _Node.sprite = m_Sprites[24];
                                        _ShadowNode.sprite = m_Sprites[24];
                                    }
                                    else if (_PreviousNodePos.x == 1)
                                    {
                                        _Node.sprite = m_Sprites[24];
                                        _ShadowNode.sprite = m_Sprites[24];
                                    }
                                    else
                                    {
                                        _Node.sprite = m_Sprites[8];
                                        _ShadowNode.sprite = m_Sprites[8];
                                    }
                                }
                            }
                            else if (_NextNodePos.x == 1 &&
                                     _NextNodePos.y == 1)
                            {
                                if (_PreviousNodePos.x == -1 &&
                                    _PreviousNodePos.y == -1)
                                {
                                    _Node.sprite = m_Sprites[12];
                                    _ShadowNode.sprite = m_Sprites[12];
                                }
                                else
                                {
                                    if (_PreviousNodePos.x == 1)
                                    {
                                        _Node.sprite = m_Sprites[4];
                                        _ShadowNode.sprite = m_Sprites[4];
                                    }
                                    else if (_PreviousNodePos.y == 1)
                                    {
                                        _Node.sprite = m_Sprites[20];
                                        _ShadowNode.sprite = m_Sprites[20];
                                    }
                                    else if (_PreviousNodePos.x == -1)
                                    {
                                        _Node.sprite = m_Sprites[20];
                                        _ShadowNode.sprite = m_Sprites[20];
                                    }
                                    else
                                    {
                                        _Node.sprite = m_Sprites[4];
                                        _ShadowNode.sprite = m_Sprites[4];
                                    }
                                }
                            }
                            else if (_NextNodePos.x == -1 &&
                                     _NextNodePos.y == 1)
                            {
                                if (_PreviousNodePos.x == 1 &&
                                    _PreviousNodePos.y == -1)
                                {
                                    _Node.sprite = m_Sprites[14];
                                    _ShadowNode.sprite = m_Sprites[14];
                                }
                                else
                                {
                                    if (_PreviousNodePos.x == -1)
                                    {
                                        _Node.sprite = m_Sprites[22];
                                        _ShadowNode.sprite = m_Sprites[22];
                                    }
                                    else if (_PreviousNodePos.y == 1)
                                    {
                                        _Node.sprite = m_Sprites[6];
                                        _ShadowNode.sprite = m_Sprites[6];
                                    }
                                    else if (_PreviousNodePos.x == 1)
                                    {
                                        _Node.sprite = m_Sprites[6];
                                        _ShadowNode.sprite = m_Sprites[6];
                                    }
                                    else
                                    {
                                        _Node.sprite = m_Sprites[22];
                                        _ShadowNode.sprite = m_Sprites[22];
                                    }
                                }
                            }
                            else if (_NextNodePos.x == 1 &&
                                     _NextNodePos.y == -1)
                            {
                                if (_PreviousNodePos.x == -1 &&
                                    _PreviousNodePos.y == 1)
                                {
                                    _Node.sprite = m_Sprites[10];
                                        _ShadowNode.sprite = m_Sprites[10];
                                }
                                else
                                {
                                    if (_PreviousNodePos.x == 1)
                                    {
                                        _Node.sprite = m_Sprites[18];
                                        _ShadowNode.sprite = m_Sprites[18];
                                    }
                                    else if (_PreviousNodePos.y == -1)
                                    {
                                        _Node.sprite = m_Sprites[2];
                                        _ShadowNode.sprite = m_Sprites[2];
                                    }
                                    else if (_PreviousNodePos.x == -1)
                                    {
                                        _Node.sprite = m_Sprites[2];
                                        _ShadowNode.sprite = m_Sprites[2];
                                    }
                                    else
                                    {
                                        _Node.sprite = m_Sprites[18];
                                        _ShadowNode.sprite = m_Sprites[18];
                                    }
                                }
                            }
                            else if (_NextNodePos.x == 1)
                            {
                                if (_PreviousNodePos.y == 1)
                                {
                                    _Node.sprite = m_Sprites[19];
                                    _ShadowNode.sprite = m_Sprites[19];
                                }
                                else if (_PreviousNodePos.y == -1)
                                {
                                    _Node.sprite = m_Sprites[3];
                                    _ShadowNode.sprite = m_Sprites[3];
                                }
                                else
                                {
                                    _Node.sprite = m_Sprites[11];
                                    _ShadowNode.sprite = m_Sprites[11];
                                }
                            }
                            else if (_NextNodePos.x == -1)
                            {
                                if (_PreviousNodePos.y == 1)
                                {
                                    _Node.sprite = m_Sprites[7];
                                    _ShadowNode.sprite = m_Sprites[7];
                                }
                                else if (_PreviousNodePos.y == -1)
                                {
                                    _Node.sprite = m_Sprites[23];
                                    _ShadowNode.sprite = m_Sprites[23];
                                }
                                else
                                {
                                    _Node.sprite = m_Sprites[15];
                                    _ShadowNode.sprite = m_Sprites[15];
                                }
                            }
                            else if (_NextNodePos.y == 1)
                            {
                                if (_PreviousNodePos.x == 1)
                                {
                                    _Node.sprite = m_Sprites[5];
                                    _ShadowNode.sprite = m_Sprites[5];
                                }
                                else if (_PreviousNodePos.x == -1)
                                {
                                    _Node.sprite = m_Sprites[21];
                                    _ShadowNode.sprite = m_Sprites[21];
                                }
                                else
                                {
                                    _Node.sprite = m_Sprites[13];
                                    _ShadowNode.sprite = m_Sprites[13];
                                }
                            }
                            else
                            {
                                if (_PreviousNodePos.x == 1)
                                {
                                    _Node.sprite = m_Sprites[17];
                                    _ShadowNode.sprite = m_Sprites[17];
                                }
                                else if (_PreviousNodePos.x == -1)
                                {
                                    _Node.sprite = m_Sprites[1];
                                    _ShadowNode.sprite = m_Sprites[1];
                                }
                                else
                                {
                                    _Node.sprite = m_Sprites[9];
                                    _ShadowNode.sprite = m_Sprites[9];
                                }
                            }
                        }

                        m_PathNodes.Add(_Node);
                        m_PathShadowNodes.Add(_ShadowNode);
                    }
                }
                else
                {
                    Debug.Log($"@@@@@@@@@@@@@@@@@@ NULL");
                }
            }
        }
    }
}
