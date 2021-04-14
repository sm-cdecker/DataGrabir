using System;
using System.IO;
using System.Text.Json;
using DataGrabir.App.Models;

namespace DataGrabir.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var DGConf = JsonSerializer.Deserialize<DGConfig>(
                File.ReadAllText(@"./DataGrabir.config.json"),
                new JsonSerializerOptions() { ReadCommentHandling= JsonCommentHandling.Skip }
            );
            IRacingMonitor monitor = new IRacingMonitor(DGConf);

            monitor.Run();
            Console.ReadKey();
        }

    }
}
