using System;

namespace BehaviourTree
{
    public class NodeInfoAttribute : Attribute
    {
        private string m_nodeTitle;
        private string m_menuItem;

        public string NodeTitle => m_nodeTitle;
        public string MenuItem => m_menuItem;

        public NodeInfoAttribute(string title, string menuItem = "")
        {
            m_nodeTitle = title;
            m_menuItem = menuItem;
        }
    }
}

