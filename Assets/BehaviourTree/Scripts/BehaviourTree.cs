using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace BehaviourTree
{
    [CreateAssetMenu(menuName = "Behaviour Tree/New Behaviour Tree")]
    public class BehaviourTree : ScriptableObject
    {
        public Node rootNode;
        public Node.State treeState = Node.State.Running;
        public List<Node> nodes = new List<Node>();         

        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running)
            {
                treeState = rootNode.Update();
            }
            return treeState;
        }

        public void FixedUpate()
        {
            if(rootNode.state == Node.State.Running)
            {
                rootNode.FixedUpdate();
            }
        }

#if UNITY_EDITOR
        //Create a new node in the Behaviour Tree
        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.Guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "Behaviour Tree (Create Node)");
            nodes.Add(node);

            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (Create Node");
            AssetDatabase.SaveAssets();
            return node;
        }
        //Delete node from Behaviour Tree
        public void DeleteNode(Node node)
        {
            RootNode root = node as RootNode;
            if (root)
            {
                Debug.LogWarning("You can not delete the root node");
            }
            else
            {
                Undo.RecordObject(this, "Behaviour Tree (Delete Node)");
                nodes.Remove(node);
                //AssetDatabase.RemoveObjectFromAsset(node);
                Undo.DestroyObjectImmediate(node);
                AssetDatabase.SaveAssets();
            }            
        }
        //Add child to currently selected node
        public void AddChild(Node parent, Node child)
        {
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
                decorator.Child = child;
                EditorUtility.SetDirty(decorator);
            }

            RootNode root = parent as RootNode;
            if (root)
            {
                Undo.RecordObject(root, "Behaviour Tree (AddChild)");
                root.Child = child;
                EditorUtility.SetDirty(root);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
                composite.Children.Add(child);
                EditorUtility.SetDirty(composite);
            }
        }
        //Remove child node from currently selected node
        public void RemoveChild(Node parent, Node child)
        {
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
                decorator.Child = child;
                EditorUtility.SetDirty(decorator);
            }

            RootNode root = parent as RootNode;
            if (root)
            {
                Undo.RecordObject(root, "Behaviour Tree (RemoveChild)");
                root.Child = null;
                EditorUtility.SetDirty(root);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
                composite.Children.Remove(child);
                EditorUtility.SetDirty(composite);
            }
        }
#endif
        //Get the child or children nodes of currently selected node
        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator && decorator.Child != null)
            {
                children.Add(decorator.Child);
                return children;
            }

            RootNode root = parent as RootNode;
            if (root && root.Child != null)
            {
                children.Add(root.Child);
                return children;
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                return composite.Children;
            }
            return children;
        }
        //traverse through the nodes and perform a function
        public void Traverse(Node node, System.Action<Node> visitor)
        {
            if (node)
            {
                visitor.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n) => Traverse(n, visitor));
            }
        }

        //Instantiate a clone of the tree attached to the Agent so it can run individually.
        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();
            tree.nodes = new List<Node>();
            Traverse(tree.rootNode, (n) =>
            {
                tree.nodes.Add(n);
            });
            return tree;
        }
        //Give all nodes access to the AIAgent
        public void Bind(AIAgent agent)
        {
            Traverse(rootNode, node =>
            {
                node.Agent = agent;
            });
        }

        public void Refresh()
        {
            Traverse(rootNode, node =>
            {
                node.state = Node.State.Running;
                node.Started = false;
            });
        }
    }
}

