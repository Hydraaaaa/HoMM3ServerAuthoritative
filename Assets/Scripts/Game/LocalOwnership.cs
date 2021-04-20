using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalOwnership : MonoBehaviour
{
    public event Action<MapHero> OnHeroAdded;
    public event Action<MapHero> OnHeroRemoved;
    public event Action<MapHero, int> OnHeroSelected;
    public event Action<MapHero> OnHeroDeselected;

    public event Action<MapTown> OnTownAdded;
    public event Action<MapTown> OnTownRemoved;
    public event Action<MapTown, int> OnTownSelected;

    List<MapHero> m_Heroes = new List<MapHero>();
    List<MapTown> m_Towns = new List<MapTown>();

    public MapHero SelectedHero { get; private set; }
    public MapTown SelectedTown { get; private set; }

    public List<MapHero> GetHeroes()
    {
        return new List<MapHero>(m_Heroes);
    }

    public List<MapTown> GetTowns()
    {
        return new List<MapTown>(m_Towns);
    }

    public int GetHeroCount()
    {
        return m_Heroes.Count;
    }

    public int GetTownCount()
    {
        return m_Towns.Count;
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

    public void AddTown(MapTown a_Town)
    {
        if (m_Towns.Contains(a_Town))
        {
            Debug.LogError("Attempted to add town that's already owned");
            return;
        }

        m_Towns.Add(a_Town);

        OnTownAdded?.Invoke(a_Town);
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
            SelectedHero.OnDeselected();
            OnHeroDeselected?.Invoke(a_Hero);
            SelectedHero = null;
        }

        OnHeroRemoved?.Invoke(a_Hero);
    }

    public void RemoveTown(MapTown a_Town)
    {
        if (!m_Towns.Contains(a_Town))
        {
            Debug.LogError("Attempted to remove town that isn't owned");
            return;
        }

        m_Towns.Remove(a_Town);

        if (SelectedTown == a_Town)
        {
            SelectedTown = null;
        }

        OnTownRemoved?.Invoke(a_Town);
    }

    public void SelectHero(MapHero a_Hero)
    {
        if (!m_Heroes.Contains(a_Hero))
        {
            Debug.LogError("Selected hero that isn't owned by local player");
            return;
        }

        if (SelectedHero != null)
        {
            SelectedHero.OnDeselected();
            OnHeroDeselected?.Invoke(SelectedHero);
        }

        SelectedHero = a_Hero;
        SelectedTown = null;

        OnHeroSelected?.Invoke(a_Hero, m_Heroes.IndexOf(a_Hero));

        a_Hero.OnSelected();
    }

    public void SelectTown(MapTown a_Town)
    {
        if (!m_Towns.Contains(a_Town))
        {
            Debug.LogError("Selected town that isn't owned by local player");
            return;
        }

        if (SelectedHero != null)
        {
            SelectedHero.OnDeselected();
            OnHeroDeselected?.Invoke(SelectedHero);
        }

        SelectedTown = a_Town;
        SelectedHero = null;

        OnTownSelected?.Invoke(a_Town, m_Towns.IndexOf(a_Town));
    }
}
