using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_rb;

    public InputActionReference Move;
    public float Accel;
    public float TopSpeed;
    [SerializeField]
    private Camera m_playerCamera;
    Animator m_anim;

    private bool m_engaged = false;
    
    private Vector3 m_moveDirection;
    

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        m_moveDirection += Move.action.ReadValue<Vector2>().x * getCameraRight(m_playerCamera);
        m_moveDirection += Move.action.ReadValue<Vector2>().y * getCameraForward(m_playerCamera);
        
        m_rb.AddForce(m_moveDirection * Accel, ForceMode.Acceleration);       

        if(m_rb.velocity.magnitude > TopSpeed)
        {
            m_rb.velocity = m_rb.velocity.normalized * TopSpeed;
        }
        m_moveDirection = Vector3.zero;
    }    

    // Update is called once per frame
    void Update()
    {      
        if(Move.action.IsPressed())
        {
            m_anim.SetTrigger("Run");
        }
        if(m_rb.velocity.magnitude == 0)
        {
            m_anim.SetTrigger("Idle");
        }
    } 
    
    private Vector3 getCameraForward(Camera _playerCamera)
    {
        Vector3 forward = _playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
        
    }

    private Vector3 getCameraRight(Camera _playerCamera)
    {
        Vector3 right = _playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

}
