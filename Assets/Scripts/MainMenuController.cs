using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject m_ScenarioScreen;
    [SerializeField] GameObject m_ScenarioList;
    [SerializeField] GameObject m_ScenarioSettings;
    [SerializeField] Image m_ScenarioArt;
    [SerializeField] Sprite[] m_ScenarioArtSprites;
    [SerializeField] GameObject[] m_DisabledForLoadGame;

    public void NewGamePressed()
    {
        m_ScenarioScreen.SetActive(true);
        m_ScenarioList.SetActive(false);
        m_ScenarioSettings.SetActive(false);

        foreach (GameObject _GO in m_DisabledForLoadGame)
        {
            _GO.SetActive(true);
        }

        m_ScenarioArt.sprite = m_ScenarioArtSprites[Random.Range(0, m_ScenarioArtSprites.Length)];
    }

    public void LoadGamePressed()
    {
        m_ScenarioScreen.SetActive(true);
        m_ScenarioList.SetActive(true);
        m_ScenarioSettings.SetActive(false);

        foreach (GameObject _GO in m_DisabledForLoadGame)
        {
            _GO.SetActive(false);
        }
    }

    public void BackPressed()
    {
        m_ScenarioScreen.SetActive(false);
    }
    
    public void ShowAvailableScenariosPressed()
    {
        if (m_ScenarioList.activeSelf)
        {
            m_ScenarioList.SetActive(false);
        }
        else
        {
            m_ScenarioList.SetActive(true);
            m_ScenarioSettings.SetActive(false);
        }
    }
    
    public void RandomMapPressed()
    {

    }
    
    public void AdvancedOptionsPressed()
    {
        if (m_ScenarioSettings.activeSelf)
        {
            m_ScenarioSettings.SetActive(false);
        }
        else
        {
            m_ScenarioList.SetActive(false);
            m_ScenarioSettings.SetActive(true);
        }
    }
}
