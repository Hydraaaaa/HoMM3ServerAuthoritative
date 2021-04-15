using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnershipUI : MonoBehaviour
{
    [SerializeField] LocalOwnership m_LocalOwnership;

    [Header("High Res UI")]
    [SerializeField] List<Image> m_HeroImages;
    [SerializeField] List<Image> m_TownImages;
    [SerializeField] GameObject m_SelectedBorder;
    [SerializeField] Button m_TownUpArrow;
    [SerializeField] Button m_TownDownArrow;

    [Header("Low Res UI")]
    [SerializeField] List<Image> m_HeroImagesLowRes;
    [SerializeField] List<Image> m_TownImagesLowRes;
    [SerializeField] GameObject m_SelectedBorderLowRes;
    [SerializeField] Button m_HeroUpArrowLowRes;
    [SerializeField] Button m_HeroDownArrowLowRes;
    [SerializeField] Button m_TownUpArrowLowRes;
    [SerializeField] Button m_TownDownArrowLowRes;

    MapHero m_SelectedHero;
    MapTown m_SelectedTown;

    int m_TownUIIndex = 0;
    int m_HeroUIIndexLowRes = 0;
    int m_TownUIIndexLowRes = 0;

    void Awake()
    {
        m_LocalOwnership.OnHeroAdded += OnHeroAdded;
        m_LocalOwnership.OnHeroRemoved += OnHeroRemoved;
        m_LocalOwnership.OnHeroSelected += OnHeroSelected;
        m_LocalOwnership.OnTownAdded += OnTownAdded;
        m_LocalOwnership.OnTownRemoved += OnTownRemoved;
        m_LocalOwnership.OnTownSelected += OnTownSelected;
    }

    void OnHeroAdded(MapHero a_Hero)
    {
        int _HeroCount = m_LocalOwnership.GetHeroCount();

        m_HeroImages[_HeroCount - 1].sprite = a_Hero.Hero.Portrait;
        m_HeroImages[_HeroCount - 1].gameObject.SetActive(true);

        UpdateHeroDisplayLowRes();
    }

    void OnHeroRemoved(MapHero a_Hero)
    {
        List<MapHero> _Heroes = m_LocalOwnership.GetHeroes();

        for (int i = 0; i < _Heroes.Count; i++)
        {
            m_HeroImages[i].sprite = _Heroes[i].Hero.Portrait;
            m_HeroImages[i].gameObject.SetActive(true);
        }

        for (int i = _Heroes.Count; i < 8; i++)
        {
            m_HeroImages[i].gameObject.SetActive(false);
        }

        UpdateHeroDisplayLowRes();
    }

    void OnHeroSelected(MapHero a_Hero, int a_Index)
    {
        m_SelectedHero = a_Hero;
        m_SelectedTown = null;

        m_SelectedBorder.SetActive(true);
        m_SelectedBorder.transform.position = m_HeroImages[a_Index].transform.position;

        if (a_Index >= m_HeroUIIndexLowRes + 5)
        {
            m_HeroUIIndexLowRes = 3;
        }
        else if (a_Index < m_HeroUIIndexLowRes)
        {
            m_HeroUIIndexLowRes = a_Index;
        }

        UpdateHeroDisplayLowRes();
    }

    void OnTownAdded(MapTown a_Town)
    {
        UpdateTownDisplay();
    }

    void OnTownRemoved(MapTown a_Town)
    {
        UpdateTownDisplay();
    }

    void OnTownSelected(MapTown a_Town, int a_Index)
    {
        m_SelectedTown = a_Town;
        m_SelectedHero = null;

        int _TownCount = m_LocalOwnership.GetTownCount();

        // High res
        if (a_Index >= m_TownUIIndex + 7 ||
            a_Index < m_TownUIIndex)
        {
            m_TownUIIndex = a_Index;

            if (m_TownUIIndex > _TownCount - 7)
            {
                m_TownUIIndex = _TownCount - 7;
            }
        }

        // Low res
        if (a_Index >= m_TownUIIndexLowRes + 5 ||
            a_Index < m_TownUIIndexLowRes)
        {
            m_TownUIIndexLowRes = a_Index;

            if (m_TownUIIndexLowRes > _TownCount - 5)
            {
                m_TownUIIndexLowRes = _TownCount - 5;
            }
        }

        UpdateTownDisplay();
    }

    public void HeroUpArrowPressed()
    {
        if (m_HeroUIIndexLowRes > 0)
        {
            m_HeroUIIndexLowRes--;
            UpdateHeroDisplayLowRes();
        }
    }

    public void HeroDownArrowPressed()
    {
        if (m_LocalOwnership.GetHeroCount() > m_HeroUIIndexLowRes + 5)
        {
            m_HeroUIIndexLowRes++;
            UpdateHeroDisplayLowRes();
        }
    }

    public void TownUpArrowPressed()
    {
        bool _Changed = false;

        if (m_TownUIIndex > 0)
        {
            m_TownUIIndex--;
            _Changed = true;
        }

        if (m_TownUIIndexLowRes > 0)
        {
            m_TownUIIndexLowRes--;
            _Changed = true;
        }

        if (_Changed)
        {
            UpdateTownDisplay();
        }
    }

    public void TownDownArrowPressed()
    {
        int _TownCount = m_LocalOwnership.GetTownCount();
        bool _Changed = false;

        if (m_TownUIIndex < _TownCount - 7)
        {
            m_TownUIIndex++;
            _Changed = true;
        }

        if (m_TownUIIndexLowRes < _TownCount - 5)
        {
            m_TownUIIndexLowRes++;
            _Changed = true;
        }

        if (_Changed)
        {
            UpdateTownDisplay();
        }
    }

    void UpdateHeroDisplayLowRes()
    {
        List<MapHero> _Heroes = m_LocalOwnership.GetHeroes();

        for (int i = 0; i < 5; i++)
        {
            if (i < _Heroes.Count)
            {
                m_HeroImagesLowRes[i].sprite = _Heroes[i + m_HeroUIIndexLowRes].Hero.Portrait;
                m_HeroImagesLowRes[i].gameObject.SetActive(true);
            }
            else
            {
                m_HeroImagesLowRes[i].gameObject.SetActive(false);
            }
        }

        m_HeroUpArrowLowRes.interactable = m_HeroUIIndexLowRes != 0;
        m_HeroDownArrowLowRes.interactable = m_HeroUIIndexLowRes < _Heroes.Count - 5;

        if (m_SelectedHero != null)
        {
            int _SelectedHeroIndex = m_LocalOwnership.GetHeroes().IndexOf(m_SelectedHero);

            if (_SelectedHeroIndex < m_HeroUIIndexLowRes ||
                _SelectedHeroIndex >= m_HeroUIIndexLowRes + 5)
            {
                m_SelectedBorderLowRes.SetActive(false);
            }
            else
            {
                m_SelectedBorderLowRes.SetActive(true);
                m_SelectedBorderLowRes.transform.position = m_HeroImagesLowRes[_SelectedHeroIndex - m_HeroUIIndexLowRes].transform.position;
            }
        }
    }

    void UpdateTownDisplay()
    {
        List<MapTown> _Towns = m_LocalOwnership.GetTowns();

        // High res UI
        for (int i = 0; i < 7; i++)
        {
            if (i < _Towns.Count)
            {
                m_TownImages[i].sprite = _Towns[i + m_TownUIIndex].Faction.TownSprite;
                m_TownImages[i].gameObject.SetActive(true);
            }
            else
            {
                m_TownImages[i].gameObject.SetActive(false);
            }
        }

        m_TownUpArrow.interactable = m_TownUIIndex != 0;
        m_TownDownArrow.interactable = m_TownUIIndex < _Towns.Count - 7;

        // Low res UI
        for (int i = 0; i < 5; i++)
        {
            if (i < _Towns.Count)
            {
                m_TownImagesLowRes[i].sprite = _Towns[i + m_TownUIIndexLowRes].Faction.TownSprite;
                m_TownImagesLowRes[i].gameObject.SetActive(true);
            }
            else
            {
                m_TownImagesLowRes[i].gameObject.SetActive(false);
            }
        }

        m_TownUpArrowLowRes.interactable = m_TownUIIndexLowRes != 0;
        m_TownDownArrowLowRes.interactable = m_TownUIIndexLowRes < _Towns.Count - 5;

        if (m_SelectedTown != null)
        {
            int _SelectedTownIndex = m_LocalOwnership.GetTowns().IndexOf(m_SelectedTown);

            if (_SelectedTownIndex < m_TownUIIndex ||
                _SelectedTownIndex >= m_TownUIIndex + 7)
            {
                m_SelectedBorder.SetActive(false);
            }
            else
            {
                m_SelectedBorder.SetActive(true);
                m_SelectedBorder.transform.position = m_TownImages[_SelectedTownIndex - m_TownUIIndex].transform.position;
            }

            if (_SelectedTownIndex < m_TownUIIndexLowRes ||
                _SelectedTownIndex >= m_TownUIIndexLowRes + 5)
            {
                m_SelectedBorderLowRes.SetActive(false);
            }
            else
            {
                m_SelectedBorderLowRes.SetActive(true);
                m_SelectedBorderLowRes.transform.position = m_TownImagesLowRes[_SelectedTownIndex - m_TownUIIndexLowRes].transform.position;
            }
        }
    }

    public void HeroPressed(int a_Index)
    {
        m_LocalOwnership.SelectHero(m_LocalOwnership.GetHeroes()[a_Index]);
    }

    public void HeroPressedLowRes(int a_Index)
    {
        HeroPressed(a_Index + m_HeroUIIndexLowRes);
    }

    public void TownPressed(int a_Index)
    {
        m_LocalOwnership.SelectTown(m_LocalOwnership.GetTowns()[a_Index + m_TownUIIndex]);
    }

    public void TownPressedLowRes(int a_Index)
    {
        m_LocalOwnership.SelectTown(m_LocalOwnership.GetTowns()[a_Index + m_TownUIIndexLowRes]);
    }
}
