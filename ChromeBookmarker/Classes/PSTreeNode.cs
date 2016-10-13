using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChromeBookmarker
{
    [Serializable]
    public class PSTreeNode<T>
    {
        /// <summary>
        /// Stores list of nodes children nodes
        /// </summary>
        public List<PSTreeNode<T>> Children;
        /// <summary>
        /// Value of tree nodes
        /// </summary>
        public T Value { get; set; }


        public PSTreeNode()
        {
            Children = new List<PSTreeNode<T>>();
        }

        public PSTreeNode (T NewNodeValue)
        {
            Children = new List<PSTreeNode<T>>();
            Value = NewNodeValue;
        }

        /// <summary>
        /// Remove children from node
        /// </summary>
        public void ClearChildren()
        {
            Children.Clear();
        }

        public void RemoveChild(PSTreeNode<T> childToRemove)
        {
            RemoveChildNode(this, childToRemove);
        }

        /// <summary>
        /// Remove child node / recursive
        /// </summary>
        /// <param name="ParentNode">Parent node whos child node we are looking for</param>
        /// <param name="childToRemove">Node to be removed</param>
        private void RemoveChildNode(PSTreeNode<T> ParentNode, PSTreeNode<T> childToRemove)
        {
            foreach (PSTreeNode<T> ChildNode in ParentNode.Children)
            {
                if (ChildNode == childToRemove)
                {
                    ParentNode.Children.Remove(ChildNode);
                    return;
                }
                RemoveChildNode(ChildNode, childToRemove);
            }
        }

        /// <summary>
        /// Adds a new child node to a node
        /// </summary>
        /// <param name="ChildValue">Value to be stored in new node</param>
        /// <returns>New child node</returns>
        public PSTreeNode<T> AddChild(T ChildValue)
        {
            PSTreeNode<T> newNode = new PSTreeNode<T>(ChildValue);
            Children.Add(newNode);
            return newNode;
        }
    }
}
