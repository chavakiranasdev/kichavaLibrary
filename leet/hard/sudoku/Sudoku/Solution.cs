using System;
using System.Collections.Generic;
using System.Linq;

public class Solution
{
    // Constants 
    const int SizeOfSudoku = 9;

    // Private variables. 
    private Cell[,] cellBoard = new Cell[SizeOfSudoku, SizeOfSudoku];

    Dictionary<int, List<int>> rowAvailableList = new Dictionary<int, List<int>>();
    Dictionary<int, List<int>> columnAvailableList = new Dictionary<int, List<int>>();
    Dictionary<int, List<int>> gridAvailableList = new Dictionary<int, List<int>>();

    public void SolveSudoku(char[][] board)
    {
        Console.WriteLine("Start...");
        InitializeAllAvailableLiistsWithEverything();
        // Initialize given char array into our cell array. 
        InitializeCellArray(board);

        // First round - find all those which doesn't need guessing.
        bool isAtleastOneBestFitFound = false;
        int currentRow = 0;
        int currentColumn = 0;
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
                            UpdateCellValue(currentRow, currentColumn, bestFit, false, true, false);
                        }
                    }
                    currentColumn++;
                }
                currentRow++;
            }
        } while (isAtleastOneBestFitFound);
        // Current cell doesn't have initial value, + we couldn't find single match. 
        // Try one of the randomly available values. (note: randomly - not in sequence or not in first) 
        // Thinking lound here .... 
        // Select one of available values - 
        // Mark it as Inprogress. 
        // Move to next - 
        // If we can not find a match for a cell - 
        // Move back to Previous InProgress cell -  (for now move back to starting)
        // repeat the same until whole row is filled. 

        // Second round - Try to do best guess
        currentRow = 0;
        while (currentRow < SizeOfSudoku)
        {
            currentColumn = 0;
            while (currentColumn < SizeOfSudoku)
            {
                var currentCell = cellBoard[currentRow, currentColumn];
                if (currentCell.CurrentValue != 0)
                {
                    if (currentCell.IsInitialValue || currentCell.IsSolvedWithConfidence)
                    {
                        currentRow++;
                        currentColumn++;
                        continue; // we already have a value, move to next.
                    }
                    else
                    {
                        // We are backtracking .... remove this value
                        RemoveCellValue(currentRow, currentColumn);
                    }
                }
                // we will still try for best fit, but don't set the value as IsCompletedWithConfidence - 
                // Reason - some of our previous values may not be best fit, thus this can not be best fit, if the other guess is wrong.
                var bestGuess = GetBestGuess(currentRow, currentColumn);
                if (bestGuess != 0)
                {
                    // We have a guess. Update the cell with this value.
                    UpdateCellValue(currentRow, currentColumn, bestGuess, false, true, false);
                }
                else 
                {
                    // We don't have a match - back track
                    // How far we go? 
                    // We could go one step at a time - 
                    // Or we could go to the beginning of current row to start with - we could improve in phase two. 
                    currentColumn = -1; // next increment will bring it to zero
                }
                currentColumn++;
            }
            currentRow++;
        }

        // we should be done now. 
        // Just put final result into char array instead of cell object array
        for (int i = 0; i < SizeOfSudoku; i++)
        {
            for (int j = 0; j < SizeOfSudoku; j++)
            {
                board[i][j] = (char)cellBoard[i, j].CurrentValue;
            }
        }
    }

    private void RemoveCellValue(int row, int column)
    {
        var currentCell = cellBoard[row, column];
        var currentValue = currentCell.CurrentValue;
        currentCell.Reset();

        ResetAvailableListsFromCellBoardFor(row, column, currentValue);
    }
    private void UpdateCellValue(int row, int column, int newValue, bool isInitialValue, bool isSolvedWithConfidence, bool isInProgress)
    {
        cellBoard[row, column].CurrentValue = newValue;
        cellBoard[row, column].IsSolvedWithConfidence = isSolvedWithConfidence;
        cellBoard[row, column].IsInitialValue = isInitialValue;
        cellBoard[row, column].IsInProgress = isInProgress;

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
        var count = availableInAllThree.Count();
        if (count != 0)
        {
            var random = new Random();
            // return a random available value
            return availableInAllThree.ElementAt(random.Next(0, count));
        }
        // Didn't find anything avaiable in all three lists? some of previous guess is wrong.
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
                    UpdateCellValue(i, j, (int)Char.GetNumericValue(board[i][j]), true, false, false);
                }
            }
        }
    }

    private void InitializeAllAvailableLiistsWithEverything()
    {
        Console.WriteLine("Start : InitializeAllAvailableLiistsWithEverything");
        for (int count = 0; count < SizeOfSudoku; count++)
        {
            // all are available to begin with.
            rowAvailableList.Add(count, new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            columnAvailableList.Add(count, new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            gridAvailableList.Add(count, new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
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
            Console.WriteLine($"Tried to remove a value from rowAvailableList which is not existing {row} and {column}");
        }
        if (!columnAvailableList[column].Remove(cellBoard[row, column].CurrentValue))
        {
            Console.WriteLine($"Tried to remove a value from columnAvailableList which is not existing {row} and {column}");
        }
        var gridKey = GetGridNumber(row, column);
        if (!gridAvailableList[gridKey].Remove(cellBoard[row, column].CurrentValue))
        {
            Console.WriteLine($"Tried to remove a value from gridAvailabelList which is not existing {row} and {column}");
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
    internal class Cell
    {
        public int CurrentValue { get; set; }
        // all next bool values can go into enum TODO:
        public bool IsInitialValue { get; set; }
        public bool IsSolvedWithConfidence { get; set; }
        public bool IsInProgress { get; set; }
        public List<int> NotFitForThisCell { get; set; } // We tried this number for this cell and think this may not fit here.

        public Cell()
        {
            NotFitForThisCell = new List<int>();
        }

        public void Reset(bool hardReset = false)
        {
            CurrentValue = 0;
            IsInitialValue = false;
            IsSolvedWithConfidence = false;
            IsInProgress = false;
            if (hardReset)
            {
                NotFitForThisCell.Clear();
            }
        }
    }
}