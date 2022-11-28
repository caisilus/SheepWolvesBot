using System;

namespace MinimaxBot
{
    public class Node
    {
        private int evaluation;
        public Node Parent { get; private set; }
        public Node[] Children { get; }
        public bool IsEvaluated { get; set; }

        public int Evaluation
        {
            get => evaluation;
            set
            {
                evaluation = value;
                IsEvaluated = true;
            }
        }

        private void SetParentToChildren()
        {
            if (IsLeaf())
                return;
            foreach (Node child in Children)
            {
                child.Parent = this;
            }
        }
        
        public Node(Node[] children, int evaluation)
        {
            Children = children;
            Evaluation = evaluation;
            SetParentToChildren();
        }
        
        public Node(Node[] children)
        {
            Children = children;
            IsEvaluated = false;
            SetParentToChildren();
        }

        public bool IsLeaf()
        {
            return Children == null || Children.Length == 0;
        }
    }
}