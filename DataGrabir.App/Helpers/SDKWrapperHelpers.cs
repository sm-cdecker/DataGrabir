using System.Collections.Generic;
using DataGrabir.App.Models;
using DataGrabir.App.TelemState;
using iRacingSdkWrapper;


namespace DataGrabir.App.Extensions
{
    public static class SDKWrapperHelpers {
        public static TelemetryState ToTelemetryState(this SdkWrapper wrapper, DGConfig config, TelemetryState oldState)
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

            tState.UpdateEvent = GetUpdateType(tState, oldState);

            foreach (KeyValuePair<string, ConfFormField> field in config.FormMapping)
            {
                tState.Fields.Add(field.Key, new TelemetryFormField()
                {
                    Name = field.Key,
                    FormId = field.Value.FormId,
                    UpdateOn = field.Value.UpdateOn,
                    Value = wrapper.GetTelemetryValue<object>(field.Key).Value
                });
            }

            return tState;
        }
        private static UpdateEvents GetUpdateType(TelemetryState newState, TelemetryState oldState)
        {
            // New Lap
            if (oldState.LapsCompleted != newState.LapsCompleted)
            {
                return UpdateEvents.LapStart;
            }

            // Pit entry
            if (!oldState.OnPitRoad && newState.OnPitRoad)
            {
                return UpdateEvents.PitRdEntry;
            }

            // Pit Stop Start
            if (!oldState.PlayerCarInPitStall && newState.PlayerCarInPitStall)
            {
                return UpdateEvents.LapStart;
            }

            // Pit stop end
            if (oldState.PlayerCarInPitStall && !newState.PlayerCarInPitStall)
            {
                return UpdateEvents.LapStart;
            }

            // Pit road exit
            if (oldState.OnPitRoad && !newState.OnPitRoad)
            {
                return UpdateEvents.PitRdExit;
            }

            // Tow start
            if (oldState.PlayerCarTowTime == 0 && newState.PlayerCarTowTime > 0)
            {
                return UpdateEvents.TowStart;
            }

            return UpdateEvents.None;
        }
    }
}
