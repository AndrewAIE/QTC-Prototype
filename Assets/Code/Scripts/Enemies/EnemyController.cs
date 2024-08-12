using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator m_anim;
    private bool m_engaged;
    private enum nonCombatStates
    {
        Idle,
        Patrol,
        ChasingPlayer
    }

    private void Awake()
    {
        
        m_engaged = false;

    }

    private void Start()
    {
        m_anim = GetComponentInChildren<Animator>();
    }

    public void GetCombatData()
    {

    }




    public void EnterCombat(PlayerController _player)
    {
        m_anim.SetTrigger("DrawWeapon");
        m_anim.SetBool("Run", false);
        transform.forward = facePlayer(_player.transform.position);        
        m_engaged = true;           
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
                m_anim.SetTrigger("Attack1");
                break;
            case 1:
                m_anim.SetTrigger("Attack2");
                break;
            case 2:
                m_anim.SetTrigger("Attack3");
                break;
            case 3:
                m_anim.SetTrigger("Attack4");
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
        Invoke("ExitCombat", 1f);
    }

    public void LoseCombat()
    {
        m_anim.SetTrigger("Attack4");
        Invoke("ExitCombat", 1f);
    }
}
