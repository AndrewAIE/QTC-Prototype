using UnityEditor;
using UnityEngine;


namespace BehaviourTree.Editor
{
    public class CreateNewNodeType
    {
        //Create New Action Node from Create Menu
        [MenuItem("Assets/Create/Behaviour Tree/New Node Type/Action Node")]
        static void CreateActionNode()
        {
            string filePath = AssetDatabase.GetAssetPath(Resources.Load("ActionNodeTemplate.cs"));
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                 filePath, "NewActionNode.cs");            

            AssetDatabase.Refresh();
        }
        //Create New Compsite Node from Create Menu
        [MenuItem("Assets/Create/Behaviour Tree/New Node Type/Composite Node")]
        static void CreatCompositeNode()
        {
            string filePath = AssetDatabase.GetAssetPath(Resources.Load("CompositeNodeTemplate.cs"));
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                 filePath, "NewCompositeNode.cs");

            AssetDatabase.Refresh();
        }
        //Create New Decorator Node from Create Menu
        [MenuItem("Assets/Create/Behaviour Tree/New Node Type/Decorator Node")]
        static void CreateDecoratorNode()
        {
            string filePath = AssetDatabase.GetAssetPath(Resources.Load("DecoratorNodeTemplate.cs"));
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                 filePath, "NewDecoratorNode.cs");

            AssetDatabase.Refresh();
        }
    }
}
