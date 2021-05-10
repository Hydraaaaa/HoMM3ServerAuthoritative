using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_GargoyleG;
    [SerializeField] Building m_Gargoyle2G;
    [SerializeField] Building m_Library;
    [SerializeField] Building m_WallOfKnowledge;
    [SerializeField] Building m_LookoutTower;
    [SerializeField] Building m_ArtifactMerchants;

    public override void SetBuildings(List<byte> a_Bytes)
    {
        base.SetBuildings(a_Bytes);

        if ((a_Bytes[3] & 8) == 8)
        {
            if ((a_Bytes[3] & 4) == 4)
            {
                m_GargoyleG.gameObject.SetActive(false);
                m_Gargoyle2G.gameObject.SetActive(true);

                m_Dwelling2Up.gameObject.SetActive(false);
            }
            else
            {
                m_GargoyleG.gameObject.SetActive(true);
                m_Gargoyle2G.gameObject.SetActive(false);

                m_Dwelling2.gameObject.SetActive(false);
            }
        }
        else
        {
            m_GargoyleG.gameObject.SetActive(false);
            m_Gargoyle2G.gameObject.SetActive(false);
        }

        m_Library.gameObject.SetActive((a_Bytes[2] & 4) == 4);
        m_WallOfKnowledge.gameObject.SetActive((a_Bytes[2] & 8) == 8);
        m_LookoutTower.gameObject.SetActive((a_Bytes[2] & 16) == 16);
        m_ArtifactMerchants.gameObject.SetActive((a_Bytes[1] & 4) == 4);
    }
}
