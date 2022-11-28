using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimaxBot
{
    public enum GameState
    {
        SheepWin,
        WolvesWin,
        GameContinues       
    }
    
    public class Position
    {
        private CheckMateCell _sheepCell;
        private readonly CheckMateCell[] _wolvesCells;

        public Position(CheckMateCell sheepCell, CheckMateCell[] wolvesCells)
        {
            if (wolvesCells.Length != 4)
            {
                throw new ArgumentException("Invalid number of wolves");
            }
            
            this._sheepCell = sheepCell;
            this._wolvesCells = wolvesCells;
            GameStatus = CheckWin();
        }

        public CheckMateCell SheepCell => _sheepCell;

        public IEnumerable<CheckMateCell> WolvesCells => _wolvesCells;

        public GameState GameStatus { get; }

        public IEnumerable<Position> NextSheepPositions()
        {
            List<Position> nextPositions = new List<Position>();
            var directions = Move.AllMoveDirections();

            foreach (var direction in directions)
            {
                var nextPosition = _sheepCell.NeighbourFrom(direction);
                if (SheepCanMoveTo(nextPosition))
                {
                    var pos = new Position(_sheepCell.NeighbourFrom(direction), _wolvesCells);
                    nextPositions.Add(pos);
                }
            }

            return nextPositions;
        }
        
        private bool SheepCanMoveTo(CheckMateCell to)
        {
            try
            {
                return _wolvesCells.All(wolfPos => to != wolfPos);
            }
            catch (ArgumentException ae)
            {
                return false;
            }
        }

        public IEnumerable<Position> NextWolvesPositions()
        {
            List<Position> nextPositions = new List<Position>();
            var directions = Move.AllMoveDirections();

            for (var i = 0; i < _wolvesCells.Length; i++)
            {
                foreach (var direction in directions)
                {
                    var newCell = _wolvesCells[i].NeighbourFrom(direction);
                    if (!WolfCanMoveTo(i, newCell))
                        continue;

                    var position = WolfMoveTo(i, newCell);
                    nextPositions.Add(position);
                }    
            }

            return nextPositions;
        }

        public Position PositionMoveTo(Move move, bool sheepTurn)
        {
            if (sheepTurn && move.From == _sheepCell)
            {
                return SheepMoveTo(move.To);
            }

            for (var i = 0; i < _wolvesCells.Length; i++)
            {
                if (_wolvesCells[i] == move.From)
                {
                    return WolfMoveTo(i, move.To);
                }
            }

            throw new ApplicationException($"No one to move from cell {move.From}");
        }

        private Position SheepMoveTo(CheckMateCell newPosition)
        {
            if (!SheepCanMoveTo(newPosition))
            {
                throw new ArgumentException($"Sheep can't move to cell {newPosition}");
            }

            return new Position(newPosition, _wolvesCells);
        }

        private Position WolfMoveTo(int wolfIndex, CheckMateCell newPosition)
        {
            if (!WolfCanMoveTo(wolfIndex, newPosition))
            {
                throw new ArgumentException($"Wolf ${wolfIndex} can't move to cell {newPosition}");
            }

            var newWolvesCells = WolfMovedWolvesPositions(wolfIndex, newPosition);

            return new Position(_sheepCell, newWolvesCells);
        }

        private CheckMateCell[] WolfMovedWolvesPositions(int wolfIndex, CheckMateCell newPosition)
        {
            var newWolvesCells = new CheckMateCell[_wolvesCells.Length];
            Array.Copy(_wolvesCells, newWolvesCells, _wolvesCells.Length);
            newWolvesCells[wolfIndex] = newPosition;
            return newWolvesCells;
        }
        
        private bool WolfCanMoveTo(int wolfIndex, CheckMateCell newPosition)
        {
            if (newPosition == _sheepCell)
                return false;

            for (int i = 0; i < _wolvesCells.Length; i++)
            {
                if (i == wolfIndex)
                    continue;

                if (newPosition == _wolvesCells[i])
                    return false;
            }

            return true;
        }

        private GameState CheckWin()
        {
            if (_sheepCell.Y == 7)
            {
                return GameState.SheepWin;
            }

            if (!NextSheepPositions().Any())
            {
                return GameState.WolvesWin;
            }

            return GameState.GameContinues;
        }
    }
}