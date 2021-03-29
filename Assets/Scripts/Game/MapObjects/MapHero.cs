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

    [SerializeField] SpriteRenderer m_HeroRenderer;
    [SerializeField] SpriteRenderer m_HeroShadowRenderer;
    [SerializeField] SpriteRenderer m_FlagRenderer;

    int m_Direction = DIRECTION_E;

    bool m_IsUnderground;

    public void Initialize(Hero a_Hero, int a_PosX, int a_PosY, bool a_IsUnderground)
    {
        Hero = a_Hero;

        transform.position = new Vector3(a_PosX + 0.5f, -a_PosY - 0.5f, 0);

        m_IsUnderground = a_IsUnderground;

        Initialize();
    }

    public void Initialize(ScenarioObject a_ScenarioObject)
    {
        // TODO: Determine which hero this object is

        Initialize();
    }
    void Initialize()
    {
        gameObject.name = Hero.Name;
        m_HeroRenderer.sprite = Hero.HeroVisualData.IdleSprites[0];
        m_HeroShadowRenderer.sprite = Hero.HeroVisualData.ShadowIdleSprites[0];
    }
}
