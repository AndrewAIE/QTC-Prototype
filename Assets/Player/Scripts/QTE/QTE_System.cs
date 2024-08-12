using QTCGlobals;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class QTE_System : MonoBehaviour
{
    [SerializeReference] private QTEState m_currentState = QTEState.Waiting;
    private QTEState m_oldState = QTEState.Default;

    PlayerInputs i_playerInputs;
    private InputAction i_North, i_South, i_East, i_West;

    
    [SerializeField] private Toggle[] ButtonCanvas;
    [SerializeField] private TextMeshProUGUI TextCanvas;

    /// <summary>
    /// reference to the player
    /// </summary>
    [SerializeField] private PlayerController m_playerReference;

    /// <summary>
    /// the QTE input for the player
    /// </summary>
    private QTEINPUTS m_playerQTEInput;
    private bool m_noInput;

    //////// DELETE FOR BUILDS
    public bool start;

    ////////

    /// <summary>
    /// public access to the current stream (need to set up) (only change when not running)
    /// </summary>
    public QTE StreamOBJ;
    /// <summary>
    /// the current stream (if any)
    /// </summary>
    private QTE m_StreamOBJ;
    /// <summary>
    /// position in the QTEStream
    /// </summary>
    [SerializeField] private int m_streamPosition = 0;

    #region Startup
    void Awake()
    {
        i_playerInputs = new PlayerInputs();
    }
    private void OnEnable()
    {
        // assign all input action to a PlayerInput action
        i_North = i_playerInputs.QTE.North;
        i_South = i_playerInputs.QTE.South;
        i_East = i_playerInputs.QTE.East;
        i_West = i_playerInputs.QTE.West;

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
        OnStateChange();
        switch (m_currentState)
        {
            case QTEState.Waiting:

                m_StreamOBJ = Instantiate(StreamOBJ); // creates a new version of 
                
                break;

            case QTEState.Running:
                gatherInput();

                StreamManager();
                if (m_StreamOBJ.stream.qteInputs.Length <= m_streamPosition)
                {
                    OnComplete();
                }
                else if (m_StreamOBJ.stream.qteInputs[m_streamPosition].TimeToComplete <= 0)
                {
                    OnFail();
                }

                break;
        }
        Display();
    }


    #endregion
    #region Input
    private void gatherInput()
    {
        m_playerQTEInput = new QTEINPUTS
        {
            inputs = new bool[] { i_North.WasPressedThisFrame(), i_East.WasPressedThisFrame(), i_South.WasPressedThisFrame(), i_West.WasPressedThisFrame() }
        };
        m_noInput = true;
        for (int i = 0; i < 4; i++)
        {
            if (m_noInput == true && m_playerQTEInput.inputs[i] == true) m_noInput = false;
        }
    }
    #endregion
    #region QTE's
    /// <summary>
    /// Manages the QTE
    /// </summary>
    private void StreamManager()
    {
        if (m_StreamOBJ.stream.qteInputs.Length > m_streamPosition)
        {
            if (!m_noInput)
            {
                bool correctInput = false;
                bool allInputsCorrect = true;
                for (int i = 0; i < 4; i++) // check inputs against
                {
                    if (m_StreamOBJ.stream.qteInputs[m_streamPosition].inputs[i] == m_playerQTEInput.inputs[i])
                    {
                        correctInput = true;
                    }
                    if (correctInput != true) allInputsCorrect = false;
                    correctInput = false;
                }
                if (allInputsCorrect) OnSuccessInput();
                else OnFailInput();
            }
            if (m_streamPosition < m_StreamOBJ.stream.qteInputs.Length) m_StreamOBJ.stream.qteInputs[m_streamPosition].TimeToComplete -= Time.unscaledDeltaTime;
        }
    }
    private void Display()
    {
        for (int i = 0; i < 4; i++)
        {
            if (m_streamPosition < m_StreamOBJ.stream.qteInputs.Length)
                ButtonCanvas[i].isOn = m_StreamOBJ.stream.qteInputs[m_streamPosition].inputs[i];
            else
                ButtonCanvas[i].isOn = false;
        }

        switch (m_currentState)
        {
            case QTEState.Waiting:
                
                break;
            case QTEState.Running:
                if (m_streamPosition < m_StreamOBJ.stream.qteInputs.Length)
                {
                    string timeLeft = string.Format("{0:0.00}", m_StreamOBJ.stream.qteInputs[m_streamPosition].TimeToComplete);
                    TextCanvas.text = $"Time: {timeLeft}";
                }
                break;
        }
    }
    #endregion
    #region Events
    public void StartCombat(EnemyController enemy)
    {
        /////////// other stuff to go here
        
        ///////////
        StartQTE();
    }
    public void StartQTE()
    {
        /////////// other stuff to go here

        ///////////
        m_currentState = QTEState.Running;
    }
    private void OnComplete()
    {
        m_currentState = QTEState.Waiting;
        Debug.Log("Complete");
    }
    private void OnFail()
    {
        m_currentState = QTEState.Waiting;
        Debug.Log("Failed");
    }
    private void OnFailInput()
    {

    }
    private void OnSuccessInput()
    {
        Debug.LogWarning("Buttons Pressed: " + m_playerQTEInput.inputs);
        if (m_StreamOBJ.stream.qteInputs[m_streamPosition].mashAmount > 0) m_StreamOBJ.stream.qteInputs[m_streamPosition].mashAmount--;
        else m_streamPosition++;
    }

    /// <summary>
    /// call once per update to enable state change code
    /// </summary>
    private void OnStateChange()
    {
        if (m_oldState == m_currentState) return; // no code above this

        switch (m_currentState)
        {
            case QTEState.Waiting:
                GetComponent<Canvas>().enabled = false;
                break;
            case QTEState.Running:
                GetComponent<Canvas>().enabled = true;
                break;
        }
        m_oldState = m_currentState; // once run old state == this state to prevent from running again
    }

    #endregion
    #region Destroy/Disable
    private void OnDisable()
    {
        i_North.Disable();
        i_South.Disable();
        i_East.Disable();
        i_West.Disable();
    }
    #endregion

}
