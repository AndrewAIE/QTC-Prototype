using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Running,
            Failure,
            Success
        }

        [HideInInspector] public State state = State.Running;
        [HideInInspector] public bool Started = false;
        [HideInInspector] public string Guid;
        [HideInInspector] public Vector2 Position;
        [HideInInspector] public AIAgent Agent;
        [TextArea] public string description;

        private string m_title;
        private string m_menuTitle;

        public string Title => m_title;
        public string MenuTitle => m_menuTitle;

        public State Update()
        {
            if (!Started)
            {
                OnStart();
                Started = true;
            }

            state = OnUpdate();

            if (state == State.Failure || state == State.Success)
            {
                OnStop();
                Started = false;
            }

            return state;
        }

        public void FixedUpdate()
        {
           OnFixedUpdate();
        }

        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        protected abstract void OnFixedUpdate();
        protected abstract void OnStart();
        protected abstract void OnStop();

        protected abstract State OnUpdate();

    }
}

