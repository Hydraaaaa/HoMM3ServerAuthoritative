using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Player Resources")]
public class PlayerResources : ScriptableObject
{
    public event System.Action OnResourcesUpdated;

    public int Wood
    {
        get { return m_Wood; }
        set
        {
            if (m_Wood != value)
            {
                m_Wood = value;
                OnResourcesUpdated?.Invoke();
            }
        }
    }

    public int Ore
    {
        get { return m_Ore; }
        set
        {
            if (m_Ore != value)
            {
                m_Ore = value;
                OnResourcesUpdated?.Invoke();
            }
        }
    }

    public int Mercury
    {
        get { return m_Mercury; }
        set
        {
            if (m_Mercury != value)
            {
                m_Mercury = value;
                OnResourcesUpdated?.Invoke();
            }
        }
    }

    public int Sulfur
    {
        get { return m_Sulfur; }
        set
        {
            if (m_Sulfur != value)
            {
                m_Sulfur = value;
                OnResourcesUpdated?.Invoke();
            }
        }
    }

    public int Crystals
    {
        get { return m_Crystals; }
        set
        {
            if (m_Crystals != value)
            {
                m_Crystals = value;
                OnResourcesUpdated?.Invoke();
            }
        }
    }

    public int Gems
    {
        get { return m_Gems; }
        set
        {
            if (m_Gems != value)
            {
                m_Gems = value;
                OnResourcesUpdated?.Invoke();
            }
        }
    }

    public int Gold
    {
        get { return m_Gold; }
        set
        {
            if (m_Gold != value)
            {
                m_Gold = value;
                OnResourcesUpdated?.Invoke();
            }
        }
    }

    [SerializeField] int m_Wood;
    [SerializeField] int m_Mercury;
    [SerializeField] int m_Ore;
    [SerializeField] int m_Sulfur;
    [SerializeField] int m_Crystals;
    [SerializeField] int m_Gems;
    [SerializeField] int m_Gold;
}
