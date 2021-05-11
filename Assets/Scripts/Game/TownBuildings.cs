using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public virtual void SetBuildings(BuildingData a_Data)
    {
        if (a_Data.Capitol)
        {
            m_VillageHall.gameObject.SetActive(false);
            m_TownHall.gameObject.SetActive(false);
            m_CityHall.gameObject.SetActive(false);
            m_Capitol.gameObject.SetActive(true);
        }
        else if (a_Data.CityHall)
        {
            m_VillageHall.gameObject.SetActive(false);
            m_TownHall.gameObject.SetActive(false);
            m_CityHall.gameObject.SetActive(true);
            m_Capitol.gameObject.SetActive(false);
        }
        else if (a_Data.TownHall)
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

        if (a_Data.Castle)
        {
            m_Fort.gameObject.SetActive(false);
            m_Citadel.gameObject.SetActive(false);
            m_Castle.gameObject.SetActive(true);
        }
        else if (a_Data.Citadel)
        {
            m_Fort.gameObject.SetActive(false);
            m_Citadel.gameObject.SetActive(true);
            m_Castle.gameObject.SetActive(false);
        }
        else if (a_Data.Fort)
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

        m_Tavern.gameObject.SetActive(a_Data.Tavern);

        m_Blacksmith.gameObject.SetActive(a_Data.Blacksmith);
        m_Market.gameObject.SetActive(a_Data.Market);
        m_Silo.gameObject.SetActive(a_Data.Silo);
        m_ArtifactMerchants?.gameObject.SetActive(a_Data.ArtifactMerchants);

        if (a_Data.MageGuild5)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(true);
        }
        else if (a_Data.MageGuild4)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(true);
            m_MageGuild5?.gameObject.SetActive(false);
        }
        else if (a_Data.MageGuild3)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(true);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(false);
        }
        else if (a_Data.MageGuild2)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(true);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(false);
        }
        else if (a_Data.MageGuild1)
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

        m_Shipyard?.gameObject.SetActive(a_Data.Shipyard);
        m_ShipyardShip?.gameObject.SetActive(false);
        m_Grail.gameObject.SetActive(a_Data.Grail);

        m_FactionBuilding1.gameObject.SetActive(a_Data.FactionBuilding1);
        m_FactionBuilding2?.gameObject.SetActive(a_Data.FactionBuilding2);
        m_FactionBuilding3?.gameObject.SetActive(a_Data.FactionBuilding3);
        m_FactionBuilding4?.gameObject.SetActive(a_Data.FactionBuilding4);

        if (a_Data.Dwelling1Up)
        {
            m_Dwelling1.gameObject.SetActive(false);
            m_Dwelling1Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling1.gameObject.SetActive(a_Data.Dwelling1);
            m_Dwelling1Up.gameObject.SetActive(false);
        }

        if (a_Data.Dwelling2Up)
        {
            m_Dwelling2.gameObject.SetActive(false);
            m_Dwelling2Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling2.gameObject.SetActive(a_Data.Dwelling2);
            m_Dwelling2Up.gameObject.SetActive(false);
        }

        if (a_Data.Dwelling3Up)
        {
            m_Dwelling3.gameObject.SetActive(false);
            m_Dwelling3Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling3.gameObject.SetActive(a_Data.Dwelling3);
            m_Dwelling3Up.gameObject.SetActive(false);
        }

        if (a_Data.Dwelling4Up)
        {
            m_Dwelling4.gameObject.SetActive(false);
            m_Dwelling4Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling4.gameObject.SetActive(a_Data.Dwelling4);
            m_Dwelling4Up.gameObject.SetActive(false);
        }

        if (a_Data.Dwelling5Up)
        {
            m_Dwelling5.gameObject.SetActive(false);
            m_Dwelling5Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling5.gameObject.SetActive(a_Data.Dwelling5);
            m_Dwelling5Up.gameObject.SetActive(false);
        }

        if (a_Data.Dwelling6Up)
        {
            m_Dwelling6.gameObject.SetActive(false);
            m_Dwelling6Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling6.gameObject.SetActive(a_Data.Dwelling6);
            m_Dwelling6Up.gameObject.SetActive(false);
        }

        if (a_Data.Dwelling7Up)
        {
            m_Dwelling7.gameObject.SetActive(false);
            m_Dwelling7Up.gameObject.SetActive(true);
        }
        else
        {
            m_Dwelling7.gameObject.SetActive(a_Data.Dwelling7);
            m_Dwelling7Up.gameObject.SetActive(false);
        }
    }
}
