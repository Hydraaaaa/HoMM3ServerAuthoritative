using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfluxBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_PixieG;
    [SerializeField] Building m_Pixie2G;

    [Space]

    [SerializeField] HallBuilding m_HallMagicUniversity;
    [SerializeField] HallBuilding m_HallGardenOfLife;
    [SerializeField] Sprite m_HallGardenOfLifeSprite;
    [SerializeField] Sprite m_HallGardenOfLifeUpSprite;
    [SerializeField] Sprite m_HallDwelling1UpSprite;

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

        if (a_Data.Dwelling1Growth)
        {
            if (a_Data.Dwelling1Up)
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
    }

    public override void UpdateHall()
    {
        // Magic University
        if (m_BuiltBuildings.FactionBuilding1)
        {
            SetHallBuildingBuilt(m_HallMagicUniversity);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallMagicUniversity);
        }

        if (m_BuiltBuildings.Dwelling1Up)
        {
            m_HallGardenOfLife.Image.sprite = m_HallGardenOfLifeUpSprite;
        }
        else
        {
            m_HallGardenOfLife.Image.sprite = m_HallGardenOfLifeSprite;
        }

        if (m_BuiltBuildings.Dwelling1Growth)
        {
            SetHallBuildingBuilt(m_HallGardenOfLife);
            m_HallDwelling1Up.Image.sprite = m_HallGardenOfLifeUpSprite;
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallGardenOfLife);
            m_HallDwelling1Up.Image.sprite = m_HallDwelling1UpSprite;
        }
        base.UpdateHall();
    }

    public override void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallMagicUniversity.BuildingData)
        {
            BuildFactionBuilding1();
        }
        else if (a_BuildingData == m_HallGardenOfLife.BuildingData)
        {
            BuildGardenOfLife();
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
            StartCoroutine(BuildBuilding(m_Pixie2G));
            StartCoroutine(RemoveBuilding(m_PixieG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling1Up));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Up = true;
    }

    protected void BuildGardenOfLife()
    {
        if (m_BuiltBuildings.Dwelling1Up)
        {
            StartCoroutine(BuildBuilding(m_Pixie2G));
            StartCoroutine(RemoveBuilding(m_Dwelling1Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_PixieG));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Growth = true;
    }

    public override bool IsBuildingBuilt(BuildingRequirements a_Building)
    {
        if (a_Building == m_HallMagicUniversity.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding1;
        }
        else if (a_Building == m_HallGardenOfLife.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling1Growth;
        }
        else
        {
            return base.IsBuildingBuilt(a_Building);
        }
    }
}
