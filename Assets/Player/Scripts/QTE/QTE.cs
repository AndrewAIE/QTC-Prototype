using QTCGlobals;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[CreateAssetMenu]
public class QTE : ScriptableObject
{
    public QTESTREAM stream;
    /// <summary>
    /// time to complete all streams
    /// </summary>
    public float timeToComp {
        get
        {
            float time = 0;
            //for (int y = 0; y < 4; y++)
            //{
                for (int i = 0; i < 4; i++)
                {
                    time += stream.qteInputs[i].TimeToComplete;
                }
            //}
            return time;
        }
        }
}