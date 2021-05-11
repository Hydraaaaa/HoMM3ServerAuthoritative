using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoCards : MonoBehaviour
{
    [SerializeField] LocalOwnership m_LocalOwnership;

    [SerializeField] GameObject m_HeroCard;
    [SerializeField] GameObject m_TownCard;
    [SerializeField] GameObject m_InfoCard;

    [SerializeField] Image m_HeroImage;
    [SerializeField] Text m_HeroName;
    [SerializeField] Text m_HeroAttack;
    [SerializeField] Text m_HeroDefense;
    [SerializeField] Text m_HeroSpellpower;
    [SerializeField] Text m_HeroKnowledge;

    [SerializeField] Image m_TownImage;

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

        m_HeroImage.sprite = a_Hero.Hero.LargePortrait;
        m_HeroName.text = a_Hero.Hero.Name;

        m_HeroAttack.text = a_Hero.Hero.Attack.ToString();
        m_HeroDefense.text = a_Hero.Hero.Defense.ToString();
        m_HeroKnowledge.text = a_Hero.Hero.Knowledge.ToString();
        m_HeroSpellpower.text = a_Hero.Hero.Spellpower.ToString();
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

        if (a_Town.Buildings.Fort)
        {
            m_TownImage.sprite = a_Town.Faction.TownSpriteLarge;
        }
        else
        {
            m_TownImage.sprite = a_Town.Faction.TownNoFortSpriteLarge;
        }
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
