using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class DebugLogNode : ActionNode
    {
        public string Message;        

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            Debug.Log(Message);
            return State.Success;
        }

        protected override void OnFixedUpdate()
        {
            
        }
    }
}

