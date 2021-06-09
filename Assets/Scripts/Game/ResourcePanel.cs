using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour
{
    [SerializeField] PlayerResources m_Resources;

    [SerializeField] Text m_Wood;
    [SerializeField] Text m_Mercury;
    [SerializeField] Text m_Ore;
    [SerializeField] Text m_Sulfur;
    [SerializeField] Text m_Crystals;
    [SerializeField] Text m_Gems;
    [SerializeField] Text m_Gold;

    void Start()
    {
        m_Wood.text = m_Resources.Wood.ToString();
        m_Mercury.text = m_Resources.Mercury.ToString();
        m_Ore.text = m_Resources.Ore.ToString();
        m_Sulfur.text = m_Resources.Sulfur.ToString();
        m_Crystals.text = m_Resources.Crystals.ToString();
        m_Gems.text = m_Resources.Gems.ToString();
        m_Gold.text = m_Resources.Gold.ToString();
    }
}
