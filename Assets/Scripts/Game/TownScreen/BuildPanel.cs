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
    [SerializeField] Image[] m_ResourceImages;
    [SerializeField] Text[] m_ResourceAmounts;

    [Space]

    [SerializeField] Sprite m_GoldSprite;
    [SerializeField] Sprite m_WoodSprite;
    [SerializeField] Sprite m_OreSprite;
    [SerializeField] Sprite m_MercurySprite;
    [SerializeField] Sprite m_SulfurSprite;
    [SerializeField] Sprite m_CrystalsSprite;
    [SerializeField] Sprite m_GemsSprite;

    BuildingData m_BuildingData;

    public void Open(HallBuilding a_Building, bool a_RightClick = false)
    {
        m_BuildButton.transform.parent.gameObject.SetActive(!a_RightClick);
        m_CancelButton.transform.parent.gameObject.SetActive(!a_RightClick);

        gameObject.SetActive(true);

        TownBuildings _Buildings = m_TownScreen.GetTownBuildings();

        m_BuildingData = a_Building.BuildingData;

        m_Title.text = "Build " + m_BuildingData.DisplayName;
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

        int _ResourceCount = 0;

        if (a_Building.BuildingData.WoodCost > 0)
        {
            m_ResourceImages[_ResourceCount].sprite = m_WoodSprite;
            m_ResourceAmounts[_ResourceCount].text = a_Building.BuildingData.WoodCost.ToString();
            _ResourceCount++;
        }

        if (a_Building.BuildingData.MercuryCost > 0)
        {
            m_ResourceImages[_ResourceCount].sprite = m_MercurySprite;
            m_ResourceAmounts[_ResourceCount].text = a_Building.BuildingData.MercuryCost.ToString();
            _ResourceCount++;
        }

        if (a_Building.BuildingData.OreCost > 0)
        {
            m_ResourceImages[_ResourceCount].sprite = m_OreSprite;
            m_ResourceAmounts[_ResourceCount].text = a_Building.BuildingData.OreCost.ToString();
            _ResourceCount++;
        }

        if (a_Building.BuildingData.SulfurCost > 0)
        {
            m_ResourceImages[_ResourceCount].sprite = m_SulfurSprite;
            m_ResourceAmounts[_ResourceCount].text = a_Building.BuildingData.SulfurCost.ToString();
            _ResourceCount++;
        }

        if (a_Building.BuildingData.CrystalCost > 0)
        {
            m_ResourceImages[_ResourceCount].sprite = m_CrystalsSprite;
            m_ResourceAmounts[_ResourceCount].text = a_Building.BuildingData.CrystalCost.ToString();
            _ResourceCount++;
        }

        if (a_Building.BuildingData.GemCost > 0)
        {
            m_ResourceImages[_ResourceCount].sprite = m_GemsSprite;
            m_ResourceAmounts[_ResourceCount].text = a_Building.BuildingData.GemCost.ToString();
            _ResourceCount++;
        }

        if (a_Building.BuildingData.GoldCost > 0)
        {
            m_ResourceImages[_ResourceCount].sprite = m_GoldSprite;
            m_ResourceAmounts[_ResourceCount].text = a_Building.BuildingData.GoldCost.ToString();
            _ResourceCount++;
        }

        if (_ResourceCount > 3)
        {
            for (int i = 0; i < 4; i++)
            {
                m_ResourceImages[i].rectTransform.anchoredPosition = new Vector2(80 * (i - 1.5f), -59);
                m_ResourceAmounts[i].rectTransform.anchoredPosition = new Vector2(80 * (i - 1.5f), -83);

                m_ResourceImages[i].gameObject.SetActive(true);
                m_ResourceAmounts[i].gameObject.SetActive(true);
            }

            for (int i = 4; i < _ResourceCount; i++)
            {
                m_ResourceImages[i].rectTransform.anchoredPosition = new Vector2(80 * ((i-4) - (_ResourceCount-5) / 2.0f), -133);
                m_ResourceAmounts[i].rectTransform.anchoredPosition = new Vector2(80 * ((i-4) - (_ResourceCount-5) / 2.0f), -157);

                m_ResourceImages[i].gameObject.SetActive(true);
                m_ResourceAmounts[i].gameObject.SetActive(true);
            }

            for (int i = _ResourceCount; i < 7; i++)
            {
                m_ResourceImages[i].gameObject.SetActive(false);
                m_ResourceAmounts[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _ResourceCount; i++)
            {
                m_ResourceImages[i].rectTransform.anchoredPosition = new Vector2(80 * (i - (_ResourceCount-1) / 2.0f), -96);
                m_ResourceAmounts[i].rectTransform.anchoredPosition = new Vector2(80 * (i - (_ResourceCount-1) / 2.0f), -120);

                m_ResourceImages[i].gameObject.SetActive(true);
                m_ResourceAmounts[i].gameObject.SetActive(true);
            }

            for (int i = _ResourceCount; i < 7; i++)
            {
                m_ResourceImages[i].gameObject.SetActive(false);
                m_ResourceAmounts[i].gameObject.SetActive(false);
            }
        }
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
