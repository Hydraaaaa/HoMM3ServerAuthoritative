using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    // Incremented when the pathfinding area changes at all
    // Allows objects to keep track of if they're looking at the up-to-date grid
    public int PathfindingVersion;

    public class Node
    {
        public int PosX;
        public int PosY;

        public int Cost;

        public TerrainTile Tile;

        public Node ParentNode;

        public List<MapObjectBase> BlockingObjects = new List<MapObjectBase>();
        public List<MapObjectBase> InteractionObjects = new List<MapObjectBase>();

        public List<(Node Node, int Cost)> Pathways = new List<(Node, int)>();
    }

    public class PrecomputedNode
    {
        public readonly int PosX;
        public readonly int PosY;
        public readonly int Cost;

        public readonly PrecomputedNode Parent;

        public PrecomputedNode(int a_PosX, int a_PosY, int a_Cost, PrecomputedNode a_Parent)
        {
            PosX = a_PosX;
            PosY = a_PosY;
            Cost = a_Cost;
            Parent = a_Parent;
        }
    }

    Scenario m_Scenario;

    List<Node> m_OverworldNodes;
    List<Node> m_UndergroundNodes;

    //void OnDrawGizmos()
    //{
        //if (Application.isPlaying)
        //{
            //for (int i = 0; i < m_OverworldNodes.Count; i++)
            //{
                //if (m_OverworldNodes[i].BlockingObjects.Count == 0 ||
                    //m_OverworldNodes[i].InteractionObjects.Count > 0)
                //{
                    //for (int j = 0; j < m_OverworldNodes[i].Pathways.Count; j++)
                    //{
                        //Node _DestinationNode = m_OverworldNodes[i].Pathways[j].Node;

                        //if (_DestinationNode.InteractionObjects.Count > 0 ||
                            //m_OverworldNodes[i].InteractionObjects.Count > 0)
                        //{
                            //Gizmos.color = Color.yellow;
                        //}
                        //else
                        //{
                            //Gizmos.color = Color.white;
                        //}

                        //if (_DestinationNode.BlockingObjects.Count == 0 ||
                            //_DestinationNode.InteractionObjects.Count > 0)
                        //{
                            //if (m_Scenario.Terrain[i].TerrainType == 8)
                            //{
                                //if (m_Scenario.Terrain[_DestinationNode.PosX + _DestinationNode.PosY * m_Scenario.Size].TerrainType == 8)
                                //{
                                    //Gizmos.DrawLine(new Vector3(m_OverworldNodes[i].PosX, -m_OverworldNodes[i].PosY), new Vector3(_DestinationNode.PosX, -_DestinationNode.PosY));
                                //}
                            //}
                            //else
                            //{
                                //if (m_Scenario.Terrain[_DestinationNode.PosX + _DestinationNode.PosY * m_Scenario.Size].TerrainType != 8)
                                //{
                                    //Gizmos.DrawLine(new Vector3(m_OverworldNodes[i].PosX, -m_OverworldNodes[i].PosY), new Vector3(_DestinationNode.PosX, -_DestinationNode.PosY));
                                //}
                            //}
                        //}
                    //}
                //}
            //}
        //}
    //}

    public void Generate(Scenario a_Scenario, List<MapObjectBase> a_MapObjects, Dictionary<ScenarioObject, DynamicMapObstacle> a_DynamicObstacles)
    {
        PathfindingVersion = 0;

        m_Scenario = a_Scenario;

        m_OverworldNodes = GeneratePathways(a_Scenario.Size, a_Scenario.Terrain);

        if (a_Scenario.HasUnderground)
        {
            m_UndergroundNodes = GeneratePathways(a_Scenario.Size, a_Scenario.UndergroundTerrain);
        }

        for (int i = 0; i < a_Scenario.Objects.Count; i++)
        {
            List<Node> _Nodes;

            if (a_Scenario.Objects[i].IsUnderground)
            {
                _Nodes = m_UndergroundNodes;
            }
            else
            {
                _Nodes = m_OverworldNodes;
            }

            for (int y = 0; y < 6; y++)
            {
                if (a_Scenario.Objects[i].Template.Passability[y] != 0)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((a_Scenario.Objects[i].Template.Passability[y] & (1 << j)) == 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - (7 - j);
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                Node _Node = _Nodes[_X + _Y * a_Scenario.Size];

                                if (a_DynamicObstacles.ContainsKey(a_Scenario.Objects[i]))
                                {
                                    a_DynamicObstacles[a_Scenario.Objects[i]].AddBlockedNode(_Node);
                                }
                                else
                                {
                                    _Node.BlockingObjects.Add(a_MapObjects[i]);
                                }
                            }
                        }
                    }
                }

                if (a_Scenario.Objects[i].Template.Interactability[y] != 255)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((a_Scenario.Objects[i].Template.Interactability[y] & (1 << j)) != 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - (7 - j);
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                Node _Node = _Nodes[_X + _Y * a_Scenario.Size];

                                if (a_DynamicObstacles.ContainsKey(a_Scenario.Objects[i]))
                                {
                                    a_DynamicObstacles[a_Scenario.Objects[i]].AddInteractedNode(_Node);
                                }
                                else
                                {
                                    _Node.InteractionObjects.Add(a_MapObjects[i]);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    List<Node> GeneratePathways(int a_ScenarioSize, List<TerrainTile> a_Terrain)
    {
        List<Node> _OutputNodes = new List<Node>(a_ScenarioSize * a_ScenarioSize);

        for (int i = 0; i < _OutputNodes.Capacity; i++)
        {
            _OutputNodes.Add(new Node());
        }

        for (int x = 0; x < a_ScenarioSize; x++)
        {
            for (int y = 0; y < a_ScenarioSize; y++)
            {
                int _Index = x + y * a_ScenarioSize;

                Node _CurrentNode = _OutputNodes[_Index];

                _CurrentNode.PosX = x;
                _CurrentNode.PosY = y;
                _CurrentNode.Tile = a_Terrain[_Index];

                if (x > 0)
                {
                    if (y > 0)
                    {
                        // Top Left
                        _CurrentNode.Pathways.Add
                        (
                            (_OutputNodes[(x - 1) + (y - 1) * a_ScenarioSize],
                            GetPathwayCost(_Index, (x - 1) + (y - 1) * a_ScenarioSize, a_Terrain, true))
                        );
                    }

                    if (y < a_ScenarioSize - 1)
                    {
                        // Bottom Left
                        _CurrentNode.Pathways.Add
                        (
                            (_OutputNodes[(x - 1) + (y + 1) * a_ScenarioSize],
                            GetPathwayCost(_Index, (x - 1) + (y + 1) * a_ScenarioSize, a_Terrain, true))
                        );
                    }

                    // Left
                    _CurrentNode.Pathways.Add
                    (
                        (_OutputNodes[(x - 1) + y * a_ScenarioSize],
                        GetPathwayCost(_Index, (x - 1) + y * a_ScenarioSize, a_Terrain, false))
                    );
                }

                if (x < a_ScenarioSize - 1)
                {
                    if (y > 0)
                    {
                        // Top Right
                        _CurrentNode.Pathways.Add
                        (
                            (_OutputNodes[(x + 1) + (y - 1) * a_ScenarioSize],
                            GetPathwayCost(_Index, (x + 1) + (y - 1) * a_ScenarioSize, a_Terrain, true))
                        );
                    }

                    if (y < a_ScenarioSize - 1)
                    {
                        // Bottom Right
                        _CurrentNode.Pathways.Add
                        (
                            (_OutputNodes[(x + 1) + (y + 1) * a_ScenarioSize],
                            GetPathwayCost(_Index, (x + 1) + (y + 1) * a_ScenarioSize, a_Terrain, true))
                        );
                    }

                    // Right
                    _CurrentNode.Pathways.Add
                    (
                        (_OutputNodes[(x + 1) + y * a_ScenarioSize],
                        GetPathwayCost(_Index, (x + 1) + y * a_ScenarioSize, a_Terrain, false))
                    );
                }

                if (y > 0)
                {
                    // Top
                    _CurrentNode.Pathways.Add
                    (
                        (_OutputNodes[x + (y - 1) * a_ScenarioSize],
                        GetPathwayCost(_Index, x + (y - 1) * a_ScenarioSize, a_Terrain, false))
                    );
                }

                if (y < a_ScenarioSize - 1)
                {
                    // Bottom
                    _CurrentNode.Pathways.Add
                    (
                        (_OutputNodes[x + (y + 1) * a_ScenarioSize],
                        GetPathwayCost(_Index, x + (y + 1) * a_ScenarioSize, a_Terrain, false))
                    );
                }
            }
        }

        return _OutputNodes;
    }

    int GetPathwayCost(int a_StartIndex, int a_DestinationIndex, List<TerrainTile> a_Terrain, bool a_Diagonal)
    {
        int _CostTileIndex = a_StartIndex;

        // If the tiles aren't both road, and the destination
        if (a_Terrain[a_DestinationIndex].RoadType == 0)
        {
            if (a_Terrain[a_StartIndex].RoadType != 0)
            {
                _CostTileIndex = a_DestinationIndex;
            }
        }

        switch (a_Terrain[_CostTileIndex].RoadType)
        {
            case 1: return a_Diagonal ? 106 : 75; // Dirt
            case 2: return a_Diagonal ? 91 : 65;  // Gravel
            case 3: return a_Diagonal ? 70 : 50;  // Cobblestone

            // No Road
            default:
                switch(a_Terrain[_CostTileIndex].TerrainType)
                {
                    case 5: return a_Diagonal ? 176 : 125; // Rough

                    case 1:                                // Sand
                    case 3: return a_Diagonal ? 212 : 150; // Snow

                    case 4: return a_Diagonal ? 247 : 175; // Swamp

                    default: return a_Diagonal ? 141 : 100;
                }
        }
    }

    public Dictionary<int, PrecomputedNode> GetPathableArea(Vector2Int a_StartPos, bool a_IsUnderground = false)
    {
        int a_StartIndex = a_StartPos.x + Mathf.Abs(a_StartPos.y) * m_Scenario.Size;

        Node _StartNode;

        if (!a_IsUnderground)
        {
            _StartNode = m_OverworldNodes[a_StartIndex];
        }
        else
        {
            _StartNode = m_UndergroundNodes[a_StartIndex];
        }

        List<Node> _OpenList = new List<Node>();
        HashSet<Node> _ClosedList = new HashSet<Node>();
        Dictionary<Node, PrecomputedNode> _PrecomputedNodes = new Dictionary<Node, PrecomputedNode>();

        _OpenList.Add(_StartNode);
        _PrecomputedNodes.Add(_StartNode, new PrecomputedNode(_StartNode.PosX, _StartNode.PosY, 0, null));

        // Added this myself, does he address this later?
        _StartNode.Cost = 0;

        Node _CurrentNode = null;

        // The first node is an interactable, which would normally block the path
        // This flag prevents that
        bool _FirstNode = true;

        while (_OpenList.Count > 0)
        {
            // Could this be optimized to keep track of index and RemoveAt?
            _CurrentNode = _OpenList[0];

            for (int i = 1; i < _OpenList.Count; i++)
            {
                if (_OpenList[i].Cost < _CurrentNode.Cost)
                {
                    _CurrentNode = _OpenList[i];
                }
            }

            _OpenList.Remove(_CurrentNode);
            _ClosedList.Add(_CurrentNode);

            if (_CurrentNode.InteractionObjects.Count > 0 &&
                !_FirstNode)
            {
                continue;
            }

            _FirstNode = false;

            for (int i = 0; i < _CurrentNode.Pathways.Count; i++)
            {
                Node _PathwayNode = _CurrentNode.Pathways[i].Node;

                if (_ClosedList.Contains(_PathwayNode) ||
                    _PathwayNode.BlockingObjects.Count > 0 &&
                    _PathwayNode.InteractionObjects.Count == 0 ||
                    (_CurrentNode.Tile.TerrainType == 8 ||
                     _PathwayNode.Tile.TerrainType == 8) &&
                    _PathwayNode.Tile.TerrainType != _CurrentNode.Tile.TerrainType)
                {
                    continue;
                }

                int _NewMovementCost = _CurrentNode.Cost + _CurrentNode.Pathways[i].Cost;
                if (_NewMovementCost < _PathwayNode.Cost ||
                    !_OpenList.Contains(_PathwayNode))
                {
                    _PathwayNode.Cost = _NewMovementCost;

                    _PathwayNode.ParentNode = _CurrentNode;

                    if (!_OpenList.Contains(_PathwayNode))
                    {
                        _OpenList.Add(_PathwayNode);
                        _PrecomputedNodes.Add(_PathwayNode, new PrecomputedNode(_PathwayNode.PosX, _PathwayNode.PosY, _PathwayNode.Cost, _PrecomputedNodes[_CurrentNode]));
                    }
                }
            }
        }

        Dictionary<int, PrecomputedNode> _OutputNodes = new Dictionary<int, PrecomputedNode>();

        foreach (var _NodePair in _PrecomputedNodes)
        {
            _OutputNodes.Add(_NodePair.Key.PosX + _NodePair.Key.PosY * m_Scenario.Size, _NodePair.Value);
        }

        return _OutputNodes;
    }

    public Node GetNode(Vector2Int a_Pos, bool a_IsUnderground)
    {
        return GetNode(a_Pos.x, a_Pos.y, a_IsUnderground);
    }

    public Node GetNode(int a_PosX, int a_PosY, bool a_IsUnderground)
    {
        if (a_IsUnderground)
        {
            return m_UndergroundNodes[a_PosX + a_PosY * m_Scenario.Size];
        }
        else
        {
            return m_OverworldNodes[a_PosX + a_PosY * m_Scenario.Size];
        }
    }
}
