using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFireApp
{
    public class PrintJob : IPrintJob
    {
        public void Print()
        {
            Console.WriteLine($"Hangfirereccurring job..!");
        }
    }
}
