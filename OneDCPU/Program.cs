namespace OneDCPU
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start!");

            // test Think func
            Board board = new([
                BoxStates.Empty,
                BoxStates.Empty,
                //BoxStates.Mine,
                BoxStates.Opponent,
                //BoxStates.Empty,
                //BoxStates.Empty,
                BoxStates.Opponent,
                //BoxStates.Mine,
                //BoxStates.Empty,
                //BoxStates.Empty,
                BoxStates.Opponent,
                //BoxStates.Mine,
                BoxStates.Empty,
                BoxStates.Empty,
                BoxStates.Mine,
                BoxStates.Opponent,
                BoxStates.Empty,
                BoxStates.Empty,
            ]);
            while (true)
            {
                // check finishing.
                if (CPU.FindAllSetableBoxes(board, BoxStates.Mine).Count == 0)
                {
                    Console.WriteLine("Finish!");
                    break;
                }

                // CPU's turn
                Console.WriteLine(board.ToString());
                var result = CPU.Think(board, 10);
                Console.WriteLine(result);
                board = board.Set(result.move, BoxStates.Mine);
                Console.WriteLine(board.ToString());

                // check finishing.
                if (CPU.FindAllSetableBoxes(board, BoxStates.Opponent).Count == 0)
                {
                    Console.WriteLine("Finish!");
                    break;
                }

                // Player's turn
                int input = int.Parse(Console.ReadLine() ?? "0");
                board = board.Set(input, BoxStates.Opponent);
            }
            Console.WriteLine(board.ToString());



        }
    }
}
