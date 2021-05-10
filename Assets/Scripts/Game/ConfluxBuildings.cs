using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfluxBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_PixieG;
    [SerializeField] Building m_Pixie2G;
    [SerializeField] Building m_MagicUniversity;
    [SerializeField] Building m_Shipyard;
    [SerializeField] Building m_ShipyardShip;
    [SerializeField] Building m_ArtifactMerchants;

    public override void SetBuildings(List<byte> a_Bytes)
    {
        base.SetBuildings(a_Bytes);

        if ((a_Bytes[3] & 1) == 1)
        {
            if ((a_Bytes[2] & 128) == 128)
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

        m_MagicUniversity.gameObject.SetActive((a_Bytes[2] & 4) == 4);

        m_ArtifactMerchants.gameObject.SetActive((a_Bytes[1] & 4) == 4);

        m_Shipyard.gameObject.SetActive((a_Bytes[2] & 1) == 1);
        m_ShipyardShip.gameObject.SetActive(false);
    }
}
