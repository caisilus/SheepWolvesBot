using System;
using System.Collections.Generic;

namespace MinimaxBot
{
    public enum MoveDirection
    {
        UpLeft, 
        UpRight,
        DownLeft,
        DownRight
    }
    
    public class Move
    {
        public CheckMateCell From { get; }
        public CheckMateCell To { get; }

        public MoveDirection Direction { get; }
        
        public Move(CheckMateCell from, CheckMateCell to)
        {
            this.Direction = from.MoveDirectionTo(to);
            From = from;
            To = to;
        }

        public Move(CheckMateCell from, MoveDirection direction)
        {
            this.Direction = direction;
            From = from;
            To = from.NeighbourFrom(direction);
        }

        public static Move FromString(string move)
        {
            var positions = move.Split(' ');
            if (positions.Length != 2)
                throw new ArgumentException($"Incorrect move string: {move}");
            var from = CheckMateCell.FromStringPosition(positions[0]);
            var to = CheckMateCell.FromStringPosition(positions[1]);
            return new Move(from, to);
        }

        public override string ToString()
        {
            return From.ToString() + " " + To.ToString();
        }

        public static MoveDirection[] AllMoveDirections()
        {
            MoveDirection[] directions = new[]
            {
                MoveDirection.UpLeft, MoveDirection.UpRight,
                MoveDirection.DownLeft, MoveDirection.DownRight
            };

            return directions;
        }

        public static MoveDirection[] WolfMoveDirections()
        {
            MoveDirection[] directions = new[]
            {
                MoveDirection.UpLeft, MoveDirection.UpRight
            };

            return directions;
        }
    }
}