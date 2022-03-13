using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongholdBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_GoblinG;
    [SerializeField] Building m_Goblin2G;

    [Space]

    [SerializeField] HallBuilding m_HallEscapeTunnel;
    [SerializeField] HallBuilding m_HallFreelancersGuild;
    [SerializeField] HallBuilding m_HallBallistaYard;
    [SerializeField] HallBuilding m_HallHallOfValhalla;
    [SerializeField] HallBuilding m_HallMessHall;
    [SerializeField] Sprite m_HallMessHallSprite;
    [SerializeField] Sprite m_HallMessHallUpSprite;
    [SerializeField] Sprite m_HallDwelling1UpSprite;

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

        if (a_Data.Dwelling1Growth)
        {
            if (a_Data.Dwelling1Up)
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
    }

    public override void UpdateHall()
    {
        // Escape Tunnel
        if (m_BuiltBuildings.FactionBuilding1)
        {
            SetHallBuildingBuilt(m_HallEscapeTunnel);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallEscapeTunnel);
        }

        // Freelancer's Guild
        if (m_BuiltBuildings.FactionBuilding2)
        {
            SetHallBuildingBuilt(m_HallFreelancersGuild);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallFreelancersGuild);
        }

        // Ballista Yard
        if (m_BuiltBuildings.FactionBuilding3)
        {
            SetHallBuildingBuilt(m_HallBallistaYard);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallBallistaYard);
        }

        // Hall of Valhalla
        if (m_BuiltBuildings.FactionBuilding4)
        {
            SetHallBuildingBuilt(m_HallHallOfValhalla);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallHallOfValhalla);
        }

        if (m_BuiltBuildings.Dwelling1Up)
        {
            m_HallMessHall.Image.sprite = m_HallMessHallUpSprite;
        }
        else
        {
            m_HallMessHall.Image.sprite = m_HallMessHallSprite;
        }

        if (m_BuiltBuildings.Dwelling1Growth)
        {
            SetHallBuildingBuilt(m_HallMessHall);
            m_HallDwelling1Up.Image.sprite = m_HallMessHallUpSprite;
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallMessHall);
            m_HallDwelling1Up.Image.sprite = m_HallDwelling1UpSprite;
        }

        base.UpdateHall();
    }

    public override void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallEscapeTunnel.BuildingData)
        {
            BuildFactionBuilding1();
        }
        else if (a_BuildingData == m_HallFreelancersGuild.BuildingData)
        {
            BuildFactionBuilding2();
        }
        else if (a_BuildingData == m_HallBallistaYard.BuildingData)
        {
            BuildFactionBuilding3();
        }
        else if (a_BuildingData == m_HallHallOfValhalla.BuildingData)
        {
            BuildFactionBuilding4();
        }
        else if (a_BuildingData == m_HallMessHall.BuildingData)
        {
            BuildMessHall();
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
            StartCoroutine(BuildBuilding(m_Goblin2G));
            StartCoroutine(RemoveBuilding(m_GoblinG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling1Up));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Up = true;
    }

    protected void BuildMessHall()
    {
        if (m_BuiltBuildings.Dwelling1Up)
        {
            StartCoroutine(BuildBuilding(m_Goblin2G));
            StartCoroutine(RemoveBuilding(m_Dwelling1Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_GoblinG));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Growth = true;
    }

    public override bool IsBuildingBuilt(BuildingRequirements a_Building)
    {
        if (a_Building == m_HallEscapeTunnel.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding1;
        }
        else if (a_Building == m_HallFreelancersGuild.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding2;
        }
        else if (a_Building == m_HallBallistaYard.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding3;
        }
        else if (a_Building == m_HallHallOfValhalla.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding4;
        }
        else if (a_Building == m_HallMessHall.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling1Growth;
        }
        else
        {
            return base.IsBuildingBuilt(a_Building);
        }
    }
}
