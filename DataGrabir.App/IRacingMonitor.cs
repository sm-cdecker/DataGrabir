using System;
using iRacingSdkWrapper;
using DataGrabir.App.Models;
using System.Net.Http;
using DataGrabir.App.Extensions;
using System.Diagnostics;
using DataGrabir.App.TelemState;

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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var newState = this.wrapper.ToTelemetryState(this.config);
            if (!String.IsNullOrWhiteSpace(this.config.FormUrl))
            {
                if (newState.IsConnected && newState.IsRunning && (newState.OnPitRoad != this.state.OnPitRoad))
                {
                    var req = new HttpRequestMessage(HttpMethod.Post, this.config.FormUrl);
                    req.Content = newState.GetForm();
                    await this.httpClient.SendAsync(req);
                }
            }
            this.state = newState;
            this.Print(e.TelemetryInfo, stopwatch.ElapsedMilliseconds);
        }

        private void Print(TelemetryInfo telemetry, long timeTaken)
        {
            
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Updated @ {0}", DateTime.Now);
            Console.WriteLine("Took: {0}ms", timeTaken);
            Console.WriteLine(this.state.GetConsoleString());
        }


    }
}
