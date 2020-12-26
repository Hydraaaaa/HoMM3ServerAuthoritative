using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public class Node
    {
        public int PosX;
        public int PosY;

        public int GCost;
        public int HCost;
        public int FCost { get { return GCost + HCost; } }

        public TerrainTile Tile;

        public Node ParentNode;

        public List<ScenarioObject> BlockingObjects = new List<ScenarioObject>();
        public List<ScenarioObject> InteractionObjects = new List<ScenarioObject>();

        public List<(Node Node, int Cost)> Pathways = new List<(Node, int)>();
    }

    Scenario m_Scenario;

    public List<Node> m_OverworldNodes;
    public List<Node> m_UndergroundNodes;

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < m_OverworldNodes.Count; i++)
            {
                if (m_OverworldNodes[i].BlockingObjects.Count == 0 ||
                    m_OverworldNodes[i].InteractionObjects.Count > 0)
                {
                    for (int j = 0; j < m_OverworldNodes[i].Pathways.Count; j++)
                    {
                        Node _DestinationNode = m_OverworldNodes[i].Pathways[j].Node;

                        if (_DestinationNode.InteractionObjects.Count > 0 ||
                            m_OverworldNodes[i].InteractionObjects.Count > 0)
                        {
                            Gizmos.color = Color.yellow;
                        }
                        else
                        {
                            Gizmos.color = Color.white;
                        }

                        if (_DestinationNode.BlockingObjects.Count == 0 ||
                            _DestinationNode.InteractionObjects.Count > 0)
                        {
                            if (m_Scenario.Terrain[i].TerrainType == 8)
                            {
                                if (m_Scenario.Terrain[_DestinationNode.PosX + _DestinationNode.PosY * m_Scenario.Size].TerrainType == 8)
                                {

                                    Gizmos.DrawLine(new Vector3(m_OverworldNodes[i].PosX, -m_OverworldNodes[i].PosY), new Vector3(_DestinationNode.PosX, -_DestinationNode.PosY));
                                }
                            }
                            else
                            {
                                if (m_Scenario.Terrain[_DestinationNode.PosX + _DestinationNode.PosY * m_Scenario.Size].TerrainType != 8)
                                {
                                    Gizmos.DrawLine(new Vector3(m_OverworldNodes[i].PosX, -m_OverworldNodes[i].PosY), new Vector3(_DestinationNode.PosX, -_DestinationNode.PosY));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Generate(Scenario a_Scenario)
    {
        m_Scenario = a_Scenario;

        m_OverworldNodes = GeneratePathways(a_Scenario.Size, a_Scenario.Terrain);

        if (a_Scenario.HasUnderground)
        {
            m_UndergroundNodes = GeneratePathways(a_Scenario.Size, a_Scenario.UndergroundTerrain);
        }

        for (int i = 0; i < a_Scenario.Objects.Count; i++)
        {
            if (!a_Scenario.Objects[i].IsUnderground)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (a_Scenario.Objects[i].Template.Passability[y] != 0)
                    {
                        if ((a_Scenario.Objects[i].Template.Passability[y] & 1) == 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 7;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].BlockingObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Passability[y] & 2) == 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 6;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].BlockingObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Passability[y] & 4) == 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 5;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].BlockingObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Passability[y] & 8) == 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 4;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].BlockingObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Passability[y] & 16) == 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 3;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].BlockingObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Passability[y] & 32) == 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 2;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].BlockingObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Passability[y] & 64) == 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 1;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].BlockingObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Passability[y] & 128) == 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].BlockingObjects.Add(a_Scenario.Objects[i]);
                            }
                        }
                    }

                    if (a_Scenario.Objects[i].Template.Interactability[y] != 255)
                    {
                        if ((a_Scenario.Objects[i].Template.Interactability[y] & 1) != 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 7;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].InteractionObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Interactability[y] & 2) != 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 6;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].InteractionObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Interactability[y] & 4) != 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 5;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].InteractionObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Interactability[y] & 8) != 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 4;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].InteractionObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Interactability[y] & 16) != 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 3;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].InteractionObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Interactability[y] & 32) != 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 2;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].InteractionObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Interactability[y] & 64) != 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX - 1;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].InteractionObjects.Add(a_Scenario.Objects[i]);
                            }
                        }

                        if ((a_Scenario.Objects[i].Template.Interactability[y] & 128) != 0)
                        {
                            int _X = a_Scenario.Objects[i].PosX;
                            int _Y = a_Scenario.Objects[i].PosY + y - 5;

                            if (_X >= 0 &&
                                _Y >= 0 &&
                                _X < a_Scenario.Size &&
                                _Y < a_Scenario.Size)
                            {
                                m_OverworldNodes[_X + _Y * a_Scenario.Size].InteractionObjects.Add(a_Scenario.Objects[i]);
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

    public List<Node> GetPath(Vector2Int a_StartPos, Vector2Int a_EndPos, bool a_IsUnderground = false)
    {
        int a_StartIndex = a_StartPos.x + Mathf.Abs(a_StartPos.y) * m_Scenario.Size;
        int a_EndIndex = a_EndPos.x + Mathf.Abs(a_EndPos.y) * m_Scenario.Size;

        Node _StartNode;
        Node _EndNode;

        if (!a_IsUnderground)
        {
            _StartNode = m_OverworldNodes[a_StartIndex];
            _EndNode = m_OverworldNodes[a_EndIndex];
        }
        else
        {
            _StartNode = m_UndergroundNodes[a_StartIndex];
            _EndNode = m_UndergroundNodes[a_EndIndex];
        }

        List<Node> _OpenList = new List<Node>();
        HashSet<Node> _ClosedList = new HashSet<Node>();

        _OpenList.Add(_StartNode);

        // Added this myself, does he address this later?
        _StartNode.GCost = 0;

        Node _CurrentNode = null;

        bool _Success = false;

        while (_OpenList.Count > 0)
        {
            // Could this be optimized to keep track of index and RemoveAt?
            _CurrentNode = _OpenList[0];

            for (int i = 1; i < _OpenList.Count; i++)
            {
                if (_OpenList[i].FCost < _CurrentNode.FCost ||

                    _OpenList[i].FCost == _CurrentNode.FCost &&
                    _OpenList[i].HCost < _CurrentNode.HCost)
                {
                    _CurrentNode = _OpenList[i];
                }
            }

            _OpenList.Remove(_CurrentNode);
            _ClosedList.Add(_CurrentNode);

            if (_CurrentNode == _EndNode)
            {
                _Success = true;
                break;
            }

            for (int i = 0; i < _CurrentNode.Pathways.Count; i++)
            {
                Node _PathwayNode = _CurrentNode.Pathways[i].Node;

                if (_ClosedList.Contains(_PathwayNode) ||
                    _PathwayNode.BlockingObjects.Count > 0 &&
                    _PathwayNode.InteractionObjects.Count == 0 ||
                    _PathwayNode.InteractionObjects.Count > 0 &&
                    _PathwayNode != _EndNode ||
                    (_CurrentNode.Tile.TerrainType == 8 ||
                     _PathwayNode.Tile.TerrainType == 8) &&
                    _PathwayNode.Tile.TerrainType != _CurrentNode.Tile.TerrainType)
                {
                    continue;
                }

                int _NewMovementCost = _CurrentNode.GCost + _CurrentNode.Pathways[i].Cost;
                if (_NewMovementCost < _PathwayNode.GCost ||
                    !_OpenList.Contains(_PathwayNode))
                {
                    _PathwayNode.GCost = _NewMovementCost;

                    int _HCost;

                    int _HeuristicX = Mathf.Abs(_PathwayNode.PosX - _EndNode.PosX);
                    int _HeuristicY = Mathf.Abs(_PathwayNode.PosX - _EndNode.PosX);

                    if (_HeuristicX > _HeuristicY)
                    {
                        _HCost = 70 * _HeuristicY + 50 * (_HeuristicX - _HeuristicY);
                    }
                    else
                    {
                        _HCost = 70 * _HeuristicX + 50 * (_HeuristicY - _HeuristicX);
                    }

                    _PathwayNode.HCost = _HCost;

                    _PathwayNode.ParentNode = _CurrentNode;

                    if (!_OpenList.Contains(_PathwayNode))
                    {
                        _OpenList.Add(_PathwayNode);
                    }
                }
            }
        }

        if (!_Success)
        {
            return null;
        }

        List<Node> _OutputPath = new List<Node>();

        while (_CurrentNode != _StartNode)
        {
            _OutputPath.Add(_CurrentNode);
            _CurrentNode = _CurrentNode.ParentNode;
        }

        _OutputPath.Reverse();

        return _OutputPath;
    }
}
