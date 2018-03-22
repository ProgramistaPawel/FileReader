using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Timer
    {

        public static TimeSpan MesureExecutionTime(Action todo)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            todo();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }


    }
}
