using System.Collections.Generic;
using DataGrabir.App.Models;
using DataGrabir.App.TelemState;
using iRacingSimulator;


namespace DataGrabir.App.Extensions
{
    public static class SDKWrapperHelpers {
        public static TelemetryState ToTelemetryState(this Sim instance, DGConfig config, TelemetryState oldState)
        {
            var tState = new TelemetryState()
            {
                IsConnected = instance.Sdk.IsConnected,
                IsRunning = instance.Sdk.IsRunning,
                DriverMarker = instance.Sdk.GetTelemetryValue<bool>("DriverMarker").Value,
                OnPitRoad = instance.Sdk.GetTelemetryValue<bool>("OnPitRoad").Value,
                PlayerCarTowTime = instance.Sdk.GetTelemetryValue<float>("PlayerCarTowTime").Value,
                PlayerCarInPitStall = instance.Sdk.GetTelemetryValue<bool>("PlayerCarInPitStall").Value,
                LapsCompleted = instance.Sdk.GetTelemetryValue<int>("LapCompleted").Value,
                CurrentDriver = instance.Driver?.Name,
                TeamName = instance.Driver?.TeamName,
                CurrentSessionNumber = instance.CurrentSessionNumber,
                SubsessionId = instance.SessionData.SubsessionId
            };

            tState.UpdateEvent = GetUpdateType(tState, oldState);

            foreach (KeyValuePair<string, ConfFormField> field in config.FormMapping)
            {
                object value;

                switch (field.Key)
                {
                    case "UpdateEvent":
                        value = tState.UpdateEvent;
                        break;
                    case "CurrentDriver":
                        value = instance.Driver?.Name;
                        break;
                    case "TeamName":
                        value = instance.Driver?.TeamName;
                        break;
                    case "CurrentSessionNumber":
                        value = instance.CurrentSessionNumber;
                        break;
                    case "SubsessionId":
                        value = instance.SessionData.SubsessionId;
                        break;
                    default:
                        value = instance.Sdk.GetTelemetryValue<object>(field.Key)?.Value;
                        break;
                }
                tState.Fields.Add(field.Key, new TelemetryFormField()
                {
                    Name = field.Key,
                    FormId = field.Value.FormId,
                    SendOn = field.Value.SendOn,
                    Value = value
                });
            }

            return tState;
        }
        private static UpdateEvents GetUpdateType(TelemetryState newState, TelemetryState oldState)
        {

            // Tow start
            if (oldState.PlayerCarTowTime == 0 && newState.PlayerCarTowTime > 0)
            {
                return UpdateEvents.TowStart;
            }

            // New Lap
            if (oldState.LapsCompleted < newState.LapsCompleted)
            {
                return UpdateEvents.LapStart;
            }

            // Pit road entry
            if (!oldState.OnPitRoad && newState.OnPitRoad)
            {
                return UpdateEvents.PitRdEntry;
            }

            // Pit Stop Start
            if (!oldState.PlayerCarInPitStall && newState.PlayerCarInPitStall)
            {
                return UpdateEvents.PitStart;
            }

            // Pit stop end
            if (oldState.PlayerCarInPitStall && !newState.PlayerCarInPitStall)
            {
                return UpdateEvents.PitFinish;
            }

            // Pit road exit
            if (oldState.OnPitRoad && !newState.OnPitRoad)
            {
                return UpdateEvents.PitRdExit;
            }

            return UpdateEvents.None;
        }
    }
}
