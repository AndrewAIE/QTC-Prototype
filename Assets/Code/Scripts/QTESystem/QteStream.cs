using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QTEStream", menuName = "QTE/QTE Stream", order = 1)]
public class QteStream : ScriptableObject
{
    public List<QteEvent> QTEEvents;
}
