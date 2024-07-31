using UnityEngine;

namespace BehaviourTree
{
    public abstract class AIAgent : MonoBehaviour
    {
        public abstract void Start();

        public abstract void Update();

        public abstract void FixedUpdate();
        
    }
}
