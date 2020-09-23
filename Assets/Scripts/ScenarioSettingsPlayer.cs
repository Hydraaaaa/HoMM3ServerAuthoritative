using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSettingsPlayer : MonoBehaviour
{
    public int PlayerIndex { get; set; }

    public Image Image => m_Image;
    [SerializeField] Image m_Image;
}
