using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroPool : MonoBehaviour
{
    static HeroPool s_Instance;

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        s_Instance = Instantiate(Resources.Load<HeroPool>("HeroPool"), Vector3.zero, Quaternion.identity);
        s_Instance.name = "HeroPool";
        DontDestroyOnLoad(s_Instance);
    }

    [SerializeField] HeroList m_Heroes;
    [SerializeField] FactionList m_Factions;

    List<HeroInfo> m_HeroInfo;
    List<Hero> m_AvailableHeroes;

    public static void Initialize(List<HeroInfo> a_HeroInfo)
    {
        s_Instance.m_HeroInfo = a_HeroInfo;
        s_Instance.m_AvailableHeroes = new List<Hero>(s_Instance.m_Heroes.Heroes.Count);

        for (int i = 0; i < s_Instance.m_Heroes.Heroes.Count; i++)
        {
            s_Instance.m_AvailableHeroes.Add(new Hero(s_Instance.m_Heroes.Heroes[i].Hero));
        }
    }

    public static Hero GetRandomHero(int a_PlayerID)
    {
        List<Hero> _Heroes = new List<Hero>();

        for (int i = 0; i < s_Instance.m_AvailableHeroes.Count; i++)
        {
            HeroInfo _HeroInfo = s_Instance.m_HeroInfo.FirstOrDefault((a_HeroInfo) => a_HeroInfo.ID == s_Instance.m_AvailableHeroes[i].ID);

            if (_HeroInfo != null)
            {
                if ((_HeroInfo.ID & 1 << a_PlayerID) == a_PlayerID)
                {
                    _Heroes.Add(s_Instance.m_AvailableHeroes[i]);
                }
            }
        }

        if (_Heroes.Count == 0)
        {
            return null;
        }

        return _Heroes[Random.Range(0, _Heroes.Count)];
    }

    public static Hero GetRandomHero(int a_PlayerID, Faction a_Faction)
    {
        List<Hero> _Heroes = new List<Hero>();

        for (int i = 0; i < s_Instance.m_AvailableHeroes.Count; i++)
        {
            if (a_Faction.Heroes.Any((a_HeroContainer) => a_HeroContainer.Hero.ID == s_Instance.m_AvailableHeroes[i].ID))
            {
                HeroInfo _HeroInfo = s_Instance.m_HeroInfo.FirstOrDefault((a_HeroInfo) => a_HeroInfo.ID == s_Instance.m_AvailableHeroes[i].ID);

                if (_HeroInfo != null)
                {
                    if ((_HeroInfo.ID & 1 << a_PlayerID) == a_PlayerID)
                    {
                        _Heroes.Add(s_Instance.m_AvailableHeroes[i]);
                    }
                }
            }
        }

        if (_Heroes.Count == 0)
        {
            return null;
        }

        return _Heroes[Random.Range(0, _Heroes.Count)];
    }

    public static List<Hero> GetFactionHeroes(int a_PlayerID, Faction a_Faction)
    {
        List<Hero> _Heroes = new List<Hero>(a_Faction.Heroes.Select((a_Hero) => a_Hero.Hero).ToList());

        for (int i = _Heroes.Count - 1; i >= 0; i--)
        {
            if (!s_Instance.m_AvailableHeroes.Any((a_Hero) => a_Hero.ID == _Heroes[i].ID))
            {
                _Heroes.RemoveAt(i);
            }
        }

        for (int i = _Heroes.Count - 1; i >= 0; i--)
        {
            HeroInfo _HeroInfo = s_Instance.m_HeroInfo.FirstOrDefault((a_HeroInfo) => a_HeroInfo.ID == _Heroes[i].ID);

            if (_HeroInfo != null)
            {
                if ((_HeroInfo.Players & 1 << a_PlayerID) == 0)
                {
                    _Heroes.RemoveAt(i);
                }
            }
        }

        return _Heroes;
    }

    public static void ClaimHero(Hero a_Hero)
    {
        Debug.Log($"!! Claim Hero {a_Hero.Name} - {a_Hero.ID}");
        Hero _AvailableHero = s_Instance.m_AvailableHeroes.First((a_AvailableHero) => a_AvailableHero.ID == a_Hero.ID);
        s_Instance.m_AvailableHeroes.Remove(_AvailableHero);
    }

    public static void FreeHero(Hero a_Hero)
    {
        Debug.Log($"!! Free Hero {a_Hero.Name} - {a_Hero.ID}");
        Hero _AvailableHero = s_Instance.m_AvailableHeroes.FirstOrDefault((a_AvailableHero) => a_AvailableHero.ID == a_Hero.ID);

        if (_AvailableHero != null)
        {
            Debug.LogWarning($"!! ATTEMPTED TO FREE A FREED HERO");
        }
        else
        {
            s_Instance.m_AvailableHeroes.Add(a_Hero);
        }
    }
}