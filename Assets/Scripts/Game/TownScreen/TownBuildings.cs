using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TownBuildings : MonoBehaviour
{
    [SerializeField] protected PlayerResources m_Resources;

    [Space]

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
    protected bool m_CanBuildShipyard;

    public virtual void SetBuildings(BuiltBuildings a_BuiltBuildings, bool a_CanBuildShipyard)
    {
        m_BuiltBuildings = a_BuiltBuildings;
        m_CanBuildShipyard = a_CanBuildShipyard;

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
            SetHallBuildingBuilt(m_HallCapitol);

            m_HallCapitol.gameObject.SetActive(true);
            m_HallTownHall.gameObject.SetActive(false);
            m_HallCityHall.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.CityHall)
        {
            SetHallBuildingNotBuilt(m_HallCapitol);

            m_HallCapitol.gameObject.SetActive(true);
            m_HallTownHall.gameObject.SetActive(false);
            m_HallCityHall.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.TownHall)
        {
            SetHallBuildingNotBuilt(m_HallCityHall);

            m_HallCityHall.gameObject.SetActive(true);
            m_HallTownHall.gameObject.SetActive(false);
            m_HallCapitol.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallTownHall);

            m_HallTownHall.gameObject.SetActive(true);
            m_HallCityHall.gameObject.SetActive(false);
            m_HallCapitol.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Castle)
        {
            SetHallBuildingBuilt(m_HallCastle);

            m_HallFort.gameObject.SetActive(false);
            m_HallCitadel.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.Citadel)
        {
            SetHallBuildingNotBuilt(m_HallCastle);

            m_HallCastle.gameObject.SetActive(true);
            m_HallFort.gameObject.SetActive(false);
            m_HallCitadel.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.Fort)
        {
            SetHallBuildingNotBuilt(m_HallCitadel);

            m_HallCitadel.gameObject.SetActive(true);
            m_HallFort.gameObject.SetActive(false);
            m_HallCastle.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallFort);

            m_HallFort.gameObject.SetActive(true);
            m_HallCitadel.gameObject.SetActive(false);
            m_HallCastle.gameObject.SetActive(false);
        }

        m_HallBlacksmith.gameObject.SetActive(true);

        if (m_BuiltBuildings.Blacksmith)
        {
            SetHallBuildingBuilt(m_HallBlacksmith);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallBlacksmith);
        }

        if (m_BuiltBuildings.Silo)
        {
            SetHallBuildingBuilt(m_HallSilo);

            m_HallSilo.gameObject.SetActive(true);
            m_HallMarket.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.Market)
        {
            SetHallBuildingNotBuilt(m_HallSilo);

            m_HallSilo.gameObject.SetActive(true);
            m_HallMarket.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallMarket);

            m_HallMarket.gameObject.SetActive(true);
            m_HallSilo.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling1)
        {
            if (m_BuiltBuildings.Dwelling1Up)
            {
                SetHallBuildingBuilt(m_HallDwelling1Up);
            }
            else
            {
                SetHallBuildingNotBuilt(m_HallDwelling1Up);
            }

            m_HallDwelling1Up.gameObject.SetActive(true);
            m_HallDwelling1.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallDwelling1);

            m_HallDwelling1.gameObject.SetActive(true);
            m_HallDwelling1Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling2)
        {
            if (m_BuiltBuildings.Dwelling2Up)
            {
                SetHallBuildingBuilt(m_HallDwelling2Up);
            }
            else
            {
                SetHallBuildingNotBuilt(m_HallDwelling2Up);
            }

            m_HallDwelling2Up.gameObject.SetActive(true);
            m_HallDwelling2.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallDwelling2);

            m_HallDwelling2.gameObject.SetActive(true);
            m_HallDwelling2Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling3)
        {
            if (m_BuiltBuildings.Dwelling3Up)
            {
                SetHallBuildingBuilt(m_HallDwelling3Up);
            }
            else
            {
                SetHallBuildingNotBuilt(m_HallDwelling3Up);
            }

            m_HallDwelling3Up.gameObject.SetActive(true);
            m_HallDwelling3.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallDwelling3);

            m_HallDwelling3.gameObject.SetActive(true);
            m_HallDwelling3Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling4)
        {
            if (m_BuiltBuildings.Dwelling4Up)
            {
                SetHallBuildingBuilt(m_HallDwelling4Up);
            }
            else
            {
                SetHallBuildingNotBuilt(m_HallDwelling4Up);
            }

            m_HallDwelling4Up.gameObject.SetActive(true);
            m_HallDwelling4.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallDwelling4);

            m_HallDwelling4.gameObject.SetActive(true);
            m_HallDwelling4Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling5)
        {
            if (m_BuiltBuildings.Dwelling5Up)
            {
                SetHallBuildingBuilt(m_HallDwelling5Up);
            }
            else
            {
                SetHallBuildingNotBuilt(m_HallDwelling5Up);
            }

            m_HallDwelling5Up.gameObject.SetActive(true);
            m_HallDwelling5.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallDwelling5);

            m_HallDwelling5.gameObject.SetActive(true);
            m_HallDwelling5Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling6)
        {
            if (m_BuiltBuildings.Dwelling6Up)
            {
                SetHallBuildingBuilt(m_HallDwelling6Up);
            }
            else
            {
                SetHallBuildingNotBuilt(m_HallDwelling6Up);
            }

            m_HallDwelling6Up.gameObject.SetActive(true);
            m_HallDwelling6.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallDwelling6);

            m_HallDwelling6.gameObject.SetActive(true);
            m_HallDwelling6Up.gameObject.SetActive(false);
        }

        if (m_BuiltBuildings.Dwelling7)
        {
            if (m_BuiltBuildings.Dwelling7Up)
            {
                SetHallBuildingBuilt(m_HallDwelling7Up);
            }
            else
            {
                SetHallBuildingNotBuilt(m_HallDwelling7Up);
            }

            m_HallDwelling7Up.gameObject.SetActive(true);
            m_HallDwelling7.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallDwelling7);

            m_HallDwelling7.gameObject.SetActive(true);
            m_HallDwelling7Up.gameObject.SetActive(false);
        }
    }

    protected void SetHallBuildingNotBuilt(HallBuilding a_Building)
    {
        bool _RequirementsMet = true;

        if (a_Building.BuildingData.Requirements != null)
        {
            for (int i = 0; i < a_Building.BuildingData.Requirements.Requirements.Length; i++)
            {
                if (!IsBuildingBuilt(a_Building.BuildingData.Requirements.Requirements[i]))
                {
                    _RequirementsMet = false;
                }
            }
        }

        if (_RequirementsMet)
        {
            if (a_Building.BuildingData.GoldCost <= m_Resources.Gold &&
                a_Building.BuildingData.WoodCost <= m_Resources.Wood &&
                a_Building.BuildingData.OreCost <= m_Resources.Ore &&
                a_Building.BuildingData.MercuryCost <= m_Resources.Mercury &&
                a_Building.BuildingData.SulfurCost <= m_Resources.Sulfur &&
                a_Building.BuildingData.CrystalCost <= m_Resources.Crystals &&
                a_Building.BuildingData.GemCost <= m_Resources.Gems)
            {
                a_Building.ButtonImage.sprite = m_Green;
                a_Building.CornerImage.gameObject.SetActive(false);
                a_Building.Buildable = true;
            }
            else
            {
                a_Building.ButtonImage.sprite = m_Red;
                a_Building.CornerImage.gameObject.SetActive(true);
                a_Building.CornerImage.sprite = m_NoMoney;
                a_Building.Buildable = false;
            }
        }
        else
        {
            a_Building.ButtonImage.sprite = m_Red;
            a_Building.CornerImage.gameObject.SetActive(true);
            a_Building.CornerImage.sprite = m_Cross;
            a_Building.Buildable = false;
        }
    }

    protected void SetHallBuildingBuilt(HallBuilding a_Building)
    {
        a_Building.ButtonImage.sprite = m_Yellow;
        a_Building.CornerImage.gameObject.SetActive(true);
        a_Building.CornerImage.sprite = m_Tick;
        a_Building.Buildable = false;
    }

    protected void SetHallBuildingUnbuildable(HallBuilding a_Building)
    {
        a_Building.ButtonImage.sprite = m_Grey;
        a_Building.CornerImage.gameObject.SetActive(true);
        a_Building.CornerImage.sprite = m_Cross;
        a_Building.Buildable = false;
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
        else if (a_BuildingData == m_HallDwelling1.BuildingData)
        {
            BuildDwelling1();
        }
        else if (a_BuildingData == m_HallDwelling1Up.BuildingData)
        {
            BuildDwelling1Up();
        }
        else if (a_BuildingData == m_HallDwelling2.BuildingData)
        {
            BuildDwelling2();
        }
        else if (a_BuildingData == m_HallDwelling2Up.BuildingData)
        {
            BuildDwelling2Up();
        }
        else if (a_BuildingData == m_HallDwelling3.BuildingData)
        {
            BuildDwelling3();
        }
        else if (a_BuildingData == m_HallDwelling3Up.BuildingData)
        {
            BuildDwelling3Up();
        }
        else if (a_BuildingData == m_HallDwelling4.BuildingData)
        {
            BuildDwelling4();
        }
        else if (a_BuildingData == m_HallDwelling4Up.BuildingData)
        {
            BuildDwelling4Up();
        }
        else if (a_BuildingData == m_HallDwelling5.BuildingData)
        {
            BuildDwelling5();
        }
        else if (a_BuildingData == m_HallDwelling5Up.BuildingData)
        {
            BuildDwelling5Up();
        }
        else if (a_BuildingData == m_HallDwelling6.BuildingData)
        {
            BuildDwelling6();
        }
        else if (a_BuildingData == m_HallDwelling6Up.BuildingData)
        {
            BuildDwelling6Up();
        }
        else if (a_BuildingData == m_HallDwelling7.BuildingData)
        {
            BuildDwelling7();
        }
        else if (a_BuildingData == m_HallDwelling7Up.BuildingData)
        {
            BuildDwelling7Up();
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

    protected virtual void BuildMageGuild1()
    {
        StartCoroutine(BuildBuilding(m_MageGuild1));
        m_BuiltBuildings.MageGuild1 = true;
    }

    protected virtual void BuildMageGuild2()
    {
        StartCoroutine(BuildBuilding(m_MageGuild2));
        StartCoroutine(RemoveBuilding(m_MageGuild1.Image));
        m_BuiltBuildings.MageGuild2 = true;
    }

    protected virtual void BuildMageGuild3()
    {
        StartCoroutine(BuildBuilding(m_MageGuild3));
        StartCoroutine(RemoveBuilding(m_MageGuild2.Image));
        m_BuiltBuildings.MageGuild3 = true;
    }

    protected virtual void BuildMageGuild4()
    {
        StartCoroutine(BuildBuilding(m_MageGuild4));
        StartCoroutine(RemoveBuilding(m_MageGuild3.Image));
        m_BuiltBuildings.MageGuild4 = true;
    }

    protected virtual void BuildMageGuild5()
    {
        StartCoroutine(BuildBuilding(m_MageGuild5));
        StartCoroutine(RemoveBuilding(m_MageGuild4.Image));
        m_BuiltBuildings.MageGuild5 = true;
    }

    protected virtual void BuildDwelling1()
    {
        StartCoroutine(BuildBuilding(m_Dwelling1));
        m_BuiltBuildings.Dwelling1 = true;
    }

    protected virtual void BuildDwelling1Up()
    {
        StartCoroutine(BuildBuilding(m_Dwelling1Up));
        StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        m_BuiltBuildings.Dwelling1Up = true;
    }

    protected virtual void BuildDwelling2()
    {
        StartCoroutine(BuildBuilding(m_Dwelling2));
        m_BuiltBuildings.Dwelling2 = true;
    }

    protected virtual void BuildDwelling2Up()
    {
        StartCoroutine(BuildBuilding(m_Dwelling2Up));
        StartCoroutine(RemoveBuilding(m_Dwelling2.Image));
        m_BuiltBuildings.Dwelling2Up = true;
    }

    protected virtual void BuildDwelling3()
    {
        StartCoroutine(BuildBuilding(m_Dwelling3));
        m_BuiltBuildings.Dwelling3 = true;
    }

    protected virtual void BuildDwelling3Up()
    {
        StartCoroutine(BuildBuilding(m_Dwelling3Up));
        StartCoroutine(RemoveBuilding(m_Dwelling3.Image));
        m_BuiltBuildings.Dwelling3Up = true;
    }

    protected virtual void BuildDwelling4()
    {
        StartCoroutine(BuildBuilding(m_Dwelling4));
        m_BuiltBuildings.Dwelling4 = true;
    }

    protected virtual void BuildDwelling4Up()
    {
        StartCoroutine(BuildBuilding(m_Dwelling4Up));
        StartCoroutine(RemoveBuilding(m_Dwelling4.Image));
        m_BuiltBuildings.Dwelling4Up = true;
    }

    protected virtual void BuildDwelling5()
    {
        StartCoroutine(BuildBuilding(m_Dwelling5));
        m_BuiltBuildings.Dwelling5 = true;
    }

    protected virtual void BuildDwelling5Up()
    {
        StartCoroutine(BuildBuilding(m_Dwelling5Up));
        StartCoroutine(RemoveBuilding(m_Dwelling5.Image));
        m_BuiltBuildings.Dwelling5Up = true;
    }

    protected virtual void BuildDwelling6()
    {
        StartCoroutine(BuildBuilding(m_Dwelling6));
        m_BuiltBuildings.Dwelling6 = true;
    }

    protected virtual void BuildDwelling6Up()
    {
        StartCoroutine(BuildBuilding(m_Dwelling6Up));
        StartCoroutine(RemoveBuilding(m_Dwelling6.Image));
        m_BuiltBuildings.Dwelling6Up = true;
    }

    protected virtual void BuildDwelling7()
    {
        StartCoroutine(BuildBuilding(m_Dwelling7));
        m_BuiltBuildings.Dwelling7 = true;
    }

    protected virtual void BuildDwelling7Up()
    {
        StartCoroutine(BuildBuilding(m_Dwelling7Up));
        StartCoroutine(RemoveBuilding(m_Dwelling7.Image));
        m_BuiltBuildings.Dwelling7Up = true;
    }

    protected virtual void BuildFactionBuilding1()
    {
        StartCoroutine(BuildBuilding(m_FactionBuilding1));
        m_BuiltBuildings.FactionBuilding1 = true;
    }

    protected virtual void BuildFactionBuilding2()
    {
        StartCoroutine(BuildBuilding(m_FactionBuilding2));
        m_BuiltBuildings.FactionBuilding2 = true;
    }

    protected virtual void BuildFactionBuilding3()
    {
        StartCoroutine(BuildBuilding(m_FactionBuilding3));
        m_BuiltBuildings.FactionBuilding3 = true;
    }

    protected virtual void BuildFactionBuilding4()
    {
        StartCoroutine(BuildBuilding(m_FactionBuilding4));
        m_BuiltBuildings.FactionBuilding4 = true;
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

    // This should probably be moved to some kind of dictionary solution
    public virtual bool IsBuildingBuilt(BuildingRequirements a_Building)
    {
        if (a_Building == m_HallTownHall.BuildingData.Requirements)
        {
            return m_BuiltBuildings.TownHall;
        }
        else if (a_Building == m_HallCityHall.BuildingData.Requirements)
        {
            return m_BuiltBuildings.CityHall;
        }
        else if (a_Building == m_HallCapitol.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Capitol;
        }
        else if (a_Building == m_HallFort.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Fort;
        }
        else if (a_Building == m_HallCitadel.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Citadel;
        }
        else if (a_Building == m_HallCastle.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Castle;
        }
        else if (a_Building == m_HallBlacksmith.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Blacksmith;
        }
        else if (a_Building == m_HallMarket.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Market;
        }
        else if (a_Building == m_HallSilo.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Silo;
        }
        else if (a_Building == m_HallTavern.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Tavern;
        }
        else if (a_Building == m_HallDwelling1.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling1;
        }
        else if (a_Building == m_HallDwelling1Up.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling1Up;
        }
        else if (a_Building == m_HallDwelling2.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling2;
        }
        else if (a_Building == m_HallDwelling2Up.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling2Up;
        }
        else if (a_Building == m_HallDwelling3.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling3;
        }
        else if (a_Building == m_HallDwelling3Up.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling3Up;
        }
        else if (a_Building == m_HallDwelling4.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling4;
        }
        else if (a_Building == m_HallDwelling4Up.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling4Up;
        }
        else if (a_Building == m_HallDwelling5.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling5;
        }
        else if (a_Building == m_HallDwelling5Up.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling5Up;
        }
        else if (a_Building == m_HallDwelling6.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling6;
        }
        else if (a_Building == m_HallDwelling6Up.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling6Up;
        }
        else if (a_Building == m_HallDwelling7.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling7;
        }
        else if (a_Building == m_HallDwelling7Up.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling7Up;
        }
        else
        {
            return false;
        }
    }
}
