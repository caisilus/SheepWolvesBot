using System;
using System.Linq;

namespace MinimaxBot
{
    public class SheepAndWolvesBot
    {
        private bool _controllingSheep;
        private int _maxDepth;
        private Heuristic _heuristic;
        
        public SheepAndWolvesBot(bool controllingSheep, int maxDepth, Heuristic heuristic)
        {
            _controllingSheep = controllingSheep;
            _maxDepth = maxDepth;
            _heuristic = heuristic;
        }
        
        public Move NextMove(Position position)
        {
            DynamicNode root = new DynamicNode(position);
            EvaluateTree(root);
            var bestNode = FindBestNode(root);
            return position.CalculateMoveToPosition(bestNode.Data);
        }

        private DynamicNode FindBestNode(DynamicNode root)
        {
            DynamicNode bestNode = null;
            var bestEvaluation = _controllingSheep ? int.MaxValue : int.MinValue;
            foreach (var node in root.Children)
            {
                if (_controllingSheep && node.Evaluation < bestEvaluation)
                {
                    bestNode = node;
                    bestEvaluation = node.Evaluation;
                    continue;
                }

                if (!_controllingSheep && node.Evaluation > bestEvaluation)
                {
                    bestNode = node;
                    bestEvaluation = node.Evaluation;
                }
            }

            return bestNode;
        }
        
        public void EvaluateTree(DynamicNode root)
        {
            EvaluateNode(root, _controllingSheep, int.MinValue, int.MaxValue);
        }

        private void EvaluateNode(DynamicNode node, bool sheepTurn, int alpha, int beta)
        {
            if (node.IsLeaf(_maxDepth))
            {
                node.EvaluateLeaf(_heuristic);
                return;
            }

            var children = node.GetChildren(sheepTurn);
            foreach (var child in children)
            {
                EvaluateNode(child, !sheepTurn, alpha, beta);
                UpdateEvaluation(node, child.Evaluation, sheepTurn);
                if (CheckPruning(node.Evaluation, sheepTurn, alpha, beta))
                    return;
                UpdateAlphaBeta(node.Evaluation, sheepTurn, ref alpha, ref beta);
            }
        }
        
        private void UpdateEvaluation(DynamicNode node, int value, bool sheepTurn)
        {
            if (!node.IsEvaluated)
                InitNodeEvaluation(node, sheepTurn);

            node.Evaluation = sheepTurn ? Math.Min(node.Evaluation, value) : Math.Max(node.Evaluation, value);
        }
        
        private void InitNodeEvaluation(DynamicNode node, bool sheepTurn)
        {
            node.Evaluation = sheepTurn ? int.MaxValue : int.MinValue;
        }

        private bool CheckPruning(int value, bool sheepTurn, int alpha, int beta)
        {
            return !sheepTurn ? Math.Max(value, alpha) >= beta : Math.Min(value, beta) <= alpha;
        }

        private void UpdateAlphaBeta(int value, bool sheepTurn, ref int alpha, ref int beta)
        {
            if (!sheepTurn)
                alpha = Math.Max(alpha, value);
            else
                beta = Math.Min(beta, value);
        }
    }
}