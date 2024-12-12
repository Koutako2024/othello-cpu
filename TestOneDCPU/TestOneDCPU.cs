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
                BoxStates.None,
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
            BoxStates[] array = [
                BoxStates.None,
                BoxStates.None,     // <-
                BoxStates.Opponent,
                BoxStates.Mine,
                BoxStates.None,
                BoxStates.None,     // <-
                BoxStates.Opponent,
                ];
            Board board = new(array);

            List<int> actual = CPU.FindAllSetableBoxes(board, BoxStates.Mine);

            List<int> expected = [1, 5];
            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [TestMethod]
        public void TestSelectMaxPointMove()
        {
            List<(int move, double point)> moves = [(0, 0), (1, 1), (2, 2), (3, 3), (4, 2), (-1, 3)];
            var result = CPU.SelectMaxPointMove(moves);
            Assert.AreEqual((3, 3), result);
        }

        [TestMethod]
        public void TestThink()
        {
            //TODO
        }
    }
}