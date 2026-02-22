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

    public Box GetOpponentBox(Box box) => box switch
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

    private List<int> GetSetableInLineOneDirection(Box setter, Box[] line)
    {
        Box opponent = GetOpponentBox(setter);
        List<int> setables = new();


        // /^.*(mine)(opponent)+(green).*$/
        for (int i = 0; i < SIZE; i++)
        {
            for (; i < SIZE && line[i] != setter; i++) ;
            if (i + 1 >= SIZE) break;
            if (line[i + 1] != opponent) continue;
            int j = i + 2;
            for (; j < SIZE && line[j] == opponent; j++) ;
            if (j < SIZE && line[j] == Box.Green) setables.Add(j);
            i = j;
        }

        return setables;
    }

    public List<int> GetSetableInLine(Box setter, Box[] line)
    {
        Box opponent = GetOpponentBox(setter);
        List<int> setables = new();

        // check one direction.
        GetSetableInLineOneDirection(setter, line)
            .ForEach(i => setables.Add(i));

        // check another direction.
        Array.Reverse(line);
        GetSetableInLineOneDirection(setter, line)
            .ForEach(i => setables.Add(SIZE - i - 1));

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

    private List<int> GetFlipperInLineOneDirection(Box toSet, Box[] line, int index)
    {
        Box opponent = GetOpponentBox(toSet);
        List<int> flippers = new();

        // from left to right
        int i = index + 1;
        for (; i < SIZE && line[i] == opponent; i++)
        {
            flippers.Add(i);
        }

        return (i < SIZE && line[i] == toSet ? flippers : []);
    }

    private List<int> GetFlipperInLine(Box toSet, Box[] line, int index)
        => GetFlipperInLineOneDirection(toSet, line, index)
            .Concat(
                GetFlipperInLineOneDirection(toSet, line.Reverse().ToArray(), SIZE - index - 1)
                    .Select(i => SIZE - i - 1)
            )
            .ToList();

    private List<(int, int)> GetFlipper(Box toSet, int i, int j)
        => GetFlipperInLine(toSet, GetRow(i), j)
            .Select(k => (i, k))
            .Concat(
                GetFlipperInLine(toSet, GetColmn(j), i)
                .Select(k => (k, j))
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
