using System;

namespace MinimaxBot
{
    public struct CheckMateCell
    {
        public int X;
        public int Y;
        public CheckMateCell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public CheckMateCell NeighbourFrom(MoveDirection direction)
        {
            var (dx, dy) = MoveDirectionToDxDy(direction);
            return new CheckMateCell(X + dx, Y + dy);
        }

        public bool IsValid()
        {
            return X > 0 && X < 7 && Y > 0 && Y < 7;
        }

        public static Tuple<int, int> MoveDirectionToDxDy(MoveDirection moveDirection)
        {
            return moveDirection switch
            {
                MoveDirection.UpLeft => new Tuple<int, int>(-1, -1),
                MoveDirection.UpRight => new Tuple<int, int>(1, -1),
                MoveDirection.DownLeft => new Tuple<int, int>(-1, 1),
                MoveDirection.DownRight => new Tuple<int, int>(1, 1),
                _ => new Tuple<int, int>(0, 0)
            };
        }

        public static bool operator ==(CheckMateCell left, CheckMateCell right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        
        public static bool operator !=(CheckMateCell left, CheckMateCell right)
        {
            return !(left == right);
        }
    }
}