using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class CompositeNode : Node
    {
        [HideInInspector] public List<Node> Children = new List<Node>();

        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node.Children = Children.ConvertAll(child => child.Clone());
            return node;
        }
    }
}

