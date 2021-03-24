using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HeroList : ScriptableObject
{
    public List<HeroContainer> Heroes => m_Heroes;

    [SerializeField] List<HeroContainer> m_Heroes;
}
