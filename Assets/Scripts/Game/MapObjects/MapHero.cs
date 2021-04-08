using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapHero : MapObjectBase
{
    const int DIRECTION_E = 0;
    const int DIRECTION_SE = 1;
    const int DIRECTION_S = 2;
    const int DIRECTION_SW = 3;
    const int DIRECTION_W = 4;
    const int DIRECTION_NW = 5;
    const int DIRECTION_N = 6;
    const int DIRECTION_NE = 7;

    public Hero Hero { get; private set; }
    public int PlayerIndex { get; private set; }
    public bool IsUnderground { get; private set; }
    public bool IsPrison { get; private set; }

    public DynamicMapObstacle DynamicObstacle => m_DynamicObstacle;

    [SerializeField] SpriteRenderer m_HeroRenderer;
    [SerializeField] SpriteRenderer m_HeroShadowRenderer;
    [SerializeField] MapObjectRenderer m_FlagRenderer;
    [SerializeField] SpriteRenderer m_FlagSpriteRenderer;
    [SerializeField] DynamicMapObstacle m_DynamicObstacle;
    [SerializeField] PlayerColors m_PlayerColors;
    [SerializeField] HeroList m_Heroes;
    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] MapObjectVisualData m_PrisonVisualData;

    [Space]

    [SerializeField] SpriteRenderer m_PathNodePrefab;
    [SerializeField] SpriteRenderer m_PathNodeShadowPrefab;
    [SerializeField] Sprite[] m_PathSprites;

    Pathfinding m_Pathfinding;
    OwnedHeroes m_OwnedHeroes;

    int m_Direction = DIRECTION_E;

    int m_LocalPathfindingVersion;

    Dictionary<int, Pathfinding.PrecomputedNode> m_PathableArea;

    int m_TargetNodeIndex;

    List<SpriteRenderer> m_PathNodes = new List<SpriteRenderer>();
    List<SpriteRenderer> m_PathShadowNodes = new List<SpriteRenderer>();

    public void Initialize(Hero a_Hero, int a_PlayerIndex, int a_PosX, int a_PosY, bool a_IsUnderground, Pathfinding a_Pathfinding, OwnedHeroes a_OwnedHeroes)
    {
        MouseCollision = new byte[6];
        MouseCollision[0] = 0b00000000;
        MouseCollision[1] = 0b00000000;
        MouseCollision[2] = 0b00000000;
        MouseCollision[3] = 0b00000000;
        MouseCollision[4] = 0b00000000;
        MouseCollision[5] = 0b01000000;

        InteractionCollision = new byte[6];
        InteractionCollision[0] = 0b00000000;
        InteractionCollision[1] = 0b00000000;
        InteractionCollision[2] = 0b00000000;
        InteractionCollision[3] = 0b00000000;
        InteractionCollision[4] = 0b00000000;
        InteractionCollision[5] = 0b01000000;

        Hero = a_Hero;
        PlayerIndex = a_PlayerIndex;
        IsUnderground = a_IsUnderground;

        m_Pathfinding = a_Pathfinding;
        m_OwnedHeroes = a_OwnedHeroes;

        transform.position = new Vector3(a_PosX + 0.5f, -a_PosY - 0.5f, 0);

        m_DynamicObstacle.Initialize(a_Pathfinding);
        m_DynamicObstacle.AddInteractedNode(a_PosX, a_PosY, a_IsUnderground);

        Initialize();
    }

    public void Initialize(ScenarioObject a_ScenarioObject, Pathfinding a_Pathfinding, OwnedHeroes a_OwnedHeroes)
    {
        PlayerIndex = a_ScenarioObject.Hero.PlayerIndex;

        m_Pathfinding = a_Pathfinding;
        m_OwnedHeroes = a_OwnedHeroes;

        if (a_ScenarioObject.Template.Name == "avxprsn0")
        {
            IsPrison = true;
        }

        Hero _BaseHero;

        bool _ClaimedMainHero = false;

        if (a_ScenarioObject.Hero.ID != 255)
        {
            _BaseHero = m_Heroes.Heroes.First((a_Hero) => a_Hero.Hero.ID == a_ScenarioObject.Hero.ID).Hero;
        }
        else
        {
            GameSettings.Player _Player = m_GameSettings.Players.First((a_Player) => a_Player.Index == PlayerIndex);

            if (m_GameSettings.Scenario.PlayerInfo[PlayerIndex].IsMainHeroRandom &&
                _Player.SetMapHero)
            {
                _BaseHero = _Player.Hero;
                _Player.SetMapHero = false;
                _ClaimedMainHero = true;
            }
            else
            {
                _BaseHero = HeroPool.GetRandomHero(PlayerIndex, m_GameSettings.Players.First((a_Player) => a_Player.Index == PlayerIndex).Faction, true);
            }
        }

        Hero = new Hero();

        Hero.ID = _BaseHero.ID;
        Hero.Faction = _BaseHero.Faction;
        Hero.HeroVisualData = _BaseHero.HeroVisualData;

        if (a_ScenarioObject.Hero.Name != "")
        {
            Hero.Name = a_ScenarioObject.Hero.Name;
        }
        else
        {
            Hero.Name = _BaseHero.Name;
        }

        if (a_ScenarioObject.Hero.Portrait != 255)
        {
            Hero.Portrait = m_Heroes.Heroes.First((a_Hero) => a_Hero.Hero.ID == a_ScenarioObject.Hero.Portrait).Hero.Portrait;
        }
        else
        {
            Hero.Portrait = _BaseHero.Portrait;
        }

        if (!_ClaimedMainHero)
        {
            HeroPool.ClaimHero(Hero);
        }

        m_DynamicObstacle.Initialize(a_Pathfinding);

        Initialize();
    }

    void Initialize()
    {
        gameObject.name = Hero.Name;

        if (!IsPrison)
        {
            m_HeroRenderer.sprite = Hero.HeroVisualData.IdleSprites[0];
            m_HeroShadowRenderer.sprite = Hero.HeroVisualData.ShadowIdleSprites[0];

            HeroFlagVisualData _FlagData = m_PlayerColors.Flags[PlayerIndex];

            m_FlagRenderer.SetSprites(_FlagData.IdleSprites);
            m_FlagRenderer.transform.localPosition = new Vector3(_FlagData.IdleOffsets[m_Direction].x, _FlagData.IdleOffsets[m_Direction].y, 0);
        }
        else
        {
            m_HeroRenderer.sprite = m_PrisonVisualData.m_Sprites[0];
            m_HeroShadowRenderer.sprite = m_PrisonVisualData.m_ShadowSprites[0];
        }

        if (PlayerIndex == m_GameSettings.LocalPlayerIndex)
        {
            m_OwnedHeroes.AddHero(this);
        }

        m_LocalPathfindingVersion = -1;
    }

    public void OnSelected()
    {
        if (m_LocalPathfindingVersion != Pathfinding.PathfindingVersion)
        {
            m_PathableArea = m_Pathfinding.GetPathableArea(new Vector2Int((int)transform.position.x - 1, (int)transform.position.y), IsUnderground);

            GeneratePath(m_TargetNodeIndex);
        }
        else
        {
            for (int i = 0; i < m_PathNodes.Count; i++)
            {
                m_PathNodes[i].gameObject.SetActive(true);
                m_PathShadowNodes[i].gameObject.SetActive(true);
            }
        }
    }

    public void OnDeselected()
    {
        for (int i = 0; i < m_PathNodes.Count; i++)
        {
            m_PathNodes[i].gameObject.SetActive(false);
            m_PathShadowNodes[i].gameObject.SetActive(false);
        }
    }

    public int GetPathingTurnCost(int a_PosX, int a_PosY)
    {
        int _Index = a_PosX + a_PosY * m_GameSettings.Scenario.Size;

        if (m_PathableArea.ContainsKey(_Index))
        {
            return 1 + m_PathableArea[_Index].Cost / 1500;
        }
        else
        {
            return 0;
        }
    }

    public Vector2Int GetTargetDestination()
    {
        if (m_PathableArea.ContainsKey(m_TargetNodeIndex))
        {
            return new Vector2Int
            (
                m_PathableArea[m_TargetNodeIndex].PosX,
                m_PathableArea[m_TargetNodeIndex].PosY
            );
        }

        return new Vector2Int(-1, -1);
    }

    public void OnLeftClick(int a_XPos, int a_YPos, List<MapObjectBase> a_Objects)
    {
        int _Index = a_XPos + a_YPos * m_GameSettings.Scenario.Size;

        if (_Index != m_TargetNodeIndex)
        {
            if (m_PathableArea.ContainsKey(_Index))
            {
                GeneratePath(_Index);
            }
        }
        else if (_Index != -1)
        {
            // Move
        }
    }

    void GeneratePath(int a_Index)
    {
        for (int i = 0; i < m_PathNodes.Count; i++)
        {
            Destroy(m_PathNodes[i].gameObject);
            Destroy(m_PathShadowNodes[i].gameObject);
        }

        m_PathNodes.Clear();
        m_PathShadowNodes.Clear();

        if (!m_PathableArea.ContainsKey(a_Index))
        {
            m_TargetNodeIndex = -1;
            return;
        }

        Pathfinding.PrecomputedNode _PreviousNode = null;
        Pathfinding.PrecomputedNode _Node = m_PathableArea[a_Index];

        while (_Node.Parent != null)
        {
            Vector3 _Position = new Vector3(_Node.PosX, -_Node.PosY, 0);
            SpriteRenderer _NodeVisual = Instantiate(m_PathNodePrefab, _Position, Quaternion.identity);
            SpriteRenderer _NodeShadowVisual = Instantiate(m_PathNodeShadowPrefab, _Position, Quaternion.identity);

            if (_PreviousNode == null)
            {
                _NodeVisual.sprite = m_PathSprites[0];
                _NodeShadowVisual.sprite = m_PathSprites[0];

                m_PathNodes.Add(_NodeVisual);
                m_PathShadowNodes.Add(_NodeShadowVisual);

                _PreviousNode = _Node;
                _Node = _Node.Parent;

                continue;
            }

            Vector2Int _PreviousNodePos = new Vector2Int(_PreviousNode.PosX - _Node.PosX, _PreviousNode.PosY - _Node.PosY);
            Vector2Int _NextNodePos = new Vector2Int(_Node.Parent.PosX - _Node.PosX, _Node.Parent.PosY - _Node.PosY);

            if (_PreviousNodePos.x == -1 &&
                _PreviousNodePos.y == -1)
            {
                if (_NextNodePos.x == 1 &&
                    _NextNodePos.y == 1)
                {
                    _NodeVisual.sprite = m_PathSprites[16];
                    _NodeShadowVisual.sprite = m_PathSprites[16];
                }
                else
                {
                    if (_NextNodePos.x == -1)
                    {
                        _NodeVisual.sprite = m_PathSprites[8];
                        _NodeShadowVisual.sprite = m_PathSprites[8];
                    }
                    else if (_NextNodePos.y == -1)
                    {
                        _NodeVisual.sprite = m_PathSprites[24];
                        _NodeShadowVisual.sprite = m_PathSprites[24];
                    }
                    else if (_NextNodePos.x == 1)
                    {
                        _NodeVisual.sprite = m_PathSprites[24];
                        _NodeShadowVisual.sprite = m_PathSprites[24];
                    }
                    else
                    {
                        _NodeVisual.sprite = m_PathSprites[8];
                        _NodeShadowVisual.sprite = m_PathSprites[8];
                    }
                }
            }
            else if (_PreviousNodePos.x == 1 &&
                     _PreviousNodePos.y == 1)
            {
                if (_NextNodePos.x == -1 &&
                    _NextNodePos.y == -1)
                {
                    _NodeVisual.sprite = m_PathSprites[12];
                    _NodeShadowVisual.sprite = m_PathSprites[12];
                }
                else
                {
                    if (_NextNodePos.x == 1)
                    {
                        _NodeVisual.sprite = m_PathSprites[4];
                        _NodeShadowVisual.sprite = m_PathSprites[4];
                    }
                    else if (_NextNodePos.y == 1)
                    {
                        _NodeVisual.sprite = m_PathSprites[20];
                        _NodeShadowVisual.sprite = m_PathSprites[20];
                    }
                    else if (_NextNodePos.x == -1)
                    {
                        _NodeVisual.sprite = m_PathSprites[20];
                        _NodeShadowVisual.sprite = m_PathSprites[20];
                    }
                    else
                    {
                        _NodeVisual.sprite = m_PathSprites[4];
                        _NodeShadowVisual.sprite = m_PathSprites[4];
                    }
                }
            }
            else if (_PreviousNodePos.x == -1 &&
                     _PreviousNodePos.y == 1)
            {
                if (_NextNodePos.x == 1 &&
                    _NextNodePos.y == -1)
                {
                    _NodeVisual.sprite = m_PathSprites[14];
                    _NodeShadowVisual.sprite = m_PathSprites[14];
                }
                else
                {
                    if (_NextNodePos.x == -1)
                    {
                        _NodeVisual.sprite = m_PathSprites[22];
                        _NodeShadowVisual.sprite = m_PathSprites[22];
                    }
                    else if (_NextNodePos.y == 1)
                    {
                        _NodeVisual.sprite = m_PathSprites[6];
                        _NodeShadowVisual.sprite = m_PathSprites[6];
                    }
                    else if (_NextNodePos.x == 1)
                    {
                        _NodeVisual.sprite = m_PathSprites[6];
                        _NodeShadowVisual.sprite = m_PathSprites[6];
                    }
                    else
                    {
                        _NodeVisual.sprite = m_PathSprites[22];
                        _NodeShadowVisual.sprite = m_PathSprites[22];
                    }
                }
            }
            else if (_PreviousNodePos.x == 1 &&
                     _PreviousNodePos.y == -1)
            {
                if (_NextNodePos.x == -1 &&
                    _NextNodePos.y == 1)
                {
                    _NodeVisual.sprite = m_PathSprites[10];
                    _NodeShadowVisual.sprite = m_PathSprites[10];
                }
                else
                {
                    if (_NextNodePos.x == 1)
                    {
                        _NodeVisual.sprite = m_PathSprites[18];
                        _NodeShadowVisual.sprite = m_PathSprites[18];
                    }
                    else if (_NextNodePos.y == -1)
                    {
                        _NodeVisual.sprite = m_PathSprites[2];
                        _NodeShadowVisual.sprite = m_PathSprites[2];
                    }
                    else if (_NextNodePos.x == -1)
                    {
                        _NodeVisual.sprite = m_PathSprites[2];
                        _NodeShadowVisual.sprite = m_PathSprites[2];
                    }
                    else
                    {
                        _NodeVisual.sprite = m_PathSprites[18];
                        _NodeShadowVisual.sprite = m_PathSprites[18];
                    }
                }
            }
            else if (_PreviousNodePos.x == 1)
            {
                if (_NextNodePos.y == 1)
                {
                    _NodeVisual.sprite = m_PathSprites[19];
                    _NodeShadowVisual.sprite = m_PathSprites[19];
                }
                else if (_NextNodePos.y == -1)
                {
                    _NodeVisual.sprite = m_PathSprites[3];
                    _NodeShadowVisual.sprite = m_PathSprites[3];
                }
                else
                {
                    _NodeVisual.sprite = m_PathSprites[11];
                    _NodeShadowVisual.sprite = m_PathSprites[11];
                }
            }
            else if (_PreviousNodePos.x == -1)
            {
                if (_NextNodePos.y == 1)
                {
                    _NodeVisual.sprite = m_PathSprites[7];
                    _NodeShadowVisual.sprite = m_PathSprites[7];
                }
                else if (_NextNodePos.y == -1)
                {
                    _NodeVisual.sprite = m_PathSprites[23];
                    _NodeShadowVisual.sprite = m_PathSprites[23];
                }
                else
                {
                    _NodeVisual.sprite = m_PathSprites[15];
                    _NodeShadowVisual.sprite = m_PathSprites[15];
                }
            }
            else if (_PreviousNodePos.y == 1)
            {
                if (_NextNodePos.x == 1)
                {
                    _NodeVisual.sprite = m_PathSprites[5];
                    _NodeShadowVisual.sprite = m_PathSprites[5];
                }
                else if (_NextNodePos.x == -1)
                {
                    _NodeVisual.sprite = m_PathSprites[21];
                    _NodeShadowVisual.sprite = m_PathSprites[21];
                }
                else
                {
                    _NodeVisual.sprite = m_PathSprites[13];
                    _NodeShadowVisual.sprite = m_PathSprites[13];
                }
            }
            else
            {
                if (_NextNodePos.x == 1)
                {
                    _NodeVisual.sprite = m_PathSprites[17];
                    _NodeShadowVisual.sprite = m_PathSprites[17];
                }
                else if (_NextNodePos.x == -1)
                {
                    _NodeVisual.sprite = m_PathSprites[1];
                    _NodeShadowVisual.sprite = m_PathSprites[1];
                }
                else
                {
                    _NodeVisual.sprite = m_PathSprites[9];
                    _NodeShadowVisual.sprite = m_PathSprites[9];
                }
            }

            m_PathNodes.Add(_NodeVisual);
            m_PathShadowNodes.Add(_NodeShadowVisual);

            _PreviousNode = _Node;
            _Node = _Node.Parent;
        }

        m_TargetNodeIndex = a_Index;
    }
}
