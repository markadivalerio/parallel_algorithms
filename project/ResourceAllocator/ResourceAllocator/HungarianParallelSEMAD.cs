using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Data.Common;
using System.Collections;
using System.Runtime.Serialization;
using System.Drawing;
using System.Threading;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Security;

namespace ResourceAllocator
{
    class HungarianParallelSEMAD
    {
        //private static const int HORIZONTAL = -1;
        //private static const int VERTICAL = 1;
        public Dictionary<string, int[,]> stats;
        public int[,] originalCosts;
        private int[][] costs;
        private int size;
        private int[][] lines;
        private int numberOfLines;
        private int[] occupiedCols;
        private int[] finalResult;

        public HungarianParallelSEMAD(int[,] costMatrix)
        {
            originalCosts = costMatrix;
            costs = Normalize();
            size = costs.GetLength(0);
            occupiedCols = new int[size];
            finalResult = new int[size];

            ////Console.WriteLine("\nCost Matrix after Normalizing");
            print(costs);
        }

        public int[] FindMax()
        {
            return Run(true);
        }

        public int[] FindMin()
        {
            return Run(false);
        }

        public int[] Run(bool findMax = false)
        {
            int[] results = new int[size];
            if (findMax)
                InvertCosts();

            //Step 1) Row operation to subtract the minimum value from each element in the row(parallel each row)
            SubRowsMin();

          //  //Console.WriteLine("After Step 1 (SubRowsMin):");
            print(costs);

            //Step 2) if can’t be solved from here(not every row has an 0 in independent columns), then perform a column operation to subtract the minimum value from each element in the column(parallel each column operation)
            SubColsMin();
            //Console.WriteLine("After Step 2 (SubColsMin):");
            print(costs);

            int[][] copy_costs = new int[size][];
            Parallel.For(0, size, r =>
            {
                copy_costs[r] = new int[size];
                Parallel.For(0, size, c =>
                {
                    copy_costs[r][c] = costs[r][c];
                });
            });

            //Step 3) assign 0’s to rows of independent / different columns, cover the most # of 0’s as possible. (Serial?)
            CoverZeros(false);

            while (true)
            {
                ReduceUncovered();
                CoverZeros(false);
                //print(costs);
                //Console.WriteLine(numberOfLines);
                if (numberOfLines >= size) // why does >= not work? nor does it work if the check is within the while loop.
                {
                    break;
                }
            }

            // step 5) somehow intelligently decide which zeros/minimums to result in smallest total cost.
            //Console.WriteLine();
            //Console.WriteLine("DONE WITH STEP 4");
            print(costs);

            bool result = determineSolution();
            if (result == false)
            {
                //Console.WriteLine("\n\n\nDID NOT FIND GOOD RESULT - TRYING FLIPPED\n\n\n");
                costs = copy_costs;
                occupiedCols = new int[size];
                finalResult = new int[size];

                CoverZeros(true);
                while (true)
                {
                    ReduceUncovered();
                    CoverZeros(true);
                    //print(costs);
                    //Console.WriteLine(numberOfLines);
                    if (numberOfLines >= size) // why does >= not work? nor does it work if the check is within the while loop.
                    {
                        break;
                    }
                }

                //int total_covered = 0;
                //while(total_covered < size)
                //{
                //    CoverZerosClass czc = new CoverZerosClass(costs);
                //    ConcurrentBag<int> covered_rows = czc.get_covered_rows();
                //    ConcurrentBag<int> covered_cols = czc.get_covered_columns();
                //    total_covered = covered_rows.Count + covered_cols.Count;
                //    if(total_covered < size)
                //    {

                //    }
                //}

                result = determineSolution();
                if (result == false)
                {
                    //Console.WriteLine("STILL false...uh oh");
                }
            }

            return finalResult;
        }

