using QTCGlobals;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
[CreateAssetMenu]
public class QTE : ScriptableObject
{
    public QTESTREAM stream;

    public float timeToComp {
       get { float time = 0;
            for (int i = 0; i < 4; i++)
            {
                time += stream.qteInputs[i].TimeToComplete;
            }
            return time; }
        }
}