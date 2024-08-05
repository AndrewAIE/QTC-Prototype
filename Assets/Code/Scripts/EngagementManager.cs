using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngagementManager : MonoBehaviour
{
    public PlayerController Player;
    private Vector3 m_offset;

    private ENEMY enemy;

    public GameObject Canvas;
    public GameObject[] Indicators;
    private KeyCode[] Inputs = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    private KeyCode m_targetKey;
    public float InputTimer;
    public bool EngagementActive = false;
    private int m_indicator = 0;

    private int[] m_attacks = { 0, 2, 0, 3, 1 };

    
    void Update()
    {
        m_offset = new Vector3(0,1.3f,3);

        transform.position = Player.transform.position + m_offset;
        transform.forward = transform.position - Player.transform.position;
        
        if(EngagementActive)
        {
            if (Input.GetKeyDown(m_targetKey))
            {
                Indicators[m_attacks[m_indicator]].SetActive(false);
                CancelInvoke("FailInput");
                if (m_indicator == m_attacks.Length - 1)
                {
                    enemy.ResetEnemy();
                    enemy = null;
                    ResetEngagement();
                   
                    
                }
                else
                {                                      
                    SetIndicator();
                }                
            }
        }
    }


    public void EngageEnemy(ENEMY _enemy)
    {
        enemy = _enemy;
        Canvas.SetActive(true);
        EngagementActive = true;
        m_indicator = 0;
        SetIndicator();
    }
    

    public void SetIndicator()
    {
        Indicators[m_attacks[m_indicator]].SetActive(true);
        m_targetKey = Inputs[m_attacks[m_indicator]];
        m_indicator++;
        Invoke("FailInput", InputTimer);        
    }

    public void FailInput()
    {
        //Player.ResetPlayer();
        ResetEngagement();
    }

    public void ResetEngagement()
    {
        foreach(GameObject GO in Indicators)
        {
            GO.SetActive(false);
        }
        Canvas.SetActive(false);
        EngagementActive = false;
        m_indicator = 0;
    }


}
