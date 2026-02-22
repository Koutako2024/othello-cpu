namespace TwoDCPU;

public class Board
{
    public const int SIZE = 8;
    public Box[,] Data;

    public Board()
    {
        Data = new Box[SIZE, SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                Data[i, j] = Box.Green;
            }
        }
        Data[3, 3] = Box.Black;
        Data[3, 4] = Box.White;
        Data[4, 3] = Box.White;
        Data[4, 4] = Box.Black;
    }

    public void Print()
    {
        Console.Write(" ");
        for (int i = 0; i < SIZE; i++) Console.Write(i % 10);
        Console.WriteLine();

        for (int i = 0; i < SIZE; i++)
        {
            Console.Write(i);

            for (int j = 0; j < SIZE; j++)
            {
                Console.Write(Data[i, j] switch
                {
                    Box.Green => "□",
                    Box.Black => "○",
                    Box.White => "●",
                    _ => "?",
                });
            }

            Console.WriteLine(i);
        }

        Console.Write(" ");
        for (int i = 0; i < SIZE; i++) Console.Write(i % 10);
        Console.WriteLine();
    }

    public static Box GetOpponentBox(Box box) => box switch
    {
        Box.Black => Box.White,
        Box.White => Box.Black,
        _ => throw new ArgumentException(box.ToString() + "does not have opponent!")
    };

    public Box[] GetRow(int index)
    {
        var line = new Box[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            line[i] = Data[index, i];
        }
        return line;
    }

    public Box[] GetColmn(int index)
    {
        var col = new Box[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            col[i] = Data[i, index];
        }
        return col;
    }

    /// <summary>
    ///     The line from left down to right up.
    /// </summary>
    /// <param name="index">
    ///     (0 &lt;= i+j &lt;= (14=2*(SIZE-1)))
    /// </param>
    /// <returns>
    ///     length is dynamic.
    /// </returns>
    public Box[] GetSlash(int index)
    {
        if (index < SIZE)
        {
            var slash = new Box[index + 1];
            for (int i = index, j = 0; i >= 0; i--, j++)
                slash[j] = Data[i, j];
            return slash;
        }
        else
        {
            var slash = new Box[2 * SIZE - index - 1];
            for (int i = SIZE - 1, j = index - SIZE + 1, k = 0; j < SIZE; i--, j++, k++)
                slash[k] = Data[i, j];
            return slash;
        }
    }

    /// <summary>
    ///     the line from left up to right down.
    /// </summary>
    /// <param name="index">
    ///     (-(SIZE-1) &lt;= i-j &lt;= (SIZE-1))
    /// </param>
    /// <returns>
    ///     length is dynamic.
    /// </returns>
    public Box[] GetBackSlash(int index)
    {
        if (index <= 0)
        {
            var backslash = new Box[index + SIZE];
            for (int i = 0, j = -index; j < SIZE; i++, j++)
                backslash[i] = Data[i, j];
            return backslash;
        }
        else
        {
            var backslash = new Box[SIZE - index];
            for (int i = index, j = 0; i < SIZE; i++, j++)
                backslash[j] = Data[i, j];
            return backslash;
        }
    }

    private static List<int> GetSetableInLineOneDirection(Box setter, Box[] line)
    {
        Box opponent = GetOpponentBox(setter);
        List<int> setables = new();

        // /^.*(mine)(opponent)+(green).*$/
        for (int i = 0; i < line.Length; i++)
        {
            for (; i < line.Length && line[i] != setter; i++) ;
            if (i + 1 >= line.Length) break;
            if (line[i + 1] != opponent) continue;
            int j = i + 2;
            for (; j < line.Length && line[j] == opponent; j++) ;
            if (j < line.Length && line[j] == Box.Green) setables.Add(j);
            i = j;
        }

        return setables;
    }

    public static List<int> GetSetableInLine(Box setter, Box[] line)
    {
        Box opponent = GetOpponentBox(setter);
        List<int> setables = new();

        // check one direction.
        GetSetableInLineOneDirection(setter, line)
            .ForEach(i => setables.Add(i));

        // check another direction.
        Array.Reverse(line);
        GetSetableInLineOneDirection(setter, line)
            .ForEach(i => setables.Add(line.Length - i - 1));

        return setables;
    }

    public List<(int, int)> GetSetable(Box setter)
    {
        List<(int, int)> setables = new();

        // check rows
        for (int i = 0; i < SIZE; i++)
            GetSetableInLine(setter, GetRow(i)).ForEach(j => setables.Add((i, j)));

        // check colmns
        for (int i = 0; i < SIZE; i++)
            GetSetableInLine(setter, GetColmn(i)).ForEach(j => setables.Add((j, i)));

        // to unique
        return setables.Distinct().ToList();
    }

    private static List<int> GetFlipperInLineOneDirection(Box toSet, Box[] line, int index)
    {
        Box opponent = GetOpponentBox(toSet);
        List<int> flippers = new();

        // from left to right
        int i = index + 1;
        for (; i < line.Length && line[i] == opponent; i++)
        {
            flippers.Add(i);
        }

        return (i < line.Length && line[i] == toSet ? flippers : []);
    }

    private static List<int> GetFlipperInLine(Box toSet, Box[] line, int index)
        => GetFlipperInLineOneDirection(toSet, line, index)
            .Concat(
                GetFlipperInLineOneDirection(toSet, line.Reverse().ToArray(), line.Length - index - 1)
                    .Select(i => line.Length - i - 1)
            )
            .ToList();

    private List<(int, int)> GetFlipper(Box toSet, int i, int j)
        => GetFlipperInLine(toSet, GetRow(i), j)
            .Select(k => (i, k))
            .Concat(
                GetFlipperInLine(toSet, GetColmn(j), i)
                    .Select(k => (k, j))
            )
            .Concat(
                GetFlipperInLine(toSet, GetSlash(i + j), (i + j < SIZE ? j : SIZE - 1 - i))
                    .Select(k => (
                        i + j < SIZE
                        ? (i + j - k, k)
                        : (SIZE - 1 - k, i + j - (SIZE - 1 - k))
                    ))
            )
            .Concat(
                GetFlipperInLine(toSet, GetBackSlash(i - j), (i - j <= 0 ? i : j))
                    .Select(k => (
                        i - j <= 0
                        ? (k, k - (i - j))
                        : (i - j + k, k)
                    ))
            )
            .Distinct()
            .ToList();

    public void Set(Box toSet, (int i, int j) pos) => Set(toSet, pos.i, pos.j);
    public void Set(Box toSet, int i, int j)
    {
        if (!GetSetable(toSet).Exists(pair => (pair == (i, j))))
            throw new ArgumentException($"{toSet} can't be set in index ({i}, {j})!");

        Data[i, j] = toSet;
        GetFlipper(toSet, i, j).ForEach(pair => Data[pair.Item1, pair.Item2] = toSet);
    }

    public (int green, int black, int white) Count()
    {
        int green = 0, black = 0, white = 0;
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                var _ = Data[i, j] switch
                {
                    Box.Green => green++,
                    Box.Black => black++,
                    Box.White => white++,
                    _ => throw new Exception("What?!"),
                };
            }
        }
        return (green, black, white);
    }

    public Board Copy() => new Board { Data = (Box[,])Data.Clone() };
}
