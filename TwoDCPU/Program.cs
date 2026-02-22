namespace TwoDCPU;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start!");
        Board board = new();
        Box player = Box.Black;
        for (Box turn = Box.Black; ; turn = board.GetOpponentBox(turn))
        {
            board.Print();

            Console.WriteLine($"{turn}'s Turn.");
            var setables = board.GetSetable(turn);
            if (setables.Count == 0)
            {
                if (board.GetSetable(board.GetOpponentBox(turn)).Count == 0)
                    break;

                Console.WriteLine("Skip. (You can't set anywhere.)");
                continue;
            }
            setables.ForEach(pair => Console.WriteLine(pair));

            if (turn == player)
            {
                do
                {
                    Console.Write($"set {turn} in row?>");
                    try
                    {
                        int i = int.Parse(Console.ReadLine() ?? "read line returned null!");
                        Console.Write("colmn?>");
                        int j = int.Parse(Console.ReadLine() ?? "read line returned null!");
                        board.Set(turn, i, j);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Error!");
                    }
                } while (true);
            }
            else
            {
                (int, int) pos = CPU.Run(board, turn);
                board.Set(turn, pos);
                Console.WriteLine($"{turn} set at {pos}.");
            }
        }

        (int green, int black, int white) = board.Count();
        Console.WriteLine($"Green: {green}");
        Console.WriteLine($"Black: {black}");
        Console.WriteLine($"White: {white}");
        Console.WriteLine((black == white ? "Draw!" : (black > white ? "Black" : "White") + " Win!"));
        Console.WriteLine("End!");
    }
}