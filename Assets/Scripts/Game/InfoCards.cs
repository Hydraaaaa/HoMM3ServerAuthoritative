using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCards : MonoBehaviour
{
    [SerializeField] LocalOwnership m_LocalOwnership;

    [SerializeField] GameObject m_HeroCard;
    [SerializeField] GameObject m_TownCard;
    [SerializeField] GameObject m_InfoCard;

    void Awake()
    {
        m_LocalOwnership.OnHeroSelected += OnHeroSelected;
        m_LocalOwnership.OnHeroDeselected += OnHeroDeselected;
        m_LocalOwnership.OnTownSelected += OnTownSelected;
        m_LocalOwnership.OnTownDeselected += OnTownDeselected;

        m_HeroCard.SetActive(false);
        m_TownCard.SetActive(false);
        m_InfoCard.SetActive(false);
    }

    void OnHeroSelected(MapHero a_Hero, int a_Index)
    {
        m_HeroCard.SetActive(true);
        m_InfoCard.SetActive(false);
    }

    void OnHeroDeselected(MapHero a_Hero)
    {
        m_HeroCard.SetActive(false);
        // Day X thing
    }

    void OnTownSelected(MapTown a_Town, int a_Index)
    {
        m_TownCard.SetActive(true);
        m_InfoCard.SetActive(false);
    }

    void OnTownDeselected(MapTown a_Town)
    {
        m_TownCard.SetActive(false);
        // Day X thing
    }

    public void OnInfoCardClicked()
    {
        if (m_InfoCard.activeSelf)
        {
            m_InfoCard.SetActive(false);

            if (m_LocalOwnership.SelectedHero != null)
            {
                m_HeroCard.SetActive(true);
            }
            else if (m_LocalOwnership.SelectedTown != null)
            {
                m_TownCard.SetActive(true);
            }
            else
            {
                // Day X thing
            }
        }
        else
        {
            m_HeroCard.SetActive(false);
            m_TownCard.SetActive(false);
            m_InfoCard.SetActive(true);
        }
    }
}
