using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioListScrollbar : MonoBehaviour
{
    [SerializeField] RectTransform m_ScrollArea;
    [SerializeField] RectTransform m_Scrollbar;

    [SerializeField] ScenarioList m_ScenarioList;

    int m_ScrollSegments;

    bool m_Scrolling;
    bool m_PotentiallyScrolling;
    Vector3 m_MouseDownPos;

    float m_ScrollLength;
    float m_SegmentLength;

    void Start()
    {
        m_ScrollLength = m_ScrollArea.rect.height - m_Scrollbar.rect.height;
        m_ScrollSegments = Mathf.Max(m_ScenarioList.Maps.Count - m_ScenarioList.ScenarioEntries.Count, 0);
        m_SegmentLength = m_ScrollLength / m_ScrollSegments;
    }

    void Update()
    {
        if (m_PotentiallyScrolling)
        {
            if (Input.mousePosition != m_MouseDownPos)
            {
                m_Scrolling = true;
                m_PotentiallyScrolling = false;
            }
        }

        if (m_Scrolling)
        {
            Vector3 _LocalMousePos = m_ScrollArea.InverseTransformPoint(Input.mousePosition);

            _LocalMousePos.y += m_Scrollbar.rect.height / 2;

            int _SegmentPos = Mathf.RoundToInt(-_LocalMousePos.y / m_SegmentLength);

            _SegmentPos = Mathf.Clamp(_SegmentPos, 0, m_ScrollSegments);

            m_Scrollbar.anchoredPosition = new Vector2(0, Mathf.Round(-m_SegmentLength * _SegmentPos));

            m_ScenarioList.PopulateScenarioEntries(_SegmentPos);
        }
    }


    public void ScrollbarDown()
    {
        m_Scrolling = true;
    }

    public void ScrollbarUp()
    {
        m_Scrolling = false;
    }

    public void ScrollAreaDown()
    {
        Vector3 _LocalMousePos = m_ScrollArea.InverseTransformPoint(Input.mousePosition);

        int _NewSegmentPos = m_ScenarioList.ListOffset;

        if (_LocalMousePos.y > m_Scrollbar.anchoredPosition.y)
        {
            _NewSegmentPos -= m_ScenarioList.ScenarioEntries.Count - 1;
        }
        else
        {
            _NewSegmentPos += m_ScenarioList.ScenarioEntries.Count - 1;
        }

        _NewSegmentPos = Mathf.Clamp(_NewSegmentPos, 0, m_ScrollSegments);

        m_Scrollbar.anchoredPosition = new Vector2(0, Mathf.Round(-m_SegmentLength * _NewSegmentPos));

        m_ScenarioList.PopulateScenarioEntries(_NewSegmentPos);

        m_PotentiallyScrolling = true;
        m_MouseDownPos = Input.mousePosition;
    }

    public void ScrollAreaUp()
    {
        m_Scrolling = false;
        m_PotentiallyScrolling = false;
    }
}
