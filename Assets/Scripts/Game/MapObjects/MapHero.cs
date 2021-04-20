using System;
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

    int m_Direction = DIRECTION_E;

    int m_LocalPathfindingVersion;

    Dictionary<int, Pathfinding.PrecomputedNode> m_PathableArea;

    List<Pathfinding.PrecomputedNode> m_Path = new List<Pathfinding.PrecomputedNode>();

    int m_TargetNodeIndex = -1;

    List<SpriteRenderer> m_PathNodes = new List<SpriteRenderer>();
    List<SpriteRenderer> m_PathNodeShadows = new List<SpriteRenderer>();

    public bool IsMoving { get; private set; }

    // Separate flag from IsMoving, because movement doesn't immediately stop when cancelled early
    // Camera needs to keep following until it stops for good
    bool m_StopMovement = false;

    Vector2Int m_PathfindingPos;

    public bool HasPath { get; private set; }
    public event Action OnPathCreated;
    public event Action OnPathRemoved;

    public void Initialize(Hero a_Hero, int a_PlayerIndex, int a_PosX, int a_PosY, bool a_IsUnderground, GameReferences a_GameReferences)
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

        m_GameReferences = a_GameReferences;

        transform.position = new Vector3(a_PosX + 0.5f, -a_PosY - 0.5f, 0);
        m_PathfindingPos = new Vector2Int(a_PosX - 1, a_PosY);

        m_DynamicObstacle.Initialize(m_GameReferences.Pathfinding, this);
        m_DynamicObstacle.AddInteractedNode(m_PathfindingPos.x, m_PathfindingPos.y, a_IsUnderground);

        Initialize();
    }

    public void Initialize(ScenarioObject a_ScenarioObject, GameReferences a_GameReferences)
    {
        PlayerIndex = a_ScenarioObject.Hero.PlayerIndex;

        m_GameReferences = a_GameReferences;

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

        m_DynamicObstacle.Initialize(m_GameReferences.Pathfinding, this);

        m_PathfindingPos = new Vector2Int(a_ScenarioObject.PosX - 1, a_ScenarioObject.PosY);

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
            m_FlagRenderer.transform.localPosition = new Vector3(Hero.HeroVisualData.IdleFlagOffsets[m_Direction].x, Hero.HeroVisualData.IdleFlagOffsets[m_Direction].y, 0);
        }
        else
        {
            m_HeroRenderer.sprite = m_PrisonVisualData.m_Sprites[0];
            m_HeroShadowRenderer.sprite = m_PrisonVisualData.m_ShadowSprites[0];
        }

        if (PlayerIndex == m_GameSettings.LocalPlayerIndex)
        {
            m_GameReferences.LocalOwnership.AddHero(this);
        }

        m_LocalPathfindingVersion = -1;
    }

    public void OnSelected()
    {
        if (m_LocalPathfindingVersion != m_GameReferences.Pathfinding.PathfindingVersion)
        {
            m_PathableArea = m_GameReferences.Pathfinding.GetPathableArea(m_PathfindingPos, IsUnderground);

            GeneratePath(m_TargetNodeIndex);
        }
        else
        {
            for (int i = 0; i < m_PathNodes.Count; i++)
            {
                m_PathNodes[i].gameObject.SetActive(true);
                m_PathNodeShadows[i].gameObject.SetActive(true);
            }
        }
    }

    public void OnDeselected()
    {
        for (int i = 0; i < m_PathNodes.Count; i++)
        {
            m_PathNodes[i].gameObject.SetActive(false);
            m_PathNodeShadows[i].gameObject.SetActive(false);
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

    public void OnLeftClick(int a_XPos, int a_YPos, MapObjectBase a_Object)
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
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        if (m_Path.Count == 0)
        {
            yield break;
        }

        m_StopMovement = false;

        IsMoving = true;

        m_FlagRenderer.enabled = false;

        HeroFlagVisualData _FlagData = m_PlayerColors.Flags[PlayerIndex];

        m_DynamicObstacle.ClearNodes();

        Pathfinding.PrecomputedNode _Node = m_Path[0];

        Destroy(m_PathNodes[0]);
        Destroy(m_PathNodeShadows[0]);

        m_PathNodes.RemoveAt(0);
        m_PathNodeShadows.RemoveAt(0);

        int _AnimationIndex = 0;
        int _Frame = 0;

        m_GameReferences.InputBlocker.SetActive(true);

        Vector3 _MovementDirection;

        void SetDirection()
        {
            _MovementDirection = new Vector3
            (
                m_Path[0].PosX - m_PathfindingPos.x,
                -(m_Path[0].PosY - m_PathfindingPos.y),
                0
            ) / 4;

            if (_MovementDirection.x > 0)
            {
                if (_MovementDirection.y > 0)
                {
                    m_Direction = DIRECTION_NE;
                }
                else if (_MovementDirection.y < 0)
                {
                    m_Direction = DIRECTION_SE;
                }
                else
                {
                    m_Direction = DIRECTION_E;
                }
            }
            else if (_MovementDirection.x < 0)
            {
                if (_MovementDirection.y > 0)
                {
                    m_Direction = DIRECTION_NW;
                }
                else if (_MovementDirection.y < 0)
                {
                    m_Direction = DIRECTION_SW;
                }
                else
                {
                    m_Direction = DIRECTION_W;
                }
            }
            else if (_MovementDirection.y > 0)
            {
                m_Direction = DIRECTION_N;
            }
            else
            {
                m_Direction = DIRECTION_S;
            }
        }

        SetDirection();

        // We don't check IsMoving here, because when cancelled, we still want to finish walking to the next tile
        while (true)
        {
            if (_Frame == 4)
            {
                _Frame = 0;

                m_PathfindingPos = new Vector2Int(_Node.PosX, _Node.PosY);
                m_Path.RemoveAt(0);

                if (m_Path.Count == 0 ||
                    m_StopMovement)
                {
                    break;
                }

                Destroy(m_PathNodes[0]);
                Destroy(m_PathNodeShadows[0]);

                m_PathNodes.RemoveAt(0);
                m_PathNodeShadows.RemoveAt(0);

                _Node = m_Path[0];

                SetDirection();
            }

            m_HeroRenderer.sprite = Hero.HeroVisualData.MovingSprites[m_Direction].Array[_AnimationIndex];
            m_HeroShadowRenderer.sprite = Hero.HeroVisualData.ShadowMovingSprites[m_Direction].Array[_AnimationIndex];
            m_FlagSpriteRenderer.sprite = _FlagData.MovingSprites[m_Direction].Array[_AnimationIndex];

            m_HeroRenderer.flipX = HeroVisualData.SPRITES_FLIPPED[m_Direction];
            m_HeroShadowRenderer.flipX = HeroVisualData.SPRITES_FLIPPED[m_Direction];
            m_FlagSpriteRenderer.flipX = HeroVisualData.SPRITES_FLIPPED[m_Direction];

            if (m_HeroRenderer.flipX)
            {
                m_HeroRenderer.transform.localPosition = new Vector3(-3, 0, 0);
                m_HeroShadowRenderer.transform.localPosition = new Vector3(-3, 0, 0);
                m_FlagSpriteRenderer.transform.localPosition = new Vector2(-3, 0) + Hero.HeroVisualData.MovingFlagOffsets[m_Direction];
            }
            else
            {
                m_HeroRenderer.transform.localPosition = Vector3.zero;
                m_HeroShadowRenderer.transform.localPosition = Vector3.zero;
                m_FlagSpriteRenderer.transform.localPosition = Hero.HeroVisualData.MovingFlagOffsets[m_Direction];
            }

            transform.position += _MovementDirection;

            yield return new WaitForSeconds(0.05f);

            _AnimationIndex++;
            _Frame++;

            if (_AnimationIndex == 8)
            {
                _AnimationIndex = 0;
            }
        }

        m_GameReferences.InputBlocker.SetActive(false);

        m_FlagRenderer.enabled = true;

        m_HeroRenderer.sprite = Hero.HeroVisualData.IdleSprites[m_Direction];
        m_HeroShadowRenderer.sprite = Hero.HeroVisualData.ShadowIdleSprites[m_Direction];

        m_HeroRenderer.flipX = HeroVisualData.SPRITES_FLIPPED[m_Direction];
        m_HeroShadowRenderer.flipX = HeroVisualData.SPRITES_FLIPPED[m_Direction];
        m_FlagSpriteRenderer.flipX = HeroVisualData.SPRITES_FLIPPED[m_Direction];

        if (m_HeroRenderer.flipX)
        {
            m_HeroRenderer.transform.localPosition = new Vector3(-3, 0, 0);
            m_HeroShadowRenderer.transform.localPosition = new Vector3(-3, 0, 0);
            m_FlagSpriteRenderer.transform.localPosition = new Vector2(-3, 0) + Hero.HeroVisualData.IdleFlagOffsets[m_Direction];
        }
        else
        {
            m_HeroRenderer.transform.localPosition = Vector3.zero;
            m_HeroShadowRenderer.transform.localPosition = Vector3.zero;
            m_FlagSpriteRenderer.transform.localPosition = Hero.HeroVisualData.IdleFlagOffsets[m_Direction];
        }

        m_DynamicObstacle.AddInteractedNode(m_GameReferences.Pathfinding.GetNode(m_PathfindingPos, IsUnderground));

        m_PathableArea = m_GameReferences.Pathfinding.GetPathableArea(m_PathfindingPos, IsUnderground);

        if (m_Path.Count == 0)
        {
            m_TargetNodeIndex = -1;
            HasPath = false;
            OnPathRemoved?.Invoke();
        }

        IsMoving = false;
    }

    void GeneratePath(int a_Index)
    {
        for (int i = 0; i < m_PathNodes.Count; i++)
        {
            Destroy(m_PathNodes[i].gameObject);
            Destroy(m_PathNodeShadows[i].gameObject);
        }

        m_PathNodes.Clear();
        m_PathNodeShadows.Clear();

        if (!m_PathableArea.ContainsKey(a_Index))
        {
            m_TargetNodeIndex = -1;
            HasPath = false;
            OnPathRemoved?.Invoke();
            return;
        }

        Pathfinding.PrecomputedNode _CurrentNode = m_PathableArea[a_Index];

        m_Path.Clear();

        while (_CurrentNode.Parent != null)
        {
            m_Path.Add(_CurrentNode);

            _CurrentNode = _CurrentNode.Parent;
        }

        m_Path.Reverse();

        for (int i = 0; i < m_Path.Count; i++)
        {
            Vector3 _Position = new Vector3(m_Path[i].PosX, -m_Path[i].PosY, 0);
            SpriteRenderer _Node = Instantiate(m_PathNodePrefab, _Position, Quaternion.identity, transform.parent);
            SpriteRenderer _NodeShadow = Instantiate(m_PathNodeShadowPrefab, _Position, Quaternion.identity, transform.parent);

            Vector2Int _PreviousNodePos;
            Vector2Int _NextNodePos;

            if (i > 0)
            {
                _PreviousNodePos = new Vector2Int(m_Path[i - 1].PosX, m_Path[i - 1].PosY);
            }
            else
            {
                _PreviousNodePos = m_PathfindingPos;
                _PreviousNodePos.x = Mathf.Abs(_PreviousNodePos.x);
                _PreviousNodePos.y = Mathf.Abs(_PreviousNodePos.y);
            }

            _PreviousNodePos -= new Vector2Int(m_Path[i].PosX, m_Path[i].PosY);

            if (i == m_Path.Count - 1)
            {
                _Node.sprite = m_PathSprites[0];
                _NodeShadow.sprite = m_PathSprites[0];
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
                        _Node.sprite = m_PathSprites[16];
                        _NodeShadow.sprite = m_PathSprites[16];
                    }
                    else
                    {
                        if (_PreviousNodePos.x == -1)
                        {
                            _Node.sprite = m_PathSprites[8];
                            _NodeShadow.sprite = m_PathSprites[8];
                        }
                        else if (_PreviousNodePos.y == -1)
                        {
                            _Node.sprite = m_PathSprites[24];
                            _NodeShadow.sprite = m_PathSprites[24];
                        }
                        else if (_PreviousNodePos.x == 1)
                        {
                            _Node.sprite = m_PathSprites[24];
                            _NodeShadow.sprite = m_PathSprites[24];
                        }
                        else
                        {
                            _Node.sprite = m_PathSprites[8];
                            _NodeShadow.sprite = m_PathSprites[8];
                        }
                    }
                }
                else if (_NextNodePos.x == 1 &&
                         _NextNodePos.y == 1)
                {
                    if (_PreviousNodePos.x == -1 &&
                        _PreviousNodePos.y == -1)
                    {
                        _Node.sprite = m_PathSprites[12];
                        _NodeShadow.sprite = m_PathSprites[12];
                    }
                    else
                    {
                        if (_PreviousNodePos.x == 1)
                        {
                            _Node.sprite = m_PathSprites[4];
                            _NodeShadow.sprite = m_PathSprites[4];
                        }
                        else if (_PreviousNodePos.y == 1)
                        {
                            _Node.sprite = m_PathSprites[20];
                            _NodeShadow.sprite = m_PathSprites[20];
                        }
                        else if (_PreviousNodePos.x == -1)
                        {
                            _Node.sprite = m_PathSprites[20];
                            _NodeShadow.sprite = m_PathSprites[20];
                        }
                        else
                        {
                            _Node.sprite = m_PathSprites[4];
                            _NodeShadow.sprite = m_PathSprites[4];
                        }
                    }
                }
                else if (_NextNodePos.x == -1 &&
                         _NextNodePos.y == 1)
                {
                    if (_PreviousNodePos.x == 1 &&
                        _PreviousNodePos.y == -1)
                    {
                        _Node.sprite = m_PathSprites[14];
                        _NodeShadow.sprite = m_PathSprites[14];
                    }
                    else
                    {
                        if (_PreviousNodePos.x == -1)
                        {
                            _Node.sprite = m_PathSprites[22];
                            _NodeShadow.sprite = m_PathSprites[22];
                        }
                        else if (_PreviousNodePos.y == 1)
                        {
                            _Node.sprite = m_PathSprites[6];
                            _NodeShadow.sprite = m_PathSprites[6];
                        }
                        else if (_PreviousNodePos.x == 1)
                        {
                            _Node.sprite = m_PathSprites[6];
                            _NodeShadow.sprite = m_PathSprites[6];
                        }
                        else
                        {
                            _Node.sprite = m_PathSprites[22];
                            _NodeShadow.sprite = m_PathSprites[22];
                        }
                    }
                }
                else if (_NextNodePos.x == 1 &&
                         _NextNodePos.y == -1)
                {
                    if (_PreviousNodePos.x == -1 &&
                        _PreviousNodePos.y == 1)
                    {
                        _Node.sprite = m_PathSprites[10];
                        _NodeShadow.sprite = m_PathSprites[10];
                    }
                    else
                    {
                        if (_PreviousNodePos.x == 1)
                        {
                            _Node.sprite = m_PathSprites[18];
                            _NodeShadow.sprite = m_PathSprites[18];
                        }
                        else if (_PreviousNodePos.y == -1)
                        {
                            _Node.sprite = m_PathSprites[2];
                            _NodeShadow.sprite = m_PathSprites[2];
                        }
                        else if (_PreviousNodePos.x == -1)
                        {
                            _Node.sprite = m_PathSprites[2];
                            _NodeShadow.sprite = m_PathSprites[2];
                        }
                        else
                        {
                            _Node.sprite = m_PathSprites[18];
                            _NodeShadow.sprite = m_PathSprites[18];
                        }
                    }
                }
                else if (_NextNodePos.x == 1)
                {
                    if (_PreviousNodePos.y == 1)
                    {
                        _Node.sprite = m_PathSprites[19];
                        _NodeShadow.sprite = m_PathSprites[19];
                    }
                    else if (_PreviousNodePos.y == -1)
                    {
                        _Node.sprite = m_PathSprites[3];
                        _NodeShadow.sprite = m_PathSprites[3];
                    }
                    else
                    {
                        _Node.sprite = m_PathSprites[11];
                        _NodeShadow.sprite = m_PathSprites[11];
                    }
                }
                else if (_NextNodePos.x == -1)
                {
                    if (_PreviousNodePos.y == 1)
                    {
                        _Node.sprite = m_PathSprites[7];
                        _NodeShadow.sprite = m_PathSprites[7];
                    }
                    else if (_PreviousNodePos.y == -1)
                    {
                        _Node.sprite = m_PathSprites[23];
                        _NodeShadow.sprite = m_PathSprites[23];
                    }
                    else
                    {
                        _Node.sprite = m_PathSprites[15];
                        _NodeShadow.sprite = m_PathSprites[15];
                    }
                }
                else if (_NextNodePos.y == 1)
                {
                    if (_PreviousNodePos.x == 1)
                    {
                        _Node.sprite = m_PathSprites[5];
                        _NodeShadow.sprite = m_PathSprites[5];
                    }
                    else if (_PreviousNodePos.x == -1)
                    {
                        _Node.sprite = m_PathSprites[21];
                        _NodeShadow.sprite = m_PathSprites[21];
                    }
                    else
                    {
                        _Node.sprite = m_PathSprites[13];
                        _NodeShadow.sprite = m_PathSprites[13];
                    }
                }
                else
                {
                    if (_PreviousNodePos.x == 1)
                    {
                        _Node.sprite = m_PathSprites[17];
                        _NodeShadow.sprite = m_PathSprites[17];
                    }
                    else if (_PreviousNodePos.x == -1)
                    {
                        _Node.sprite = m_PathSprites[1];
                        _NodeShadow.sprite = m_PathSprites[1];
                    }
                    else
                    {
                        _Node.sprite = m_PathSprites[9];
                        _NodeShadow.sprite = m_PathSprites[9];
                    }
                }
            }

            m_PathNodes.Add(_Node);
            m_PathNodeShadows.Add(_NodeShadow);
        }

        m_TargetNodeIndex = a_Index;

        HasPath = true;
        OnPathCreated?.Invoke();
    }

    public void MoveToDestination()
    {
        StartCoroutine(Move());
    }

    public void CancelMovement()
    {
        m_StopMovement = true;
    }
}
