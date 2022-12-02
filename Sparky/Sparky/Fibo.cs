using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    public class Fibo
    {
        public Fibo()
        {
            Range = 5;
        }
        public int Range { get; set; }
        public List<int> GetFiboSeries()
        {
            List<int> series = new List<int>();
            int a = 0, b = 1, c = 0;
            if (Range == 1)
            {
                series.Add(0);
                return series;
            }
            series.Add(0);
            series.Add(1);
            for (int i = 2; i < Range; i++)
            {
                c = a + b;
                series.Add(c);
                a = b;
                b = c;
            }
            return series;
        }
    }
}
