using System.Collections.Generic;
using DataGrabir.App.Models;
using DataGrabir.App.TelemState;
using iRacingSdkWrapper;


namespace DataGrabir.App.Extensions
{
    public static class SDKWrapperHelpers {
        public static TelemetryState ToTelemetryState(this SdkWrapper wrapper, DGConfig config)
        {
            var tState = new TelemetryState()
            {
                IsConnected = wrapper.IsConnected,
                IsRunning = wrapper.IsRunning,
                DriverMarker = wrapper.GetTelemetryValue<bool>("DriverMarker").Value,
                OnPitRoad = wrapper.GetTelemetryValue<bool>("OnPitRoad").Value,
                PlayerCarTowTime = wrapper.GetTelemetryValue<float>("PlayerCarTowTime").Value,
                PlayerCarInPitStall = wrapper.GetTelemetryValue<bool>("PlayerCarInPitStall").Value,
                LapsCompleted = wrapper.GetTelemetryValue<int>("LapsCompleted").Value
            };

            foreach(KeyValuePair<string, string> field in config.FormMapping)
            {
                tState.Fields.Add(field.Key, new TelemetryFormField()
                {
                    Name = field.Key,
                    FormId = field.Value,
                    Value = wrapper.GetTelemetryValue<object>(field.Key).Value
                });
            }

            return tState;
        }
    }
}
