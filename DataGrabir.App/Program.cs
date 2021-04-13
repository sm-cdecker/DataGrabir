using System;
using iRacingSdkWrapper;

namespace DataGrabir.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<object> action = (object obj) =>
                {
                    new IRacingMonitor().Run();
                };
            IRacingMonitor monitor = new IRacingMonitor();

            monitor.Run();
            Console.ReadKey();
        }

    }
}
