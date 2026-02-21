namespace TwoDCPU;

public class Board
{
    public int Size;
    public Box[,] Data;

    public Board(int size = 8)
    {
        Size = size;
        Data = new Box[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
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
        for (int i = 0; i < Size; i++) Console.Write(i % 10);
        Console.WriteLine();

        for (int i = 0; i < Size; i++)
        {
            Console.Write(i);

            for (int j = 0; j < Size; j++)
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
        for (int i = 0; i < Size; i++) Console.Write(i % 10);
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
        var line = new Box[Size];
        for (int i = 0; i < Size; i++)
        {
            line[i] = Data[index, i];
        }
        return line;
    }

    public Box[] GetColmn(int index)
    {
        var col = new Box[Size];
        for (int i = 0; i < Size; i++)
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
        for (int i = 0; i < Size; i++)
        {
            for (; i < Size && line[i] != setter; i++) ;
            if (i + 1 >= Size) break;
            if (line[i + 1] != opponent) continue;
            int j = i + 2;
            for (; j < Size && line[j] == opponent; j++) ;
            if (line[j] == Box.Green) setables.Add(j);
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
            .ForEach(i => setables.Add(Size - i - 1));

        return setables;
    }

    public List<(int, int)> GetSetable(Box setter)
    {
        List<(int, int)> setables = new();

        // check rows
        for (int i = 0; i < Size; i++)
            GetSetableInLine(setter, GetRow(i)).ForEach(j => setables.Add((i, j)));

        // check colmns
        for (int i = 0; i < Size; i++)
            GetSetableInLine(setter, GetColmn(i)).ForEach(j => setables.Add((j, i)));

        // to unique
        return setables.Distinct().ToList();
    }

    public void Set(Box box, int i, int j)
    {
        if (!GetSetable(box).Exists(pair => (pair == (i, j))))
            throw new ArgumentException($"{box} can't be set in index ({i}, {j})!");

        Data[i, j] = box;
    }

}
