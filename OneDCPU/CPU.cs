using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDCPU
{
    public static class CPU
    {
        public static int EvaluateBoard(Board board)
        {
            // count mine
            int point = 0;
            point += board.array.Count(s => { return s == BoxStates.Mine; });

            // add special points
            // edge bonus
            if (board.array[0] == BoxStates.Mine)
            {
                point++;
            }
            if (board.array[^1] == BoxStates.Mine)
            {
                point++;
            }

            return point;
        }

        public static double EvaluatePoints(List<double> points)
        {
            double point = points.Average();
            return point;
        }

        public static double EvaluatePoints(List<int> points)
        {
            List<double> doubleList = [.. points];
            return EvaluatePoints(doubleList);
        }

        public static List<int> FindAllSetableBoxes(Board board, BoxStates side)
        {
            List<int> setables = new();
            BoxStates opposite = (side == BoxStates.Mine) ? BoxStates.Opponent : BoxStates.Mine;

            for (int i = 0; i < board.array.Length - 1; i++)
            {
                if (board.array[i] == BoxStates.Empty && board.array[i + 1] == opposite)
                {
                    setables.Add(i);
                }
                else if (board.array[i] == opposite && board.array[i + 1] == BoxStates.Empty)
                {
                    setables.Add(i + 1);
                }
            }

            return setables;
        }

        public static (int move, double point) SelectMaxPointMove(List<(int move, double point)> moves)
        {
            (int move, double point) maxPointMove = moves.First();
            foreach (var move in moves)
            {
                if (maxPointMove.point < move.point)
                {
                    maxPointMove = move;
                }
            }
            return maxPointMove;
        }

        public static (int move, double point) Think(Board board, int depth)
        {
            List<int> setables = FindAllSetableBoxes(board, BoxStates.Mine);

            if (depth <= 0)
            {
                List<double> points = new();

                foreach (var indexToSet in setables)
                {
                    var newBoard = board.Set(indexToSet, BoxStates.Mine);

                    // reflection
                    List<int> reflectionPoints = ThinkReflection(newBoard);
                    double pointAfterReflection = EvaluatePoints(reflectionPoints);

                    points.Add(pointAfterReflection);
                }

                double pointAtLast = points.Max();
                int retMove = points.IndexOf(pointAtLast); // It returns only one even if there were the same moves!

                return (retMove, pointAtLast);

            }
            else
            {

                // [TODO] なんかおかしいねぇ。全然処理してないねぇ。
                // My turn.
                if (setables.Count() == 0)
                {
                    return (0, EvaluateBoard(board));
                }

                List<(int move, double point)> selects = new();
                foreach (var indexToSet in setables)
                {
                    var boardAfterMyThinking = board.Set(indexToSet, BoxStates.Mine);

                    // Opponent's turn.
                    List<double> secondTurnSelects = new();
                    List<int> OpponentsSetables = FindAllSetableBoxes(boardAfterMyThinking, BoxStates.Opponent);
                    
                    if (OpponentsSetables.Count() == 0)
                    {
                        selects.Add((indexToSet, EvaluateBoard(boardAfterMyThinking)));
                        continue;
                    }

                    foreach (var OpponentsIndexToSet in OpponentsSetables)
                    {
                        var boardAfterOpponentsThinking = board.Set(OpponentsIndexToSet, BoxStates.Opponent);

                        // My turn again.
                        var selected = Think(boardAfterOpponentsThinking,depth - 1);
                        secondTurnSelects.Add(selected.point);
                    }

                    selects.Add((indexToSet, secondTurnSelects.Max()));
                }

                var retSelect = SelectMaxPointMove(selects);
                return retSelect;
            }
        }

        public static List<int> ThinkReflection(Board board)
        {
            List<int> points = new();
            List<int> allSetables = FindAllSetableBoxes(board, BoxStates.Opponent);

            // finished case
            if (allSetables.Count() == 0)
            {
                points.Add(EvaluateBoard(board));
                return points;
            }

            foreach (var indexToSet in allSetables)
            {
                var newBoard = board.Set(indexToSet, BoxStates.Opponent);
                points.Add(EvaluateBoard(newBoard));
            }
            return points;
        }
    }
}