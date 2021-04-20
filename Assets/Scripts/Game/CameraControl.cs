using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] GameSettings m_GameSettings;
    [SerializeField] LocalOwnership m_LocalOwnership;
    [SerializeField] Map m_Map;

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

        m_LocalOwnership.OnHeroSelected += OnHeroSelected;
        m_LocalOwnership.OnTownSelected += OnTownSelected;
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

        if (!(m_LocalOwnership.SelectedHero != null &&
            m_LocalOwnership.SelectedHero.IsMoving))
        {
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

                        _Position.x = Mathf.Clamp(_Position.x, 0, m_GameSettings.Scenario.Size - 1);
                        _Position.y = Mathf.Clamp(_Position.y, -m_GameSettings.Scenario.Size + 1, 0);

                        transform.position = _Position;
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        if (m_LocalOwnership.SelectedHero != null &&
            m_LocalOwnership.SelectedHero.IsMoving)
        {
            transform.position = new Vector3
            (
                m_LocalOwnership.SelectedHero.transform.position.x - 1.5f,
                m_LocalOwnership.SelectedHero.transform.position.y + 0.5f,
                transform.position.z
            );
        }
    }

    void OnHeroSelected(MapHero a_Hero, int a_Index)
    {
        m_Map.ShowUnderground(a_Hero.IsUnderground);

        transform.position = new Vector3
        (
            a_Hero.transform.position.x - 1.5f,
            a_Hero.transform.position.y + 0.5f,
            transform.position.z
        );
    }

    void OnTownSelected(MapTown a_Town, int a_Index)
    {
        m_Map.ShowUnderground(a_Town.IsUnderground);

        transform.position = new Vector3
        (
            a_Town.transform.position.x - 2.5f,
            a_Town.transform.position.y + 0.5f,
            transform.position.z
        );
    }
}
