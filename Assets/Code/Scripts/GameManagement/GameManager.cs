using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    #region StartUp
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }
    #endregion
    
    void Update()
    {
        
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        //Debug.Log("Application Version : " + Application.version);
    }
#endif

}
