using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecropolisBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_SkeletonG;
    [SerializeField] Building m_Skeleton2G;
    [SerializeField] Building m_SkeletonTransformer;
    [SerializeField] Building m_NecromancyAmplifier;
    [SerializeField] Building m_CoverOfDarkness;

    public override void SetBuildings(BuildingData a_Data)
    {
        base.SetBuildings(a_Data);

        if (a_Data.Dwelling1Growth)
        {
            if (a_Data.Dwelling1Up)
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
    }
}
