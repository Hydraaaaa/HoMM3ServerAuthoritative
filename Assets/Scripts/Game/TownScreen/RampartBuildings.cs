using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampartBuildings : TownBuildings
{
    [Space]
    [SerializeField] GameObject m_VillageHouses;
    [SerializeField] GameObject m_TownHouses;
    [SerializeField] GameObject m_CityHouses;
    [SerializeField] GameObject m_CapitolHouses;

    [SerializeField] Building m_DwarfG;
    [SerializeField] Building m_Dwarf2G;
    [SerializeField] Building m_DendroidG;
    [SerializeField] Building m_Dendroid2G;

    [Space]

    [SerializeField] HallBuilding m_HallMysticPond;
    [SerializeField] HallBuilding m_HallFountainOfFortune;
    [SerializeField] HallBuilding m_HallTreasury;

    [SerializeField] HallBuilding m_HallMinersGuild;
    [SerializeField] Sprite m_HallMinersGuildSprite;
    [SerializeField] Sprite m_HallMinersGuildUpSprite;
    [SerializeField] Sprite m_HallDwelling2UpSprite;

    [SerializeField] HallBuilding m_HallDendroidSaplings;
    [SerializeField] Sprite m_HallDendroidSaplingsSprite;
    [SerializeField] Sprite m_HallDendroidSaplingsUpSprite;
    [SerializeField] Sprite m_HallDwelling5UpSprite;

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

        m_VillageHouses.SetActive(true);
        m_TownHouses.SetActive(a_Data.TownHall);
        m_CityHouses.SetActive(a_Data.CityHall);
        m_CapitolHouses.SetActive(a_Data.Capitol);

        // Fountain of Fortune replacing Mystic Pond
        if (a_Data.FactionBuilding2)
        {
            m_FactionBuilding1.gameObject.SetActive(false);
        }

        if (a_Data.Dwelling2Growth)
        {
            if (a_Data.Dwelling2Up)
            {
                m_DwarfG.gameObject.SetActive(false);
                m_Dwarf2G.gameObject.SetActive(true);

                m_Dwelling2Up.gameObject.SetActive(false);
            }
            else
            {
                m_DwarfG.gameObject.SetActive(true);
                m_Dwarf2G.gameObject.SetActive(false);

                m_Dwelling2.gameObject.SetActive(false);
            }
        }
        else
        {
            m_DwarfG.gameObject.SetActive(false);
            m_Dwarf2G.gameObject.SetActive(false);
        }

        if (a_Data.Dwelling5Growth)
        {
            if (a_Data.Dwelling5Up)
            {
                m_DendroidG.gameObject.SetActive(false);
                m_Dendroid2G.gameObject.SetActive(true);

                m_Dwelling5Up.gameObject.SetActive(false);
            }
            else
            {
                m_DendroidG.gameObject.SetActive(true);
                m_Dendroid2G.gameObject.SetActive(false);

                m_Dwelling5.gameObject.SetActive(false);
            }
        }
        else
        {
            m_DendroidG.gameObject.SetActive(false);
            m_Dendroid2G.gameObject.SetActive(false);
        }
    }

    public override void UpdateHall()
    {
        // Fountain of Fortune / Mystic Pond
        if (m_BuiltBuildings.FactionBuilding2)
        {
            SetHallBuildingBuilt(m_HallFountainOfFortune);

            m_HallFountainOfFortune.gameObject.SetActive(true);
            m_HallMysticPond.gameObject.SetActive(false);
        }
        else if (m_BuiltBuildings.FactionBuilding1)
        {
            SetHallBuildingNotBuilt(m_HallFountainOfFortune);

            m_HallFountainOfFortune.gameObject.SetActive(true);
            m_HallMysticPond.gameObject.SetActive(false);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallMysticPond);

            m_HallMysticPond.gameObject.SetActive(true);
            m_HallFountainOfFortune.gameObject.SetActive(false);
        }

        // Treasury
        if (m_BuiltBuildings.FactionBuilding3)
        {
            SetHallBuildingBuilt(m_HallTreasury);
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallTreasury);
        }

        if (m_BuiltBuildings.Dwelling2Up)
        {
            m_HallMinersGuild.Image.sprite = m_HallMinersGuildUpSprite;
        }
        else
        {
            m_HallMinersGuild.Image.sprite = m_HallMinersGuildSprite;
        }

        if (m_BuiltBuildings.Dwelling2Growth)
        {
            SetHallBuildingBuilt(m_HallMinersGuild);
            m_HallDwelling2Up.Image.sprite = m_HallMinersGuildUpSprite;
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallMinersGuild);
            m_HallDwelling2Up.Image.sprite = m_HallDwelling2UpSprite;
        }

        if (m_BuiltBuildings.Dwelling5Up)
        {
            m_HallDendroidSaplings.Image.sprite = m_HallDendroidSaplingsUpSprite;
        }
        else
        {
            m_HallDendroidSaplings.Image.sprite = m_HallDendroidSaplingsSprite;
        }

        if (m_BuiltBuildings.Dwelling5Growth)
        {
            SetHallBuildingBuilt(m_HallDendroidSaplings);
            m_HallDwelling5Up.Image.sprite = m_HallDendroidSaplingsUpSprite;
        }
        else
        {
            SetHallBuildingNotBuilt(m_HallDendroidSaplings);
            m_HallDwelling5Up.Image.sprite = m_HallDwelling5UpSprite;
        }

        base.UpdateHall();
    }

    public override void BuildBuilding(BuildingData a_BuildingData)
    {
        if (a_BuildingData == m_HallMysticPond.BuildingData)
        {
            BuildFactionBuilding1();
        }
        else if (a_BuildingData == m_HallFountainOfFortune.BuildingData)
        {
            BuildFactionBuilding2();
        }
        else if (a_BuildingData == m_HallTreasury.BuildingData)
        {
            BuildFactionBuilding3();
        }
        else if (a_BuildingData == m_HallMinersGuild.BuildingData)
        {
            BuildMinersGuild();
        }
        else if (a_BuildingData == m_HallDendroidSaplings.BuildingData)
        {
            BuildDendroidSaplings();
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
            StartCoroutine(BuildBuilding(m_Dwarf2G));
            StartCoroutine(RemoveBuilding(m_DwarfG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling2Up));
            StartCoroutine(RemoveBuilding(m_Dwelling2.Image));
        }

        m_BuiltBuildings.Dwelling2Up = true;
    }

    protected void BuildMinersGuild()
    {
        if (m_BuiltBuildings.Dwelling2Up)
        {
            StartCoroutine(BuildBuilding(m_Dwarf2G));
            StartCoroutine(RemoveBuilding(m_Dwelling2Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_DwarfG));
            StartCoroutine(RemoveBuilding(m_Dwelling2.Image));
        }

        m_BuiltBuildings.Dwelling2Growth = true;
    }

    protected override void BuildDwelling5Up()
    {
        if (m_BuiltBuildings.Dwelling5Growth)
        {
            StartCoroutine(BuildBuilding(m_Dendroid2G));
            StartCoroutine(RemoveBuilding(m_DendroidG.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_Dwelling5Up));
            StartCoroutine(RemoveBuilding(m_Dwelling5.Image));
        }

        m_BuiltBuildings.Dwelling5Up = true;
    }

    protected void BuildDendroidSaplings()
    {
        if (m_BuiltBuildings.Dwelling5Up)
        {
            StartCoroutine(BuildBuilding(m_Dendroid2G));
            StartCoroutine(RemoveBuilding(m_Dwelling5Up.Image));
        }
        else
        {
            StartCoroutine(BuildBuilding(m_DendroidG));
            StartCoroutine(RemoveBuilding(m_Dwelling5.Image));
        }

        m_BuiltBuildings.Dwelling5Growth = true;
    }

    public override bool IsBuildingBuilt(BuildingRequirements a_Building)
    {
        if (a_Building == m_HallMysticPond.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding1;
        }
        else if (a_Building == m_HallFountainOfFortune.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding2;
        }
        else if (a_Building == m_HallTreasury.BuildingData.Requirements)
        {
            return m_BuiltBuildings.FactionBuilding3;
        }
        else if (a_Building == m_HallMinersGuild.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling2Growth;
        }
        else if (a_Building == m_HallDendroidSaplings.BuildingData.Requirements)
        {
            return m_BuiltBuildings.Dwelling5Growth;
        }
        else
        {
            return base.IsBuildingBuilt(a_Building);
        }
    }
}
