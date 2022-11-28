using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimaxBot
{
    public enum MoveDirection
    {
        UpLeft, 
        UpRight,
        DownLeft,
        DownRight
    }
    
    public class Position
    {
        private CheckMateCell _sheepPosition;
        private readonly CheckMateCell[] _wolfPositions = new CheckMateCell[4];

        public Position(string sheepPosition, string[] wolfPositions)
        {
            this._sheepPosition = ParseCheckMatePosition(sheepPosition);

            for (var i = 0; i < 4; i++)
            {
                this._wolfPositions[i] = ParseCheckMatePosition(wolfPositions[i]);
            }
        }

        public Position(CheckMateCell sheepPosition, CheckMateCell[] wolfPositions)
        {
            if (wolfPositions.Length != 4)
            {
                throw new ArgumentException("Invalid number of wolves");
            }

            if (!sheepPosition.IsValid())
            {
                throw new ArgumentException("Sheep position is invalid");
            }

            if (!wolfPositions.All(pos => pos.IsValid()))
            {
                throw new ArgumentException($"Wolf position is invalid");
            }
            
            this._sheepPosition = sheepPosition;
            this._wolfPositions = wolfPositions;
        }
        
        private CheckMateCell ParseCheckMatePosition(string position)
        {
            if (position.Length > 2 || !char.IsLetter(position[0]) || !char.IsDigit(position[1]))
            {
                throw new ArgumentException("Checkmate cell string should contain 2 chars: letter and digit");
            }

            var x = position[0] - 'a';
            var y = position[1] - '0';
            var res = new CheckMateCell(x, y);
            if (!res.IsValid())
            {
                throw new ArgumentException($"Invalid checkmate position: {position}");
            }

            return res;
        }

        public CheckMateCell SheepPosition => _sheepPosition;

        public bool SheepCanMove(MoveDirection moveDirection)
        {
            var nextCell = _sheepPosition.NeighbourFrom(moveDirection);
            return nextCell.IsValid() && _wolfPositions.All(wolfPos => nextCell != wolfPos);
        }

        public IEnumerable<Position> NextSheepPositions()
        {
            List<Position> possibleNextPositions = new List<Position>();
            MoveDirection[] directions = new[]
            {
                MoveDirection.UpLeft, MoveDirection.UpRight,
                MoveDirection.DownLeft, MoveDirection.DownRight
            };

            foreach (var direction in directions)
            {
                if (SheepCanMove(direction))
                {
                    var pos = new Position(_sheepPosition.NeighbourFrom(direction), _wolfPositions);
                    possibleNextPositions.Add(pos);
                }
            }

            return possibleNextPositions;
        }

    }
}