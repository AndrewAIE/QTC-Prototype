using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QTEEncounter", menuName = "QTE/QTE Encounter", order = 2)]
public class QteEncounter : ScriptableObject
{
    public List<QteStream> Streams;
}
