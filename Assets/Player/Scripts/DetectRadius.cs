using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRadius : MonoBehaviour
{
    private SphereCollider m_detectionRadius;
    private PlayerController m_player;
    // Start is called before the first frame update
    void Awake()
    {
        m_detectionRadius = GetComponent<SphereCollider>();
        m_player = GetComponent<PlayerController>();
    }

    

}
