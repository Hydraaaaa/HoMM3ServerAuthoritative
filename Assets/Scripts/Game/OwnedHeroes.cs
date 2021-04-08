using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedHeroes : MonoBehaviour
{
    public event Action<MapHero> OnHeroAdded;
    public event Action<MapHero> OnHeroRemoved;
    public event Action<MapHero, int> OnHeroSelected;

    List<MapHero> m_Heroes = new List<MapHero>();

    public MapHero SelectedHero { get; private set; }

    public List<MapHero> GetHeroes()
    {
        return new List<MapHero>(m_Heroes);
    }

    public int GetHeroCount()
    {
        return m_Heroes.Count;
    }

    public void AddHero(MapHero a_Hero)
    {
        if (m_Heroes.Contains(a_Hero))
        {
            Debug.LogError("Attempted to add hero that's already owned");
            return;
        }

        m_Heroes.Add(a_Hero);

        OnHeroAdded?.Invoke(a_Hero);
    }

    public void RemoveHero(MapHero a_Hero)
    {
        if (!m_Heroes.Contains(a_Hero))
        {
            Debug.LogError("Attempted to remove hero that isn't owned");
            return;
        }

        m_Heroes.Remove(a_Hero);

        if (SelectedHero == a_Hero)
        {
            SelectedHero = null;
        }

        OnHeroRemoved?.Invoke(a_Hero);
    }

    public void SelectHero(MapHero a_Hero)
    {
        if (!m_Heroes.Contains(a_Hero))
        {
            Debug.LogError("Selected hero that isn't owned by local player");
            return;
        }

        SelectedHero?.OnDeselected();

        SelectedHero = a_Hero;

        OnHeroSelected?.Invoke(a_Hero, m_Heroes.IndexOf(a_Hero));

        a_Hero.OnSelected();
    }
}
