using System;
using iRacingSdkWrapper;
using DataGrabir.App.Models;
using System.Net.Http;
using DataGrabir.App.Extensions;

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

        private void TelemetryUpdate(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            var newState = this.wrapper.ToTelemetryState(this.config);
            if (newState.IsConnected && newState.IsRunning && (newState.OnPitRoad != this.state.OnPitRoad) && newState.DriverMarker)
            {
                var req = new HttpRequestMessage(HttpMethod.Post, this.config.FormUrl);
                req.Content = newState.GetForm();
                this.httpClient.SendAsync(req);
            }
            this.state = newState;
            this.Print(e.TelemetryInfo);
        }

        private void Print(TelemetryInfo telemetry)
        {
            
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Updated @ {0}", DateTime.Now);
            Console.WriteLine("is iRacing Connected: {0}", this.wrapper.IsConnected ? "Y" : "N");
            Console.WriteLine("is iRacing Running: {0}", this.wrapper.IsRunning ? "Y" : "N");
            
            Console.WriteLine("FuelLevel: {0}", telemetry.FuelLevel.Value);
            Console.WriteLine("IsOnTrack: {0}", telemetry.IsOnTrack.Value ? "Y" : "N");
            Console.WriteLine("OnPitRoad: {0}", telemetry.CarIdxOnPitRoad.Value[telemetry.PlayerCarIdx.Value]);
        }

    }
}
