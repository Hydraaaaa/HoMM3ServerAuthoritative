using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    static CursorManager s_Instance;

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        s_Instance = Instantiate(Resources.Load<CursorManager>("CursorManager"), Vector3.zero, Quaternion.identity);
        s_Instance.name = "CursorManager";
        DontDestroyOnLoad(s_Instance);
    }

    public static void SetCursor(Sprite a_Cursor, Vector2 a_Offset = default(Vector2))
    {
        s_Instance.m_Offset = new Vector3(a_Offset.x, a_Offset.y, 0);

        s_Instance.m_Cursor.sprite = a_Cursor;
        s_Instance.m_Cursor.rectTransform.sizeDelta = new Vector2(a_Cursor.rect.width, a_Cursor.rect.height);
        s_Instance.m_Cursor.rectTransform.anchoredPosition = Input.mousePosition + s_Instance.m_Offset;
    }

    public static void SetCursorVisible(bool a_Visible)
    {
        s_Instance.m_Cursor.enabled = a_Visible;
    }

    public static void ResetCursor()
    {
        SetCursor(s_Instance.m_DefaultSprite);
    }

    [SerializeField] Image m_Cursor;
    [SerializeField] Sprite m_DefaultSprite;

    Vector3 m_Offset;

    void Awake()
    {
        m_Cursor.sprite = m_DefaultSprite;
        m_Cursor.rectTransform.sizeDelta = new Vector2(m_DefaultSprite.rect.width, m_DefaultSprite.rect.height);
    }

    void Update()
    {
        Cursor.visible = false;
        m_Cursor.rectTransform.anchoredPosition = Input.mousePosition + m_Offset;
    }
}
