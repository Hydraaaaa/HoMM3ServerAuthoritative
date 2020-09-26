using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSettingsPlayer : MonoBehaviour
{
    public int PlayerIndex { get; private set; }

    [SerializeField] Image m_BackgroundImage = null;
    [SerializeField] Button m_FlagButton = null;

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
    }

    public void FlagPressed()
    {

    }
}
