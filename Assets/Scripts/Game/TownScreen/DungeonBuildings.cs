using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_TroglodyteG;
    [SerializeField] Building m_Troglodyte2G;
    [SerializeField] Building m_ManaVortexMageGuild5;

    [Space]

    [SerializeField] HallBuilding m_HallManaVortex;
    [SerializeField] HallBuilding m_HallPortalOfSummoning;
    [SerializeField] HallBuilding m_HallBattleScholarAcademy;
    [SerializeField] HallBuilding m_HallMushroomRings;
    [SerializeField] Sprite m_HallMushroomRingsSprite;
    [SerializeField] Sprite m_HallMushroomRingsUpSprite;
    [SerializeField] Sprite m_HallDwelling1UpSprite;

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

        if (a_Data.Dwelling1Growth)
        {
            if (a_Data.Dwelling1Up)
            {
                m_TroglodyteG.gameObject.SetActive(false);
                m_Troglodyte2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_TroglodyteG.gameObject.SetActive(true);
                m_Troglodyte2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_TroglodyteG.gameObject.SetActive(false);
            m_Troglodyte2G.gameObject.SetActive(false);
        }

        if (a_Data.MageGuild5 && a_Data.FactionBuilding1)
        {
            m_ManaVortexMageGuild5.gameObject.SetActive(true);
            m_FactionBuilding1.gameObject.SetActive(false);
        }
        else
        {
            m_ManaVortexMageGuild5.gameObject.SetActive(false);
        }
    }

    public override void UpdateHall()
    {
        // Mana Vortex
        if (m_BuiltBuildings.FactionBuilding1)
        {
            SetHallBuildingBuilt(m_HallManaVortex);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallManaVortex);
        }

        // Portal of Summoning
        if (m_BuiltBuildings.FactionBuilding2)
        {
            SetHallBuildingBuilt(m_HallPortalOfSummoning);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallPortalOfSummoning);
        }

        // Battle Scholar Academy
        if (m_BuiltBuildings.FactionBuilding3)
        {
            SetHallBuildingBuilt(m_HallBattleScholarAcademy);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallBattleScholarAcademy);
        }

        if (m_BuiltBuildings.Dwelling1Up)
        {
            m_HallMushroomRings.Image.sprite = m_HallMushroomRingsUpSprite;
        }
        else
        {
            m_HallMushroomRings.Image.sprite = m_HallMushroomRingsSprite;
        }

        if (m_BuiltBuildings.Dwelling1Growth)
        {
            SetHallBuildingBuilt(m_HallMushroomRings);
            m_HallDwelling1Up.Image.sprite = m_HallMushroomRingsUpSprite;
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallMushroomRings);
            m_HallDwelling1Up.Image.sprite = m_HallDwelling1UpSprite;
        }

        base.UpdateHall();
    }

    public override void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallManaVortex.BuildingData)
        {
            BuildFactionBuilding1();
        }
        else if (a_BuildingData == m_HallPortalOfSummoning.BuildingData)
        {
            BuildFactionBuilding2();
        }
        else if (a_BuildingData == m_HallBattleScholarAcademy.BuildingData)
        {
            BuildFactionBuilding3();
        }
        else if (a_BuildingData == m_HallMushroomRings.BuildingData)
        {
            BuildMushroomRings();
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
            StartCoroutine(BuildBuilding(m_Troglodyte2G));
            StartCoroutine(RemoveBuilding(m_TroglodyteG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling1Up));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Up = true;
    }

    protected void BuildMushroomRings()
    {
        if (m_BuiltBuildings.Dwelling1Up)
        {
            StartCoroutine(BuildBuilding(m_Troglodyte2G));
            StartCoroutine(RemoveBuilding(m_Dwelling1Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_TroglodyteG));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Growth = true;
    }

    public override bool IsBuildingBuilt(BuildingRequirements a_Building)
    {
        if (a_Building == m_HallManaVortex.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding1;
        }
        else if (a_Building == m_HallPortalOfSummoning.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding2;
        }
        else if (a_Building == m_HallBattleScholarAcademy.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding3;
        }
        else if (a_Building == m_HallMushroomRings.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling1Growth;
        }
        else
        {
            return base.IsBuildingBuilt(a_Building);
        }
    }
}
