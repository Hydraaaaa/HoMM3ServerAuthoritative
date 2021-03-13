using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Faction : ScriptableObject
{
    public Sprite TownSprite => m_TownSprite;
    public Sprite TownBuiltSprite => m_TownBuiltSprite;

    public Sprite TownNoFortSprite => m_TownNoFortSprite;
    public Sprite TownNoFortBuiltSprite => m_TownNoFortBuiltSprite;

    public List<Hero> Heroes => m_Heroes;

    [SerializeField] Sprite m_TownSprite;
    [SerializeField] Sprite m_TownBuiltSprite;

    [SerializeField] Sprite m_TownNoFortSprite;
    [SerializeField] Sprite m_TownNoFortBuiltSprite;

    [Space]

    [SerializeField] List<Hero> m_Heroes;
}
