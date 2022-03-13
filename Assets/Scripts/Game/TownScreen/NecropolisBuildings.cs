using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecropolisBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_SkeletonG;
    [SerializeField] Building m_Skeleton2G;
    [SerializeField] Building m_SkeletonTransformer;
    [SerializeField] Building m_NecromancyAmplifier;
    [SerializeField] Building m_CoverOfDarkness;

    [Space]

    [SerializeField] HallBuilding m_HallCoverOfDarkness;
    [SerializeField] HallBuilding m_HallNecromancyAmplifier;
    [SerializeField] HallBuilding m_HallSkeletonTransformer;
    [SerializeField] HallBuilding m_HallUnearthedGraves;

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

        if (a_Data.Dwelling1Growth)
        {
            if (a_Data.Dwelling1Up)
            {
                m_SkeletonG.gameObject.SetActive(false);
                m_Skeleton2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_SkeletonG.gameObject.SetActive(true);
                m_Skeleton2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_SkeletonG.gameObject.SetActive(false);
            m_Skeleton2G.gameObject.SetActive(false);
        }
    }

    public override void UpdateHall()
    {
        // Cover of Darkness
        if (m_BuiltBuildings.FactionBuilding1)
        {
            SetHallBuildingBuilt(m_HallCoverOfDarkness);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallCoverOfDarkness);
        }

        // Necromancy Amplifier
        if (m_BuiltBuildings.FactionBuilding2)
        {
            SetHallBuildingBuilt(m_HallNecromancyAmplifier);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallNecromancyAmplifier);
        }

        // Skeleton Transformer
        if (m_BuiltBuildings.FactionBuilding3)
        {
            SetHallBuildingBuilt(m_HallSkeletonTransformer);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallSkeletonTransformer);
        }

        if (m_BuiltBuildings.Dwelling1Growth)
        {
            SetHallBuildingBuilt(m_HallUnearthedGraves);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallUnearthedGraves);
        }

        base.UpdateHall();
    }

    public override void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallCoverOfDarkness.BuildingData)
        {
            BuildFactionBuilding1();
        }
        else if (a_BuildingData == m_HallNecromancyAmplifier.BuildingData)
        {
            BuildFactionBuilding2();
        }
        else if (a_BuildingData == m_HallSkeletonTransformer.BuildingData)
        {
            BuildFactionBuilding3();
        }
        else if (a_BuildingData == m_HallUnearthedGraves.BuildingData)
        {
            BuildUnearthedGraves();
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
            StartCoroutine(BuildBuilding(m_Skeleton2G));
            StartCoroutine(RemoveBuilding(m_SkeletonG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling1Up));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Up = true;
    }

    protected void BuildUnearthedGraves()
    {
        if (m_BuiltBuildings.Dwelling1Up)
        {
            StartCoroutine(BuildBuilding(m_Skeleton2G));
            StartCoroutine(RemoveBuilding(m_Dwelling1Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_SkeletonG));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Growth = true;
    }

    public override bool IsBuildingBuilt(BuildingRequirements a_Building)
    {
        if (a_Building == m_HallCoverOfDarkness.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding1;
        }
        else if (a_Building == m_HallNecromancyAmplifier.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding2;
        }
        else if (a_Building == m_HallSkeletonTransformer.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding3;
        }
        else if (a_Building == m_HallUnearthedGraves.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling1Growth;
        }
        else
        {
            return base.IsBuildingBuilt(a_Building);
        }
    }
}
