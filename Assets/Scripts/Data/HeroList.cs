using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HeroList : ScriptableObject
{
    public List<Hero> Heroes => m_Heroes;

    [SerializeField] List<Hero> m_Heroes;
}
