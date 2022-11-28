using System.Collections.Generic;
using System.Linq;

namespace MinimaxBot
{
    public delegate int Heuristic(Position pos);
    
    public class BasicHeuristic
    {
        public static int Evaluate(Position position)
        {
            var distances = ZeroMatrix(8);
            
            Queue<Position> queue = new Queue<Position>();
            queue.Enqueue(position);
            while (queue.Count > 0)
            {
                var currentPosition = queue.Dequeue();
                var nextPositions = currentPosition.NextSheepPositions();
                foreach (var nextPosition in nextPositions)
                {
                    CheckMateCell currentCell = currentPosition.SheepPosition;
                    CheckMateCell nextCell = nextPosition.SheepPosition;
                    distances[nextCell.X][nextCell.Y] = distances[currentCell.X][currentCell.Y] + 1;
                }
            }

            var downRow = distances[7];
            return downRow.Where(x=>x>0).Min();
        }

        private static int[][] ZeroMatrix(int mSize)
        {
            int[][] m = new int[][] { };
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