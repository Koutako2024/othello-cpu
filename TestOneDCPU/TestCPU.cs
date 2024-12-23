using OneDCPU;

namespace TestOneDCPU
{
    [TestClass]
    public class TestCPU
    {
        [TestMethod]
        public void TestEvaluateBoardExcludingEdges()
        {
            BoxStates[] array = [
                BoxStates.Opponent,
                BoxStates.Mine,
                BoxStates.Opponent,
                BoxStates.Mine,
                BoxStates.Empty,
                ];
            Board board = new(array);


            int result = CPU.EvaluateBoard(board);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void TestEvaluateBoardOnlyAnEdge()
        {
            BoxStates[] array = [
                BoxStates.Mine,
                ];
            Board board = new(array);

            int result = CPU.EvaluateBoard(board);

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestEvaluatePoints()
        {
            List<int> points = [1, 2, 3, 4, 5];
            var result = CPU.EvaluatePoints(points);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestFindAllSetables()
        {
            Board board = new([
                BoxStates.Empty,
                BoxStates.Empty,
                BoxStates.Opponent,
                BoxStates.Mine,
                BoxStates.Opponent,
                BoxStates.Opponent,
                BoxStates.Opponent,
                BoxStates.Empty,
                BoxStates.Mine,
                BoxStates.Empty,
                BoxStates.Opponent,
                BoxStates.Empty,
                BoxStates.Opponent,
            ]);

            List<int> actual = CPU.FindAllSetableBoxes(board, BoxStates.Mine);

            List<int> expected = [1, 7];
            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [TestMethod]
        public void TestFindHalfSetableBoxes()
        {
            Board board = new([
                BoxStates.Empty,
                BoxStates.Empty,
                BoxStates.Opponent,
                BoxStates.Mine,
                BoxStates.Empty,
                BoxStates.Opponent,
                BoxStates.Opponent,
                BoxStates.Opponent,
                BoxStates.Mine,
                BoxStates.Empty,
                BoxStates.Opponent,
                BoxStates.Empty,
                BoxStates.Opponent,
            ]);

            var result = CPU.FindHalfSetableBoxes(board, BoxStates.Mine);

            Assert.IsTrue(result.SequenceEqual([1, 4]));
        }

        [TestMethod]
        public void TestSelectMaxPointMove()
        {
            List<(int move, double point)> moves = [(0, 0), (1, 1), (2, 2), (3, 3), (4, 2), (-1, 3)];
            var result = CPU.SelectMaxPointMove(moves);
            Assert.AreEqual((3, 3), result);
        }

    }
}