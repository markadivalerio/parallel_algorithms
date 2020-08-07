using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project;
using project.src;

namespace project
{
    public class Test
    {
        static int[,] generateMatrix(int size)
        {
            var matrix = new int[size, size];

            var rnd = new Random();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    matrix[i, j] = rnd.Next() % (10*size);

            return matrix;
        }

        static int[,] createMatrix(int testCase)
        {
            int[,] matrix;
            switch (testCase)
            {
                case -1:
                    matrix = new int[2, 2]
                        {
                            {6,8},
                            {6,2}
                        };
                    break;
                case 0:
                    matrix = new int[10, 10]
                        {
                            {1,8,8,8,5,5,0,0,3,1},
                            {9,8,2,5,2,1,1,5,7,4},
                            {4,9,0,6,9,5,8,3,0,2},
                            {8,7,6,9,4,0,7,4,2,0},
                            {2,4,3,9,3,1,3,2,1,5},
                            {6,2,2,6,6,2,5,2,9,7},
                            {4,4,0,1,0,2,2,3,1,9},
                            {1,7,0,4,9,7,6,8,0,5},
                            {9,6,9,2,3,6,2,0,0,2},
                            {5,5,5,5,6,5,4,2,9,1}
                        };
                    break;
                case 1:
                    matrix = new int[9, 10]
                        {
                            {1,8,8,8,5,5,0,0,3,1},
                            {9,8,2,5,2,1,1,5,7,4},
                            {4,9,0,6,9,5,8,3,0,2},
                            {8,7,6,9,4,0,7,4,2,0},
                            {2,4,3,9,3,1,3,2,1,5},
                            {6,2,2,6,6,2,5,2,9,7},
                            {4,4,0,1,0,2,2,3,1,9},
                            {1,7,0,4,9,7,6,8,0,5},
                            //{9,6,9,2,3,6,2,0,0,2},
                            {5,5,5,5,6,5,4,2,9,1}
                        };
                    break;

                case 2:
                    matrix = new int[10, 9]
                        {
                            {1,8,8,8,5,5,0,0,3},
                            {9,8,2,5,2,1,1,5,7},
                            {4,9,0,6,9,5,8,3,0},
                            {8,7,6,9,4,0,7,4,2},
                            {2,4,3,9,3,1,3,2,1},
                            {6,2,2,6,6,2,5,2,9},
                            {4,4,0,1,0,2,2,3,1},
                            {1,7,0,4,9,7,6,8,0},
                            {9,6,9,2,3,6,2,0,0},
                            {5,5,5,5,6,5,4,2,9}
                        };
                    break;
                case 3:
                    matrix = new int[4, 4]
                    {
                       {25,36,16,19},
                       { 39,26,4,37 },
                       { 32,36,0,30 },
                       { 31,30,21,37 }
                    };
                    break;
                case 4:
                    matrix = new int[4, 4]
                    {
                       { 38,0,34,30},
                       { 19,34,28,4 },
                       { 7,16,12,24 },
                       { 16,3,27,35 }
                    };
                    break;
                case 9:
                    matrix = new int[9, 9]
                    {
                        {   44,1,16,72,77,88,27,54,40},
                        {   54,58,21,81,88,23,71,83,27},
                        {   32,72,29,26,7,10,7,7,42},
                        {   71,13,13,33,34,50,72,4,50},
                        {   50,50,27,37,41,62,16,68,81},
                        {   56,17,42,55,79,77,79,10,67},
                        {    4,55,84,43,12,17,83,83,41},
                        {   64,21,61,8,46,84,10,58,70},
                        {    0,32,31,57,10,29,19,41,27}
                    };
                    break;
                case 10:
                    matrix = new int[10, 10]
                    {
                        {49,52,72,82,22,54,77,77,6,52},
{    9,38,51,11,39,16,77,93,37,3},
{   60,25,71,30,50,60,69,28,44,62},
{   34,54,81,14,10,89,69,21,29,69},
{   85,90,73,18,76,41,4,87,26,10},
{   17,64,67,72,48,71,81,89,79,62},
{   32,96,80,44,65,2,57,37,48,97},
{   94,3,64,15,18,17,99,53,54,9},
{    3,82,49,81,45,70,34,56,13,86},
{    8,80,42,67,37,36,57,29,53,74}
                    };
                    break;
                case 16:
                    matrix = new int[16, 16]
                    {
                        {118,110,71,2,101,112,142,145,116,90,65,35,72,114,22,134},
{   16,22,89,159,119,58,6,72,49,40,128,15,45,86,108,35},
{  153,97,104,91,144,61,44,10,105,1,17,14,20,30,68,53},
{    5,135,88,106,88,11,158,27,71,159,113,28,111,127,67,30},
{  109,157,9,102,10,58,150,115,105,46,21,85,41,152,52,146},
{  132,42,137,70,125,2,74,142,99,44,20,145,75,16,59,138},
{   94,114,65,150,21,126,43,74,145,133,128,42,117,40,146,46},
{   74,94,23,69,117,157,65,74,83,27,130,126,38,132,7,89},
{  147,10,151,87,87,62,125,133,159,63,52,152,34,32,152,59},
{   58,101,38,141,120,117,21,97,16,118,34,121,123,44,3,140},
{   93,39,22,74,49,26,119,12,96,50,37,54,72,73,40,18},
{   35,71,68,97,94,26,96,157,149,21,38,129,155,4,10,82},
{   27,39,26,155,137,45,10,159,142,153,12,70,80,127,25,103},
{   78,16,86,52,136,66,46,67,70,25,85,155,115,117,17,92},
{    7,115,154,0,47,45,8,117,28,36,115,68,141,26,77,149},
{  156,95,51,106,78,118,106,88,153,86,52,112,120,15,11,152}
                    };
                    break;
                default:
                    matrix = generateMatrix(testCase);
                    break;
            }
            return matrix;
        }

