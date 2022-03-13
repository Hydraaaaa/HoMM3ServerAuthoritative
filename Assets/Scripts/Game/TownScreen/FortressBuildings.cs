using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_GnollG;
    [SerializeField] Building m_Gnoll2G;

    [Space]

    [SerializeField] HallBuilding m_HallGlyphsOfFear;
    [SerializeField] HallBuilding m_HallBloodObelisk;
    [SerializeField] HallBuilding m_HallCageOfWarlords;
    [SerializeField] HallBuilding m_HallCaptainsQuarters;
    [SerializeField] Sprite m_HallCaptainsQuartersSprite;
    [SerializeField] Sprite m_HallCaptainsQuartersUpSprite;
    [SerializeField] Sprite m_HallDwelling1UpSprite;

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

        if (a_Data.Dwelling1Growth)
        {
            if (a_Data.Dwelling1Up)
            {
                m_GnollG.gameObject.SetActive(false);
                m_Gnoll2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_GnollG.gameObject.SetActive(true);
                m_Gnoll2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_GnollG.gameObject.SetActive(false);
            m_Gnoll2G.gameObject.SetActive(false);
        }
    }

    public override void UpdateHall()
    {
        // Glyphs Of Fear / Blood Obelisk
        if (m_BuiltBuildings.FactionBuilding1)
        {
            SetHallBuildingBuilt(m_HallBloodObelisk);

            m_HallBloodObelisk.gameObject.SetActive(true);
            m_HallGlyphsOfFear.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.FactionBuilding2)
        {
            SetHallBuildingNotBuilt(m_HallBloodObelisk);

            m_HallBloodObelisk.gameObject.SetActive(true);
            m_HallGlyphsOfFear.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallGlyphsOfFear);

            m_HallGlyphsOfFear.gameObject.SetActive(true);
            m_HallBloodObelisk.gameObject.SetActive(false);
        }

        // Cage of Warlords
        if (m_BuiltBuildings.FactionBuilding3)
        {
            SetHallBuildingBuilt(m_HallCageOfWarlords);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallCageOfWarlords);
        }

        if (m_BuiltBuildings.Dwelling1Up)
        {
            m_HallCaptainsQuarters.Image.sprite = m_HallCaptainsQuartersUpSprite;
        }
        else
        {
            m_HallCaptainsQuarters.Image.sprite = m_HallCaptainsQuartersSprite;
        }

        if (m_BuiltBuildings.Dwelling1Growth)
        {
            SetHallBuildingBuilt(m_HallCaptainsQuarters);
            m_HallDwelling1Up.Image.sprite = m_HallCaptainsQuartersUpSprite;
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallCaptainsQuarters);
            m_HallDwelling1Up.Image.sprite = m_HallDwelling1UpSprite;
        }

        base.UpdateHall();
    }

    public override void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallGlyphsOfFear.BuildingData)
        {
            BuildFactionBuilding1();
        }
        else if (a_BuildingData == m_HallBloodObelisk.BuildingData)
        {
            BuildFactionBuilding2();
        }
        else if (a_BuildingData == m_HallCageOfWarlords.BuildingData)
        {
            BuildFactionBuilding3();
        }
        else if (a_BuildingData == m_HallCaptainsQuarters.BuildingData)
        {
            BuildCaptainsQuarters();
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
            StartCoroutine(BuildBuilding(m_Gnoll2G));
            StartCoroutine(RemoveBuilding(m_GnollG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling1Up));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Up = true;
    }

    protected void BuildCaptainsQuarters()
    {
        if (m_BuiltBuildings.Dwelling1Up)
        {
            StartCoroutine(BuildBuilding(m_Gnoll2G));
            StartCoroutine(RemoveBuilding(m_Dwelling1Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_GnollG));
            StartCoroutine(RemoveBuilding(m_Dwelling1.Image));
        }

        m_BuiltBuildings.Dwelling1Growth = true;
    }

    public override bool IsBuildingBuilt(BuildingRequirements a_Building)
    {
        if (a_Building == m_HallGlyphsOfFear.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding1;
        }
        else if (a_Building == m_HallBloodObelisk.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding2;
        }
        else if (a_Building == m_HallCageOfWarlords.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding3;
        }
        else if (a_Building == m_HallCaptainsQuarters.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling1Growth;
        }
        else
        {
            return base.IsBuildingBuilt(a_Building);
        }
    }
}
