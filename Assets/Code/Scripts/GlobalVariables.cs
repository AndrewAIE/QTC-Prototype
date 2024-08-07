using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QTECombatGlobalVars
{
    public enum QteInput
    {
        northFace,
        eastFace,
        southFace,
        westFace,
        northDirection,
        northEastDirection,
        eastDirection,
        southEastDirection,
        southDirection,
        southWestDirection,
        westDirection,
        northWestDirection,
        rightShoulder,
        rightTrigger,
        leftShoulder,
        leftTrigger
    }

    public enum InputType
    {
        single,
        compound
    }

    public enum InputSubType
    {
        press,
        hold,
        mash,
        sequence
    }
    
    public struct EnemyData
    {
        int PoiseBar;
        QteEncounter NeutralSequence;
        QteEncounter OffenseSequence;
        QteEncounter DefenseSequence;
    }
}

