using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DataGrabir.App.TelemState
{
    public static class TelemetryStateHelpers
    {
        public static MultipartFormDataContent GetForm(this TelemetryState state, TelemetryState oldState)
        {
            var form = new MultipartFormDataContent();
            var updateEvent = GetUpdateType(state, oldState);
            foreach (KeyValuePair<string, TelemetryFormField> field in state.Fields)
            {
                if (!String.IsNullOrWhiteSpace(field.Value.FormId) && field.Value.UpdateOn.Contains(updateEvent))
                {
                    form.Add(new StringContent(field.Value.Value.ToString()), field.Value.FormId);
                }
            }
            return form;
        }

        public static string GetConsoleString(this TelemetryState state)
        {
            var sb = new StringBuilder()
                .AppendLine("IsConnected: " + (state.IsConnected ? "Y" : "N"))
                .AppendLine("IsRunning: " + (state.IsRunning ? "Y" : "N"));

            foreach (KeyValuePair<string, TelemetryFormField> field in state.Fields)
            {
                sb.AppendLine(field.Value.Name + " : " + field.Value.Value);
            }

            return sb.ToString();
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
