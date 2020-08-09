using System;
using System.IO;

namespace CompareHungarianAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter Max limit:");
            int Limit = int.Parse(Console.ReadLine().Trim());
            string strFileName = "Comparison_" + DateTime.Now.ToString("HHmmss") + ".txt";
            for (int i = 1; i < Limit; i++)
            {
                Console.WriteLine("Starting "+i);
                var arrMatrixTaskResource = new int[i, i];
                for (int j = 0; i < i; j++)
                {
                    for (int k = 0; k < i; k++)
                    {
                        Random rnd = new Random();
                        arrMatrixTaskResource[j, k] = rnd.Next(1, 1000);
                    }
                }


                DateTime dateParallelStart = DateTime.Now;
                var algorithm = new HungarianParallelSEMAD(arrMatrixTaskResource);//, UpdateLabel);            
                var result = algorithm.Run();
                DateTime dateParallelEnd = DateTime.Now;
                TimeSpan spanParalel = dateParallelEnd - dateParallelStart;

                DateTime dateSerialStart = DateTime.Now;
                var algorithmSerial = new clsHungarianAlgorithm(arrMatrixTaskResource);//,UpdateLabel);
                var resultSerial = algorithmSerial.Run();
                DateTime dateSerialEnd = DateTime.Now;
                TimeSpan spanSerial = dateSerialEnd - dateSerialStart;

                File.AppendAllText(strFileName, i + "," + spanParalel.TotalMilliseconds + "," + spanSerial.TotalMilliseconds + "\r\n");
            }
        }
    }
}
