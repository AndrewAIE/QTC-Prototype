
using UnityEngine;


namespace BehaviourTree
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree Tree;
        private AIAgent m_agent;
        private void OnEnable()
        {
            m_agent = GetComponent<AIAgent>();
        }

        void Start()
        {
            //Instantiate a new copy of the behaviour tree and give all nodes access to the AI Agent
            Tree = Tree.Clone();
            Tree.Bind(m_agent);
        }

        // Update is called once per frame
        public void OnUpdate()
        {
            Tree.Update();
        }

        public void OnFixedUpdate()
        {
            Tree.FixedUpate();
        }

        public void Refresh()
        {
            Tree.Refresh();
        }
    }
}
    


