using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_GargoyleG;
    [SerializeField] Building m_Gargoyle2G;

    [Space]

    [SerializeField] HallBuilding m_HallLibrary;
    [SerializeField] HallBuilding m_HallWallOfKnowledge;
    [SerializeField] HallBuilding m_HallLookoutTower;

    [SerializeField] HallBuilding m_HallSculptorsWings;
    [SerializeField] Sprite m_HallSculptorsWingsSprite;
    [SerializeField] Sprite m_HallSculptorsWingsUpSprite;
    [SerializeField] Sprite m_HallDwelling2UpSprite;

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

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

    public override void UpdateHall()
    {
        // Library
        if (m_BuiltBuildings.FactionBuilding1)
        {
            SetHallBuildingBuilt(m_HallLibrary);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallLibrary);
        }

        // Wall of Knowledge
        if (m_BuiltBuildings.FactionBuilding2)
        {
            SetHallBuildingBuilt(m_HallWallOfKnowledge);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallWallOfKnowledge);
        }

        // Lookout Tower
        if (m_BuiltBuildings.FactionBuilding3)
        {
            SetHallBuildingBuilt(m_HallLookoutTower);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallLookoutTower);
        }

        if (m_BuiltBuildings.Dwelling2Up)
        {
            m_HallSculptorsWings.Image.sprite = m_HallSculptorsWingsUpSprite;
        }
        else
        {
            m_HallSculptorsWings.Image.sprite = m_HallSculptorsWingsSprite;
        }

        if (m_BuiltBuildings.Dwelling2Growth)
        {
            SetHallBuildingBuilt(m_HallSculptorsWings);
            m_HallDwelling2Up.Image.sprite = m_HallSculptorsWingsUpSprite;
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallSculptorsWings);
            m_HallDwelling2Up.Image.sprite = m_HallDwelling2UpSprite;
        }

        base.UpdateHall();
    }

    public override void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallLibrary.BuildingData)
        {
            BuildFactionBuilding1();
        }
        else if (a_BuildingData == m_HallWallOfKnowledge.BuildingData)
        {
            BuildFactionBuilding2();
        }
        else if (a_BuildingData == m_HallLookoutTower.BuildingData)
        {
            BuildFactionBuilding3();
        }
        else if (a_BuildingData == m_HallSculptorsWings.BuildingData)
        {
            BuildSculptorsWings();
        }
        else
        {
            base.BuildBuilding(a_BuildingData);
        }

        UpdateHall();
    }

    protected override void BuildDwelling2Up()
    {
        if (m_BuiltBuildings.Dwelling2Growth)
        {
            StartCoroutine(BuildBuilding(m_Gargoyle2G));
            StartCoroutine(RemoveBuilding(m_GargoyleG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling2Up));
            StartCoroutine(RemoveBuilding(m_Dwelling2.Image));
        }

        m_BuiltBuildings.Dwelling2Up = true;
    }

    protected void BuildSculptorsWings()
    {
        if (m_BuiltBuildings.Dwelling2Up)
        {
            StartCoroutine(BuildBuilding(m_Gargoyle2G));
            StartCoroutine(RemoveBuilding(m_Dwelling2Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_GargoyleG));
            StartCoroutine(RemoveBuilding(m_Dwelling2.Image));
        }

        m_BuiltBuildings.Dwelling2Growth = true;
    }

    public override bool IsBuildingBuilt(BuildingRequirements a_Building)
    {
        if (a_Building == m_HallLibrary.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding1;
        }
        else if (a_Building == m_HallWallOfKnowledge.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding2;
        }
        else if (a_Building == m_HallLookoutTower.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding3;
        }
        else if (a_Building == m_HallSculptorsWings.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling2Growth;
        }
        else
        {
            return base.IsBuildingBuilt(a_Building);
        }
    }
}
