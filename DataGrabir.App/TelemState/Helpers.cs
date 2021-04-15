using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DataGrabir.App.TelemState
{
    public static class TelemetryStateHelpers
    {
        public static MultipartFormDataContent GetForm(this TelemetryState state)
        {
            var form = new MultipartFormDataContent();
            foreach (KeyValuePair<string, TelemetryFormField> field in state.Fields)
            {
                if (!String.IsNullOrWhiteSpace(field.Value.FormId) && field.Value.UpdateOn.Contains(state.UpdateEvent))
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


    }
}
