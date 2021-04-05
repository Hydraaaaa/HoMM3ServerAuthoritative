using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioEntry : MonoBehaviour
{
    public ScenarioList ScenarioList { get; set; }
    public Scenario Scenario { get; private set; }
    public Text NameText => m_NameText;
    public Text SizeText => m_SizeText;
    [SerializeField] Text m_NameText = null;
    [SerializeField] Text m_SizeText = null;
    [SerializeField] Text m_PlayerCountText = null;
    [SerializeField] Image m_VersionImage = null;
    [SerializeField] Image m_WinCondition = null;
    [SerializeField] Image m_LossCondition = null;
    [SerializeField] Color m_SelectedColor = Color.yellow;

    [Space]

    [SerializeField] Sprite m_ROESprite = null;
    [SerializeField] Sprite m_ABSprite = null;
    [SerializeField] Sprite m_SODSprite = null;

    public void SetScenario(Scenario a_Scenario)
    {
        Scenario = a_Scenario;

        if (a_Scenario != null)
        {
            if (a_Scenario.Name != "")
            {
                m_NameText.text = a_Scenario.Name;
            }
            else
            {
                m_NameText.text = "Unnamed";
            }

            switch (a_Scenario.Size)
            {
                case 36: m_SizeText.text = "S"; break;
                case 72: m_SizeText.text = "M"; break;
                case 108: m_SizeText.text = "L"; break;
                case 144: m_SizeText.text = "XL"; break;
                default: m_SizeText.text = "?"; break;
            }

            m_VersionImage.gameObject.SetActive(true);

            if (a_Scenario.Version == Scenario.RESTORATION_OF_ERATHIA)
            {
                m_VersionImage.sprite = m_ROESprite;
            }
            else if (a_Scenario.Version == Scenario.ARMAGEDDONS_BLADE)
            {
                m_VersionImage.sprite = m_ABSprite;
            }
            else
            {
                m_VersionImage.sprite = m_SODSprite;
            }

            m_PlayerCountText.text = a_Scenario.ComputerCount + "/" + a_Scenario.PlayerCount;

            m_WinCondition.gameObject.SetActive(true);

            switch (a_Scenario.WinCondition)
            {
                case 255: m_WinCondition.sprite = ScenarioList.NormalWinSprite; break;
                case 0: m_WinCondition.sprite = ScenarioList.AcquireArtifactSprite; break;
                case 1: m_WinCondition.sprite = ScenarioList.CreaturesSprite; break;
                case 2: m_WinCondition.sprite = ScenarioList.ResourcesSprite; break;
                case 3: m_WinCondition.sprite = ScenarioList.UpgradeTownSprite; break;
                case 4: m_WinCondition.sprite = ScenarioList.BuildGrailSprite; break;
                case 5: m_WinCondition.sprite = ScenarioList.DefeatHeroSprite; break;
                case 6: m_WinCondition.sprite = ScenarioList.CaptureTownSprite; break;
                case 7: m_WinCondition.sprite = ScenarioList.DefeatMonsterSprite; break;
                case 8: m_WinCondition.sprite = ScenarioList.DwellingsSprite; break;
                case 9: m_WinCondition.sprite = ScenarioList.MinesSprite; break;
                case 10: m_WinCondition.sprite = ScenarioList.TransportSprite; break;
            }

            m_LossCondition.gameObject.SetActive(true);

            switch (a_Scenario.LossCondition)
            {
                case 255: m_LossCondition.sprite = ScenarioList.NormalLossSprite; break;
                case 0: m_LossCondition.sprite = ScenarioList.LoseTownSprite; break;
                case 1: m_LossCondition.sprite = ScenarioList.LoseHeroSprite; break;
                case 2: m_LossCondition.sprite = ScenarioList.TimeExpiresSprite; break;
            }
        }
        else
        {
            m_NameText.text = "";
            m_SizeText.text = "";
            m_VersionImage.gameObject.SetActive(false);
            m_PlayerCountText.text = "";
            m_WinCondition.gameObject.SetActive(false);
            m_LossCondition.gameObject.SetActive(false);
        }

        if (m_NameText.preferredWidth > m_NameText.rectTransform.rect.width)
        {
            m_NameText.alignment = TextAnchor.UpperLeft;

            do
            {
                m_NameText.text = m_NameText.text.Substring(0, m_NameText.text.Length - 1);
            }
            while (m_NameText.preferredWidth > m_NameText.rectTransform.rect.width);
        }
        else
        {
            m_NameText.alignment = TextAnchor.UpperCenter;
        }
    }
    
    public void SetSelected(Scenario a_Scenario)
    {
        if (a_Scenario == Scenario)
        {
            m_NameText.color = m_SelectedColor;
            m_SizeText.color = m_SelectedColor;
            m_PlayerCountText.color = m_SelectedColor;
        }
        else
        {
            m_NameText.color = Color.white;
            m_SizeText.color = Color.white;
            m_PlayerCountText.color = Color.white;
        }
    }

    public void OnClick()
    {
        ScenarioList.EntryClicked(Scenario);
    }
}
