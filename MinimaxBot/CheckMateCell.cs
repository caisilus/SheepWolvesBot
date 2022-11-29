using System;

namespace MinimaxBot
{
    public struct CheckMateCell
    {
        public int X;
        public int Y;
        public CheckMateCell(int x, int y)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                throw new ArgumentException($"invalid x or y: {x},{y}");
            }
            X = x;
            Y = y;
        }

        public bool HasNeighbourFrom(MoveDirection direction)
        {
            var (dx, dy) = MoveDirectionToDxDy(direction);
            return ((X + dx) >= 0) && ((X + dx) <= 7) && ((Y + dy) >= 0) && ((Y + dy) <= 7);
        }
        
        public CheckMateCell NeighbourFrom(MoveDirection direction)
        {
            var (dx, dy) = MoveDirectionToDxDy(direction);
            return new CheckMateCell(X + dx, Y + dy);
        }

        public MoveDirection MoveDirectionTo(CheckMateCell other)
        {
            var dx = other.X - X;
            var dy = other.Y - Y;
            return MoveDirectionFromDxDy(dx, dy);
        }
        
        private static Tuple<int, int> MoveDirectionToDxDy(MoveDirection moveDirection)
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

        private static MoveDirection MoveDirectionFromDxDy(int dx, int dy)
        {
            return (dx, dy) switch
            {
                (-1, -1) => MoveDirection.UpLeft,
                (1, -1) => MoveDirection.UpRight,
                (-1, 1) => MoveDirection.DownLeft,
                (1, 1) => MoveDirection.DownRight,
                _ => throw new ArgumentException($"Cannot convert {dx},{dy} to MoveDirection")
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
        
        public static CheckMateCell FromStringPosition(string position)
        {
            if (position.Length > 2 || !char.IsLetter(position[0]) || !char.IsDigit(position[1]))
            {
                throw new ArgumentException("Checkmate cell string should contain 2 chars: letter and digit");
            }

            var x = position[0] - 'a';
            var y = 7 - (position[1] - '1');
            return new CheckMateCell(x, y);
        }

        public override string ToString()
        {
            var xChar = (char)('a' + X);
            return $"{xChar}{7 - Y + 1}";
        }
    }
}