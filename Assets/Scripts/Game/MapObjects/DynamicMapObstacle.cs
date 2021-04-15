using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMapObstacle : MonoBehaviour
{
    Pathfinding m_Pathfinding;
    MapObjectBase m_Object;

    List<Pathfinding.Node> m_BlockedNodes = new List<Pathfinding.Node>();
    List<Pathfinding.Node> m_InteractedNodes = new List<Pathfinding.Node>();

    public void Initialize(Pathfinding a_Pathfinding, MapObjectBase a_Object)
    {
        m_Pathfinding = a_Pathfinding;
        m_Object = a_Object;
    }

    public void AddBlockedNode(Pathfinding.Node a_Node)
    {
        m_BlockedNodes.Add(a_Node);

        a_Node.BlockingObjects.Add(m_Object);

        m_Pathfinding.PathfindingVersion++;
    }

    public void AddBlockedNode(int a_PosX, int a_PosY, bool a_IsUnderground)
    {
        Pathfinding.Node _Node = m_Pathfinding.GetNode(a_PosX, a_PosY, a_IsUnderground);

        m_BlockedNodes.Add(_Node);

        _Node.BlockingObjects.Add(m_Object);

        m_Pathfinding.PathfindingVersion++;
    }

    public void AddInteractedNode(Pathfinding.Node a_Node)
    {
        m_InteractedNodes.Add(a_Node);

        a_Node.InteractionObjects.Add(m_Object);

        m_Pathfinding.PathfindingVersion++;
    }

    public void AddInteractedNode(int a_PosX, int a_PosY, bool a_IsUnderground)
    {
        Pathfinding.Node _Node = m_Pathfinding.GetNode(a_PosX, a_PosY, a_IsUnderground);

        m_InteractedNodes.Add(_Node);

        _Node.InteractionObjects.Add(m_Object);

        m_Pathfinding.PathfindingVersion++;
    }

    public void ClearNodes()
    {
        for (int i = 0; i < m_BlockedNodes.Count; i++)
        {
            m_BlockedNodes[i].BlockingObjects.Remove(m_Object);
        }

        for (int i = 0; i < m_InteractedNodes.Count; i++)
        {
            m_InteractedNodes[i].InteractionObjects.Remove(m_Object);
        }

        m_BlockedNodes = new List<Pathfinding.Node>();
        m_InteractedNodes = new List<Pathfinding.Node>();

        m_Pathfinding.PathfindingVersion++;
    }
}
