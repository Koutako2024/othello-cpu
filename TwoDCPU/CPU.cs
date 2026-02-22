namespace TwoDCPU;

public static class CPU
{
    public static double Evaluate(Board board, Box player)
    {
        (int green, int black, int white) = board.Count();
        return player switch
        {
            Box.Black => (double)black / (green + white),
            Box.White => (double)white / (green + black),
            Box other => throw new ArgumentException($"player can't be {other}!"),
        };
    }

    public static (int, int) Run(Board board, Box player)
        => board.GetSetable(player).MaxBy(pos =>
        {
            Board prediction = board.Copy();
            prediction.Set(player, pos.Item1, pos.Item2);
            return Evaluate(prediction, player);
        });

}