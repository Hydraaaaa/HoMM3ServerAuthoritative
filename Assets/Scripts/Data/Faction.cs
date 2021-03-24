using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Faction : ScriptableObject
{
    public Sprite TownSprite => m_TownSprite;
    public Sprite TownBuiltSprite => m_TownBuiltSprite;

    public Sprite TownNoFortSprite => m_TownNoFortSprite;
    public Sprite TownNoFortBuiltSprite => m_TownNoFortBuiltSprite;

    public MapObjectVisualData MapVisualData => m_MapVisualData;
    public MapObjectVisualData MapVisualDataCapitol => m_MapVisualDataCapitol;
    public MapObjectVisualData MapVisualDataNoFort => m_MapVisualDataNoFort;

    public List<HeroContainer> Heroes => m_Heroes;

    [SerializeField] Sprite m_TownSprite;
    [SerializeField] Sprite m_TownBuiltSprite;

    [SerializeField] Sprite m_TownNoFortSprite;
    [SerializeField] Sprite m_TownNoFortBuiltSprite;

    [Space]

    [SerializeField] MapObjectVisualData m_MapVisualData;
    [SerializeField] MapObjectVisualData m_MapVisualDataCapitol;
    [SerializeField] MapObjectVisualData m_MapVisualDataNoFort;

    [Space]

    [SerializeField] List<HeroContainer> m_Heroes;
}
