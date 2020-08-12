using System.Collections.Generic;

namespace project.src
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class HungarianAlgorithmSerial
    {

        private int[] arrVertexMatchX;  
        private int[] arrVertexMatchY; 
        private int MaximumMatch;
        private int[] _slack;
        private int[] _slackx;
        private int[] PreviousPath; 
        private readonly int[,] MatrixTable;
        private int MaxInt;
        private int NumElements;  
        private int[] WorkersLabel;  
        private int[] JobsLabel; 
        private bool[] _s;
        private bool[] _t;

        public HungarianAlgorithmSerial(int[,] costMatrix)
        {
            MatrixTable = costMatrix;
        }

        private void UpdateWorkerSlackAndJobLabels()
        {
            var delta = MaxInt;
            for (var i = 0; i < NumElements; i++)
                if (!_t[i])
                    if (delta > _slack[i])
                        delta = _slack[i];
            for (var i = 0; i < NumElements; i++)
            {
                if (_s[i])
                    WorkersLabel[i] = WorkersLabel[i] + delta;
                if (_t[i])
                    JobsLabel[i] = JobsLabel[i] - delta;
                else _slack[i] = _slack[i] - delta;
            }
        }
        //
        private void populateSlackArray(int x, int prevx)
        {
            _s[x] = true;
            PreviousPath[x] = prevx;

            var lxx = WorkersLabel[x];
            for (var y = 0; y < NumElements; y++)
            {
                if (MatrixTable[x, y] - lxx - JobsLabel[y] >= _slack[y]) continue;
                _slack[y] = MatrixTable[x, y] - lxx - JobsLabel[y];
                _slackx[y] = x;
            }
        }
        //
        private void FillMatchArray()
        {
            for (var x = 0; x < NumElements; x++)
            {
                for (var y = 0; y < NumElements; y++)
                {
                    if (MatrixTable[x, y] != WorkersLabel[x] + JobsLabel[y] || arrVertexMatchY[y] != -1) continue;
                    arrVertexMatchX[x] = y;
                    arrVertexMatchY[y] = x;
                    MaximumMatch++;
                    break;
                }
            }
        }
        //This will assign default VertexMatch to -1
        private void AssignDefaultValues()
        {
            for (var i = 0; i < NumElements; i++)
            {
                arrVertexMatchX[i] = -1;
                arrVertexMatchY[i] = -1;
            }
        }

        //Thsui will initialise S and T arrray
        private void InitSt()
        {
            for (var i = 0; i < NumElements; i++)
            {
                _s[i] = false;
                _t[i] = false;
            }
        }
        //
        private void InitializeLabel()
        {
            for (var i = 0; i < NumElements; i++)
            {
                var minRow = MatrixTable[i, 0];
                for (var j = 0; j < NumElements; j++)
                {
                    if (MatrixTable[i, j] < minRow) minRow = MatrixTable[i, j];
                    if (minRow == 0) break;
                }
                WorkersLabel[i] = minRow;
            }
            for (var j = 0; j < NumElements; j++)
            {
                var minColumn = MatrixTable[0, j] - WorkersLabel[0];
                for (var i = 0; i < NumElements; i++)
                {
                    if (MatrixTable[i, j] - WorkersLabel[i] < minColumn) minColumn = MatrixTable[i, j] - WorkersLabel[i];
                    if (minColumn == 0) break;
                }
                JobsLabel[j] = minColumn;
            }
        }
        public int[] Run()
        {
            NumElements = MatrixTable.GetLength(0);

            WorkersLabel = new int[NumElements];
            JobsLabel = new int[NumElements];
            _s = new bool[NumElements];
            _t = new bool[NumElements];
            arrVertexMatchX = new int[NumElements];
            arrVertexMatchY = new int[NumElements];
            _slack = new int[NumElements];
            _slackx = new int[NumElements];
            PreviousPath = new int[NumElements];
            MaxInt = int.MaxValue;


            AssignDefaultValues();

            if (NumElements != MatrixTable.GetLength(1))
                return null;

            InitializeLabel();

            MaximumMatch = 0;

            FillMatchArray();

             var queue = new Queue<int>();



            while (MaximumMatch != NumElements)
            {
                queue.Clear();

                InitSt();
               //parameters for keeping the position of root node and two other nodes
                var root = 0;
                int x;
                var y = 0;

                //find root of the tree
                for (x = 0; x < NumElements; x++)
                {
                    if (arrVertexMatchX[x] != -1) continue;
                    queue.Enqueue(x);
                    root = x;
                    PreviousPath[x] = -2;

                    _s[x] = true;
                    break;
                }

                for (var i = 0; i < NumElements; i++)
                {
                    _slack[i] = MatrixTable[root, i] - WorkersLabel[root] - JobsLabel[i];
                    _slackx[i] = root;
                }

                //Find label path
                while (true)
                {
                    while (queue.Count != 0)
                    {
                        x = queue.Dequeue();
                        var lxx = WorkersLabel[x];
                        for (y = 0; y < NumElements; y++)
                        {
                            if (MatrixTable[x, y] != lxx + JobsLabel[y] || _t[y]) continue;
                            if (arrVertexMatchY[y] == -1) break;  
                            _t[y] = true;
                            queue.Enqueue(arrVertexMatchY[y]);

                            populateSlackArray(arrVertexMatchY[y], x);
                        }
                        if (y < NumElements) break;  
                    }
                    if (y < NumElements) break; 
                    UpdateWorkerSlackAndJobLabels();  

                    for (y = 0; y < NumElements; y++)
                    {
                       if (_t[y] || _slack[y] != 0) continue;
                        if (arrVertexMatchY[y] == -1) 
                        {
                            x = _slackx[y];
                            break;
                        }
                        _t[y] = true;
                        if (_s[arrVertexMatchY[y]]) continue;
                        queue.Enqueue(arrVertexMatchY[y]);
                        populateSlackArray(arrVertexMatchY[y], _slackx[y]);
                    }
                    if (y < NumElements) break;
                }

                MaximumMatch++;

                //Reverse Edges
                int ty;
                for (int cx = x, cy = y; cx != -2; cx = PreviousPath[cx], cy = ty)
                {
                    ty = arrVertexMatchX[cx];
                    arrVertexMatchY[cy] = cx;
                    arrVertexMatchX[cx] = cy;
                }
            }


            return arrVertexMatchX;
        }
    }
}
