using System;
using System.Collections.Generic;
using System.Linq;

public class Solution
{
    // Constants 
    private int SizeOfSudoku { get; set; }

    // Private variables. 
    private Cell[,] cellBoard;

    Dictionary<int, List<int>> rowAvailableList = new Dictionary<int, List<int>>();
    Dictionary<int, List<int>> columnAvailableList = new Dictionary<int, List<int>>();
    Dictionary<int, List<int>> gridAvailableList = new Dictionary<int, List<int>>();

    public Solution(int sizeOfSudoku = 9)
    {
        SizeOfSudoku = sizeOfSudoku;
        cellBoard = new Cell[SizeOfSudoku, SizeOfSudoku];
    }

    public void SolveSudoku(char[][] board)
    {
        Console.WriteLine("Start...");
        InitializeAllAvailableLiistsWithEverything();
        // Initialize given char array into our cell array. 
        InitializeCellArray(board);

        // First round - find all those which doesn't need guessing.
        bool isAtleastOneBestFitFound;
        int currentRow;
        int currentColumn;
        do
        {
            isAtleastOneBestFitFound = false;
            // Solve each row. 
            currentRow = 0;
            while (currentRow < SizeOfSudoku)
            {
                currentColumn = 0;
                while (currentColumn < SizeOfSudoku)
                {
                    if (cellBoard[currentRow, currentColumn].CurrentValue == 0)
                    {
                        var bestFit = GetBestFit(currentRow, currentColumn);
                        if (bestFit != 0)
                        {
                            // We have a match. Update the cell with this value.
                            isAtleastOneBestFitFound = true;
                            UpdateCellValue(currentRow, currentColumn, bestFit, CellState.SolvedWithConfidence);
                        }
                    }
                    currentColumn++;
                }
                currentRow++;
            }
        } while (isAtleastOneBestFitFound);

        // Second round - Try to do best guess
        bool response = true;
        var rand = new Random().Next(1, 3);
        rand = 1; // for now only by row
        switch (rand)
        {
            case 1:
                response = SolveByRow();
                break;
            case 2:
                response = SolveByColumn();
                break;
            case 3:
                response = SolveByGrid();
                break;
        }
        if (response == false)
        {
            Console.WriteLine("Bug: Unable to solve given Sudoku");
        }

        // we should be done now. 
        // Just put final result into char array instead of cell object array
        for (int i = 0; i < SizeOfSudoku; i++)
        {
            for (int j = 0; j < SizeOfSudoku; j++)
            {
                board[i][j] = Convert.ToChar(cellBoard[i, j].CurrentValue + 48);
            }
        }
    }

    private bool SolveByGrid()
    {
        bool response = true;
        //  Solve grid by grid
        for (int currentGrid = 0; currentGrid < SizeOfSudoku; currentGrid++)
        {
            var rowStart = (int)(Math.Floor((double)(currentGrid / 3)) * 3);
            var columnStart = (currentGrid % 3) * 3;
            if (!SolveGivenSubBlock(rowStart, rowStart + 2, columnStart, columnStart + 2))
            {
                response = false;
                Console.WriteLine("Bug? Unable to solve some of the grid");
            }
        }
        return response;
    }

    private bool SolveGivenSubBlockByColumn(int rowStart, int rowEnd, int columnStart, int columnEnd)
    {
        bool response = true;
        for (int currentColumn = columnStart; currentColumn <= columnEnd; currentColumn++)
        {
            for (int currentRow = rowStart; currentRow <= rowEnd; currentRow++)
            {
                if (!SolveGivenCell(currentRow, currentColumn))
                {
                    response = false; // do not overwrite this value for next cell's success, and thus above if
                }
            }
        }
        return response;
    }
    private bool SolveGivenSubBlock(int rowStart, int rowEnd, int columnStart, int columnEnd)
    {
        bool response = true;
        for (int currentRow = rowStart; currentRow <= rowEnd; currentRow++)
        {
            for (int currentColumn = columnStart; currentColumn <= columnEnd; currentColumn++)
            {
                if (!SolveGivenCell(currentRow, currentColumn))
                {
                    response = false; // do not overwrite this value for next cell's success, and thus above if
                }
            }
        }
        return response;
    }
    private bool SolveGivenCell(int currentRow, int currentColumn)
    {
        bool response = true;
        var currentCell = cellBoard[currentRow, currentColumn];
        if (currentCell.CurrentValue == 0)
        {
            // go back tracking. 
            // This will first attempt to find best guess if not found then back tracks.
            if (!BackTrackRealThisTime(currentRow, currentColumn))
            {
                response = false;
                Console.WriteLine($"Bug? Unable to solve {currentRow} :: {currentColumn}");
            }
        }
        return response;
    }

