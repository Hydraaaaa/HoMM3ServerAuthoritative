using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] GameSettings m_GameSettings = null;

    [Space]

    [SerializeField] float m_PanSpeed = 1.0f;

    bool m_Active;

    float m_MoveCooldown = 0.07f;
    float m_CurrentMoveCooldown;
    bool m_IsMoving;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        m_Active = true;
        m_IsMoving = false;

        transform.position = new Vector3
        (
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            transform.position.z
        );
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
            bool _IsMoving = false;

            Vector3 _Position = transform.position;

            if (Input.mousePosition.x <= 2)
            {
                _Position.x -= 2;
                _IsMoving = true;
            }
            else if (Input.mousePosition.x >= Screen.width - 3)
            {
                _Position.x += 2;
                _IsMoving = true;
            }

            if (Input.mousePosition.y <= 2)
            {
                _Position.y -= 2;
                _IsMoving = true;
            }
            else if (Input.mousePosition.y >= Screen.height - 3)
            {
                _Position.y += 2;
                _IsMoving = true;
            }

            m_IsMoving = _IsMoving;

            if (!m_IsMoving)
            {
                m_CurrentMoveCooldown = 0;
            }
            else
            {
                m_CurrentMoveCooldown -= Time.deltaTime;

                if (m_CurrentMoveCooldown <= 0)
                {
                    m_CurrentMoveCooldown += m_MoveCooldown;

                    _Position.x = Mathf.Clamp(_Position.x, -0.5f, m_GameSettings.Scenario.Size - 0.5f);
                    _Position.y = Mathf.Clamp(_Position.y, -m_GameSettings.Scenario.Size - 0.5f, 0.5f);

                    transform.position = _Position;
                }
            }
        }
    }
}
