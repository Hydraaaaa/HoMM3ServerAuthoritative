using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampartBuildings : TownBuildings
{
    [Space]
    [SerializeField] GameObject m_VillageHouses;
    [SerializeField] GameObject m_TownHouses;
    [SerializeField] GameObject m_CityHouses;
    [SerializeField] GameObject m_CapitolHouses;

    [SerializeField] Building m_MysticPond;
    [SerializeField] Building m_FountainOfFortune;

    [SerializeField] Building m_DwarfG;
    [SerializeField] Building m_Dwarf2G;
    [SerializeField] Building m_DendroidG;
    [SerializeField] Building m_Dendroid2G;
    [SerializeField] Building m_Treasury;

    public override void SetBuildings(List<byte> a_Bytes)
    {
        base.SetBuildings(a_Bytes);

        m_VillageHouses.SetActive(true);
        m_TownHouses.SetActive((a_Bytes[0] & 1) == 1);
        m_CityHouses.SetActive((a_Bytes[0] & 2) == 2);
        m_CapitolHouses.SetActive((a_Bytes[0] & 4) == 4);

        if ((a_Bytes[2] & 8) == 8)
        {
            m_MysticPond.gameObject.SetActive(false);
            m_FountainOfFortune.gameObject.SetActive(true);
        }
        else if ((a_Bytes[2] & 4) == 4)
        {
            m_MysticPond.gameObject.SetActive(true);
            m_FountainOfFortune.gameObject.SetActive(false);
        }
        else
        {
            m_MysticPond.gameObject.SetActive(false);
            m_FountainOfFortune.gameObject.SetActive(false);
        }

        m_Treasury.gameObject.SetActive((a_Bytes[2] & 16) == 16);

        if ((a_Bytes[3] & 8) == 8)
        {
            if ((a_Bytes[3] & 4) == 4)
            {
                m_DwarfG.gameObject.SetActive(false);
                m_Dwarf2G.gameObject.SetActive(true);

                m_Dwelling2Up.gameObject.SetActive(false);
            }
            else
            {
                m_DwarfG.gameObject.SetActive(true);
                m_Dwarf2G.gameObject.SetActive(false);

                m_Dwelling2.gameObject.SetActive(false);
            }
        }
        else
        {
            m_DwarfG.gameObject.SetActive(false);
            m_Dwarf2G.gameObject.SetActive(false);
        }

        if ((a_Bytes[4] & 16) == 16)
        {
            if ((a_Bytes[4] & 8) == 8)
            {
                m_DendroidG.gameObject.SetActive(false);
                m_Dendroid2G.gameObject.SetActive(true);

                m_Dwelling5Up.gameObject.SetActive(false);
            }
            else
            {
                m_DendroidG.gameObject.SetActive(true);
                m_Dendroid2G.gameObject.SetActive(false);

                m_Dwelling5.gameObject.SetActive(false);
            }
        }
        else
        {
            m_DendroidG.gameObject.SetActive(false);
            m_Dendroid2G.gameObject.SetActive(false);
        }
    }
}
