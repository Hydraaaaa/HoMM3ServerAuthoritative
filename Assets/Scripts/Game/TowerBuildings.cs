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

    public override void SetBuildings(BuildingData a_Data)
    {
        base.SetBuildings(a_Data);

        if (a_Data.Dwelling2Growth)
        {
            if (a_Data.Dwelling2Up)
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
    }
}