        private int[][] Normalize()
        {
            // adds columns/rows so that it's a perfect square matrix. 
            // Also converts into multidimensional array [,] into jagged array [][]
            // (easier to handle).

            int numRows = originalCosts.GetLength(0);
            int numCols = originalCosts.GetLength(1);
            int diff = numRows - numCols;
            //Console.WriteLine("Original Size: " + numRows + " x " + numCols);
            int n = Math.Max(numRows, numCols);
            int[][] norm = new int[n][];
            int[] maximums = new int[n];
            Parallel.For(0, n, x =>
            {
                norm[x] = new int[n];
                if (diff > 0)
                    maximums[x] = Enumerable.Range(0, numCols).Max(i => originalCosts[x, i]);
                else
                    maximums[x] = Enumerable.Range(0, numRows).Max(i => originalCosts[i, x]);
            });

            Parallel.For(0, n, r =>
            {
                Parallel.For(0, n, c =>
                {
                    if (r < numRows && c < numCols)
                        norm[r][c] = originalCosts[r, c];
                    else if (diff > 0)
                        norm[r][c] = maximums[r] + 1;
                    else if (diff < 0)
                        norm[r][c] = maximums[c] + 1;
                });
            });

            return norm;
        }

        private void InvertCosts()
        {
            Parallel.For(0, size, r =>
            {
                Parallel.For(0, size, c =>
                {
                    costs[r][c] = -1 * costs[r][c];
                });
            });
        }

        private int[] getRowsMins()
        {
            int[] mins = new int[size];
            Parallel.For(0, size, i =>
            {
                mins[i] = costs[i].AsParallel().Min();
            });
            return mins;
        }

        private int[] getColsMins()
        {
            int[] mins = new int[size];

            Parallel.For(0, size, c =>
            {
                int[] colAsRow = new int[size];
                Parallel.For(0, size, r =>
                {
                    colAsRow[r] = costs[r][c];
                });
                mins[c] = colAsRow.AsParallel().Min();
            });
            return mins;
        }

        private void SubRowsMin()
        {
            int[] mins = getRowsMins();
            Parallel.For(0, size, r =>
            {
                Parallel.For(0, size, c =>
                {
                    costs[r][c] -= mins[r];
                });
            });
        }

        private void SubColsMin()
        {
            int[] mins = getColsMins();
            Parallel.For(0, size, r =>
            {
                Parallel.For(0, size, c =>
                {
                    costs[r][c] -= mins[c];
                });
            });
        }

        private void CoverZeros(bool flipIt)
        {
            int nLines = 0;
            int[][] linesArr = new int[size][];

            Parallel.For(0, size, r =>
            {
                linesArr[r] = new int[size];
            });

            CoverZeros(nLines, linesArr, 0, flipIt);
        }

        private void CoverZeros(int nLines, int[][] linesArr, int loopCount, bool flipIt)
        {
            numberOfLines = nLines;
            lines = linesArr;

            bool skippedTiedZeros = false;
            int tieBreakerLineDir = (loopCount < size) ? 0 : (loopCount % 2 == 0) ? -1 : 1; //alternate between horizontal lines and virtical lines
            if (flipIt)
            {
                tieBreakerLineDir = -1 * tieBreakerLineDir;
            }
            for (int r = 0; r < size; r++) // SERIALFOR
            {
                for (int c = 0; c < size; c++) // SERIALFOR
                {
                    if (costs[r][c] == 0 && lines[r][c] == 0)
                    {
                        skippedTiedZeros |= drawLine(r, c, tieBreakerLineDir);
                    }
                }
            }

            if (skippedTiedZeros)
            {
                loopCount += 1;
                //Console.WriteLine("Looping " + loopCount);
                CoverZeros(numberOfLines, lines, loopCount, flipIt);
            }
        }

        // max of vertical vs horizontal at index row col
        public int determineLineDirection(int r, int c)
        {
            int zCounter = 0;

            Parallel.For(0, size, i =>
            {
                if (costs[i][c] == 0 && lines[i][c] == 0)
                    Interlocked.Increment(ref zCounter);
                if (costs[r][i] == 0 && lines[r][i] == 0)
                    Interlocked.Decrement(ref zCounter);
            });

            // positive for vertical, negative for horizontal
            return zCounter;
        }

