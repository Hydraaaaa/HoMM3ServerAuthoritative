using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Heroes 3/Faction")]
public class Faction : ScriptableObject
{
    public Sprite TownSpriteSmall => m_TownSpriteSmall;
    public Sprite TownBuiltSpriteSmall => m_TownBuiltSpriteSmall;

    public Sprite TownNoFortSpriteSmall => m_TownNoFortSpriteSmall;
    public Sprite TownNoFortBuiltSpriteSmall => m_TownNoFortBuiltSpriteSmall;

    public Sprite TownSpriteLarge => m_TownSpriteLarge;
    public Sprite TownBuiltSpriteLarge => m_TownBuiltSpriteLarge;

    public Sprite TownNoFortSpriteLarge => m_TownNoFortSpriteLarge;
    public Sprite TownNoFortBuiltSpriteLarge => m_TownNoFortBuiltSpriteLarge;

    public Sprite TownScreenBackground => m_TownScreenBackground;

    public MapObjectVisualData MapVisualData => m_MapVisualData;
    public MapObjectVisualData MapVisualDataCapitol => m_MapVisualDataCapitol;
    public MapObjectVisualData MapVisualDataNoFort => m_MapVisualDataNoFort;

    public List<HeroContainer> Heroes => m_Heroes;

    [SerializeField] Sprite m_TownSpriteSmall;
    [SerializeField] Sprite m_TownBuiltSpriteSmall;

    [SerializeField] Sprite m_TownNoFortSpriteSmall;
    [SerializeField] Sprite m_TownNoFortBuiltSpriteSmall;

    [SerializeField] Sprite m_TownSpriteLarge;
    [SerializeField] Sprite m_TownBuiltSpriteLarge;

    [SerializeField] Sprite m_TownNoFortSpriteLarge;
    [SerializeField] Sprite m_TownNoFortBuiltSpriteLarge;

    [SerializeField] Sprite m_TownScreenBackground;

    [Space]

    [SerializeField] MapObjectVisualData m_MapVisualData;
    [SerializeField] MapObjectVisualData m_MapVisualDataCapitol;
    [SerializeField] MapObjectVisualData m_MapVisualDataNoFort;

    [Space]

    [SerializeField] List<HeroContainer> m_Heroes;
}
