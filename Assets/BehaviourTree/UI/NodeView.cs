using System;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

namespace BehaviourTree.Editor
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        public Node node;
        public Port inputPort;
        public Port outputPort;

        public NodeView(Node node) : base("Assets/BehaviourTree/UI/NodeView.uxml")
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.Guid;
            style.left = node.Position.x;
            style.top = node.Position.y;

            CreateInputPorts();
            CreateOutputPorts();
            SetUpClasses();

            Label descriptionLabel = this.Q<Label>("description");
            descriptionLabel.bindingPath = "description";
            descriptionLabel.Bind(new SerializedObject(node));
        }
        //Set up and update classes for the Behaviour Tree Editor to access and highlight the nodes depending on their state
        private void SetUpClasses()
        {
            if (node is ActionNode)
            {
                AddToClassList("action");
            }
            else if (node is DecoratorNode)
            {
                AddToClassList("decorator");
            }
            else if (node is CompositeNode)
            {
                AddToClassList("composite");
            }
            else if (node is RootNode)
            {
                AddToClassList("root");
            }
        }
        public void UpdateState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("failure");
            RemoveFromClassList("success");

            if (Application.isPlaying)
            {
                switch (node.state)
                {
                    case Node.State.Running:
                        if (node.Started)
                        {
                            AddToClassList("running");
                        }
                        break;
                    case Node.State.Failure:
                        AddToClassList("failure");
                        break;
                    case Node.State.Success:
                        AddToClassList("success");
                        break;
                }
            }
        }

        //create the appropriate ports for the node
        private void CreateInputPorts()
        {
            
            if (node is ActionNode)
            {
                inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is DecoratorNode)
            {
                inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is CompositeNode)
            {
                inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is RootNode)
            {

            }

            if (inputPort != null)
            {
                //adjust positioning of the ports so they are centred
                inputPort.portName = "";
                inputPort.contentContainer.style.flexDirection = FlexDirection.Column;
                inputPort.contentContainer.style.flexWrap = Wrap.Wrap;
                inputPort.style.flexDirection = FlexDirection.Column;
                inputPort.style.flexWrap = Wrap.Wrap;
                inputContainer.Add(inputPort);
            }
        }
        private void CreateOutputPorts()
        {
            if (node is ActionNode)
            {

            }
            else if (node is DecoratorNode)
            {
                outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (node is CompositeNode)
            {
                outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if (node is RootNode)
            {
                outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            //adjust positioning of the ports so they are centred
            if (outputPort != null)
            {
                outputPort.portName = "";
                outputPort.style.flexDirection = FlexDirection.ColumnReverse;
                outputPort.style.flexWrap = Wrap.Wrap;
                outputPort.style.flexDirection = FlexDirection.ColumnReverse;
                outputPort.style.flexWrap = Wrap.Wrap;
                outputContainer.Add(outputPort);
            }
        }
        //Set node's position in the Behaviour Tree Editor
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Behaviour Tree (Set Position)");
            node.Position.x = newPos.xMin;
            node.Position.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            if (OnNodeSelected != null)
            {
                OnNodeSelected.Invoke(this);
            }
        }
        //Sort the children of the composite node by their x position on the Behaviour Tree Editor
        public void SortChildren()
        {
            
            CompositeNode composite = node as CompositeNode;
            if (composite)
            {
                composite.Children.Sort(SortByHorizontalPosition);
            }
        }

        private int SortByHorizontalPosition(Node left, Node right)
        {
            return left.Position.x < right.Position.x ? -1 : 1;
        }
        
    }
}
