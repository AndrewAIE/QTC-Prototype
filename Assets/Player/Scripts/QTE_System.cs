using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class QTE_System: MonoBehaviour
{

    private PlayerInputs i_InputSys;
    private InputAction i_North, i_South, i_East, i_West, i_currentKey;
    


    #region Startup
    private void Awake()
    {
        i_InputSys = new PlayerInputs();

    }


    void Start()
    {
        
    }
    private void OnEnable()
    {
        i_North = i_InputSys.QTE.North;
        i_South = i_InputSys.QTE.South;
        i_East = i_InputSys.QTE.East;
        i_West = i_InputSys.QTE.West;

    }
    #endregion

    #region Updates
    // Update is called once per frame
    void Update()
    {
        
    }
    private bool QTECheck()
    {
        return false;

    }


    #endregion

    #region Destroy/Disable
    private void OnDisable()
    {
        
    }
    #endregion

}