        // returns true if determineLineDirection resulted in a tie
        public bool drawLine(int r, int c, int tieBreakerLineDir)
        {
            int lineDir = determineLineDirection(r, c);
            if (lines[r][c] == 2                        // if cell is covered twice before (2 = intersection cell), don't process it again
                || (lineDir > 0 && lines[r][c] == 1)    // if cell is covered vertically and needs to be recovered vertically, don't cover it again 
                || (lineDir < 0 && lines[r][c] == -1))  // if cell is covered horizontally and needs to be recolored horizontally, don't cover it again
                return false; // skip incrementing numberOfLines
            if (lineDir == 0 && tieBreakerLineDir == 0)
            {
                return true;
            }
            //Console.WriteLine(r + "," + c + " = " + costs[r][c] + " : " + (lineDir > 0 ? "vertical" : "horizontal"));

            // look at neighbors.
            Parallel.For(0, size, i =>
            {
                if (lineDir > 0 || (lineDir == 0 && tieBreakerLineDir > 0))  // if positive, line is vertical (1) - if already horizontal line, mark as intersection (2)
                    lines[i][c] = lines[i][c] == -1 || lines[i][c] == 2 ? 2 : 1;
                else if (lineDir < 0 || (lineDir == 0 && tieBreakerLineDir < 0))// if negative, line is horizontally (-1) - if already vertical line, mark as intersection (2)
                    lines[r][i] = lines[r][i] == 1 || lines[r][i] == 2 ? 2 : -1;
            });

            Interlocked.Increment(ref numberOfLines);
            return false;
        }
        static readonly object _object = new object();
        public void ReduceUncovered()
        {
            print(costs);
            print(lines);
            ConcurrentBag<Cell> uncoveredCells = new ConcurrentBag<Cell>();
            Parallel.For(0, size * size, i =>
            {
                int r = (int)i / size;
                int c = i % size;
                if (lines[r][c] == 0)
                {
                    uncoveredCells.Add(new Cell(r, c, costs[r][c]));
                }
            });
            if(uncoveredCells.Count()==0)
            {
                return;
            }
            Cell minCell = uncoveredCells.AsParallel().Aggregate((cell1, cell2) => cell1.val < cell2.val ? cell1 : cell2);
            ////Console.WriteLine("Minimum Uncovered Cell is (" + minCell.row + "," + minCell.col + ") = " + minCell.val);

            Parallel.For(0, size, r =>
            {
                Parallel.For(0, size, c =>
                {
                    if (lines[r][c] == 0)
                    {
                        costs[r][c] -= minCell.val;
                    }
                    else if (lines[r][c] > 1)//intersection lines
                    {
                        costs[r][c] += minCell.val;
                    }
                });
            });

            //print(costs);
        }


        /**
 * Optimization, assign every row a cell in a unique column. Since a row can contain more than one zero,
 * we need to make sure that all rows are assigned one cell from one unique column. To do this, use brute force
 * @param row
 * @param boolean If all rows were assigned a cell from a unique column, return true (at the end, guarantee true)
 * @return true
 * */

        public bool determineSolution()
        {
            return determineSolution(0);
        }

        private bool determineSolution(int row)
        {
            if (row == size) // If all rows were assigned a cell
                return true;

            for (int col = 0; col < size; col++) // SERIALFOR
            { // Try all columns
                //if(row >= 8)
                //{
                //    //Console.WriteLine("Hello");
                //}
                if (costs[row][col] == 0 && occupiedCols[col] == 0)
                { // If the current cell at column `col` has a value of zero, and the column is not reserved by a previous row
                    finalResult[row] = col; // Assign the current row the current column cell
                    occupiedCols[col] = 1; // Mark the column as reserved
                    if (determineSolution(row + 1)) // If the next rows were assigned successfully a cell from a unique column, return true
                        return true;
                    occupiedCols[col] = 0; // If the next rows were not able to get a cell, go back and try for the previous rows another cell from another column
                }
            }
            return false; // If no cell were assigned for the current row, return false to go back one row to try to assign to it another cell from another column
        }


