using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridApproach
{
    class NearestNeighborTour
    {
        public static double GetLength(double[,] d, int start)
        {
            int n = d.GetLength(0);
            List<int> openedList = new List<int>();
            double result = 0;
            for (int i = 0; i < n; i++)
            {
                openedList.Add(i);
            }
            int current = start;
            for (int i = 0; i < n - 1; i++)
            {
                openedList.Remove(current);
                int indexMin = openedList[0];
                for (int j = 0; j < openedList.Count; j++)
                {
                    if (d[current, openedList[j]] < d[current, indexMin])
                        indexMin = openedList[j];
                }
                result += d[current, indexMin];
                current = indexMin;
            }
            return result;
        }
    }
}
