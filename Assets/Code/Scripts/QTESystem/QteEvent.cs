using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTECombatGlobalVars;
[CreateAssetMenu(fileName = "QTEEvent", menuName = "QTE/QTE Event", order = 0)]
public class QteEvent : ScriptableObject
{    
    public List<QteInput> Inputs;    
    public InputType InputType;
    public InputSubType InputSubType;
}
