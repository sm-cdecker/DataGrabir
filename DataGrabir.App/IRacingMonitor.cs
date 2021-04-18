using System;
using DataGrabir.App.Models;
using System.Net.Http;
using DataGrabir.App.Extensions;
using DataGrabir.App.TelemState;
using System.Collections.Generic;
using iRacingSimulator;
using iRacingSdkWrapper;

namespace DataGrabir.App
{
    class IRacingMonitor
    {
        TelemetryState state;
        HttpClient httpClient;
        DGConfig config;

        public IRacingMonitor(DGConfig config)
        {
            this.config = config;

            Sim.Instance.TelemetryUpdated += TelemetryUpdate;
            
            this.httpClient = new HttpClient();
            this.state = new TelemetryState();
            
        }

        public void Run()
        {
            Console.WriteLine("Waiting for iRacing to start...");
            Sim.Instance.Start(this.config.UpdateFreq);
        }

        private async void TelemetryUpdate(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            List<long> myTimes = new List<long>();

            var newState = Sim.Instance.ToTelemetryState(this.config, this.state);

            if (!String.IsNullOrWhiteSpace(this.config.FormUrl))
            {
                if (newState.UpdateEvent != UpdateEvents.None)
                {
                    var req = new HttpRequestMessage(HttpMethod.Post, this.config.FormUrl);
                    req.Content = newState.GetForm();
                    await this.httpClient.SendAsync(req);
                    Console.WriteLine("Posted to sheet @ {0}", DateTime.Now);
                }
            }
            this.state = newState;
            if (this.config.PostTelemetryToConsole)
            {
                this.Print(e.TelemetryInfo, string.Join(" | ", myTimes));
            }
        }

        private void Print(TelemetryInfo telemetry, string myTimes)
        {
            
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Updated @ {0}", DateTime.Now);
            Console.WriteLine("Took: {0}", myTimes);
            Console.WriteLine(this.state.GetConsoleString());
        }


    }
}
