using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownScreenAnimator : MonoBehaviour
{
    [SerializeField] Image m_Image;
    [SerializeField] Sprite[] m_Sprites;

    float m_CurrentFrameTime;

    int m_CurrentFrame;

    Color m_Invisible;
    Color m_Visible;

    void Awake()
    {
        m_Invisible = new Color(1, 1, 1, 0);
        m_Visible = new Color(1, 1, 1, 1);
    }

    void Update()
    {
        m_CurrentFrameTime += Time.deltaTime;

        if (m_CurrentFrameTime > 0.181818f)
        {
            m_CurrentFrameTime -= 0.181818f;
            m_CurrentFrame++;

            if (m_CurrentFrame == m_Sprites.Length)
            {
                m_CurrentFrame = 0;
            }

            m_Image.sprite = m_Sprites[m_CurrentFrame];

            if (m_Image.sprite == null)
            {
                m_Image.color = m_Invisible;
            }
            else
            {
                m_Image.color = m_Visible;
            }
        }
    }
}
