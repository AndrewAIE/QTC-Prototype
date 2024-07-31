using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class RootNode : Node
    {
        [HideInInspector] public Node Child;
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            return Child.Update();
        }

        protected override void OnFixedUpdate()
        {
            Child.FixedUpdate();
        }

        public override Node Clone()
        {
            
            RootNode node = Instantiate(this);
            node.Child = Child.Clone();
            return node;
        }

        
    }
}

