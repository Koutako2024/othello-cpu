namespace TwoDCPU;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start!");
        Board board = new();
        for (Box turn = Box.Black; ; turn = board.GetOpponentBox(turn))
        {
            board.Print();

            Console.WriteLine($"Setable of {turn}");
            var setables = board.GetSetable(turn);
            if (setables.Count == 0)
            {
                if (board.GetSetable(board.GetOpponentBox(turn)).Count == 0)
                {
                    break;
                }
                Console.WriteLine("Skip. (You can't set anywhere.)");
                continue;
            }
            foreach (var pair in setables) Console.WriteLine(pair);

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

        (int green, int black, int white) = board.Count();
        Console.WriteLine($"Green: {green}");
        Console.WriteLine($"Black: {black}");
        Console.WriteLine($"White: {white}");
        Console.WriteLine((black == white ? "Draw!" : (black > white ? "Black" : "White") + " Win!"));
        Console.WriteLine("End!");
    }
}