using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGrabir.App.Models;
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
                // OnPitRoad = telemetry

                /*
                // race info
                LapCompleted = wrapper.GetTelemetryValue<bool>(DriverMarker.Value,
                RaceLaps = wrapper.GetTelemetryValue<bool>(DriverMarker.Value,
                TrackTemp = wrapper.GetTelemetryValue<bool>(DriverMarker.Value,

                // LF
                LFwearL = wrapper.GetTelemetryValue<bool>(DriverMarker.Value,
                LFwearM = wrapper.GetTelemetryValue<bool>(DriverMarker.Value,
                LFwearR = wrapper.GetTelemetryValue<bool>(DriverMarker.Value,
                LFtempCL = wrapper.GetTelemetryValue<bool>(DriverMarker.Value,
                LFtempCM = wrapper.GetTelemetryValue<bool>(DriverMarker.Value,
                LFtempCR = wrapper.GetTelemetryValue<bool>(DriverMarker.Value,

                // RF
                RFwearR = wrapper.DriverMarker.Value,
                RFwearM = wrapper.DriverMarker.Value,
                RFwearL = wrapper.DriverMarker.Value,
                RFtempCL = wrapper.DriverMarker.Value,
                RFtempCM = wrapper.DriverMarker.Value,
                RFtempCR = wrapper.DriverMarker.Value,

                // LR
                LRwearL = wrapper.DriverMarker.Value,
                LRwearM = wrapper.DriverMarker.Value,
                LRwearR = wrapper.DriverMarker.Value,
                LRtempCL = wrapper.DriverMarker.Value,
                LRtempCM = wrapper.DriverMarker.Value,
                LRtempCR = wrapper.DriverMarker.Value,

                // RR
                RRwearR = wrapper.RRwearR.Value,
                RRwearM = wrapper.RRwearM.Value,
                RRwearL = wrapper.RRwearL.Value,
                RRtempCL = wrapper.RRtempCL.Value,
                RRtempCM = wrapper.DriverMarker.Value,
                RRtempCR = wrapper.DriverMarker.Value*/
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
