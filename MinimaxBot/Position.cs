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
            var nextSheepCells = NextSheepCells();
            foreach (var sheepCell in nextSheepCells)
            {
                nextPositions.Add(new Position(sheepCell, _wolvesCells));
            }

            return nextPositions;
        }

        private IEnumerable<CheckMateCell> NextSheepCells()
        {
            var directions = Move.AllMoveDirections().Reverse();
            var cells = new List<CheckMateCell>();
            
            foreach (var direction in directions)
            {
                if (!_sheepCell.HasNeighbourFrom(direction))
                    continue;
                
                var nextCell = _sheepCell.NeighbourFrom(direction);
                if (SheepCanMoveTo(nextCell))
                {
                    cells.Add(nextCell);
                }
            }

            return cells;
        }
        
        private bool SheepCanMoveTo(CheckMateCell to)
        {
            return _wolvesCells.All(wolfPos => to != wolfPos);
        }

        public IEnumerable<Position> NextWolvesPositions()
        {
            List<Position> nextPositions = new List<Position>();
            var directions = Move.WolfMoveDirections();

            for (var i = 0; i < _wolvesCells.Length; i++)
            {
                foreach (var direction in directions)
                {
                    if (!_wolvesCells[i].HasNeighbourFrom(direction))
                        continue;
                    
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

            if (!NextSheepCells().Any())
            {
                return GameState.WolvesWin;
            }

            return GameState.GameContinues;
        }

        public Move CalculateMoveToPosition(Position destination)
        {
            if (_sheepCell != destination.SheepCell)
            {
                return new Move(_sheepCell, destination.SheepCell);
            }

            for (var i = 0; i < _wolvesCells.Length; i++)
            {
                if (_wolvesCells[i] != destination._wolvesCells[i])
                {
                    return new Move(_wolvesCells[i], destination._wolvesCells[i]);
                }
            }

            throw new ApplicationException("Cannot calculate move");
        }
    }
}