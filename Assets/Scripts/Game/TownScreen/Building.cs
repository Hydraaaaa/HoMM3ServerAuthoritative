using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Building : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image Image => m_Image;
    public GameObject Outline => m_Outline;

    [SerializeField] Image m_Image;
    [SerializeField] GameObject m_Outline;
    [SerializeField] Image m_Collider;

    void Reset()
    {
        m_Image = GetComponent<Image>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Outline")
            {
                m_Outline = transform.GetChild(i).gameObject;
            }

            if (transform.GetChild(i).name == "Collider")
            {
                m_Collider = transform.GetChild(i).GetComponent<Image>();
            }
        }
    }

    void Awake()
    {
        m_Collider.alphaHitTestMinimumThreshold = 1;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        m_Outline.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        m_Outline.SetActive(false);
    }
}
