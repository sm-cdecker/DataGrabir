using System;
using iRacingSdkWrapper;
using DataGrabir.App.Models;
using System.Net.Http;
using DataGrabir.App.Extensions;
using System.Diagnostics;
using DataGrabir.App.TelemState;
using System.Collections.Generic;

namespace DataGrabir.App
{
    class IRacingMonitor
    {
        SdkWrapper wrapper;
        TelemetryState state;
        HttpClient httpClient;
        DGConfig config;

        public IRacingMonitor(DGConfig config)
        {
            this.config = config;
            this.wrapper = new SdkWrapper();
            this.wrapper.TelemetryUpdateFrequency = config.UpdateFreq;
            this.wrapper.TelemetryUpdated += TelemetryUpdate;
            this.httpClient = new HttpClient();
            this.state = new TelemetryState();
        }

        public void Run()
        {
            Console.WriteLine("Waiting for iRacing to start...");
            this.wrapper.Start();
        }

        private async void TelemetryUpdate(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            List<long> myTimes = new List<long>();
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();

            var newState = this.wrapper.ToTelemetryState(this.config, this.state);
            myTimes.Add(stopwatch.ElapsedMilliseconds);
            if (!String.IsNullOrWhiteSpace(this.config.FormUrl))
            {
                if (newState.UpdateEvent != UpdateEvents.None)
                {
                    var req = new HttpRequestMessage(HttpMethod.Post, this.config.FormUrl);
                    req.Content = newState.GetForm();
                    await this.httpClient.SendAsync(req);
                }
            }
            myTimes.Add(stopwatch.ElapsedMilliseconds);
            this.state = newState;
            this.Print(e.TelemetryInfo, string.Join(" | ", myTimes));
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
