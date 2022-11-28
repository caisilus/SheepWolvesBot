using System;
using System.Linq;

namespace MinimaxBot
{
    public static class MinimaxAlgorithm
    {
        public static Tuple<int, int> EvaluationForRoot(Node root)
        {
            int processedNodes = EvaluateNodeMinimax(root, true);
            return new Tuple<int, int>(root.Evaluation, processedNodes);
        }

        // returns count of processed nodes
        private static int EvaluateNodeMinimax(Node node, bool firstPlayerTurn)
        {
            if (node.IsLeaf())
            {
                return 1;
            }

            int visitedNodes = 1;
            for (int i = 0; i < node.Children.Length; i++)
            {
                Node child = node.Children[i];
                visitedNodes += EvaluateNodeMinimax(child, !firstPlayerTurn);
                UpdateEvaluation(node, child.Evaluation, firstPlayerTurn);
            }
            
            return visitedNodes;
        }

        private static void UpdateEvaluation(Node node, int value, bool firstPlayerTurn)
        {
            if (!node.IsEvaluated)
                InitNodeEvaluation(node, firstPlayerTurn);

            node.Evaluation = firstPlayerTurn ? Math.Max(node.Evaluation, value) : Math.Min(node.Evaluation, value);
        }

        private static void InitNodeEvaluation(Node node, bool firstPlayerTurn)
        {
            node.Evaluation = firstPlayerTurn ? int.MinValue : int.MaxValue;
        }
        
        private static int EvaluationForNodeWithEvaluatedChildren(Node node, bool firstPlayerTurn)
        {
            if (firstPlayerTurn)
            {
                return node.Children.Max(child => child.Evaluation);
            }
            else
            {
                return node.Children.Min(child => child.Evaluation);
            }
        }
    }
}