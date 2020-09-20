using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioInfo : MonoBehaviour
{
    [SerializeField] GameSettings m_Settings;
    [SerializeField] Text m_RatingText;

    public void EasyPressed()
    {
        m_Settings.Rating = 0;
        m_RatingText.text = "80%";
    }

    public void NormalPressed()
    {
        m_Settings.Rating = 1;
        m_RatingText.text = "100%";
    }

    public void HardPressed()
    {
        m_Settings.Rating = 2;
        m_RatingText.text = "130%";
    }

    public void ExpertPressed()
    {
        m_Settings.Rating = 3;
        m_RatingText.text = "160%";
    }

    public void ImpossiblePressed()
    {
        m_Settings.Rating = 4;
        m_RatingText.text = "200%";
    }
}
