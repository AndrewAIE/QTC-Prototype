using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : MonoBehaviour
{
    private CharacterController m_cC;

    public InputActionReference Move;    
    public float Speed;
    [SerializeField]
    private Camera m_playerCamera;
    Animator m_anim;

    private bool m_engaged;
    private bool m_moving;

    public float m_turnSpeed;

    private Vector3 m_moveDirection;
    [SerializeField]
    private QTE_System m_combatSystem;

    private Vector3 m_startPos;

    private EnemyController m_enemy;

    public bool CombatAnimRunning = false;

    private void Awake()
    {
        m_cC = GetComponent<CharacterController>();
        m_anim = GetComponentInChildren<Animator>();
        m_engaged = false;
        m_startPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {        
        if (m_moving)
        {            
            Vector3 rightVector = Move.action.ReadValue<Vector2>().x * getCameraRight(m_playerCamera);
            Vector3 forwardVector = Move.action.ReadValue<Vector2>().y * getCameraForward(m_playerCamera);
            
            m_moveDirection = rightVector + forwardVector;
            m_moveDirection.Normalize();

            transform.forward = m_moveDirection;
            m_cC.SimpleMove(m_moveDirection * Speed);
        }             
    }    

    // Update is called once per frame
    void Update()
    {
        //set facing direction based off of input
        if(m_engaged)
        {
            
        }
        else
        {
            handleOnInput();            
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

    private void handleOnInput()
    {
        if (Move.action.IsInProgress())
        {
            m_moving = true;
            m_anim.SetBool("Run", true);
        }
        if (Move.action.WasReleasedThisFrame())
        {
            m_moving = false;
            m_anim.SetBool("Run", false);
        }
    }

    private void resetPlayer()
    {
        ExitCombat();
        m_anim.SetBool("Dead", false);
        transform.position = m_startPos;
    }

    #region QTECombat
    public void EnterCombat(EnemyController _enemy)
    {
        m_anim.SetTrigger("DrawWeapon");
        m_anim.SetBool("Run", false);
        transform.forward = FaceTarget(_enemy.gameObject);
        m_enemy = _enemy;
        Invoke("startQTE", 1f);
        m_engaged = true;
        m_moving = false;
    }

    private void startQTE()
    {
        m_combatSystem.StartCombat(m_enemy);
    }

    public void ExitCombat()
    {
        
        
        m_engaged = false;
    }

    public Vector3 FaceTarget(GameObject _otherObject)
    {
        Vector3 facing = _otherObject.transform.position - transform.position;
        return facing;
    }

    public void Block(int _blockNum)
    {
        switch(_blockNum)
        {
            case 0:
                m_anim.SetTrigger("Block1");
                CombatAnimRunning = false;
                break;
            case 1:
                m_anim.SetTrigger("Block1");
                CombatAnimRunning = false;
                break;
            case 2:
                m_anim.SetTrigger("Block1");
                CombatAnimRunning = false;
                break;
            case 3:
                m_anim.SetTrigger("Block1");
                CombatAnimRunning = false;
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
                CombatAnimRunning = true;
                Debug.Log("Combat Anim Finished = " + CombatAnimRunning);
                break;
            case 1:
                m_anim.SetTrigger("Block1");                
                CombatAnimRunning = true;
                Debug.Log("Combat Anim Finished = " + CombatAnimRunning);
                break;
            case 2:
                m_anim.SetTrigger("Attack2");
                CombatAnimRunning = true;
                Debug.Log("Combat Anim Finished = " + CombatAnimRunning);
                break;
            case 3:
                m_anim.SetTrigger("Block1");
                CombatAnimRunning = true;
                Debug.Log("Combat Anim Finished = " + CombatAnimRunning);
                break;
            default:
                break;
        }
    }

    public void FailedInput()
    {
        m_anim.SetTrigger("Impact");
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
        m_anim.SetTrigger("Attack3");
        m_anim.SetTrigger("SheatheWeapon");
        Invoke("ExitCombat", 3f);
    }

    public void LoseCombat()
    {
        m_anim.SetTrigger("Dead");
        Invoke("resetPlayer", 4f);
    }

    public void AnimationFinished()
    {
        CombatAnimRunning = false;
        Debug.Log("Combat Anim Running = " + CombatAnimRunning);
    }
    #endregion
}
