using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TownBuildings : MonoBehaviour
{
    [SerializeField] protected Building m_VillageHall;
    [SerializeField] protected Building m_TownHall;
    [SerializeField] protected Building m_CityHall;
    [SerializeField] protected Building m_Capitol;

    [SerializeField] protected Building m_Fort;
    [SerializeField] protected Building m_Citadel;
    [SerializeField] protected Building m_Castle;

    [SerializeField] protected Building m_Tavern;

    [SerializeField] protected Building m_Blacksmith;

    [SerializeField] protected Building m_Market;
    [SerializeField] protected Building m_Silo;
    [SerializeField] protected Building m_ArtifactMerchants;

    [SerializeField] protected Building m_MageGuild1;
    [SerializeField] protected Building m_MageGuild2;
    [SerializeField] protected Building m_MageGuild3;
    [SerializeField] protected Building m_MageGuild4;
    [SerializeField] protected Building m_MageGuild5;

    [SerializeField] protected Building m_Shipyard;
    [SerializeField] protected Building m_ShipyardShip;
    [SerializeField] protected Building m_Grail;

    [SerializeField] protected Building m_FactionBuilding1;
    [SerializeField] protected Building m_FactionBuilding2;
    [SerializeField] protected Building m_FactionBuilding3;
    [SerializeField] protected Building m_FactionBuilding4;

    [SerializeField] protected Building m_Dwelling1;
    [SerializeField] protected Building m_Dwelling1Up;
    [SerializeField] protected Building m_Dwelling2;
    [SerializeField] protected Building m_Dwelling2Up;
    [SerializeField] protected Building m_Dwelling3;
    [SerializeField] protected Building m_Dwelling3Up;
    [SerializeField] protected Building m_Dwelling4;
    [SerializeField] protected Building m_Dwelling4Up;
    [SerializeField] protected Building m_Dwelling5;
    [SerializeField] protected Building m_Dwelling5Up;
    [SerializeField] protected Building m_Dwelling6;
    [SerializeField] protected Building m_Dwelling6Up;
    [SerializeField] protected Building m_Dwelling7;
    [SerializeField] protected Building m_Dwelling7Up;

    [Space]

    // Hall screen
    [SerializeField] protected HallBuilding m_HallTownHall;
    [SerializeField] protected HallBuilding m_HallCityHall;
    [SerializeField] protected HallBuilding m_HallCapitol;

    [SerializeField] protected HallBuilding m_HallFort;
    [SerializeField] protected HallBuilding m_HallCitadel;
    [SerializeField] protected HallBuilding m_HallCastle;

    [SerializeField] protected HallBuilding m_HallTavern;

    [SerializeField] protected HallBuilding m_HallBlacksmith;

    [SerializeField] protected HallBuilding m_HallMarket;
    [SerializeField] protected HallBuilding m_HallSilo;
    [SerializeField] protected HallBuilding m_HallArtifactMerchants;

    [SerializeField] protected HallBuilding m_HallShipyard;

    [SerializeField] protected HallBuilding m_HallDwelling1;
    [SerializeField] protected HallBuilding m_HallDwelling1Up;
    [SerializeField] protected HallBuilding m_HallDwelling2;
    [SerializeField] protected HallBuilding m_HallDwelling2Up;
    [SerializeField] protected HallBuilding m_HallDwelling3;
    [SerializeField] protected HallBuilding m_HallDwelling3Up;
    [SerializeField] protected HallBuilding m_HallDwelling4;
    [SerializeField] protected HallBuilding m_HallDwelling4Up;
    [SerializeField] protected HallBuilding m_HallDwelling5;
    [SerializeField] protected HallBuilding m_HallDwelling5Up;
    [SerializeField] protected HallBuilding m_HallDwelling6;
    [SerializeField] protected HallBuilding m_HallDwelling6Up;
    [SerializeField] protected HallBuilding m_HallDwelling7;
    [SerializeField] protected HallBuilding m_HallDwelling7Up;

    [Space]

    // Hall Sprites
    [SerializeField] protected Sprite m_Cross;
    [SerializeField] protected Sprite m_Tick;
    [SerializeField] protected Sprite m_NoMoney;

    [SerializeField] protected Sprite m_Green;
    [SerializeField] protected Sprite m_Red;
    [SerializeField] protected Sprite m_Yellow;
    [SerializeField] protected Sprite m_Grey;

    protected BuiltBuildings m_BuiltBuildings;

    public virtual void SetBuildings(BuiltBuildings a_BuiltBuildings)
    {
        m_BuiltBuildings = a_BuiltBuildings;

        if (m_BuiltBuildings.Capitol)
        {
            m_VillageHall.gameObject.SetActive(false);
            m_TownHall.gameObject.SetActive(false);
            m_CityHall.gameObject.SetActive(false);
            m_Capitol.gameObject.SetActive(true);
        }
        else if (m_BuiltBuildings.CityHall)
        {
            m_VillageHall.gameObject.SetActive(false);
            m_TownHall.gameObject.SetActive(false);
            m_CityHall.gameObject.SetActive(true);
            m_Capitol.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.TownHall)
        {
            m_VillageHall.gameObject.SetActive(false);
            m_TownHall.gameObject.SetActive(true);
            m_CityHall.gameObject.SetActive(false);
            m_Capitol.gameObject.SetActive(false);
        }
        else
        {
            m_VillageHall.gameObject.SetActive(true);
            m_TownHall.gameObject.SetActive(false);
            m_CityHall.gameObject.SetActive(false);
            m_Capitol.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Castle)
        {
            m_Fort.gameObject.SetActive(false);
            m_Citadel.gameObject.SetActive(false);
            m_Castle.gameObject.SetActive(true);
        }
        else if (m_BuiltBuildings.Citadel)
        {
            m_Fort.gameObject.SetActive(false);
            m_Citadel.gameObject.SetActive(true);
            m_Castle.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.Fort)
        {
            m_Fort.gameObject.SetActive(true);
            m_Citadel.gameObject.SetActive(false);
            m_Castle.gameObject.SetActive(false);
        }
        else
        {
            m_Fort.gameObject.SetActive(false);
            m_Citadel.gameObject.SetActive(false);
            m_Castle.gameObject.SetActive(false);
        }

        m_Tavern.gameObject.SetActive(m_BuiltBuildings.Tavern);

        m_Blacksmith.gameObject.SetActive(m_BuiltBuildings.Blacksmith);
        m_Market.gameObject.SetActive(m_BuiltBuildings.Market);
        m_Silo.gameObject.SetActive(m_BuiltBuildings.Silo);
        m_ArtifactMerchants?.gameObject.SetActive(m_BuiltBuildings.ArtifactMerchants);

        if (m_BuiltBuildings.MageGuild5)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(true);
        }
        else if (m_BuiltBuildings.MageGuild4)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(true);
            m_MageGuild5?.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.MageGuild3)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(true);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.MageGuild2)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(true);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.MageGuild1)
        {
            m_MageGuild1.gameObject.SetActive(true);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(false);
        }
        else
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(false);
        }

        m_Shipyard?.gameObject.SetActive(m_BuiltBuildings.Shipyard);
        m_ShipyardShip?.gameObject.SetActive(false);
        m_Grail.gameObject.SetActive(m_BuiltBuildings.Grail);

        m_FactionBuilding1.gameObject.SetActive(m_BuiltBuildings.FactionBuilding1);
        m_FactionBuilding2?.gameObject.SetActive(m_BuiltBuildings.FactionBuilding2);
        m_FactionBuilding3?.gameObject.SetActive(m_BuiltBuildings.FactionBuilding3);
        m_FactionBuilding4?.gameObject.SetActive(m_BuiltBuildings.FactionBuilding4);

        if (m_BuiltBuildings.Dwelling1Up)
        {
            m_Dwelling1.gameObject.SetActive(false);
            m_Dwelling1Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling1.gameObject.SetActive(m_BuiltBuildings.Dwelling1);
            m_Dwelling1Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling2Up)
        {
            m_Dwelling2.gameObject.SetActive(false);
            m_Dwelling2Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling2.gameObject.SetActive(m_BuiltBuildings.Dwelling2);
            m_Dwelling2Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling3Up)
        {
            m_Dwelling3.gameObject.SetActive(false);
            m_Dwelling3Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling3.gameObject.SetActive(m_BuiltBuildings.Dwelling3);
            m_Dwelling3Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling4Up)
        {
            m_Dwelling4.gameObject.SetActive(false);
            m_Dwelling4Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling4.gameObject.SetActive(m_BuiltBuildings.Dwelling4);
            m_Dwelling4Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling5Up)
        {
            m_Dwelling5.gameObject.SetActive(false);
            m_Dwelling5Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling5.gameObject.SetActive(m_BuiltBuildings.Dwelling5);
            m_Dwelling5Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling6Up)
        {
            m_Dwelling6.gameObject.SetActive(false);
            m_Dwelling6Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling6.gameObject.SetActive(m_BuiltBuildings.Dwelling6);
            m_Dwelling6Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling7Up)
        {
            m_Dwelling7.gameObject.SetActive(false);
            m_Dwelling7Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling7.gameObject.SetActive(m_BuiltBuildings.Dwelling7);
            m_Dwelling7Up.gameObject.SetActive(false);
        }

        UpdateHall();
    }

    public virtual void UpdateHall()
    {
        if (m_BuiltBuildings.Capitol)
        {
            m_HallCapitol.gameObject.SetActive(true);
            m_HallCapitol.ButtonImage.sprite = m_Yellow;
            m_HallCapitol.CornerImage.gameObject.SetActive(true);
            m_HallCapitol.CornerImage.sprite = m_Tick;
            m_HallCapitol.Buildable = false;

            m_HallTownHall.gameObject.SetActive(false);
            m_HallCityHall.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.CityHall)
        {
            m_HallCapitol.gameObject.SetActive(true);

            if (m_BuiltBuildings.Castle)
            {
                m_HallCapitol.ButtonImage.sprite = m_Green;
                m_HallCapitol.CornerImage.gameObject.SetActive(false);
                m_HallCapitol.Buildable = true;
            }
            else
            {
                m_HallCapitol.ButtonImage.sprite = m_Red;
                m_HallCapitol.CornerImage.gameObject.SetActive(true);
                m_HallCapitol.CornerImage.sprite = m_Cross;
                m_HallCapitol.Buildable = false;
            }

            m_HallTownHall.gameObject.SetActive(false);
            m_HallCityHall.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.TownHall)
        {
            m_HallCityHall.gameObject.SetActive(true);

            if (m_BuiltBuildings.Blacksmith &&
                m_BuiltBuildings.Market &&
                m_BuiltBuildings.MageGuild1)
            {
                m_HallCityHall.ButtonImage.sprite = m_Green;
                m_HallCityHall.CornerImage.gameObject.SetActive(false);
                m_HallCityHall.Buildable = true;
            }
            else
            {
                m_HallCityHall.ButtonImage.sprite = m_Red;
                m_HallCityHall.CornerImage.gameObject.SetActive(true);
                m_HallCityHall.CornerImage.sprite = m_Cross;
                m_HallCityHall.Buildable = false;
            }

            m_HallTownHall.gameObject.SetActive(false);
            m_HallCapitol.gameObject.SetActive(false);
        }
        else
        {
            m_HallTownHall.gameObject.SetActive(true);

            if (m_BuiltBuildings.Tavern)
            {
                m_HallTownHall.ButtonImage.sprite = m_Green;
                m_HallTownHall.CornerImage.gameObject.SetActive(false);
                m_HallTownHall.Buildable = true;
            }
            else
            {
                m_HallTownHall.ButtonImage.sprite = m_Red;
                m_HallTownHall.CornerImage.gameObject.SetActive(true);
                m_HallTownHall.CornerImage.sprite = m_Cross;
                m_HallTownHall.Buildable = false;
            }

            m_HallCityHall.gameObject.SetActive(false);
            m_HallCapitol.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Castle)
        {
            m_HallCastle.gameObject.SetActive(true);
            m_HallCastle.ButtonImage.sprite = m_Yellow;
            m_HallCastle.CornerImage.gameObject.SetActive(true);
            m_HallCastle.CornerImage.sprite = m_Tick;
            m_HallCastle.Buildable = false;

            m_HallFort.gameObject.SetActive(false);
            m_HallCitadel.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.Citadel)
        {
            m_HallCastle.gameObject.SetActive(true);

            m_HallCastle.ButtonImage.sprite = m_Green;
            m_HallCastle.CornerImage.gameObject.SetActive(false);
            m_HallCastle.Buildable = true;

            m_HallFort.gameObject.SetActive(false);
            m_HallCitadel.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.Fort)
        {
            m_HallCitadel.gameObject.SetActive(true);

            m_HallCitadel.ButtonImage.sprite = m_Green;
            m_HallCitadel.CornerImage.gameObject.SetActive(false);
            m_HallCitadel.Buildable = true;

            m_HallFort.gameObject.SetActive(false);
            m_HallCastle.gameObject.SetActive(false);
        }
        else
        {
            m_HallFort.gameObject.SetActive(true);

            m_HallFort.ButtonImage.sprite = m_Green;
            m_HallFort.CornerImage.gameObject.SetActive(false);
            m_HallFort.Buildable = true;

            m_HallCitadel.gameObject.SetActive(false);
            m_HallCastle.gameObject.SetActive(false);
        }

        m_HallBlacksmith.gameObject.SetActive(true);

        if (m_BuiltBuildings.Blacksmith)
        {
            m_HallBlacksmith.ButtonImage.sprite = m_Yellow;
            m_HallBlacksmith.CornerImage.gameObject.SetActive(true);
            m_HallBlacksmith.CornerImage.sprite = m_Tick;
            m_HallBlacksmith.Buildable = false;
        }
        else
        {
            m_HallBlacksmith.ButtonImage.sprite = m_Green;
            m_HallBlacksmith.CornerImage.gameObject.SetActive(false);
            m_HallBlacksmith.Buildable = true;
        }

        if (m_BuiltBuildings.Silo)
        {
            m_HallSilo.gameObject.SetActive(true);
            m_HallSilo.ButtonImage.sprite = m_Yellow;
            m_HallSilo.CornerImage.gameObject.SetActive(true);
            m_HallSilo.CornerImage.sprite = m_Tick;
            m_HallSilo.Buildable = false;

            m_HallMarket.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.Market)
        {
            m_HallSilo.gameObject.SetActive(true);
            m_HallSilo.ButtonImage.sprite = m_Green;
            m_HallSilo.CornerImage.gameObject.SetActive(false);
            m_HallSilo.Buildable = true;

            m_HallMarket.gameObject.SetActive(false);
        }
        else
        {
            m_HallMarket.gameObject.SetActive(true);
            m_HallMarket.ButtonImage.sprite = m_Green;
            m_HallMarket.CornerImage.gameObject.SetActive(false);
            m_HallMarket.Buildable = true;

            m_HallSilo.gameObject.SetActive(false);
        }
    }

    public virtual void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallTownHall.BuildingData)
        {
            BuildTownHall();
        }
        else if (a_BuildingData == m_HallCityHall.BuildingData)
        {
            BuildCityHall();
        }
        else if (a_BuildingData == m_HallCapitol.BuildingData)
        {
            BuildCapitol();
        }
        else if (a_BuildingData == m_HallFort.BuildingData)
        {
            BuildFort();
        }
        else if (a_BuildingData == m_HallCitadel.BuildingData)
        {
            BuildCitadel();
        }
        else if (a_BuildingData == m_HallCastle.BuildingData)
        {
            BuildCastle();
        }
        else if (a_BuildingData == m_HallBlacksmith.BuildingData)
        {
            BuildBlacksmith();
        }
        else if (a_BuildingData == m_HallMarket.BuildingData)
        {
            BuildMarket();
        }
        else if (a_BuildingData == m_HallSilo.BuildingData)
        {
            BuildSilo();
        }
    }

    protected virtual void BuildTownHall()
    {
        StartCoroutine(BuildBuilding(m_TownHall));
        StartCoroutine(RemoveBuilding(m_VillageHall.Image));
        m_BuiltBuildings.TownHall = true;
    }

    protected virtual void BuildCityHall()
    {
        StartCoroutine(BuildBuilding(m_CityHall));
        StartCoroutine(RemoveBuilding(m_TownHall.Image));
        m_BuiltBuildings.CityHall = true;
    }

    protected virtual void BuildCapitol()
    {
        StartCoroutine(BuildBuilding(m_Capitol));
        StartCoroutine(RemoveBuilding(m_CityHall.Image));
        m_BuiltBuildings.Capitol = true;
    }

    protected virtual void BuildFort()
    {
        StartCoroutine(BuildBuilding(m_Fort));
        m_BuiltBuildings.Fort = true;
    }

    protected virtual void BuildCitadel()
    {
        StartCoroutine(BuildBuilding(m_Citadel));
        StartCoroutine(RemoveBuilding(m_Fort.Image));
        m_BuiltBuildings.Citadel = true;
    }

    protected virtual void BuildCastle()
    {
        StartCoroutine(BuildBuilding(m_Castle));
        StartCoroutine(RemoveBuilding(m_Citadel.Image));
        m_BuiltBuildings.Castle = true;
    }

    protected virtual void BuildBlacksmith()
    {
        StartCoroutine(BuildBuilding(m_Blacksmith));
        m_BuiltBuildings.Blacksmith = true;
    }

    protected virtual void BuildMarket()
    {
        StartCoroutine(BuildBuilding(m_Market));
        m_BuiltBuildings.Market = true;
    }

    protected virtual void BuildSilo()
    {
        StartCoroutine(BuildBuilding(m_Silo));
        m_BuiltBuildings.Silo = true;
    }

    protected IEnumerator BuildBuilding(Building a_Building)
    {
        Image _Image = a_Building.Image;

        a_Building.gameObject.SetActive(true);

        Color _Color = new Color(1, 1, 1, 0);

        _Image.color = _Color;

        float _CurrentDuration = 0.0f;

        const float DURATION = 0.5f;

        while (_CurrentDuration < DURATION)
        {
            _CurrentDuration += Time.deltaTime;

            _Color.a = _CurrentDuration / DURATION;

            _Image.color = _Color;

            yield return null;
        }

        a_Building.Outline.SetActive(true);

        yield return new WaitForSeconds(0.75f);

        a_Building.Outline.SetActive(false);
    }

    // For things like the houses in Rampart
    protected IEnumerator BuildBuilding(Image a_Building)
    {
        a_Building.gameObject.SetActive(true);
        Color _Color = new Color(1, 1, 1, 0);

        a_Building.color = _Color;

        const float DURATION = 0.5f;

        float _CurrentDuration = 0.0f;

        while (_CurrentDuration < DURATION)
        {
            _CurrentDuration += Time.deltaTime;

            _Color.a = _CurrentDuration / DURATION;

            a_Building.color = _Color;

            yield return null;
        }
    }

    protected IEnumerator RemoveBuilding(Image a_Building)
    {
        Color _Color = new Color(1, 1, 1, 1);

        a_Building.color = _Color;

        float _CurrentDuration = 0.0f;

        const float DURATION = 0.5f;

        while (_CurrentDuration < DURATION)
        {
            _CurrentDuration += Time.deltaTime;

            _Color.a = DURATION - _CurrentDuration;

            a_Building.color = _Color;

            yield return null;
        }

        a_Building.gameObject.SetActive(false);
    }
}
