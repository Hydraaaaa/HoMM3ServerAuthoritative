using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_GnollG;
    [SerializeField] Building m_Gnoll2G;
    [SerializeField] Building m_GlyphsOfFear;
    [SerializeField] Building m_BloodObelisk;
    [SerializeField] Building m_CageOfWarlords;
    [SerializeField] Building m_Shipyard;
    [SerializeField] Building m_ShipyardShip;

    public override void SetBuildings(List<byte> a_Bytes)
    {
        base.SetBuildings(a_Bytes);

        if ((a_Bytes[3] & 1) == 1)
        {
            if ((a_Bytes[2] & 128) == 128)
            {
                m_GnollG.gameObject.SetActive(false);
                m_Gnoll2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_GnollG.gameObject.SetActive(true);
                m_Gnoll2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_GnollG.gameObject.SetActive(false);
            m_Gnoll2G.gameObject.SetActive(false);
        }

        m_BloodObelisk.gameObject.SetActive((a_Bytes[2] & 4) == 4);
        m_GlyphsOfFear.gameObject.SetActive((a_Bytes[2] & 8) == 8);
        m_CageOfWarlords.gameObject.SetActive((a_Bytes[2] & 16) == 16);

        m_Shipyard.gameObject.SetActive((a_Bytes[2] & 1) == 1);
        m_ShipyardShip.gameObject.SetActive(false);
    }
}
