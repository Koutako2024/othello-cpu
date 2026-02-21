namespace TwoDCPU;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start!");
        Board board = new();
        board.Print();
        Console.WriteLine("Setable of black");
        board.GetSetable(Box.Black).ForEach(pair => Console.WriteLine(pair));
        Console.WriteLine("End!");
    }
}