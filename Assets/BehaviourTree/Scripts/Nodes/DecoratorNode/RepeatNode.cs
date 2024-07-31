using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class RepeatNode : DecoratorNode
    {
        

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            Child.Update();
            return State.Running;
        }

        protected override void OnFixedUpdate()
        {
            Child.FixedUpdate();
        }
    }
}

