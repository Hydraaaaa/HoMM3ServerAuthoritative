using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfluxBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_PixieG;
    [SerializeField] Building m_Pixie2G;

    public override void SetBuildings(BuiltBuildings a_Data)
    {
        base.SetBuildings(a_Data);

        if (a_Data.Dwelling1Growth)
        {
            if (a_Data.Dwelling1Up)
            {
                m_PixieG.gameObject.SetActive(false);
                m_Pixie2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_PixieG.gameObject.SetActive(true);
                m_Pixie2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_PixieG.gameObject.SetActive(false);
            m_Pixie2G.gameObject.SetActive(false);
        }
    }
}
