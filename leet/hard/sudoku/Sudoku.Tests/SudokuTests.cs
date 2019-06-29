using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sudoku.Tests
{
    [TestClass]
    public class SudokuTests
    {
        [TestMethod]
        public void FirstSudokuTest()
        {
            var solution = new Solution();
            char[][] board = new char[9][]{
                new char[9] {'5', '3', '.', '.', '7', '.', '.', '.', '.'},
                new char[9] {'6', '.', '.', '1', '9', '5', '.', '.', '.'},
                new char[9] {'.', '9', '8', '.', '.', '.', '.', '6', '.'},
                new char[9] {'8', '.', '.', '.', '6', '.', '.', '.', '3'},
                new char[9] {'4', '.', '.', '8', '.', '3', '.', '.', '1'},
                new char[9] {'7', '.', '.', '.', '2', '.', '.', '.', '6'},
                new char[9] {'.', '6', '.', '.', '.', '.', '2', '8', '.'},
                new char[9] {'.', '.', '.', '4', '1', '9', '.', '.', '5'},
                new char[9] {'.', '.', '.', '.', '8', '.', '.', '7', '9'},
            };
            solution.SolveSudoku(board);
        }

        [TestMethod]
        public void SecondSudokuTest()
        {
            var solution = new Solution();
            char[][] board = new char[9][]{
            new char[9] {'.', '.', '9', '7', '4', '8', '.', '.', '.' },
            new char[9] {'7', '.', '.', '.', '.', '.', '.', '.', '.' },
            new char[9] {'.', '2', '.', '1', '.', '9', '.', '.', '.'},
            new char[9] {'.', '.', '7', '.', '.', '.', '2', '4', '.'},
            new char[9] {'.', '6', '4', '.', '1', '.', '5', '9', '.'},
            new char[9] {'.', '9', '8', '.', '.', '.', '3', '.', '.'},
            new char[9] {'.', '.', '.', '8', '.', '3', '.', '2', '.'},
            new char[9] {'.', '.', '.', '.', '.', '.', '.', '.', '6'},
            new char[9] {'.', '.', '.', '2', '7', '5', '9', '.', '.'},
            };
            solution.SolveSudoku(board);
        }

        [TestMethod]
        public void ThirdSudokuTest()
        {
            var solution = new Solution();
            char[][] board = new char[9][] {
                new char[9] {'.','.','.','2','.','.','.','6','3'},
                new char[9] {'3','.','.','.','.','5','4','.','1'},
                new char[9] {'.','.','1','.','.','3','9','8','.'},
                new char[9] {'.','.','.','.','.','.','.','9','.'},
                new char[9] {'.','.','.','5','3','8','.','.','.'},
                new char[9] {'.','3','.','.','.','.','.','.','.'},
                new char[9] {'.','2','6','3','.','.','5','.','.'},
                new char[9] {'5','.','3','7','.','.','.','.','8'},
                new char[9] {'4','7','.','.','.','1','.','.','.'}
            };
            solution.SolveSudoku(board);
        }
    }
}
