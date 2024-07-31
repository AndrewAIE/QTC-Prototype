using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionTest : MonoBehaviour
{
    private MovementForTest m_player;

    private void Start()
    {
        m_player = GetComponentInParent<MovementForTest>();
    }


    private void OnTriggerEnter(Collider collision)
    {        
        if (collision.gameObject.CompareTag("EnemyDetect"))
        {
            ENEMY enemy = collision.gameObject.GetComponentInParent<ENEMY>();
            m_player.Engage(enemy);
        }
    }
}
