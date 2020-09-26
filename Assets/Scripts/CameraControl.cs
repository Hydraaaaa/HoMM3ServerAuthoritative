using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] GameSettings m_GameSettings;

    [Space]

    [SerializeField] float m_PanSpeed = 1.0f;

    bool m_Active;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        m_Active = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            m_Active = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Confined;
            m_Active = true;
        }

        if (m_Active)
        {
            Vector3 _Position = transform.position;

            if (Input.mousePosition.x <= 2)
            {
                _Position.x -= m_PanSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.x >= Screen.width - 3)
            {
                _Position.x += m_PanSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y <= 2)
            {
                _Position.y -= m_PanSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.y >= Screen.height - 3)
            {
                _Position.y += m_PanSpeed * Time.deltaTime;
            }

            _Position.x = Mathf.Clamp(_Position.x, -0.5f, m_GameSettings.Map.Size - 0.5f);
            _Position.y = Mathf.Clamp(_Position.y, -m_GameSettings.Map.Size - 0.5f, 0.5f);

            transform.position = _Position;
        }
    }
}
