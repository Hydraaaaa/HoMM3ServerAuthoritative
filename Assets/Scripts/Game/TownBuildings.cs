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

    [SerializeField] protected Building m_MageGuild1;
    [SerializeField] protected Building m_MageGuild2;
    [SerializeField] protected Building m_MageGuild3;
    [SerializeField] protected Building m_MageGuild4;
    [SerializeField] protected Building m_MageGuild5;

    [SerializeField] protected Building m_Grail;

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

    public virtual void SetBuildings(List<byte> a_Bytes)
    {
        if (a_Bytes == null ||
            a_Bytes.Count == 0)
        {
            Debug.Log($"!! NULL");
            return;
        }

        if ((a_Bytes[0] & 4) == 4)
        {
            m_VillageHall.gameObject.SetActive(false);
            m_TownHall.gameObject.SetActive(false);
            m_CityHall.gameObject.SetActive(false);
            m_Capitol.gameObject.SetActive(true);
        }
        else if ((a_Bytes[0] & 2) == 2)
        {
            m_VillageHall.gameObject.SetActive(false);
            m_TownHall.gameObject.SetActive(false);
            m_CityHall.gameObject.SetActive(true);
            m_Capitol.gameObject.SetActive(false);
        }
        else if ((a_Bytes[0] & 1) == 1)
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

        if ((a_Bytes[0] & 32) == 32)
        {
            m_Fort.gameObject.SetActive(false);
            m_Citadel.gameObject.SetActive(false);
            m_Castle.gameObject.SetActive(true);
        }
        else if ((a_Bytes[0] & 16) == 16)
        {
            m_Fort.gameObject.SetActive(false);
            m_Citadel.gameObject.SetActive(true);
            m_Castle.gameObject.SetActive(false);
        }
        else if ((a_Bytes[0] & 8) == 8)
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

        m_Tavern.gameObject.SetActive((a_Bytes[0] & 64) == 64);

        m_Blacksmith.gameObject.SetActive((a_Bytes[0] & 128) == 128);
        m_Market.gameObject.SetActive((a_Bytes[1] & 1) == 1);
        m_Silo.gameObject.SetActive((a_Bytes[1] & 2) == 2);

        if ((a_Bytes[1] & 128) == 128)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(true);
        }
        else if ((a_Bytes[1] & 64) == 64)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(true);
            m_MageGuild5?.gameObject.SetActive(false);
        }
        else if ((a_Bytes[1] & 32) == 32)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(false);
            m_MageGuild3.gameObject.SetActive(true);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(false);
        }
        else if ((a_Bytes[1] & 16) == 16)
        {
            m_MageGuild1.gameObject.SetActive(false);
            m_MageGuild2.gameObject.SetActive(true);
            m_MageGuild3.gameObject.SetActive(false);
            m_MageGuild4?.gameObject.SetActive(false);
            m_MageGuild5?.gameObject.SetActive(false);
        }
        else if ((a_Bytes[1] & 8) == 8)
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

        m_Grail.gameObject.SetActive((a_Bytes[2] & 2) == 2);

        if ((a_Bytes[2] & 128) == 128)
        {
            m_Dwelling1.gameObject.SetActive(false);
            m_Dwelling1Up.gameObject.SetActive(true);
        }
        else if ((a_Bytes[2] & 64) == 64)
        {
            m_Dwelling1.gameObject.SetActive(true);
            m_Dwelling1Up.gameObject.SetActive(false);
        }
        else
        {
            m_Dwelling1.gameObject.SetActive(false);
            m_Dwelling1Up.gameObject.SetActive(false);
        }

        if ((a_Bytes[3] & 4) == 4)
        {
            m_Dwelling2.gameObject.SetActive(false);
            m_Dwelling2Up.gameObject.SetActive(true);
        }
        else if ((a_Bytes[3] & 2) == 2)
        {
            m_Dwelling2.gameObject.SetActive(true);
            m_Dwelling2Up.gameObject.SetActive(false);
        }
        else
        {
            m_Dwelling2.gameObject.SetActive(false);
            m_Dwelling2Up.gameObject.SetActive(false);
        }

        if ((a_Bytes[3] & 32) == 32)
        {
            m_Dwelling3.gameObject.SetActive(false);
            m_Dwelling3Up.gameObject.SetActive(true);
        }
        else if ((a_Bytes[3] & 16) == 16)
        {
            m_Dwelling3.gameObject.SetActive(true);
            m_Dwelling3Up.gameObject.SetActive(false);
        }
        else
        {
            m_Dwelling3.gameObject.SetActive(false);
            m_Dwelling3Up.gameObject.SetActive(false);
        }

        if ((a_Bytes[4] & 1) == 1)
        {
            m_Dwelling4.gameObject.SetActive(false);
            m_Dwelling4Up.gameObject.SetActive(true);
        }
        else if ((a_Bytes[3] & 128) == 128)
        {
            m_Dwelling4.gameObject.SetActive(true);
            m_Dwelling4Up.gameObject.SetActive(false);
        }
        else
        {
            m_Dwelling4.gameObject.SetActive(false);
            m_Dwelling4Up.gameObject.SetActive(false);
        }

        if ((a_Bytes[4] & 4) == 4)
        {
            m_Dwelling5.gameObject.SetActive(false);
            m_Dwelling5Up.gameObject.SetActive(true);
        }
        else if ((a_Bytes[4] & 8) == 8)
        {
            m_Dwelling5.gameObject.SetActive(true);
            m_Dwelling5Up.gameObject.SetActive(false);
        }
        else
        {
            m_Dwelling5.gameObject.SetActive(false);
            m_Dwelling5Up.gameObject.SetActive(false);
        }

        if ((a_Bytes[4] & 64) == 64)
        {
            m_Dwelling6.gameObject.SetActive(false);
            m_Dwelling6Up.gameObject.SetActive(true);
        }
        else if ((a_Bytes[4] & 32) == 32)
        {
            m_Dwelling6.gameObject.SetActive(true);
            m_Dwelling6Up.gameObject.SetActive(false);
        }
        else
        {
            m_Dwelling6.gameObject.SetActive(false);
            m_Dwelling6Up.gameObject.SetActive(false);
        }

        if ((a_Bytes[5] & 1) == 1)
        {
            m_Dwelling7.gameObject.SetActive(false);
            m_Dwelling7Up.gameObject.SetActive(true);
        }
        else if ((a_Bytes[4] & 128) == 128)
        {
            m_Dwelling7.gameObject.SetActive(true);
            m_Dwelling7Up.gameObject.SetActive(false);
        }
        else
        {
            m_Dwelling7.gameObject.SetActive(false);
            m_Dwelling7Up.gameObject.SetActive(false);
        }
    }
}
