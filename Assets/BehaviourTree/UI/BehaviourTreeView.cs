using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

namespace BehaviourTree.Editor
{
    public class BehaviourTreeView : GraphView
    {
        public Action<NodeView> OnNodeSelected;
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }
        private BehaviourTree m_tree;
        
        
        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());
            //Add different pre built functionalities to the Behaviour Tree Editor and access the style sheet to 
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourTree/UI/BehaviourTreeEditor.uss");
            styleSheets.Add(styleSheet);
            Undo.undoRedoPerformed += OnUndoRedo;

        }
        //Repopulate the Behaviour Tree Editor and update Asset Database when undoing/redoing an action
        private void OnUndoRedo()
        {
            PopulateView(m_tree);
            AssetDatabase.SaveAssets();
        }

        public NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.Guid) as NodeView;
        }

        public void PopulateView(BehaviourTree tree)
        {
            this.m_tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (tree.rootNode == null)
            {
                tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            //create node view
            tree.nodes.ForEach(n => CreateNodeView(n));
            //create edges
            tree.nodes.ForEach(n =>
            {
                //connect 
                var children = tree.GetChildren(n);
                children.ForEach(c =>
                {
                    NodeView parentView = FindNodeView(n);
                    NodeView childView = FindNodeView(c);

                    Edge edge = parentView.outputPort.ConnectTo(childView.inputPort);
                    AddElement(edge);

                });
            });

        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
            endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
        }
        //Update the visuals of the Behaviour Tree when a change is made
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    
                    NodeView nodeView = elem as NodeView;

                    if (nodeView != null)
                    {
                        m_tree.DeleteNode(nodeView.node);
                    }

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;
                        m_tree.RemoveChild(parentView.node, childView.node);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    m_tree.AddChild(parentView.node, childView.node);
                });
            }

            if (graphViewChange.movedElements != null)
            {
                nodes.ForEach((n) =>
                {
                    NodeView view = n as NodeView;
                    view.SortChildren();
                });
            }

            PopulateView(m_tree);
            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //Instantiate different types of nodes from the Contextual menu (right-clicking in the Behavior Tree Editor)
            {
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
                foreach (var type in types)
                {
                    Vector3 screenMousePosition = evt.localMousePosition;
                    Vector2 worldMousePosition = screenMousePosition - contentViewContainer.transform.position;
                    worldMousePosition *= 1 / contentViewContainer.transform.scale.x;
                    evt.menu.AppendAction($"Action Nodes/{type.Name}", (a) => CreateNode(type, worldMousePosition));
                }
            }
            
            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
                foreach (var type in types)
                {
                    Vector3 screenMousePosition = evt.localMousePosition;
                    Vector2 worldMousePosition = screenMousePosition - contentViewContainer.transform.position;
                    worldMousePosition *= 1 / contentViewContainer.transform.scale.x;
                    evt.menu.AppendAction($"Composite Nodes/{type.Name}", (a) => CreateNode(type, worldMousePosition));
                }
            }
            
            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
                foreach (var type in types)
                {
                    Vector3 screenMousePosition = evt.localMousePosition;
                    Vector2 worldMousePosition = screenMousePosition - contentViewContainer.transform.position;
                    worldMousePosition *= 1 / contentViewContainer.transform.scale.x;
                    evt.menu.AppendAction($"Decorator Nodes/{type.Name}", (a) => CreateNode(type, worldMousePosition));
                }
            }
            
            {
                var types = TypeCache.GetTypesDerivedFrom<RootNode>();
                foreach (var type in types)
                {
                    Vector3 screenMousePosition = evt.localMousePosition;
                    Vector2 worldMousePosition = screenMousePosition - contentViewContainer.transform.position;
                    worldMousePosition *= 1 / contentViewContainer.transform.scale.x;
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type, worldMousePosition));
                }
            }
        }

        private void CreateNode(System.Type type, Vector2 mousePos)
        {
            Node node = m_tree.CreateNode(type);
            node.Position = mousePos;
            CreateNodeView(node);
        }

        public void CreateNodeView(Node node)
        {
            NodeView nodeView = new NodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }

        public void UpdateNodeState()
        {
            nodes.ForEach(n =>
            {
                NodeView view = n as NodeView;
                view.UpdateState();
            });
        }
    }
}


    
