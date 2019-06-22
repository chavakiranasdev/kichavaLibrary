using System;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
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
    }
}
