using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OneDCPU
{
    public class Board
    {
        public BoxStates[] array;
        public Board(BoxStates[] array)
        {
            this.array = array;
        }

        public void ReverseOneDirection(bool doPositiveDirection, int index, BoxStates toSet, BoxStates[] array)
        {
            BoxStates opposite = (toSet == BoxStates.Mine) ? BoxStates.Opponent : BoxStates.Mine;

            // search
            int i = index;
            while (true)
            {
                if (doPositiveDirection)
                {
                    i++;
                    if (i >= array.Length) { return; }
                }
                else
                {
                    i--;
                    if (i < 0) { return; }
                }

                if (array[i] != opposite)
                {
                    if (array[i] == toSet)
                    {
                        break;
                    }
                    else
                    {
                        return; // None
                    }
                }
            }

            // reverse
            int end = i;
            if (doPositiveDirection)
            {
                for (i = index; i < end; i++)
                {
                    array[i] = toSet;
                }
            }
            else
            {
                for (i = index; i > end; i--)
                {
                    array[i] = toSet;
                }
            }
        }

        public Board Set(int index, BoxStates toSet)
        {
            // new array
            var newArray = new BoxStates[array.Length];
            array.CopyTo(newArray, 0);
            newArray[index] = toSet;

            // reverse
            ReverseOneDirection(doPositiveDirection: true, index, toSet, newArray);
            ReverseOneDirection(doPositiveDirection: false, index, toSet, newArray);

            // instantiate new board
            Board newBoard = new(newArray);

            return newBoard;
        }

        public override string ToString()
        {
            string output = "";
            foreach (var box in array)
            {

                output += box switch
                {
                    BoxStates.Empty => "_",
                    BoxStates.Mine => "O",
                    BoxStates.Opponent => "*",
                    _ => "？"
                };
            }
            return output;
        }
    }
}
