using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanel : MonoBehaviour
{
    [SerializeField] TownScreen m_TownScreen;

    [SerializeField] Text m_Title;
    [SerializeField] Image m_Image;
    [SerializeField] Text m_Description;
    [SerializeField] Text m_Requirements;
    [SerializeField] Button m_BuildButton;
    [SerializeField] Button m_CancelButton;

    BuildingData m_BuildingData;

    public void Open(HallBuilding a_Building, bool a_RightClick = false)
    {
        m_BuildButton.transform.parent.gameObject.SetActive(!a_RightClick);
        m_CancelButton.transform.parent.gameObject.SetActive(!a_RightClick);

        gameObject.SetActive(true);

        TownBuildings _Buildings = m_TownScreen.GetTownBuildings();

        m_BuildingData = a_Building.BuildingData;

        m_Title.text = m_BuildingData.DisplayName;
        m_Image.sprite = a_Building.Image.sprite;
        m_Description.text = m_BuildingData.Description;
        m_Requirements.text = "Requires:\n";

        List<BuildingData> _RemainingRequirements = new List<BuildingData>();

        if (m_BuildingData.Requirements != null)
        {
            for (int i = 0; i < m_BuildingData.Requirements.Length; i++)
            {
                if (!_Buildings.IsBuildingBuilt(m_BuildingData.Requirements[i]))
                {
                    _RemainingRequirements.Add(m_BuildingData.Requirements[i]);
                }
            }
        }

        for (int i = 0; i < _RemainingRequirements.Count; i++)
        {
            m_Requirements.text += _RemainingRequirements[i].DisplayName;

            if (i < _RemainingRequirements.Count - 1)
            {
                m_Requirements.text += ",  ";
            }
        }

        if (_RemainingRequirements.Count == 0)
        {
            m_Requirements.text = "All prerequisites for this building have been met.";
        }

        m_BuildButton.interactable = a_Building.Buildable;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Build()
    {
        gameObject.SetActive(false);

        m_TownScreen.BuildBuilding(m_BuildingData);
    }
}
