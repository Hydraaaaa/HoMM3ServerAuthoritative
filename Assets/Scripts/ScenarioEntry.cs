using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioEntry : MonoBehaviour
{
    public ScenarioList ScenarioList { get; set; }
    public Map Map { get; private set; }
    public Text NameText => m_NameText;
    public Text SizeText => m_SizeText;
    [SerializeField] Text m_NameText;
    [SerializeField] Text m_SizeText;
    [SerializeField] Image m_VersionImage;
    [SerializeField] Color m_SelectedColor;

    [SerializeField] Sprite m_ROESprite;
    [SerializeField] Sprite m_ABSprite;
    [SerializeField] Sprite m_SODSprite;

    public void SetMap(Map a_Map)
    {
        Map = a_Map;

        if (a_Map != null)
        {
            m_NameText.text = a_Map.Name;

            switch (a_Map.Size)
            {
                case 36: m_SizeText.text = "S"; break;
                case 72: m_SizeText.text = "M"; break;
                case 108: m_SizeText.text = "L"; break;
                case 144: m_SizeText.text = "XL"; break;
                default: m_SizeText.text = "?"; break;
            }
        }
        else
        {
            m_NameText.text = "";
            m_SizeText.text = "";
        }

        if (a_Map.Version == Map.RESTORATION_OF_ERATHIA)
        {
            m_VersionImage.sprite = m_ROESprite;
        }
        else if (a_Map.Version == Map.ARMAGEDDONS_BLADE)
        {
            m_VersionImage.sprite = m_ABSprite;
        }
        else
        {
            m_VersionImage.sprite = m_SODSprite;
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
    
    public void SetSelected(Map a_Map)
    {
        if (a_Map == Map)
        {
            m_NameText.color = m_SelectedColor;
            m_SizeText.color = m_SelectedColor;
        }
        else
        {
            m_NameText.color = Color.white;
            m_SizeText.color = Color.white;
        }
    }

    public void OnClick()
    {
        ScenarioList.EntryClicked(Map);
    }
}
