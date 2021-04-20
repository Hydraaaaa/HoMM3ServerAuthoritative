using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReferences : MonoBehaviour
{
    public Map Map => m_Map;
    public Pathfinding Pathfinding => m_Pathfinding;
    public LocalOwnership LocalOwnership => m_LocalOwnership;
    public GameObject InputBlocker => m_InputBlocker;

    public GameSettings GameSettings => m_GameSettings;
    public PlayerColors PlayerColors => m_PlayerColors;

    [Header("Scene")]
    [SerializeField] Map m_Map;
    [SerializeField] Pathfinding m_Pathfinding;
    [SerializeField] LocalOwnership m_LocalOwnership;
    [SerializeField] GameObject m_InputBlocker;

    [Header("Project")]
    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] PlayerColors m_PlayerColors;
}
