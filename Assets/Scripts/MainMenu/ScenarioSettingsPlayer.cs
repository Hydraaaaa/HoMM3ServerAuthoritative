using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScenarioSettingsPlayer : MonoBehaviour
{
    public int PlayerIndex { get; private set; }

    [SerializeField] ScenarioSettings m_ScenarioSettings = null;
    [SerializeField] Image m_BackgroundImage = null;
    [SerializeField] Button m_FlagButton = null;
    [SerializeField] Text m_NameText = null;
    [SerializeField] Text m_HumanOrCPUText = null;

    [Space]

    [SerializeField] Sprite[] m_BackgroundSprites = null;
    [SerializeField] Sprite[] m_FlagSprites = null;
    [SerializeField] Sprite[] m_FlagHoverSprites = null;
    [SerializeField] Sprite[] m_FlagPressedSprites = null;

    public void Initialize(int a_Index, PlayerInfo a_PlayerInfo)
    {
        PlayerIndex = a_Index;

        m_BackgroundImage.sprite = m_BackgroundSprites[a_Index];

        m_FlagButton.image.sprite = m_FlagSprites[a_Index];

        SpriteState _State = m_FlagButton.spriteState;

        _State.highlightedSprite = m_FlagHoverSprites[a_Index];
        _State.pressedSprite = m_FlagPressedSprites[a_Index];

        m_FlagButton.spriteState = _State;

        m_FlagButton.gameObject.SetActive(a_PlayerInfo.HumanPlayable);

        if (a_PlayerInfo.HumanPlayable)
        {
            m_HumanOrCPUText.text = "Human or CPU";
            m_HumanOrCPUText.rectTransform.anchoredPosition = new Vector2
            (
                m_HumanOrCPUText.rectTransform.anchoredPosition.x,
                -23
            );
        }
        else
        {
            m_HumanOrCPUText.text = "CPU";
            m_HumanOrCPUText.rectTransform.anchoredPosition = new Vector2
            (
                m_HumanOrCPUText.rectTransform.anchoredPosition.x,
                -22
            );
        }
    }

    public void SetName(string a_Name)
    {
        m_NameText.text = a_Name;
    }

    public void FlagPressed()
    {
        m_ScenarioSettings.FlagPressed(this);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
