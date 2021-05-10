using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecropolisBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_SkeletonG;
    [SerializeField] Building m_Skeleton2G;
    [SerializeField] Building m_Shipyard;
    [SerializeField] Building m_ShipyardShip;
    [SerializeField] Building m_SkeletonTransformer;
    [SerializeField] Building m_NecromancyAmplifier;
    [SerializeField] Building m_CoverOfDarkness;

    public override void SetBuildings(List<byte> a_Bytes)
    {
        base.SetBuildings(a_Bytes);

        if ((a_Bytes[3] & 1) == 1)
        {
            if ((a_Bytes[2] & 128) == 128)
            {
                m_SkeletonG.gameObject.SetActive(false);
                m_Skeleton2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_SkeletonG.gameObject.SetActive(true);
                m_Skeleton2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_SkeletonG.gameObject.SetActive(false);
            m_Skeleton2G.gameObject.SetActive(false);
        }

        m_Shipyard.gameObject.SetActive((a_Bytes[2] & 1) == 1);
        m_ShipyardShip.gameObject.SetActive(false);

        m_CoverOfDarkness.gameObject.SetActive((a_Bytes[2] & 4) == 4);
        m_NecromancyAmplifier.gameObject.SetActive((a_Bytes[2] & 8) == 8);
        m_SkeletonTransformer.gameObject.SetActive((a_Bytes[2] & 16) == 16);
    }
}
