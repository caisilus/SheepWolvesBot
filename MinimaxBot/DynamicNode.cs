using System.Collections.Generic;
using System.Linq;

namespace MinimaxBot
{
    public class DynamicNode
    {
        private int _evaluation;
        public Position Data { get; }
        public DynamicNode Parent { get; private set; }
        public DynamicNode[] Children { get; private set; }
        public int Depth { get; }
        public bool IsEvaluated { get; set; }
        
        public int Evaluation
        {
            get => _evaluation;
            set
            {
                _evaluation = value;
                IsEvaluated = true;
            }
        }

        public DynamicNode(Position position)
        {
            Parent = null;
            Data = position;
            Depth = 0;
            IsEvaluated = false;
        }
        
        public DynamicNode(Position position, DynamicNode parent)
        {
            Data = position;
            Parent = parent;
            Depth = parent.Depth + 1;
            IsEvaluated = false;
        }

        public IEnumerable<DynamicNode> GetChildren(bool sheepTurn)
        {
            if (sheepTurn)
            {
                Children = Data.NextSheepPositions().Select(p => new DynamicNode(p, this)).ToArray();
            }
            else
            {
                Children = Data.NextWolvesPositions().Select(p => new DynamicNode(p, this)).ToArray();
            }

            return Children;
        }

        public void EvaluateLeaf(Heuristic heuristic)
        {
            Evaluation = heuristic(Data);
        }
        
        public bool IsLeaf(int depth)
        {
            return Depth == depth || Data.GameStatus != GameState.GameContinues;
        }
    }
}