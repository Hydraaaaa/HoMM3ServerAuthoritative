using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroListUI : MonoBehaviour
{
    [SerializeField] OwnedHeroes m_OwnedHeroes;

    [SerializeField] List<Image> m_Images;
    [SerializeField] GameObject m_SelectedBorder;

    [SerializeField] List<Image> m_ImagesSmall;
    [SerializeField] GameObject m_SelectedBorderSmall;
    [SerializeField] GameObject m_UpArrow;
    [SerializeField] GameObject m_DownArrow;

    MapHero m_SelectedHero;

    int m_SmallIndex = 0;

    void Awake()
    {
        m_OwnedHeroes.OnHeroAdded += OnHeroAdded;
        m_OwnedHeroes.OnHeroRemoved += OnHeroRemoved;
        m_OwnedHeroes.OnHeroSelected += OnHeroSelected;
    }

    void OnHeroAdded(MapHero a_Hero)
    {
        int _HeroCount = m_OwnedHeroes.GetHeroCount();

        m_Images[_HeroCount - 1].sprite = a_Hero.Hero.Portrait;
        m_Images[_HeroCount - 1].gameObject.SetActive(true);
    }

    void OnHeroRemoved(MapHero a_Hero)
    {
        if (a_Hero == m_SelectedHero)
        {
            m_SelectedBorder.SetActive(false);
        }

        List<MapHero> _Heroes = m_OwnedHeroes.GetHeroes();

        for (int i = 0; i < _Heroes.Count; i++)
        {
            m_Images[i].sprite = _Heroes[i].Hero.Portrait;
            m_Images[i].gameObject.SetActive(true);
        }

        for (int i = _Heroes.Count; i < 8; i++)
        {
            m_Images[i].gameObject.SetActive(false);
        }
    }

    void OnHeroSelected(MapHero a_Hero, int a_Index)
    {
        m_SelectedHero = a_Hero;

        m_SelectedBorder.SetActive(true);
        m_SelectedBorder.transform.position = m_Images[a_Index].transform.position;
    }

    public void UpArrowPressed()
    {

    }

    public void DownArrowPressed()
    {

    }

    public void HeroPressed(int a_Index)
    {
        m_OwnedHeroes.SelectHero(m_OwnedHeroes.GetHeroes()[a_Index]);
    }

    public void HeroPressedSmall(int a_Index)
    {
        HeroPressed(a_Index + m_SmallIndex);
    }
}
