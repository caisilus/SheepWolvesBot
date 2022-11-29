using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimaxBot
{
    class Program
    {
        static Node TreeFromLeafs(int[] leafs, int b)
        {
            List<Node> currentLayer = leafs.Select(v => new Node(null, v)).ToList();
            while (currentLayer.Count != 1)
            {
                List<Node> newLayer = new List<Node>();
                for (int i = 0; i < currentLayer.Count; i += b)
                {
                    List<Node> children = new List<Node>();
                    for (int j = 0; j < b; j++)
                    {
                        children.Add(currentLayer[i + j]);
                    }

                    newLayer.Add(new Node(children.ToArray()));
                }

                currentLayer = newLayer;
            }

            return currentLayer[0];
        }

        static void TestAlphaBeta()
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int d = int.Parse(inputs[0]);
            int b = int.Parse(inputs[1]);
            int[] leafs = Console.ReadLine().Split(' ').Select(s=>int.Parse(s)).ToArray();
            Node root = TreeFromLeafs(leafs, b);
            //Tuple<int, int> t = MinimaxAlgorithm.EvaluationForRoot(root);
            //Console.WriteLine($"{t.Item1} {t.Item2}");
            AlphaBetaPruning alphaBetaPruning = new AlphaBetaPruning();
            alphaBetaPruning.EvaluateTree(root);
            Console.WriteLine($"{root.Evaluation} {alphaBetaPruning.CountVisited}");
        }
        
        static int Main(string[] args)
        {
            var controllingSheep = int.Parse(args.Last()) == 0;
            var bot = new SheepAndWolvesBot(controllingSheep, 7, BasicHeuristic.Evaluate);
            var sheepCell = CheckMateCell.FromStringPosition("d8");
            var wolvesCells = new[]
            {
                CheckMateCell.FromStringPosition("a1"),
                CheckMateCell.FromStringPosition("c1"),
                CheckMateCell.FromStringPosition("e1"),
                CheckMateCell.FromStringPosition("g1")
            };
            var startPosition = new Position(sheepCell, wolvesCells);
            //Console.WriteLine(bot.NextMove(startPosition));
            return MainLoop(startPosition, bot);
        }

        static int MainLoop(Position startPosition, SheepAndWolvesBot bot)
        {
            var positionAtPlayerTurn = startPosition;
            if (bot.ControllingSheep)
            {
                var botMove = bot.NextMove(positionAtPlayerTurn);
                Console.Error.WriteLine(botMove);
                Console.WriteLine(botMove);
                positionAtPlayerTurn = positionAtPlayerTurn.PositionMoveTo(botMove, true);
            }
            
            while (true)
            {
                if (positionAtPlayerTurn.GameStatus != GameState.GameContinues)
                {
                    Console.WriteLine(WinMessage(positionAtPlayerTurn.GameStatus, bot.ControllingSheep));
                    
                    if (BotWin(positionAtPlayerTurn.GameStatus, bot.ControllingSheep))
                    {
                        return 0;
                    }

                    return 3;
                }

                // player turn
                var playerMoveString = Console.ReadLine();
                var playerMove = Move.FromString(playerMoveString?.Trim());
                var positionAtBotTurn = positionAtPlayerTurn.PositionMoveTo(playerMove, !bot.ControllingSheep);
                
                if (positionAtBotTurn.GameStatus != GameState.GameContinues)
                {;
                    Console.WriteLine(WinMessage(positionAtPlayerTurn.GameStatus, !bot.ControllingSheep));
                    
                    if (BotWin(positionAtPlayerTurn.GameStatus, bot.ControllingSheep))
                    {
                        return 0;
                    }

                    return 3;
                }
                
                // bot turn
                var botMove = bot.NextMove(positionAtBotTurn);
                Console.Error.WriteLine(botMove);
                Console.WriteLine(botMove);
                positionAtPlayerTurn = positionAtBotTurn.PositionMoveTo(botMove, bot.ControllingSheep);
            }
        }

        static string WinMessage(GameState gameStatus, bool controllingSheep)
        {
            if (controllingSheep)
            {
                if (gameStatus == GameState.SheepWin)
                {
                    return "I win!";
                }
                    
                return "You win!";
            }
            
            if (gameStatus == GameState.WolvesWin)
            {
                return "I win!";
            }
                
            return "You win!";
        }

        static bool BotWin(GameState gameStatus, bool controllingSheep)
        {
            if (controllingSheep)
            {
                return gameStatus == GameState.SheepWin;
            }

            return gameStatus == GameState.WolvesWin;
        }
    }
}