        class CoverZerosClass
        {
            int[][] zCosts;
            int zSize;
            bool[][] choices;
            ConcurrentBag<Cell> zeroCells;
            ConcurrentBag<int> marked_rows;
            ConcurrentBag<int> marked_cols;

            ConcurrentBag<int> covered_rows;
            ConcurrentBag<int> covered_columns;


            public CoverZerosClass(int[][] matrix)
            {
                zCosts = matrix;
                zSize = matrix.Length;
                choices = new bool[zSize][];
                zeroCells = new ConcurrentBag<Cell>();
                marked_rows = new ConcurrentBag<int>();
                marked_cols = new ConcurrentBag<int>();

                Parallel.For(0, zSize, r =>
                {
                    choices[r] = new bool[zSize];
                    Parallel.For(0, zSize, c =>
                    {
                        if (zCosts[r][c] == 0)
                        {
                            zeroCells.Add(new Cell(r, c, zCosts[r][c]));
                        }
                    });
                });

                calculate();

                covered_columns = marked_cols;
                covered_rows = new ConcurrentBag<int>(); //unmarked rows
                for (int r = 0; r < zSize; r++)
                {
                    if (!marked_rows.Contains(r))
                    {
                        covered_rows.Add(r);
                    }
                }
            }

            public ConcurrentBag<int> get_covered_rows()
            {
                return covered_rows;
            }

            public ConcurrentBag<int> get_covered_columns()
            {
                return covered_columns;
            }

            public void calculate()
            {
                while (true)
                {
                    marked_rows = new ConcurrentBag<int>();
                    marked_cols = new ConcurrentBag<int>();


                    for (int r = 0; r < zSize; r++)
                    {
                        if (choices[r].Any())
                        {
                            marked_rows.Add(r);
                        }
                    }

                    if (marked_rows.Count() == 0)
                        return;

                    int num_newly_marked_columns = mark_new_columns_with_zeros_in_marked_rows();

                    if (num_newly_marked_columns == 0)
                        return;

                    // While there is some choice in every marked column
                    while (choice_in_all_marked_columns())
                    {
                        int num_newly_marked_rows = mark_new_rows_with_choices_in_marked_columns();

                        if (num_newly_marked_rows == 0)
                            return;

                        num_newly_marked_columns = mark_new_columns_with_zeros_in_marked_rows();

                        if (num_newly_marked_columns == 0)
                            return;

                        int choice_column_idx = find_marked_column_without_choice();

                        while (choice_column_idx != -1)
                        {
                            int choice_row_index = find_row_without_choice(choice_column_idx);

                            int new_choice_column_index = -1;

                            if (choice_row_index == -1)
                            {
                                (choice_row_index, new_choice_column_index) = find_best_choice_row_and_new_column(choice_column_idx);

                                choices[choice_row_index][new_choice_column_index] = false;
                            }

                            choices[choice_row_index][choice_column_idx] = true;

                            choice_column_idx = new_choice_column_index;
                        }
                    }
                }
            }

            public int mark_new_columns_with_zeros_in_marked_rows()
            {
                int num_newly_marked_columns = 0;
                bool[] unmarkedColumnsWithZeros = new bool[zSize];

                for (int r = 0; r < zSize; r++)
                {
                    for (int c = 0; c < zSize; c++)
                    {
                        if (zCosts[r][c] == 0
                            && !marked_cols.Contains(c)
                            && marked_rows.Contains(r))
                        {
                            marked_cols.Add(c);
                            unmarkedColumnsWithZeros[c] |= true;
                        }
                    }
                }

                for (int c = 0; c < zSize; c++)
                {
                    if (unmarkedColumnsWithZeros[c] == true)
                        num_newly_marked_columns++;
                }

                return num_newly_marked_columns;
            }

