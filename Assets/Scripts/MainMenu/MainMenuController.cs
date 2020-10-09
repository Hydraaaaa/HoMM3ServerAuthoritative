using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject m_ScenarioScreen = null;
    [SerializeField] GameObject m_ScenarioList = null;
    [SerializeField] GameObject m_ScenarioSettings = null;
    [SerializeField] Image m_ScenarioArt = null;
    [SerializeField] Sprite[] m_ScenarioArtSprites = null;
    [SerializeField] GameObject[] m_DisabledForLoadGame = null;
    [SerializeField] GameObject[] m_EnabledForLoadGame = null;
    [SerializeField] EventSystem m_EventSystem = null;
    [SerializeField] Button m_BackButton = null;
    [SerializeField] RectTransform m_CenterAnchor = null;

    void Update()
    {
        Vector2 _AnchoredPosition = m_CenterAnchor.anchoredPosition;

        if (Screen.width / 2.0f != Mathf.Round(Screen.width / 2.0f))
        {
            _AnchoredPosition.x = 0.5f;
        }
        else
        {
            _AnchoredPosition.x = 0.0f;
        }

        if (Screen.height / 2.0f != Mathf.Round(Screen.height / 2.0f))
        {
            _AnchoredPosition.y = 0.5f;
        }
        else
        {
            _AnchoredPosition.y = 0.0f;
        }

        m_CenterAnchor.anchoredPosition = _AnchoredPosition;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_BackButton.OnPointerDown(new PointerEventData(m_EventSystem));
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_BackButton.OnPointerUp(new PointerEventData(m_EventSystem));
            BackPressed();
        }
    }
    public void NewGamePressed()
    {
        m_ScenarioScreen.SetActive(true);
        m_ScenarioList.SetActive(false);
        m_ScenarioSettings.SetActive(false);

        foreach (GameObject _GO in m_DisabledForLoadGame)
        {
            _GO.SetActive(true);
        }

        foreach (GameObject _GO in m_EnabledForLoadGame)
        {
            _GO.SetActive(false);
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

        foreach (GameObject _GO in m_EnabledForLoadGame)
        {
            _GO.SetActive(true);
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
