using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapMonster : MapObjectBase
{
    public Monster Monster { get; private set; }

    public DynamicMapObstacle DynamicObstacle => m_DynamicObstacle;

    [SerializeField] MonsterList m_Monsters;

    [SerializeField] MapObjectRenderer m_Renderer;
    [SerializeField] MapObjectRenderer m_ShadowRenderer;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] DynamicMapObstacle m_DynamicObstacle;

    public void Initialize(ScenarioObject a_ScenarioObject, GameReferences a_GameReferences)
    {
        m_GameReferences = a_GameReferences;

        m_SpriteRenderer.sortingOrder = -32767 + a_ScenarioObject.SortOrder;

        switch (a_ScenarioObject.Template.Name)
        {
            case "avwmon1":
                List<Monster> _Tier1Monsters = m_Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 0).ToList();
                Monster = _Tier1Monsters[Random.Range(0, _Tier1Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon2":
                List<Monster> _Tier2Monsters = m_Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 1).ToList();
                Monster = _Tier2Monsters[Random.Range(0, _Tier2Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon3":
                List<Monster> _Tier3Monsters = m_Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 2).ToList();
                Monster = _Tier3Monsters[Random.Range(0, _Tier3Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon4":
                List<Monster> _Tier4Monsters = m_Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 3).ToList();
                Monster = _Tier4Monsters[Random.Range(0, _Tier4Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon5":
                List<Monster> _Tier5Monsters = m_Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 4).ToList();
                Monster = _Tier5Monsters[Random.Range(0, _Tier5Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon6":
                List<Monster> _Tier6Monsters = m_Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 5).ToList();
                Monster = _Tier6Monsters[Random.Range(0, _Tier6Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon7":
                List<Monster> _Tier7Monsters = m_Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 6).ToList();
                Monster = _Tier7Monsters[Random.Range(0, _Tier7Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmrnd0":
                Monster = m_Monsters.Monsters[Random.Range(0, m_Monsters.Monsters.Count)];
                break;

            default:
                Monster = m_Monsters.Monsters.FirstOrDefault((a_Monster) => a_Monster.MapVisualData.name == a_ScenarioObject.Template.Name);
                break;
        }

        int _Offset = Random.Range(0, Monster.MapVisualData.m_Sprites.Length);

        m_Renderer.SetSprites(Monster.MapVisualData.m_Sprites);
        m_Renderer.SetOffset(_Offset);
        m_ShadowRenderer.SetSprites(Monster.MapVisualData.m_ShadowSprites);
        m_ShadowRenderer.SetOffset(_Offset);

        m_DynamicObstacle.Initialize(m_GameReferences.Pathfinding, this);

        gameObject.name = Monster.name;
    }
}