    private bool SolveByColumn()
    {
        bool response = true;
        if (!SolveGivenSubBlockByColumn(rowStart: 0, rowEnd: SizeOfSudoku - 1, columnStart: 0, columnEnd: SizeOfSudoku - 1))
        {
            response = false;
            Console.WriteLine("Bug? Unable to solve some of the grid");
        }
        return response;
    }
    private bool SolveByRow()
    {
        bool response = true;
        if (!SolveGivenSubBlock(rowStart: 0, rowEnd: SizeOfSudoku - 1, columnStart: 0, columnEnd: SizeOfSudoku - 1))
        {
            response = false;
            Console.WriteLine("Bug? Unable to solve some of the grid");
        }
        return response;
    }
    private bool BackTrackRealThisTime(int currentRow, int currentColumn)
    {
        var stack = new Stack<CellLocation>();
        // clear not fit for this, if any from prevous attempts.
        cellBoard[currentRow, currentColumn].ClearNotFitForThis();
        stack.Push(new CellLocation() { Row = currentRow, Column = currentColumn });

        while (stack.Count != 0)
        {
            var currentTopOfStack = stack.Peek();
            var bestGuess = GetBestGuess(currentTopOfStack.Row, currentTopOfStack.Column);
            if (bestGuess != 0)
            {
                // We have a guess. Update the cell with this value.
                UpdateCellValue(currentTopOfStack.Row, currentTopOfStack.Column, bestGuess, CellState.InProgress);
                stack.Pop(); // Remove this from stack.
                continue;
            }
            var previousCellLocation = GetPreviousInProgressCellLocation(currentTopOfStack);
            // Push this cell onto stack after emptying it.;
            var previousCell = cellBoard[previousCellLocation.Row, previousCellLocation.Column];
            previousCell.NotFitForThisCell.Add(previousCell.CurrentValue);
            RemoveCellValue(previousCellLocation.Row, previousCellLocation.Column);
            // Now that we are moving further back, current cell's notfit is no longer valid.
            cellBoard[currentTopOfStack.Row, currentTopOfStack.Column].ClearNotFitForThis();
            stack.Push(previousCellLocation);
        }
        return true; // stack is empty
    }

    private CellLocation GetPreviousInProgressCellLocation(CellLocation currentCellLocation)
    {
        do
        {
            if (currentCellLocation.Row <= 0 && currentCellLocation.Column <= 0)
            {
                // No more previous cells. 
                throw new Exception("No more previous cells, you are at the beginning of cell");
            }
            var previousCellLocation = new CellLocation() { Row = currentCellLocation.Row, Column = currentCellLocation.Column - 1 };
            if (previousCellLocation.Column == -1)
            {
                previousCellLocation.Column = SizeOfSudoku - 1;
                previousCellLocation.Row = currentCellLocation.Row - 1;
            }
            if (cellBoard[previousCellLocation.Row, previousCellLocation.Column].State == CellState.InProgress)
            {
                return previousCellLocation;
            }
            currentCellLocation = previousCellLocation;
        } while (true);
    }

    private void RemoveCellValue(int row, int column)
    {
        var currentCell = cellBoard[row, column];
        var currentValue = currentCell.CurrentValue;
        currentCell.Reset();
        if (!currentCell.NotFitForThisCell.Contains(currentValue))
        {
            //currentCell.NotFitForThisCell.Add(currentValue); // Just make a note, so next time we try some other value, if possible
        }
        ResetAvailableListsFromCellBoardFor(row, column, currentValue);
    }
    private void UpdateCellValue(int row, int column, int newValue, CellState state)
    {
        if (newValue == 0)
        {
            Console.WriteLine("Bug? Updating zero value?");
        }
        cellBoard[row, column].CurrentValue = newValue;
        cellBoard[row, column].State = state;

        UpdateAvailableListsFromCellBoardFor(row, column);
    }

    // This checks if there is only one fit fot this cell. 
    // If it finds more than one fit, then we return zero.
    private int GetBestFit(int row, int column)
    {
        var gridKey = GetGridNumber(row, column);
        var availableInAllThree = rowAvailableList[row].Intersect(columnAvailableList[column]).Intersect(gridAvailableList[gridKey]);
        if (availableInAllThree.Count() == 1)
        {
            return availableInAllThree.First();
        }
        return 0;
    }