        static int[,] InvertCosts(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    matrix[i, j] = -1 * matrix[i, j];
                }
            }
            return matrix;
        }

        static void printMatrix(int[,] matrix)
        {
            Console.WriteLine("Matrix:");
            var rsize = matrix.GetLength(0);
            var csize = matrix.GetLength(1);
            for (int i = 0; i < rsize; i++)
            {
                for (int j = 0; j < csize; j++)
                    Console.Write("{0,5:0}", matrix[i, j]);
                Console.WriteLine();
            }
        }

        static void printResults(int[,] matrix, int[] arr)
        {
            Console.WriteLine("Array:");
            var n = arr.Length;
            int total = 0;
            for (int i = 0; i < n; i++)
            {
                //if(arr[i] > matrix.GetLength(1) || i > matrix.GetLength(0))
                //{
                //    continue;
                //}
                total += matrix[i, arr[i]];
                Console.Write("{0,4:0} is assigned to ", i);
                Console.Write("{0,4:0} with cost of ", arr[i]);
                Console.Write("{0,4:0} for a total cost of ", matrix[i, arr[i]]);
                Console.Write("{0,4:0}", total);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static int Run(int size, int numTimes, Boolean timeIt)
        {

            //var matrix = generateMatrix(size);
            //var matrix = createMatrix(4);
            //var matrix = createMatrix(-1);
            var matrix = createMatrix(10);
            //var m2 = getStaticMatrix2();

            printMatrix(matrix);

            //var algorithm = new HungarianSerial(matrix);
            //var algorithm = new HungarianParallel(matrix);
            var algorithm = new HungarianParallelSEMAD(matrix);

            var result = algorithm.Run(false);

            printResults(matrix, result);

            Console.ReadKey();
            return 0;
        }

        static void Main()
        {
            Run(10, 1, false);
        }
    }

    
}