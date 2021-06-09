using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenarioInfo : MonoBehaviour
{
    [SerializeField] GameSettings m_Settings;
    [SerializeField] Text m_RatingText;
    [SerializeField] Button m_EasyButton;
    [SerializeField] Button m_NormalButton;
    [SerializeField] Button m_HardButton;
    [SerializeField] Button m_ExpertButton;
    [SerializeField] Button m_ImpossibleButton;

    [Space]

    [SerializeField] Sprite m_EasySelectedSprite;
    [SerializeField] Sprite m_NormalSelectedSprite;
    [SerializeField] Sprite m_HardSelectedSprite;
    [SerializeField] Sprite m_ExpertSelectedSprite;
    [SerializeField] Sprite m_ImpossibleSelectedSprite;

    Sprite m_EasySprite;
    Sprite m_NormalSprite;
    Sprite m_HardSprite;
    Sprite m_ExpertSprite;
    Sprite m_ImpossibleSprite;

    public Action OnGameStart;

    void Awake()
    {
        m_EasySprite = m_EasyButton.image.sprite;
        m_NormalSprite = m_NormalButton.image.sprite;
        m_HardSprite = m_HardButton.image.sprite;
        m_ExpertSprite = m_ExpertButton.image.sprite;
        m_ImpossibleSprite = m_ImpossibleButton.image.sprite;

        NormalPressed();
    }

    void ResetSprites()
    {
        m_EasyButton.image.sprite = m_EasySprite;
        m_NormalButton.image.sprite = m_NormalSprite;
        m_HardButton.image.sprite = m_HardSprite;
        m_ExpertButton.image.sprite = m_ExpertSprite;
        m_ImpossibleButton.image.sprite = m_ImpossibleSprite;
    }

    public void EasyPressed()
    {
        m_Settings.Rating = GameSettings.RATING_EASY;
        m_RatingText.text = "80%";

        ResetSprites();
        m_EasyButton.image.sprite = m_EasySelectedSprite;
    }

    public void NormalPressed()
    {
        m_Settings.Rating = GameSettings.RATING_NORMAL;
        m_RatingText.text = "100%";

        ResetSprites();
        m_NormalButton.image.sprite = m_NormalSelectedSprite;
    }

    public void HardPressed()
    {
        m_Settings.Rating = GameSettings.RATING_HARD;
        m_RatingText.text = "130%";

        ResetSprites();
        m_HardButton.image.sprite = m_HardSelectedSprite;
    }

    public void ExpertPressed()
    {
        m_Settings.Rating = GameSettings.RATING_EXPERT;
        m_RatingText.text = "160%";

        ResetSprites();
        m_ExpertButton.image.sprite = m_ExpertSelectedSprite;
    }

    public void ImpossiblePressed()
    {
        m_Settings.Rating = GameSettings.RATING_IMPOSSIBLE;
        m_RatingText.text = "200%";

        ResetSprites();
        m_ImpossibleButton.image.sprite = m_ImpossibleSelectedSprite;
    }

    public void BeginPressed()
    {
        OnGameStart?.Invoke();

        SceneManager.LoadScene("Game");
    }
}
