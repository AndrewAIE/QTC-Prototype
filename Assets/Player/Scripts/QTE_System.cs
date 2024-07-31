using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QTE_System: MonoBehaviour
{

    private PlayerInputs i_InputSys;
    private InputAction i_North, i_South, i_East, i_West;
    [SerializeField] private Toggle[] ButtonCanvas;


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
        // assign all input action to a PlayerInput action
        i_North = i_InputSys.QTE.North;
        i_South = i_InputSys.QTE.South;
        i_East = i_InputSys.QTE.East;
        i_West = i_InputSys.QTE.West;

        // enable all keys
        i_North.Enable();
        i_South.Enable();
        i_East.Enable();
        i_West.Enable();
    }
    #endregion

    #region Updates
    // Update is called once per frame
    void Update()
    {
        QTECheck();
    }
    private bool QTECheck()
    {
        bool[] Input = new bool[] { i_North.WasPressedThisFrame(), i_East.WasPressedThisFrame(), i_South.WasPressedThisFrame(), i_West.WasPressedThisFrame() };
        bool[] RequiredIn = new bool[] {true, false, false, false };

        bool isEmpty = true;
        

        for(int i = 0; i < Input.Length; i++)
        {
            if(isEmpty == true && Input[i] == true) isEmpty = false;
        }

        if (!isEmpty)
        {
            for (int i = 0; i < Input.Length; i++)
            {
                ButtonCanvas[i].isOn = Input[i];
            }
        }
        
        if (Input == RequiredIn)
        {
            return true;
        }

        return false;
    }


    #endregion

    #region Destroy/Disable
    private void OnDisable()
    {
        
    }
    #endregion

}
