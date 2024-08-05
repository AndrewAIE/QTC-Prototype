using UnityEngine;


public class QTE_System: MonoBehaviour
{
    [SerializeReference]private QTE m_currentQTE;
    PlayerInputs m_playerInputs;

    #region Startup
    void Start()
    {
        
    }

    #endregion

    #region Updates
    // Update is called once per frame
    void Update()
    {

    }

    

    private void ChooseQTE()
    {

    }
    #endregion

    #region Destroy/Disable
    private void OnDisable()
    {
        
    }

    /// <summary>
    /// removes the current QTE script based on what m_currentQTE is
    /// </summary>
    private void RemoveQTE()
    {
        Destroy(m_currentQTE);
        if (m_currentQTE == null) Debug.Log("QTE was Destroyed");
    }
    #endregion

}

