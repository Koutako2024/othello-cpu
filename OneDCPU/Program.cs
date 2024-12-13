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
                BoxStates.Empty,
                BoxStates.Mine, 
                BoxStates.Opponent,
                BoxStates.Empty,
                BoxStates.Empty,
                BoxStates.Empty,
                BoxStates.Mine,
                BoxStates.Opponent,
                BoxStates.Empty,
                BoxStates.Empty,
                BoxStates.Empty,
            ]);
            Console.WriteLine(board.ToString());
            var result= CPU.Think(board, 1);
            Console.WriteLine(result);
            board = board.Set(result.move, BoxStates.Mine);
            Console.WriteLine(board.ToString());
            //int input = int.Parse(Console.ReadLine()??"0");
            //Console.WriteLine(input);
            //board=board.Set(input, BoxStates.Opponent);
            //Console.WriteLine(board.ToString());
            //result= CPU.Think(board, 10);
            //Console.WriteLine(result);
            //board = board.Set(result.move, BoxStates.Mine);
            //Console.WriteLine(board.ToString());

        }
    }
}
