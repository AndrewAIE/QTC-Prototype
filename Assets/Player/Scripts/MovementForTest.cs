using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementForTest : MonoBehaviour
{
    public bool Engaged;
    public float Speed;
    public EngagementManager EngagementMan;
    private Vector3 m_resetPos;
    private Input m_playerInput;


    public void Start()
    {
        m_resetPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if(!Engaged)
        {
            if(Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.forward * -Speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.right * -Speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Speed * Time.deltaTime);
            }
        }
    }

    public void Engage(ENEMY _enemy)
    {
        Engaged = true;
        EngagementMan.EngageEnemy(_enemy);

    }

    public void ResetPlayer()
    {
        transform.position = m_resetPos;
        if(Engaged)
        {
            Engaged = false;
        }
    }
}
