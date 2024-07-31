using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SequencerNode : CompositeNode
    {
        private int m_current;

        

        protected override void OnStart()
        {
            m_current = 0;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            var child = Children[m_current];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    m_current++;
                    break;
            }
            return m_current == Children.Count ? State.Success : State.Running;
        }

        protected override void OnFixedUpdate()
        {
            Children[m_current].FixedUpdate();
        }
    }
}

