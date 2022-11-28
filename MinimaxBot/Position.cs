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
            this._sheepPosition = CheckMateCell.FromStringPosition(sheepPosition);

            for (var i = 0; i < 4; i++)
            {
                this._wolfPositions[i] = CheckMateCell.FromStringPosition(wolfPositions[i]);
            }
        }

        public Position(CheckMateCell sheepPosition, CheckMateCell[] wolfPositions)
        {
            if (wolfPositions.Length != 4)
            {
                throw new ArgumentException("Invalid number of wolves");
            }
            
            this._sheepPosition = sheepPosition;
            this._wolfPositions = wolfPositions;
        }

        public CheckMateCell SheepPosition => _sheepPosition;

        public bool SheepCanMove(MoveDirection moveDirection)
        {
            try
            {
                var nextCell = _sheepPosition.NeighbourFrom(moveDirection);
                return _wolfPositions.All(wolfPos => nextCell != wolfPos);
            }
            catch (ArgumentException ae)
            {
                return false;
            }
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

        public Position PositionMoveTo(Move move, bool sheepTurn)
        {
            if (sheepTurn && move.From == _sheepPosition)
            {
                return SheepMoveTo(move.To);
            }

            for (var i = 0; i < _wolfPositions.Length; i++)
            {
                if (_wolfPositions[i] == move.From)
                {
                    return WolfMoveTo(i, move.To);
                }
            }

            throw new ApplicationException($"No one to move from cell {move.From}");
        }

        private Position SheepMoveTo(CheckMateCell newPosition)
        {
            var direction = _sheepPosition.MoveDirectionTo(newPosition);
            
            if (!SheepCanMove(direction))
            {
                throw new ArgumentException($"Sheep can't move to cell {newPosition}");
            }

            return new Position(newPosition, _wolfPositions);
        }

        private Position WolfMoveTo(int wolfIndex, CheckMateCell newPosition)
        {
            if (!WolfCanMoveTo(wolfIndex, newPosition))
            {
                throw new ArgumentException($"Wolf ${wolfIndex} can't move to cell {newPosition}");
            }

            var newWolfCells = new CheckMateCell[_wolfPositions.Length];
            for (int i = 0; i < _wolfPositions.Length; i++)
            {
                newWolfCells[i] = _wolfPositions[i];
            }

            newWolfCells[wolfIndex] = newPosition;

            return new Position(_sheepPosition, newWolfCells);
        }

        private bool WolfCanMoveTo(int wolfIndex, CheckMateCell newPosition)
        {
            if (newPosition == _sheepPosition)
                return false;

            for (int i = 0; i < _wolfPositions.Length; i++)
            {
                if (i == wolfIndex)
                    continue;

                if (newPosition == _wolfPositions[i])
                    return false;
            }

            return true;
        }
    }
}