    private int GetBestGuess(int row, int column)
    {
        var gridKey = GetGridNumber(row, column);
        var availableInAllThree = rowAvailableList[row].Intersect(columnAvailableList[column]).Intersect(gridAvailableList[gridKey]);
        var notFitList = cellBoard[row, column].NotFitForThisCell;
        var availableInAllThreeExcludingNotFit = availableInAllThree.Except(notFitList);
        var count = availableInAllThreeExcludingNotFit.Count();
        if (count != 0)
        {
            var random = new Random();
            return availableInAllThreeExcludingNotFit.ElementAt(random.Next(0, count));
            // Notfit is just a guideline. We respect only if we have a choice. Otherwise no. 
        }
        return 0;
    }

    private void InitializeCellArray(char[][] board)
    {
        for (int i = 0; i < SizeOfSudoku; i++)
        {
            for (int j = 0; j < SizeOfSudoku; j++)
            {
                var cell = new Cell();
                cellBoard[i, j] = cell;
                if (board[i][j] != '.')
                {
                    UpdateCellValue(i, j, (int)Char.GetNumericValue(board[i][j]), CellState.InitialValue);
                }
            }
        }
    }

    private void InitializeAllAvailableLiistsWithEverything()
    {
        Console.WriteLine("Start : InitializeAllAvailableLiistsWithEverything");
        for (int count = 0; count < SizeOfSudoku; count++)
        {
            // All are available to begin with.
            // There will be "9" available rows, columns, grids starting with. 
            // Each of these 27 will contain all nine as available.
            rowAvailableList.Add(count, new List<int>());
            columnAvailableList.Add(count, new List<int>());
            gridAvailableList.Add(count, new List<int>());
            for (int i = 1; i <= SizeOfSudoku; i++)
            {
                rowAvailableList[count].Add(i);
                columnAvailableList[count].Add(i);
                gridAvailableList[count].Add(i);
            }
        }
        Console.WriteLine("End : InitializeAllAvailableLiistsWithEverything");
    }

    private void ResetAvailableListsFromCellBoardFor(int row, int column, int currentValue)
    {
        // A new value became available - add it back to available list
        rowAvailableList[row].Add(currentValue);
        columnAvailableList[column].Add(currentValue);
        var gridKey = GetGridNumber(row, column);
        gridAvailableList[gridKey].Add(currentValue);
    }

    private void UpdateAvailableListsFromCellBoardFor(int row, int column)
    {
        if (!rowAvailableList[row].Remove(cellBoard[row, column].CurrentValue))
        {
            Console.WriteLine($"Bug? Tried to remove a value from rowAvailableList which is not existing {row} and {column}");
        }
        if (!columnAvailableList[column].Remove(cellBoard[row, column].CurrentValue))
        {
            Console.WriteLine($"Bug? Tried to remove a value from columnAvailableList which is not existing {row} and {column}");
        }
        var gridKey = GetGridNumber(row, column);
        if (!gridAvailableList[gridKey].Remove(cellBoard[row, column].CurrentValue))
        {
            Console.WriteLine($"Bug? Tried to remove a value from gridAvailabelList which is not existing {row} and {column}");
        }
    }

    private int GetGridNumber(int row, int column)
    {
        int rowGrid = row / 3;
        int columnGrid = column / 3;

        /*
        0,0 0,1 0,2
        0.   1.  2
        1,0 1,1 1,2
        3.   4.  5
        2,0 2,1 2,2
        6.   7.  8 

        row*SizeOfSudoku/3 + column 
        */

        return rowGrid * SizeOfSudoku / 3 + columnGrid;
    }

    internal enum CellState
    {
        Empty,

        InitialValue,

        SolvedWithConfidence,

        InProgress,
    }

    internal class Cell
    {
        public CellState State { get; set; }
        public int CurrentValue { get; set; }

        // We tried this number for this cell and think this may not fit here.
        public List<int> NotFitForThisCell { get; set; }

        public Cell()
        {
            State = CellState.Empty;
            NotFitForThisCell = new List<int>();
        }

        public void Reset(bool hardReset = false)
        {
            CurrentValue = 0;
            State = CellState.Empty;
            if (hardReset)
            {
                ClearNotFitForThis();
            }
        }

        public void ClearNotFitForThis()
        {
            NotFitForThisCell.Clear();
        }
    }

    internal class CellLocation
    {
        public int Row { set; get; }
        public int Column { set; get; }
    }
}