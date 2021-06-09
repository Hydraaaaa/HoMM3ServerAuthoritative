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

    BuildingData m_BuildingData;

    public void Open(HallBuilding a_Building)
    {
        gameObject.SetActive(true);

        m_BuildingData = a_Building.BuildingData;

        m_Title.text = m_BuildingData.DisplayName;
        m_Image.sprite = a_Building.Image.sprite;
        m_Description.text = m_BuildingData.Description;
        //m_Requirements.text =

        m_BuildButton.interactable = a_Building.Buildable;
    }

    public void Build()
    {
        gameObject.SetActive(false);

        m_TownScreen.BuildBuilding(m_BuildingData);
    }
}
