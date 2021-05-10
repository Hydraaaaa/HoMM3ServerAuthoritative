using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernoBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_ImpG;
    [SerializeField] Building m_Imp2G;
    [SerializeField] Building m_HellhoundG;
    [SerializeField] Building m_Hellhound2G;
    [SerializeField] Building m_CastleGate;
    [SerializeField] Building m_BrimstoneStormclouds;
    [SerializeField] Building m_OrderOfFire;

    public override void SetBuildings(List<byte> a_Bytes)
    {
        base.SetBuildings(a_Bytes);

        if ((a_Bytes[3] & 1) == 1)
        {
            if ((a_Bytes[2] & 128) == 128)
            {
                m_ImpG.gameObject.SetActive(false);
                m_Imp2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_ImpG.gameObject.SetActive(true);
                m_Imp2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_ImpG.gameObject.SetActive(false);
            m_Imp2G.gameObject.SetActive(false);
        }

        if ((a_Bytes[3] & 128) == 128)
        {
            if ((a_Bytes[3] & 64) == 64)
            {
                m_HellhoundG.gameObject.SetActive(false);
                m_Hellhound2G.gameObject.SetActive(true);

                m_Dwelling3Up.gameObject.SetActive(false);
            }
            else
            {
                m_HellhoundG.gameObject.SetActive(true);
                m_Hellhound2G.gameObject.SetActive(false);

                m_Dwelling3.gameObject.SetActive(false);
            }
        }
        else
        {
            m_HellhoundG.gameObject.SetActive(false);
            m_Hellhound2G.gameObject.SetActive(false);
        }

        m_BrimstoneStormclouds.gameObject.SetActive((a_Bytes[2] & 4) == 4);
        m_CastleGate.gameObject.SetActive((a_Bytes[2] & 8) == 8);
        m_OrderOfFire.gameObject.SetActive((a_Bytes[2] & 16) == 16);
    }
}
