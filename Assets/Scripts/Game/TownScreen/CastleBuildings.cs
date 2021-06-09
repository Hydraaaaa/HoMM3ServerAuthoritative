using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBuildings : TownBuildings
{
    [Space]

    [SerializeField] Building m_GriffinG;
    [SerializeField] Building m_Griffin2G;
    [SerializeField] Building m_ShipyardMoat;
    [SerializeField] Building m_ShipyardMoatShip;

    [Space]

    [SerializeField] protected HallBuilding m_HallMageGuild1;
    [SerializeField] protected HallBuilding m_HallMageGuild2;
    [SerializeField] protected HallBuilding m_HallMageGuild3;
    [SerializeField] protected HallBuilding m_HallMageGuild4;
    [SerializeField] protected HallBuilding m_HallLighthouse;
    [SerializeField] protected HallBuilding m_HallBrotherhood;
    [SerializeField] protected HallBuilding m_HallStables;

    public override void SetBuildings(BuiltBuildings a_Data)
    {
        base.SetBuildings(a_Data);

        if (a_Data.Dwelling3Growth)
        {
            if (a_Data.Dwelling3Up)
            {
                m_GriffinG.gameObject.SetActive(false);
                m_Griffin2G.gameObject.SetActive(true);

                m_Dwelling3Up.gameObject.SetActive(false);
            }
            else
            {
                m_GriffinG.gameObject.SetActive(true);
                m_Griffin2G.gameObject.SetActive(false);

                m_Dwelling3.gameObject.SetActive(false);
            }
        }
        else
        {
            m_GriffinG.gameObject.SetActive(false);
            m_Griffin2G.gameObject.SetActive(false);
        }

        // Brotherhood of the Sword
        if (a_Data.FactionBuilding2)
        {
            m_Tavern.gameObject.SetActive(false);
        }

        if (a_Data.Shipyard)
        {
            if (a_Data.Citadel || a_Data.Castle)
            {
                m_Shipyard.gameObject.SetActive(false);
                m_ShipyardMoat.gameObject.SetActive(true);
            }
            else
            {
                m_Shipyard.gameObject.SetActive(true);
                m_ShipyardMoat.gameObject.SetActive(false);
            }
        }
        else
        {
            m_Shipyard.gameObject.SetActive(false);
            m_ShipyardMoat.gameObject.SetActive(false);
        }

        m_ShipyardMoatShip.gameObject.SetActive(false);
    }

    public override void UpdateHall()
    {
        base.UpdateHall();

        if (m_BuiltBuildings.MageGuild4)
        {
            m_HallMageGuild4.gameObject.SetActive(true);
            m_HallMageGuild4.ButtonImage.sprite = m_Yellow;
            m_HallMageGuild4.CornerImage.gameObject.SetActive(true);
            m_HallMageGuild4.CornerImage.sprite = m_Tick;
            m_HallMageGuild4.Buildable = false;

            m_HallMageGuild1.gameObject.SetActive(false);
            m_HallMageGuild2.gameObject.SetActive(false);
            m_HallMageGuild3.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.MageGuild3)
        {
            m_HallMageGuild4.gameObject.SetActive(true);
            m_HallMageGuild4.ButtonImage.sprite = m_Green;
            m_HallMageGuild4.CornerImage.gameObject.SetActive(false);
            m_HallMageGuild4.Buildable = true;

            m_HallMageGuild1.gameObject.SetActive(false);
            m_HallMageGuild2.gameObject.SetActive(false);
            m_HallMageGuild3.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.MageGuild2)
        {
            m_HallMageGuild3.gameObject.SetActive(true);
            m_HallMageGuild3.ButtonImage.sprite = m_Green;
            m_HallMageGuild3.CornerImage.gameObject.SetActive(false);
            m_HallMageGuild3.Buildable = true;

            m_HallMageGuild1.gameObject.SetActive(false);
            m_HallMageGuild2.gameObject.SetActive(false);
            m_HallMageGuild4.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.MageGuild1)
        {
            m_HallMageGuild2.gameObject.SetActive(true);
            m_HallMageGuild2.ButtonImage.sprite = m_Green;
            m_HallMageGuild2.CornerImage.gameObject.SetActive(false);
            m_HallMageGuild2.Buildable = true;

            m_HallMageGuild1.gameObject.SetActive(false);
            m_HallMageGuild3.gameObject.SetActive(false);
            m_HallMageGuild4.gameObject.SetActive(false);
        }
        else
        {
            m_HallMageGuild1.gameObject.SetActive(true);
            m_HallMageGuild1.ButtonImage.sprite = m_Green;
            m_HallMageGuild1.CornerImage.gameObject.SetActive(false);
            m_HallMageGuild1.Buildable = true;

            m_HallMageGuild2.gameObject.SetActive(false);
            m_HallMageGuild3.gameObject.SetActive(false);
            m_HallMageGuild4.gameObject.SetActive(false);
        }

        // Lighthouse
        if (m_BuiltBuildings.FactionBuilding1)
        {

        }
        else if (m_BuiltBuildings.Shipyard)
        {

        }
        else
        {

        }

        // Brotherhood of the Sword
        if (m_BuiltBuildings.FactionBuilding2)
        {
            m_HallBrotherhood.gameObject.SetActive(true);
            m_HallBrotherhood.ButtonImage.sprite = m_Yellow;
            m_HallBrotherhood.CornerImage.gameObject.SetActive(true);
            m_HallBrotherhood.CornerImage.sprite = m_Tick;
            m_HallBrotherhood.Buildable = false;

            m_HallTavern.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.Tavern)
        {
            m_HallBrotherhood.gameObject.SetActive(true);
            m_HallBrotherhood.ButtonImage.sprite = m_Green;
            m_HallBrotherhood.CornerImage.gameObject.SetActive(false);
            m_HallBrotherhood.Buildable = true;

            m_HallTavern.gameObject.SetActive(false);
        }
        else
        {
            m_HallTavern.gameObject.SetActive(true);
            m_HallTavern.ButtonImage.sprite = m_Green;
            m_HallTavern.CornerImage.gameObject.SetActive(false);
            m_HallTavern.Buildable = true;

            m_HallBrotherhood.gameObject.SetActive(false);
        }
    }

    public override void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallMageGuild1.BuildingData)
        {
            BuildMageGuild1();
        }
        else if (a_BuildingData == m_HallMageGuild2.BuildingData)
        {
            BuildMageGuild2();
        }
        else if (a_BuildingData == m_HallMageGuild3.BuildingData)
        {
            BuildMageGuild3();
        }
        else if (a_BuildingData == m_HallMageGuild4.BuildingData)
        {
            BuildMageGuild4();
        }
        else
        {
            base.BuildBuilding(a_BuildingData);
        }

        UpdateHall();
    }

    void BuildMageGuild1()
    {
        StartCoroutine(BuildBuilding(m_MageGuild1));
        m_BuiltBuildings.MageGuild1 = true;
    }

    void BuildMageGuild2()
    {
        StartCoroutine(BuildBuilding(m_MageGuild2));
        StartCoroutine(RemoveBuilding(m_MageGuild1.Image));
        m_BuiltBuildings.MageGuild2 = true;
    }

    void BuildMageGuild3()
    {
        StartCoroutine(BuildBuilding(m_MageGuild3));
        StartCoroutine(RemoveBuilding(m_MageGuild2.Image));
        m_BuiltBuildings.MageGuild3 = true;
    }

    void BuildMageGuild4()
    {
        StartCoroutine(BuildBuilding(m_MageGuild4));
        StartCoroutine(RemoveBuilding(m_MageGuild3.Image));
        m_BuiltBuildings.MageGuild4 = true;
    }
}
