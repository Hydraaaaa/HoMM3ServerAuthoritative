using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongholdBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_GoblinG;
    [SerializeField] Building m_Goblin2G;
    [SerializeField] Building m_HallOfValhalla;
    [SerializeField] Building m_EscapeTunnel;
    [SerializeField] Building m_FreelancersGuild;
    [SerializeField] Building m_BallistaYard;

    public override void SetBuildings(List<byte> a_Bytes)
    {
        base.SetBuildings(a_Bytes);

        if ((a_Bytes[3] & 1) == 1)
        {
            if ((a_Bytes[2] & 128) == 128)
            {
                m_GoblinG.gameObject.SetActive(false);
                m_Goblin2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_GoblinG.gameObject.SetActive(true);
                m_Goblin2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_GoblinG.gameObject.SetActive(false);
            m_Goblin2G.gameObject.SetActive(false);
        }

        m_EscapeTunnel.gameObject.SetActive((a_Bytes[2] & 4) == 4);
        m_FreelancersGuild.gameObject.SetActive((a_Bytes[2] & 8) == 8);
        m_BallistaYard.gameObject.SetActive((a_Bytes[2] & 16) == 16);
        m_HallOfValhalla.gameObject.SetActive((a_Bytes[2] & 32) == 32);
    }
}
