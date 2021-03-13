using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectRenderer : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_SpriteRenderer;

    Sprite[] m_Sprites;

    bool m_Active = false;

    int m_Offset = 0;

    void OnDestroy()
    {
        if (m_Active)
        {
            MapObjectRendererManager.RemoveObject(this);
        }
    }

    public void SetOffset(int a_Offset)
    {
        m_Offset = a_Offset;
    }

    public void SetSprites(Sprite[] a_Sprites)
    {
        if (a_Sprites == null ||
            a_Sprites.Length < 2)
        {
            if (m_Active)
            {
                MapObjectRendererManager.RemoveObject(this);
            }

            m_Active = false;
        }
        else
        {
            if (!m_Active)
            {
                MapObjectRendererManager.AddObject(this);
            }

            m_Active = true;
        }

        m_Sprites = a_Sprites;

        if (m_Sprites != null &&
            m_Sprites.Length > 0)
        {
            m_SpriteRenderer.sprite = m_Sprites[0];
        }
        else
        {
            m_SpriteRenderer.sprite = null;
        }
    }

    public void Animate(int a_Frame)
    {
        m_SpriteRenderer.sprite = m_Sprites[(a_Frame + m_Offset) % m_Sprites.Length];
    }
}
