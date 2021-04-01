using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapHero : MonoBehaviour
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

    public DynamicMapObstacle DynamicObstacle => m_DynamicObstacle;

    [SerializeField] SpriteRenderer m_HeroRenderer;
    [SerializeField] SpriteRenderer m_HeroShadowRenderer;
    [SerializeField] MapObjectRenderer m_FlagRenderer;
    [SerializeField] SpriteRenderer m_FlagSpriteRenderer;
    [SerializeField] DynamicMapObstacle m_DynamicObstacle;
    [SerializeField] PlayerColors m_PlayerColors;

    int m_Direction = DIRECTION_E;

    public void Initialize(Hero a_Hero, int a_PlayerIndex, int a_PosX, int a_PosY, bool a_IsUnderground, Pathfinding a_Pathfinding)
    {
        Hero = a_Hero;
        PlayerIndex = a_PlayerIndex;
        IsUnderground = a_IsUnderground;

        transform.position = new Vector3(a_PosX + 0.5f, -a_PosY - 0.5f, 0);

        m_DynamicObstacle.Initialize(a_Pathfinding);
        m_DynamicObstacle.AddInteractedNode(a_PosX, a_PosY, a_IsUnderground);

        Initialize();
    }

    public void Initialize(ScenarioObject a_ScenarioObject, Pathfinding a_Pathfinding)
    {
        //a_ScenarioObject.

        m_DynamicObstacle.Initialize(a_Pathfinding);

        Initialize();
    }
    void Initialize()
    {
        gameObject.name = Hero.Name;
        m_HeroRenderer.sprite = Hero.HeroVisualData.IdleSprites[0];
        m_HeroShadowRenderer.sprite = Hero.HeroVisualData.ShadowIdleSprites[0];

        HeroFlagVisualData _FlagData = m_PlayerColors.Flags[PlayerIndex];

        m_FlagRenderer.SetSprites(_FlagData.IdleSprites);
        m_FlagRenderer.transform.localPosition = new Vector3(1 + _FlagData.IdleOffsets[m_Direction].x, _FlagData.IdleOffsets[m_Direction].y, 0);
    }
}
