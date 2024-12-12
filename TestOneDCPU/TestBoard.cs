using OneDCPU;

namespace TestOneDCPU
{
    [TestClass]
    public class TestBoard
    {
        [TestMethod]
        [DynamicData(nameof(DataOfTestSet))]
        public void TestSet(BoxStates[] startArray, int index, BoxStates toSet, BoxStates[] expected)
        {
            Board startBoard = new(startArray);
            Board newBoard = startBoard.Set(index, toSet);
            Assert.IsTrue(newBoard.array.SequenceEqual(expected));
        }

        [DataTestMethod]
        [DynamicData(nameof(DataOfTestReverseOneDirectionPositiveDirection))]
        public void TestReverseOneDirection(bool doPositiveDirection, BoxStates[] startArray, int index, BoxStates toSet, BoxStates[] expected)
        {
            Board board = new(startArray);
            // new array
            var newArray = new BoxStates[board.array.Length];
            board.array.CopyTo(newArray, 0);
            newArray[index] = toSet;

            board.ReverseOneDirection(doPositiveDirection, index, toSet, newArray);

            Assert.IsTrue(newArray.SequenceEqual(expected));
        }

        public static IEnumerable<object[]> DataOfTestSet
        {
            get
            {
                return [
                    [
                        new BoxStates[]
                        {
                            BoxStates.Empty,
                            BoxStates.Opponent,
                            BoxStates.Opponent,
                            BoxStates.Empty, // <- set mine here
                            BoxStates.Opponent,
                            BoxStates.Opponent,
                            BoxStates.Mine,
                            BoxStates.Empty,
                        },
                        3,
                        BoxStates.Mine,
                        new BoxStates[]
                        {
                            BoxStates.Empty,
                            BoxStates.Opponent,
                            BoxStates.Opponent,
                            BoxStates.Mine,
                            BoxStates.Mine,
                            BoxStates.Mine,
                            BoxStates.Mine,
                            BoxStates.Empty,
                        }
                    ],

                    [
                        new BoxStates[]
                        {
                            BoxStates.Empty, // <- set mine here
                        },
                        0,
                        BoxStates.Mine,
                        new BoxStates[]
                        {
                            BoxStates.Mine,
                        }
                    ],

                    [
                        new BoxStates[]
                        {
                            BoxStates.Opponent,
                            BoxStates.Empty, // <- set mine here
                            BoxStates.Mine,
                        },
                        1,
                        BoxStates.Opponent,
                        new BoxStates[]
                        {
                            BoxStates.Opponent,
                            BoxStates.Opponent,
                            BoxStates.Mine,
                        }
                    ]
                ];
            }
        }

        public static IEnumerable<object[]> DataOfTestReverseOneDirectionPositiveDirection
        {
            get
            {
                object[] positiveAndMine = [
                    true,
                new BoxStates[] {
                    BoxStates.Empty,
                    BoxStates.Mine,
                    BoxStates.Opponent,
                    BoxStates.Opponent,
                    BoxStates.Empty, //<- set mine here.
                    BoxStates.Opponent,
                    BoxStates.Opponent,
                    BoxStates.Mine,
                    BoxStates.Empty,
                },
                    4,
                    BoxStates.Mine,
                new BoxStates[] {
                    BoxStates.Empty,
                    BoxStates.Mine,
                    BoxStates.Opponent,
                    BoxStates.Opponent,
                    BoxStates.Mine,
                    BoxStates.Mine,
                    BoxStates.Mine,
                    BoxStates.Mine,
                    BoxStates.Empty,
                }];

                object[] negativeAndOpponent = [
                    false,
                new BoxStates[] {
                    BoxStates.Opponent,
                    BoxStates.Mine,
                    BoxStates.Mine,
                    BoxStates.Empty, //<- set mine here.
                },
                    3,
                    BoxStates.Opponent,
                new BoxStates[] {
                    BoxStates.Opponent,
                    BoxStates.Opponent,
                    BoxStates.Opponent,
                    BoxStates.Opponent,
                }];

                return [positiveAndMine, negativeAndOpponent];
            }
        }
    }
}
