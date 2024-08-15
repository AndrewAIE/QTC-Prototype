using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator m_anim;
    private GameObject m_chaseTarget;
    private Rigidbody m_rb;
    private Collider m_collider;
    public float RunSpeed;
    private bool m_chasingTarget;
    private bool m_playerWithinRange;
    private bool m_engaged;
    private int m_mask = 1 << 3;
    public enum NonCombatStates
    {
        Idle,
        Patrol,
        ChasingPlayer,
        Dead
            
    }

    private NonCombatStates m_state;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponentInChildren<Animator>();
        m_collider = GetComponentInChildren<Collider>();
        m_mask = ~m_mask;
        m_engaged = false;
    }

    private void Start()
    {
        
        m_state = NonCombatStates.Idle;
    }

    public void GetCombatData()
    {

    }

    private void Update()
    {
        if(!m_engaged)
        {
            switch (m_state)
            {
                case NonCombatStates.Idle:
                    IdleUpdate();
                    break;
                case NonCombatStates.ChasingPlayer:
                    ChasingPlayerUpdate();
                    break;
                case NonCombatStates.Dead:
                    break;
            }
        }
        
    }

    private void FixedUpdate()
    {
        if(m_chasingTarget)
        {
            m_rb.AddForce(transform.forward * RunSpeed, ForceMode.Force);
        }
    }

    public void EnterCombat(PlayerController _player)
    {
        m_anim.SetTrigger("DrawWeapon");
        m_anim.SetBool("Run", false);
        transform.forward = facePlayer(_player.transform.position);
        m_rb.velocity = new Vector3(0,0,0);
        m_chasingTarget = false;
        m_engaged = true;           
    }

    public void ExitCombat()
    {        
        
        m_engaged = false;
    }

    public void Block(int _blockNum)
    {
        switch (_blockNum)
        {
            case 0:
                m_anim.SetTrigger("Block1");
                break;
            case 1:
                m_anim.SetTrigger("Block2");
                break;
            case 2:
                m_anim.SetTrigger("Block3");
                break;
            case 3:
                m_anim.SetTrigger("Block4");
                break;
            default:
                break;
        }
    }

    public void Attack(int _attackNum)
    {
        switch (_attackNum)
        {
            case 0:
                m_anim.SetTrigger("Attack2");
                break;
            case 1:
                m_anim.SetTrigger("Block1");
                break;
            case 2:
                m_anim.SetTrigger("Attack2");
                break;
            case 3:
                m_anim.SetTrigger("Block1");
                break;
            default:
                break;
        }
    }

    public void FailedInput()
    {
        Attack(1);
    }

    private Vector3 facePlayer(Vector3 _playerPos)
    {
        Vector3 facing = _playerPos - transform.position;
        return facing;
    }

    public void GainPoiseAdvantage()
    {
        m_anim.SetTrigger("Attack4");
    }

    public void LosePoiseAdvantage()
    {
        m_anim.SetTrigger("Impact");
    }

    public void WinCombat()
    {
        m_anim.SetTrigger("Attack4");
        m_anim.SetTrigger("SheatheWeapon");
        Invoke("ExitCombat", 5f);
        enterState(NonCombatStates.Idle);
    }

    public void LoseCombat()
    {        
        enterState(NonCombatStates.Dead);
        Invoke("ExitCombat", 1f);
    }


    private void enterState(NonCombatStates _state)
    {
        m_state = _state;
        switch(m_state)
        {
            case NonCombatStates.Idle:
                m_anim.SetBool("Run", false);
                break;
            case NonCombatStates.ChasingPlayer:
                m_chasingTarget = true;
                m_anim.SetBool("Run", true);
                break;
            case NonCombatStates.Dead:
                m_anim.SetBool("Dead", true);
                m_rb.isKinematic = true;
                m_collider.enabled = false;
                break;
        }
    }


    private void IdleUpdate()
    {
        if(m_playerWithinRange)
        {
            searchForPlayer();
        }
        
    }

    private void ChasingPlayerUpdate()
    {
        Vector3 facingVector = m_chaseTarget.transform.position - transform.position;
        transform.forward = facingVector;       
    }

    internal void PlayerFound()
    {        
        enterState(NonCombatStates.ChasingPlayer);
    }

    private void searchForPlayer()
    {
        RaycastHit hit;
        Vector3 facingVector = m_chaseTarget.transform.position - transform.position;
        if(Physics.Raycast(transform.position, facingVector, out hit, facingVector.magnitude, m_mask))
        {
            Debug.Log("Cant Find Player");
        }
        else
        {
            Debug.Log("Player Found");
            PlayerFound();
        }
    }

    public void PlayerWithinRange(GameObject _targetObject)
    {
        m_chaseTarget = _targetObject;
        m_playerWithinRange = true;
    }

    public void CantFindPlayer()
    {
        m_chaseTarget = null;
        m_playerWithinRange = false;
    }
}
