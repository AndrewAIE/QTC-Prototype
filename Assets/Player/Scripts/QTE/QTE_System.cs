using QTCGlobals;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class QTE_System : MonoBehaviour
{
    [SerializeReference] private QTEState m_currentState = QTEState.Default;
    private QTEState m_oldState;

    PlayerInputs i_playerInputs;
    private InputAction i_North, i_South, i_East, i_West;

    private float poisness = 0.5f;
    
    [SerializeField] private Toggle[] ButtonCanvas;
    [SerializeField] private TextMeshProUGUI TextCanvas;
    [SerializeField] private Scrollbar scrollCanvas;
    /// <summary>
    /// reference to the player
    /// </summary>
    [SerializeField] private PlayerController m_playerReference;
    [SerializeField] private EnemyController m_enemyReference;

    /// <summary>
    /// the QTE input for the player
    /// </summary>
    private QTEINPUTS m_playerQTEInput;
    private bool m_noInput;

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
        m_StreamOBJ = Instantiate(StreamOBJ);
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
        /*if (start) m_currentState = QTEState.Running;*/
        //scrollCanvas.value = poisness;

        OnStateChange();
        switch (m_currentState)
        {
            case QTEState.Waiting:
               if (!m_playerReference.CombatAnimRunning) m_currentState = QTEState.Running;
                break;

            case QTEState.Running:
                if (m_playerReference.CombatAnimRunning) m_currentState = QTEState.Waiting;

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

                if (poisness <= 0) OnFail();
                else if (poisness >= 1) OnComplete();

                break;
            case QTEState.Default:
                
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
            if (m_streamPosition < m_StreamOBJ.stream.qteInputs.Length && !m_playerReference.CombatAnimRunning)
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
            case QTEState.Default:

                break;
        }
    }
    #endregion
    #region Events
    public void StartCombat(EnemyController enemy)
    {
        /////////// other stuff to go here
        m_enemyReference = enemy;
        m_streamPosition = 0;
        poisness = 0.5f;
        ///////////
        StartQTE();
    }
    public void StartQTE()
    {
        /////////// other stuff to go here

        ///////////
        m_currentState = QTEState.Running;
    }
    /// <summary>
    /// if the player completes all inputs this runs
    /// </summary>
    private void OnComplete()
    {
        if(poisness < 1)
        {
            m_streamPosition = 0;
            return;
        }

        m_playerReference.WinCombat();
        m_enemyReference.LoseCombat();
        m_currentState = QTEState.Default;
        m_enemyReference = null;
        Debug.Log("Complete");
    }
    /// <summary>
    /// if the player fails for any reason this runs
    /// </summary>
    private void OnFail()
    {
        if(poisness > 0)
        {
            poisness -= .1f;
            m_streamPosition = 0;
            return;
        }

        m_playerReference.LoseCombat();
        m_enemyReference.WinCombat();
        m_currentState = QTEState.Default;
        Debug.Log("Failed");
    }
    /// <summary>
    /// if QTE Input is wrong this runs
    /// </summary>
    private void OnFailInput()
    {
        poisness -= .1f;
        m_playerReference.FailedInput();
    }
    /// <summary>
    /// if QTE Input if correct this runs
    /// </summary>
    private void OnSuccessInput()
    {
        poisness += 0.1f;
        for (int i = 0; i < 4; i++)
        {
            if (m_playerQTEInput.inputs[i] == true)
            {
                m_playerReference.Attack(i);
                m_enemyReference.Attack(i);
                break;
            }
        }
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
                GetComponent<Canvas>().enabled = true;
                break;
            case QTEState.Running:
                m_StreamOBJ = Instantiate(StreamOBJ); // creates a new version of a Scriptable Object
                GetComponent<Canvas>().enabled = true;
                break;
            case QTEState.Default:
                GetComponent<Canvas>().enabled = false;
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
