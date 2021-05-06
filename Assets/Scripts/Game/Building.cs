using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Building : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image m_Collider;
    [SerializeField] GameObject m_Outline;

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
