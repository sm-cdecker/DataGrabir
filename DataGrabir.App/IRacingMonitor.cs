using System;
using iRacingSdkWrapper;

namespace DataGrabir.App
{
    class IRacingMonitor
    {
        SdkWrapper wrapper;

        public IRacingMonitor()
        {
            wrapper = new SdkWrapper();
            wrapper.TelemetryUpdateFrequency = 1;
            wrapper.TelemetryUpdated += TelemetryUpdate;
        }

        public void Run()
        {
            Console.WriteLine("Waiting for iRacing to start...");
            wrapper.Start();
        }

        private void TelemetryUpdate(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            this.Print(e.TelemetryInfo);
        }

        private void Print(TelemetryInfo telemetry)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Updated @ {0}", DateTime.Now);
            Console.WriteLine("is iRacing Connected: {0}", wrapper.IsConnected);
            Console.WriteLine("is iRacing Running: {0}", wrapper.IsRunning);
            
            Console.WriteLine("FuelLevel: {0}", telemetry.FuelLevel);
            Console.WriteLine("IsOnTrack: {0}", telemetry.IsOnTrack);
            Console.WriteLine("OnPitRoad: {0}", telemetry.CarIdxOnPitRoad.Value[telemetry.PlayerCarIdx.Value]);
        }

    }
}
