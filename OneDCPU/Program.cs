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
                BoxStates.Opponent, 
                BoxStates.Mine, 
                BoxStates.Opponent, 
                BoxStates.Empty, 
                BoxStates.Empty,
                BoxStates.Empty]);
            var result= CPU.Think(board, 10);
            Console.WriteLine(result);

        }
    }
}
