using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_WarrenG;
    [SerializeField] Building m_Warren2G;
    [SerializeField] Building m_ManaVortex;
    [SerializeField] Building m_ManaVortex2;
    [SerializeField] Building m_BattleScholarAcademy;
    [SerializeField] Building m_SummoningPortal;
    [SerializeField] Building m_ArtifactMerchants;

    public override void SetBuildings(List<byte> a_Bytes)
    {
        base.SetBuildings(a_Bytes);

        if ((a_Bytes[3] & 1) == 1)
        {
            if ((a_Bytes[2] & 128) == 128)
            {
                m_WarrenG.gameObject.SetActive(false);
                m_Warren2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_WarrenG.gameObject.SetActive(true);
                m_Warren2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_WarrenG.gameObject.SetActive(false);
            m_Warren2G.gameObject.SetActive(false);
        }

        if ((a_Bytes[2] & 4) == 4)
        {
            if ((a_Bytes[1] & 128) == 128)
            {
                m_ManaVortex2.gameObject.SetActive(true);
                m_ManaVortex.gameObject.SetActive(false);
            }
            else
            {
                m_ManaVortex2.gameObject.SetActive(false);
                m_ManaVortex.gameObject.SetActive(true);
            }
        }
        else
        {
            m_ManaVortex2.gameObject.SetActive(false);
            m_ManaVortex.gameObject.SetActive(false);
        }

        m_SummoningPortal.gameObject.SetActive((a_Bytes[2] & 8) == 8);
        m_BattleScholarAcademy.gameObject.SetActive((a_Bytes[2] & 16) == 16);

        m_ArtifactMerchants.gameObject.SetActive((a_Bytes[1] & 4) == 4);
    }
}
