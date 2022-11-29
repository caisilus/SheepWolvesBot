using System.Collections.Generic;
using System.Linq;

namespace MinimaxBot
{
    public delegate int Heuristic(Position pos);
    
    public class BasicHeuristic
    {
        const int BoardSize = 8;
        
        public static int Evaluate(Position position)
        {
            var distances = ZeroMatrix(BoardSize);
            
            Queue<Position> queue = new Queue<Position>();
            queue.Enqueue(position);
            while (queue.Count > 0)
            {
                var currentPosition = queue.Dequeue();
                var nextPositions = currentPosition.NextSheepPositions();
                foreach (var nextPosition in nextPositions)
                {
                    CheckMateCell currentCell = currentPosition.SheepCell;
                    CheckMateCell nextCell = nextPosition.SheepCell;
                    distances[nextCell.X][nextCell.Y] = distances[currentCell.X][currentCell.Y] + 1;
                }
            }

            var downRow = distances[BoardSize - 1];
            var nonZeroDistances = downRow.Where(x=>x > 0).ToArray();
            if (nonZeroDistances.Length == 0)
            {
                return 254;
            }

            return nonZeroDistances.Min();
        }

        private static int[][] ZeroMatrix(int mSize)
        {
            int[][] m = new int[mSize][];
            for (int i = 0; i < mSize; i++)
            {
                int[] row = new int[mSize];
                for (int j = 0; j < mSize; j++)
                {
                    row[j] = 0;
                }

                m[i] = row;
            }

            return m;
        }
    }
}