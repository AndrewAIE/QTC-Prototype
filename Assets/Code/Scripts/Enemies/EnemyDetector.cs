using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDetector : MonoBehaviour
{
    private EnemyController m_enemy;

    private void Awake()
    {
        m_enemy = GetComponentInParent<EnemyController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_enemy.PlayerFound(other.gameObject);        

        }
    }

}
