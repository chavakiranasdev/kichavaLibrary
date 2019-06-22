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

    private bool SolveGivenSubBlock(int rowStart, int rowEnd, int columnStart, int columnEnd)
    {
        bool response = true;
        for (int currentRow = rowStart; currentRow <= rowEnd; currentRow++)
        {
            for (int currentColumn = columnStart; currentColumn <= columnEnd; currentColumn++)
            {
                var currentCell = cellBoard[currentRow, currentColumn];
                if (currentCell.CurrentValue == 0)
                {
                    var bestGuess = GetBestGuess(currentRow, currentColumn);
                    if (bestGuess != 0)
                    {
                        // We have a guess. Update the cell with this value.
                        UpdateCellValue(currentRow, currentColumn, bestGuess, false, false, true);
                    }
                    else
                    {
                        // go back tracking.
                        if (!BackTrackRealThisTime(currentRow, currentColumn))
                        {
                            response = false;
                            Console.WriteLine($"Bug? Unable to solve {currentRow} :: {currentColumn}");
                        }
                    }
                }
            }
        }
        return response;
    }

    private bool SolveByColumn()
    {
        // Cheating until we implement this 
        return SolveByRow();
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
        // Is there a match readily available? Before we try deleting other cells, let us try as is with current setting.
        var preBackTrackBestGuess = GetBestGuess(currentRow, currentColumn);
        if (preBackTrackBestGuess != 0)
        {
            // We have a guess. Update the cell with this value. No need to back track any further
            UpdateCellValue(currentRow, currentColumn, preBackTrackBestGuess, false, false, true);
            return true;
        }

        int startColumnInCurrentRow = currentColumn; // first row, we start with given column, then onwards we always start at end
        for (int tempRow = currentRow; tempRow >= 0; tempRow--)
        {
            for (int tempColumn = startColumnInCurrentRow; tempColumn >= 0; tempColumn--)
            {
                var tempCell = cellBoard[tempRow, tempColumn];
                if (tempCell.IsInProgress && tempCell.CurrentValue != 0)
                {
                    var tempValue = tempCell.CurrentValue;
                    RemoveCellValue(tempRow, tempColumn);
                    // Now that a value is removed, we might be lucky to find a best match for our current cell
                    // lazy to write another function to check whether tempValue fits here. reusing existing logic, mostly same performance though
                    var currentBestGuess = GetBestGuess(currentRow, currentColumn);
                    if (currentBestGuess != 0)
                    {
                        // We have a guess. Update the cell with this value.
                        UpdateCellValue(currentRow, currentColumn, currentBestGuess, false, false, true);
                        // Now back trak starting with removed cell again. If return value is true, we can keep it otherwise revert it
                        if (BackTrackRealThisTime(tempRow, tempColumn))
                        {
                            return true; // found perfect match up to this point.
                        }
                        else
                        {
                            // Give up claim. 
                            RemoveCellValue(currentRow, currentColumn);
                            UpdateCellValue(tempRow, tempColumn, tempValue, false, false, true);
                            // Search continues .... 
                        }
                    }
                    else
                    {
                        // Replace the value we removed just now
                        UpdateCellValue(tempRow, tempColumn, tempValue, false, false, true);
                    }
                }
            }
            // Except given row, previous rows starts at the end
            startColumnInCurrentRow = SizeOfSudoku - 1;
        }
        return false;
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
    private void UpdateCellValue(int row, int column, int newValue, bool isInitialValue, bool isSolvedWithConfidence, bool isInProgress)
    {
        if (newValue == 0)
        {
            Console.WriteLine("Bug? Updating zero value?");
        }
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
        //var notFitList = cellBoard[row, column].NotFitForThisCell;
        //var availableInAllThreeExcludingNotFit = availableInAllThree.Except(notFitList);
        //var count = availableInAllThreeExcludingNotFit.Count();

        //if (count != 0)
        //{
        //var random = new Random();
        //return availableInAllThreeExcludingNotFit.ElementAt(random.Next(0, count));
        // Notfit is just a guideline. We respect only if we have a choice. Otherwise no. 
        //}
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