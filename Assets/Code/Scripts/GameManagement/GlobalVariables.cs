using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTCGlobals
{
    public enum QTEState
    {
        Waiting,
        Running,
        Default
    }
    /// <summary>
    /// Holds the Input and how many times it needs to be pressed
    /// </summary>
    [System.Serializable]
    public struct QTEINPUTS
    {
        /// <summary>
        /// inputs needed, inputs as North, East, South, West
        /// </summary>
        public bool[] inputs;
        /// <summary>
        /// how many times the button needs to be pressed
        /// </summary>
        public float mashAmount;
        /// <summary>
        /// seconds to complete
        /// </summary>
        public float TimeToComplete;
    }
    /// <summary>
    /// holds all QTEINPUTS and the time to complete the QTE
    /// </summary>
    [System.Serializable]
    public struct QTESTREAM
    {
        /// <summary>
        /// all required input combos
        /// </summary>
        public QTEINPUTS[] qteInputs;
        
    }
}
