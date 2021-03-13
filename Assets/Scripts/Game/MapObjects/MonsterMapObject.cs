using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterMapObject : MonoBehaviour
{
    public MapObject MapObject { get; private set; }
    public Monster Monster { get; private set; }

    public void Initialize(MapObject a_Object)
    {
        MapObject = a_Object;

        switch (MapObject.ScenarioObject.Template.Name)
        {
            case "avwmon1":
                List<Monster> _Tier1Monsters = a_Object.Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 0).ToList();
                Monster = _Tier1Monsters[Random.Range(0, _Tier1Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon2":
                List<Monster> _Tier2Monsters = a_Object.Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 1).ToList();
                Monster = _Tier2Monsters[Random.Range(0, _Tier2Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon3":
                List<Monster> _Tier3Monsters = a_Object.Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 2).ToList();
                Monster = _Tier3Monsters[Random.Range(0, _Tier3Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon4":
                List<Monster> _Tier4Monsters = a_Object.Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 3).ToList();
                Monster = _Tier4Monsters[Random.Range(0, _Tier4Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon5":
                List<Monster> _Tier5Monsters = a_Object.Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 4).ToList();
                Monster = _Tier5Monsters[Random.Range(0, _Tier5Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon6":
                List<Monster> _Tier6Monsters = a_Object.Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 5).ToList();
                Monster = _Tier6Monsters[Random.Range(0, _Tier6Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            case "avwmon7":
                List<Monster> _Tier7Monsters = a_Object.Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 6).ToList();
                Monster = _Tier7Monsters[Random.Range(0, _Tier7Monsters.Count)];
                gameObject.name = Monster.MapVisualData.name;
                break;

            //case "avwmon":
                //List<Monster> _Tier7Monsters = a_Object.Monsters.Monsters.Where((a_Monster) => a_Monster.Tier == 6).ToList();
                //Monster = _Tier7Monsters[Random.Range(0, _Tier7Monsters.Count)];
                //break;

            default:
                Monster = a_Object.Monsters.Monsters.FirstOrDefault((a_Monster) => a_Monster.Type == MapObject.ScenarioObject.Monster.Type);
                break;
        }

        int _Offset = Random.Range(0, 100);

        MapObject.Renderer.SetSprites(Monster?.MapVisualData?.m_Sprites);
        MapObject.Renderer.SetOffset(_Offset);
        MapObject.Shadow.Renderer.SetSprites(Monster?.MapVisualData?.m_ShadowSprites);
        MapObject.Shadow.Renderer.SetOffset(_Offset);
    }
}
