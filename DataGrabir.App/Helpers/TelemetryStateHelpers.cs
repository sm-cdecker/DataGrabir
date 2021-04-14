using System;
using System.Collections.Generic;
using System.Net.Http;
using DataGrabir.App.Models;

namespace DataGrabir.App.Extensions
{
    public static class TelemetryStateHelpers
    {
        public static MultipartFormDataContent GetForm(this TelemetryState state)
        {
            var form = new MultipartFormDataContent();
            foreach (KeyValuePair<string, TelemetryFormField> field in state.Fields)
            {
                form.Add(new StringContent(field.Value.Value.ToString()), field.Value.FormId);
            }
            return form;
        }
    }
}
