﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenarioInfo : MonoBehaviour
{
    [SerializeField] GameSettings m_Settings = null;
    [SerializeField] Text m_RatingText = null;
    [SerializeField] Button m_EasyButton = null;
    [SerializeField] Button m_NormalButton = null;
    [SerializeField] Button m_HardButton = null;
    [SerializeField] Button m_ExpertButton = null;
    [SerializeField] Button m_ImpossibleButton = null;

    [Space]

    [SerializeField] Sprite m_EasySelectedSprite = null;
    [SerializeField] Sprite m_NormalSelectedSprite = null;
    [SerializeField] Sprite m_HardSelectedSprite = null;
    [SerializeField] Sprite m_ExpertSelectedSprite = null;
    [SerializeField] Sprite m_ImpossibleSelectedSprite = null;

    Sprite m_EasySprite;
    Sprite m_NormalSprite;
    Sprite m_HardSprite;
    Sprite m_ExpertSprite;
    Sprite m_ImpossibleSprite;

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
        m_Settings.Rating = 0;
        m_RatingText.text = "80%";

        ResetSprites();
        m_EasyButton.image.sprite = m_EasySelectedSprite;
    }

    public void NormalPressed()
    {
        m_Settings.Rating = 1;
        m_RatingText.text = "100%";

        ResetSprites();
        m_NormalButton.image.sprite = m_NormalSelectedSprite;
    }

    public void HardPressed()
    {
        m_Settings.Rating = 2;
        m_RatingText.text = "130%";

        ResetSprites();
        m_HardButton.image.sprite = m_HardSelectedSprite;
    }

    public void ExpertPressed()
    {
        m_Settings.Rating = 3;
        m_RatingText.text = "160%";

        ResetSprites();
        m_ExpertButton.image.sprite = m_ExpertSelectedSprite;
    }

    public void ImpossiblePressed()
    {
        m_Settings.Rating = 4;
        m_RatingText.text = "200%";

        ResetSprites();
        m_ImpossibleButton.image.sprite = m_ImpossibleSelectedSprite;
    }

    public void BeginPressed()
    {
        SceneManager.LoadScene("Game");
    }
}
