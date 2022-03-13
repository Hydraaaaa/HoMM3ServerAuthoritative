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

    [Space]

    [SerializeField] HallBuilding m_HallBrimstoneStormclouds;
    [SerializeField] HallBuilding m_HallCastleGate;
    [SerializeField] HallBuilding m_HallOrderOfFire;

    [SerializeField] HallBuilding m_HallBirthingPools;
    [SerializeField] Sprite m_HallBirthingPoolsSprite;
    [SerializeField] Sprite m_HallBirthingPoolsUpSprite;
    [SerializeField] Sprite m_HallDwelling1UpSprite;

    [SerializeField] HallBuilding m_HallCages;
    [SerializeField] Sprite m_HallCagesSprite;
    [SerializeField] Sprite m_HallCagesUpSprite;
    [SerializeField] Sprite m_HallDwelling3UpSprite;

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

    public override void UpdateHall()
    {
        // Brimstone Stormclouds
        if (m_BuiltBuildings.FactionBuilding1)
        {
            SetHallBuildingBuilt(m_HallBrimstoneStormclouds);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallBrimstoneStormclouds);
        }

        // Castle Gate
        if (m_BuiltBuildings.FactionBuilding2)
        {
            SetHallBuildingBuilt(m_HallCastleGate);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallCastleGate);
        }

        // Order of Fire
        if (m_BuiltBuildings.FactionBuilding3)
        {
            SetHallBuildingBuilt(m_HallOrderOfFire);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallOrderOfFire);
        }

        if (m_BuiltBuildings.Dwelling1Up)
        {
            m_HallBirthingPools.Image.sprite = m_HallBirthingPoolsUpSprite;
        }
        else
        {
            m_HallBirthingPools.Image.sprite = m_HallBirthingPoolsSprite;
        }

        if (m_BuiltBuildings.Dwelling1Growth)
        {
            SetHallBuildingBuilt(m_HallBirthingPools);
            m_HallDwelling1Up.Image.sprite = m_HallBirthingPoolsUpSprite;
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallBirthingPools);
            m_HallDwelling1Up.Image.sprite = m_HallDwelling1UpSprite;
        }

        if (m_BuiltBuildings.Dwelling3Up)
        {
            m_HallCages.Image.sprite = m_HallCagesUpSprite;
        }
        else
        {
            m_HallCages.Image.sprite = m_HallCagesSprite;
        }

        if (m_BuiltBuildings.Dwelling3Growth)
        {
            SetHallBuildingBuilt(m_HallCages);
            m_HallDwelling3Up.Image.sprite = m_HallCagesUpSprite;
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallCages);
            m_HallDwelling3Up.Image.sprite = m_HallDwelling3UpSprite;
        }

        base.UpdateHall();
    }

    public override void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallBrimstoneStormclouds.BuildingData)
        {
            BuildFactionBuilding1();
        }
        else if (a_BuildingData == m_HallCastleGate.BuildingData)
        {
            BuildFactionBuilding2();
        }
        else if (a_BuildingData == m_HallOrderOfFire.BuildingData)
        {
            BuildFactionBuilding3();
        }
        else if (a_BuildingData == m_HallBirthingPools.BuildingData)
        {
            BuildBirthingPools();
        }
        else if (a_BuildingData == m_HallCages.BuildingData)
        {
            BuildCages();
        }
        else
        {
            base.BuildBuilding(a_BuildingData);
        }

        UpdateHall();
    }

    protected override void BuildDwelling1Up()
    {
        if (m_BuiltBuildings.Dwelling1Growth)
        {
            StartCoroutine(BuildBuilding(m_Imp2G));
            StartCoroutine(RemoveBuilding(m_ImpG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling1Up));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Up = true;
    }

    protected void BuildBirthingPools()
    {
        if (m_BuiltBuildings.Dwelling1Up)
        {
            StartCoroutine(BuildBuilding(m_Imp2G));
            StartCoroutine(RemoveBuilding(m_Dwelling1Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_ImpG));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Growth = true;
    }

    protected override void BuildDwelling3Up()
    {
        if (m_BuiltBuildings.Dwelling3Growth)
        {
            StartCoroutine(BuildBuilding(m_Hellhound2G));
            StartCoroutine(RemoveBuilding(m_HellhoundG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling3Up));
            StartCoroutine(RemoveBuilding(m_Dwelling3.Image));
        }

        m_BuiltBuildings.Dwelling3Up = true;
    }

    protected void BuildCages()
    {
        if (m_BuiltBuildings.Dwelling3Up)
        {
            StartCoroutine(BuildBuilding(m_Hellhound2G));
            StartCoroutine(RemoveBuilding(m_Dwelling3Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_HellhoundG));
            StartCoroutine(RemoveBuilding(m_Dwelling3.Image));
        }

        m_BuiltBuildings.Dwelling3Growth = true;
    }

    public override bool IsBuildingBuilt(BuildingRequirements a_Building)
    {
        if (a_Building == m_HallBrimstoneStormclouds.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding1;
        }
        else if (a_Building == m_HallCastleGate.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding2;
        }
        else if (a_Building == m_HallOrderOfFire.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding3;
        }
        else if (a_Building == m_HallBirthingPools.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling1Growth;
        }
        else if (a_Building == m_HallCages.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling3Growth;
        }
        else
        {
            return base.IsBuildingBuilt(a_Building);
        }
    }
}
