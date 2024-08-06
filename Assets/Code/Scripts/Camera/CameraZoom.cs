using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private CinemachineFreeLook m_freeLook;    
    public InputActionReference GamePadRightStick;


    [SerializeField]
    private float m_maxZoom, m_minZoom, m_startingZoom, m_raddiChangeRate, m_heightChangeRate, m_zoomSpeed;    
    private float m_zoomDistance;
    [SerializeField]
    private float[] orbitRadii, orbitHeight;


    private void Awake()
    {
        m_freeLook = GetComponent<CinemachineFreeLook>();
        m_freeLook.m_CommonLens = true;
        m_zoomDistance = m_startingZoom;
    }

    private void Update()
    {
        
        m_zoomDistance -= GamePadRightStick.action.ReadValue<Vector2>().y * Time.deltaTime * m_zoomSpeed;

        //If Camera Goes beyond its max or min, set to corresponding extremity
        if (m_zoomDistance > m_maxZoom)
        {
            m_zoomDistance = m_maxZoom;
        }
        if (m_zoomDistance < m_minZoom)
        {
            m_zoomDistance = m_minZoom;
        }        

        for (int i = 0; i < 3; i++)
        {
            m_freeLook.m_Orbits[i].m_Radius = orbitRadii[i] * m_zoomDistance * m_raddiChangeRate;
            m_freeLook.m_Orbits[i].m_Height = orbitHeight[i] * m_zoomDistance * m_heightChangeRate;
        }        
    }


}
