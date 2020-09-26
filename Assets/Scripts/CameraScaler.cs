using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] Camera m_Camera;

    void Update()
    {
        m_Camera.orthographicSize = Screen.height / 64.0f;
    }
}
