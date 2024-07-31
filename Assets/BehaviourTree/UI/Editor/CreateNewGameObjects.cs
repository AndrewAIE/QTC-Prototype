using UnityEngine;
using UnityEditor;

namespace BehaviourTree.Editor
{
    public class CreateNewGameObjects
    {
        //Create New AI Agent Script from the Create Menu
        [MenuItem("Assets/Create/Behaviour Tree/New AI Agent")]
        static private void CreateAIAgent()
        {
            string filePath = AssetDatabase.GetAssetPath(Resources.Load("AIAgentTemplate.cs"));            
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                 filePath, "NewAIAgent.cs");
            AssetDatabase.Refresh();
        }
    }
}
