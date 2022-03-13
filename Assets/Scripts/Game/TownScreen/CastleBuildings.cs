using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleBuildings : TownBuildings
{
    [Space]

    [SerializeField] Building m_GriffinG;
    [SerializeField] Building m_Griffin2G;
    [SerializeField] Building m_ShipyardMoat;
    [SerializeField] Building m_ShipyardMoatShip;

    [Space]

    [SerializeField] HallBuilding m_HallLighthouse;
    [SerializeField] HallBuilding m_HallBrotherhood;
    [SerializeField] HallBuilding m_HallStables;
    [SerializeField] HallBuilding m_HallGriffinBastion;
    [SerializeField] Sprite m_HallGriffinBastionSprite;
    [SerializeField] Sprite m_HallGriffinBastionUpSprite;
    [SerializeField] Sprite m_HallDwelling3UpSprite;

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
            m_HallGriffinBastion.Image.sprite = m_HallGriffinBastionUpSprite;
        }
        else
        {
            m_HallGriffinBastion.Image.sprite = m_HallGriffinBastionSprite;
        }

        if (m_BuiltBuildings.Dwelling3Growth)
        {
            SetHallBuildingBuilt(m_HallGriffinBastion);
            m_HallDwelling3Up.Image.sprite = m_HallGriffinBastionUpSprite;
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallGriffinBastion);
            m_HallDwelling3Up.Image.sprite = m_HallDwelling3UpSprite;
        }
    }

    public override void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallLighthouse.BuildingData)
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

    public override bool IsBuildingBuilt(BuildingRequirements a_Building)
    {
        if (a_Building == m_HallLighthouse.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding1;
        }
        else if (a_Building == m_HallBrotherhood.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding2;
        }
        else if (a_Building == m_HallStables.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding3;
        }
        else
        {
            return base.IsBuildingBuilt(a_Building);
        }
    }
}
