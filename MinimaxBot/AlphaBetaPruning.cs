using System;

namespace MinimaxBot
{
    public class AlphaBetaPruning
    {
        public int CountVisited { get; private set; }
        
        public void EvaluateTree(Node root)
        {
            EvaluateNode(root, true, int.MinValue, int.MaxValue);
        }

        private void EvaluateNode(Node node, bool firstPlayerTurn, int alpha, int beta)
        {
            CountVisited++;
            if (node.IsLeaf())
            {
                return;
            }

            foreach (var child in node.Children)
            {
                EvaluateNode(child, !firstPlayerTurn, alpha, beta);
                UpdateEvaluation(node, child.Evaluation, firstPlayerTurn);
                if (CheckPruning(node.Evaluation, firstPlayerTurn, alpha, beta))
                    return;
                UpdateAlphaBeta(node.Evaluation, firstPlayerTurn, ref alpha, ref beta);
            }
        }
        
        private void UpdateEvaluation(Node node, int value, bool firstPlayerTurn)
        {
            if (!node.IsEvaluated)
                InitNodeEvaluation(node, firstPlayerTurn);

            node.Evaluation = firstPlayerTurn ? Math.Max(node.Evaluation, value) : Math.Min(node.Evaluation, value);
        }
        
        private void InitNodeEvaluation(Node node, bool firstPlayerTurn)
        {
            node.Evaluation = firstPlayerTurn ? int.MinValue : int.MaxValue;
        }

        private bool CheckPruning(int value, bool firstPlayerTurn, int alpha, int beta)
        {
            return firstPlayerTurn ? Math.Max(value, alpha) >= beta : Math.Min(value, beta) <= alpha;
        }

        private void UpdateAlphaBeta(int value, bool firstPlayerTurn, ref int alpha, ref int beta)
        {
            if (firstPlayerTurn)
                alpha = Math.Max(alpha, value);
            else
                beta = Math.Min(beta, value);
        }
    }
}