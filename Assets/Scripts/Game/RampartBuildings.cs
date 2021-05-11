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

    [SerializeField] Building m_DwarfG;
    [SerializeField] Building m_Dwarf2G;
    [SerializeField] Building m_DendroidG;
    [SerializeField] Building m_Dendroid2G;

    public override void SetBuildings(BuildingData a_Data)
    {
        base.SetBuildings(a_Data);

        m_VillageHouses.SetActive(true);
        m_TownHouses.SetActive(a_Data.TownHall);
        m_CityHouses.SetActive(a_Data.CityHall);
        m_CapitolHouses.SetActive(a_Data.Capitol);

        // Fountain of Fortune replacing Mystic Pond
        if (a_Data.FactionBuilding2)
        {
            m_FactionBuilding1.gameObject.SetActive(false);
        }

        if (a_Data.Dwelling2Growth)
        {
            if (a_Data.Dwelling2Up)
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

        if (a_Data.Dwelling5Growth)
        {
            if (a_Data.Dwelling5Up)
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
