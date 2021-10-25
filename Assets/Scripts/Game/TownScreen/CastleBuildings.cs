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
    [SerializeField] protected HallBuilding m_HallGriffinBastion;
    [SerializeField] protected HallBuilding m_HallGriffinBastionUp;

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

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
            SetHallBuildingBuilt(m_HallMageGuild4);

            m_HallMageGuild4.gameObject.SetActive(true);
            m_HallMageGuild1.gameObject.SetActive(false);
            m_HallMageGuild2.gameObject.SetActive(false);
            m_HallMageGuild3.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.MageGuild3)
        {
            SetHallBuildingNotBuilt(m_HallMageGuild4);

            m_HallMageGuild4.gameObject.SetActive(true);
            m_HallMageGuild1.gameObject.SetActive(false);
            m_HallMageGuild2.gameObject.SetActive(false);
            m_HallMageGuild3.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.MageGuild2)
        {
            SetHallBuildingNotBuilt(m_HallMageGuild3);

            m_HallMageGuild3.gameObject.SetActive(true);
            m_HallMageGuild1.gameObject.SetActive(false);
            m_HallMageGuild2.gameObject.SetActive(false);
            m_HallMageGuild4.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.MageGuild1)
        {

            SetHallBuildingNotBuilt(m_HallMageGuild2);

            m_HallMageGuild2.gameObject.SetActive(true);
            m_HallMageGuild1.gameObject.SetActive(false);
            m_HallMageGuild3.gameObject.SetActive(false);
            m_HallMageGuild4.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallMageGuild1);

            m_HallMageGuild1.gameObject.SetActive(true);
            m_HallMageGuild2.gameObject.SetActive(false);
            m_HallMageGuild3.gameObject.SetActive(false);
            m_HallMageGuild4.gameObject.SetActive(false);
        }

        // Lighthouse
        if (m_BuiltBuildings.FactionBuilding1)
        {
            SetHallBuildingBuilt(m_HallLighthouse);

            m_HallLighthouse.gameObject.SetActive(true);
            m_HallShipyard.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.Shipyard)
        {
            SetHallBuildingNotBuilt(m_HallLighthouse);

            m_HallLighthouse.gameObject.SetActive(true);
            m_HallShipyard.gameObject.SetActive(false);
        }
        else
        {
            if (m_CanBuildShipyard)
            {
                SetHallBuildingNotBuilt(m_HallShipyard);
            }
            else
            {
                SetHallBuildingUnbuildable(m_HallShipyard);
            }

            m_HallShipyard.gameObject.SetActive(true);
            m_HallLighthouse.gameObject.SetActive(false);
        }

        // Brotherhood of the Sword
        if (m_BuiltBuildings.FactionBuilding2)
        {
            SetHallBuildingBuilt(m_HallBrotherhood);

            m_HallBrotherhood.gameObject.SetActive(true);
            m_HallTavern.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.Tavern)
        {
            SetHallBuildingNotBuilt(m_HallBrotherhood);

            m_HallBrotherhood.gameObject.SetActive(true);
            m_HallTavern.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallTavern);

            m_HallTavern.gameObject.SetActive(true);
            m_HallBrotherhood.gameObject.SetActive(false);
        }

        // Stables
        if (m_BuiltBuildings.FactionBuilding3)
        {
            SetHallBuildingBuilt(m_HallStables);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallStables);
        }

        if (m_BuiltBuildings.Dwelling3Up)
        {
            m_HallGriffinBastionUp.gameObject.SetActive(true);
            m_HallGriffinBastion.gameObject.SetActive(false);

            if (m_BuiltBuildings.Dwelling3Growth)
            {
                SetHallBuildingBuilt(m_HallGriffinBastionUp);
            }
            else
            {
                SetHallBuildingNotBuilt(m_HallGriffinBastionUp);
            }
        }
        else
        {
            m_HallGriffinBastion.gameObject.SetActive(true);
            m_HallGriffinBastionUp.gameObject.SetActive(false);

            if (m_BuiltBuildings.Dwelling3Growth)
            {
                SetHallBuildingBuilt(m_HallGriffinBastion);
            }
            else
            {
                SetHallBuildingNotBuilt(m_HallGriffinBastion);
            }
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
        else if (a_BuildingData == m_HallLighthouse.BuildingData)
        {
            BuildFactionBuilding1();
        }
        else if (a_BuildingData == m_HallBrotherhood.BuildingData)
        {
            BuildFactionBuilding2();
        }
        else if (a_BuildingData == m_HallStables.BuildingData)
        {
            BuildFactionBuilding3();
        }
        else if (a_BuildingData == m_HallGriffinBastion.BuildingData)
        {
            BuildGriffinBastion();
        }
        else
        {
            base.BuildBuilding(a_BuildingData);
        }

        UpdateHall();
    }

    protected override void BuildFactionBuilding1()
    {
        StartCoroutine(BuildBuilding(m_FactionBuilding1));
        StartCoroutine(RemoveBuilding(m_Tavern.Image));
        m_BuiltBuildings.FactionBuilding1 = true;
    }

    protected override void BuildDwelling3Up()
    {
        if (m_BuiltBuildings.Dwelling3Growth)
        {
            StartCoroutine(BuildBuilding(m_Griffin2G));
            StartCoroutine(RemoveBuilding(m_GriffinG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling3Up));
            StartCoroutine(RemoveBuilding(m_Dwelling3.Image));
        }

        m_BuiltBuildings.Dwelling3Up = true;
    }

    protected void BuildGriffinBastion()
    {
        if (m_BuiltBuildings.Dwelling3Up)
        {
            StartCoroutine(BuildBuilding(m_Griffin2G));
            StartCoroutine(RemoveBuilding(m_Dwelling3Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_GriffinG));
            StartCoroutine(RemoveBuilding(m_Dwelling3.Image));
        }

        m_BuiltBuildings.Dwelling3Growth = true;
    }

    public override bool IsBuildingBuilt(BuildingData a_Building)
    {
        if (a_Building == m_HallMageGuild1.BuildingData)
        {
            return m_BuiltBuildings.MageGuild1;
        }
        else if (a_Building == m_HallMageGuild2.BuildingData)
        {
            return m_BuiltBuildings.MageGuild2;
        }
        else if (a_Building == m_HallMageGuild3.BuildingData)
        {
            return m_BuiltBuildings.MageGuild3;
        }
        else if (a_Building == m_HallMageGuild4.BuildingData)
        {
            return m_BuiltBuildings.MageGuild4;
        }
        else if (a_Building == m_HallLighthouse.BuildingData)
        {
            return m_BuiltBuildings.FactionBuilding1;
        }
        else if (a_Building == m_HallBrotherhood.BuildingData)
        {
            return m_BuiltBuildings.FactionBuilding2;
        }
        else if (a_Building == m_HallStables.BuildingData)
        {
            return m_BuiltBuildings.FactionBuilding3;
        }
        else
        {
            return base.IsBuildingBuilt(a_Building);
        }
    }
}