            public int mark_new_rows_with_choices_in_marked_columns()
            {
                int num_newly_marked_rows = 0;

                for (int r = 0; r < zSize; r++)
                {

                    for (int c = 0; c < zSize; c++)
                    {
                        if (choices[r].Contains(true)
                            && !marked_rows.Contains(r)
                            && marked_cols.Contains(c))
                        {
                            marked_rows.Add(r);
                            num_newly_marked_rows++;
                        }
                    }
                }

                return num_newly_marked_rows;
            }


            public bool choice_in_all_marked_columns()
            {
                foreach (int c in marked_cols)
                {
                    for (int r = 0; r < zSize; r++)
                    {
                        if (choices[r][c] == false)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }



            public int find_marked_column_without_choice()
            {
                foreach (int c in marked_cols)
                {
                    bool has_choice = false;
                    for (int r = 0; r < zSize; r++)
                    {
                        if (choices[r][c])
                            has_choice |= true;
                    }
                    if (!has_choice)
                        return c;
                }
                throw new Exception("Could not find a column without a choice. Failed to cover matrix zeros. Algorithm has failed.");
            }


            public int find_row_without_choice(int choice_column_idx)
            {
                ConcurrentBag<int> rowsWithZeros = new ConcurrentBag<int>();
                for (int r = 0; r < zSize; r++)
                {
                    if (zCosts[r].Contains(0))
                    {
                        rowsWithZeros.Add(r);
                    }
                }
                for (int c = 0; c < zSize; c++)
                {
                    foreach (int r in rowsWithZeros)
                    {
                        if (!choices[r].Contains(true))
                        {
                            return r;
                        }
                    }
                }
                return -1;
            }


            public (int, int) find_best_choice_row_and_new_column(int choice_column_index)
            {
                ConcurrentBag<int> rowsWithZeros = new ConcurrentBag<int>();

                for (int i = 0; i < zSize; i++)
                {
                    if (zCosts[i][choice_column_index] == 0)
                    {
                        rowsWithZeros.Add(i);
                    }
                }

                foreach (int r in rowsWithZeros)
                {
                    int first_col_has_choice = -1;
                    for (int c = 0; c < zSize; c++)
                    {
                        if (choices[r][c])
                        {
                            first_col_has_choice = c;
                            break;
                        }
                    }
                    if (find_row_without_choice(first_col_has_choice) != -1)
                    {
                        return (r, first_col_has_choice);
                    }
                }

                Random rand = new Random();
                int r2 = rand.Next(0, rowsWithZeros.Count);
                ConcurrentBag<int> colsWithChoices = new ConcurrentBag<int>();
                for (int c = 0; c < zSize; c++)
                {
                    if (choices[r2][c])
                    {
                        colsWithChoices.Add(c);
                    }
                }
                int c2 = rand.Next(0, colsWithChoices.Count);
                return (r2, c2);
            }
        }



        public struct Cell
        {
            public int row; // aka person/item
            public int col; // aka assignment/task
            public int val; // aka cost

            public Cell(int row, int col, int val)
            {
                this.row = row;
                this.col = col;
                this.val = val;
            }

            public override string ToString()
            {
                return string.Format("({0}, {1} = {2})", row, col, val);
            }
        }

        public void print(int[][] matrix)
        {
            //var n = matrix.GetLength(0);
            //////Console.WriteLine("Matrix (" + n + "x" + n + ")");
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < n; j++)
            //        //Console.Write("{0,5:0}", matrix[i][j]);
            //    //Console.WriteLine();
            //}
            ////Console.WriteLine();
        }

        public void print(int[] arr)
        {
            //var n = arr.GetLength(0);
            ////Console.WriteLine("Array (Length " + n + "):");
            //for (int i = 0; i < n; i++)
            //{
            //    //Console.Write("{0,5:0}", arr[i]);
            //}
            ////Console.WriteLine();
        }

        public void print(List<Cell> list)
        {
            //var n = list.Count;
            ////Console.WriteLine("List (Count " + n + "):");
            //for (int i = 0; i < n; i++)
            //{
            //    //Console.Write("{0,5:0}", list[i]);
            //}
            ////Console.WriteLine();
        }
    }
}
