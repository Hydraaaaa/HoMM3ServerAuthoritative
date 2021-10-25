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

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

        if (a_Data.Dwelling1Growth)
        {
            if (a_Data.Dwelling1Up)
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

        if (a_Data.Dwelling3Growth)
        {
            if (a_Data.Dwelling3Up)
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
    }
}
