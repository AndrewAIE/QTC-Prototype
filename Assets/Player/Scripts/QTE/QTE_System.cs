using QTCGlobals;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class QTE_System: MonoBehaviour
{
    [SerializeReference] private QTEState m_currentState = QTEState.Waiting;

    PlayerInputs i_playerInputs;
    private InputAction i_North, i_South, i_East, i_West;

    [SerializeField] private Toggle[] ButtonCanvas;
    [SerializeField] private TextMeshProUGUI TextCanvas;

    /// <summary>
    /// the QTE input for the player
    /// </summary>
    private QTEINPUTS m_playerQTEInput;
    private bool m_noInput;
    /// <summary>
    /// the current stream (if any)
    /// </summary>
    public QTE m_Stream;
    /// <summary>
    /// position in the QTEStream
    /// </summary>
    [SerializeField] private int m_streamPosition = 0;

    #region Startup
    public void StartQTE()
    {
        m_currentState = QTEState.Running;
    }

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
        switch (m_currentState)
        {
            case QTEState.Waiting:
                if (i_playerInputs.Any())
                {
                    m_currentState = QTEState.Running;
                }
            break;

            case QTEState.Running:
                gatherInput();
                    StreamManager();
                if (m_Stream.stream.qteInputs.Length <= m_streamPosition)
                {
                    m_currentState = QTEState.Complete;
                }
                else if (m_Stream.stream.qteInputs[m_streamPosition].TimeToComplete <= 0)
                {
                    m_currentState = QTEState.Failed;
                }
                
                break;
            case QTEState.Complete:
                Debug.Log("Complete");
                break;
            case QTEState.Failed:
                Debug.Log("Failed");
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
        if(m_Stream.stream.qteInputs.Length > m_streamPosition)
        {
            if (!m_noInput)
            {
                bool correctInput = false;
                bool allInputsCorrect = true;
                for (int i = 0; i < 4; i++) // check inputs against
                {
                    if (m_Stream.stream.qteInputs[m_streamPosition].inputs[i] == m_playerQTEInput.inputs[i])
                    {
                        correctInput = true;
                    }
                    if (correctInput != true) allInputsCorrect = false;
                }
                if (allInputsCorrect)
                {
                    if (m_Stream.stream.qteInputs[m_streamPosition].mashAmount > 0) m_Stream.stream.qteInputs[m_streamPosition].mashAmount--;
                    else m_streamPosition++;
                }
            }
            m_Stream.stream.qteInputs[m_streamPosition].TimeToComplete -= Time.unscaledDeltaTime;
        }
    }
    private void Display()
    {
        for(int i = 0; i < 4; i++)
        {
            if (m_streamPosition < m_Stream.stream.qteInputs.Length)
                ButtonCanvas[i].isOn = m_Stream.stream.qteInputs[m_streamPosition].inputs[i];
            else
                ButtonCanvas[i].isOn = false;
        }

        switch (m_currentState)
        {
            case QTEState.Waiting:

                break;
            case QTEState.Running:
                string timeLeft = string.Format("{0:0.00}", m_Stream.stream.qteInputs[m_streamPosition].TimeToComplete);
                TextCanvas.text = $"Time: {timeLeft}";
                break;
            case QTEState.Complete:

                break;
            case QTEState.Failed:

                break;
        }
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
