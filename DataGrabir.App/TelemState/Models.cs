using System;
using System.Collections.Generic;
using DataGrabir.App.Enums;

namespace DataGrabir.App.TelemState
{
    public class TelemetryState
    {
        public TelemetryState()
        {
            this.Fields = new Dictionary<string, TelemetryFormField>();
        }
        public bool IsConnected { get; set; }
        public bool IsRunning { get; set; }
        public bool OnPitRoad { get; set; }
        public bool DriverMarker { get; set; }
        public int LapsCompleted { get; set; }
        public bool PlayerCarInPitStall { get; set; }
        public float PlayerCarTowTime { get; set; }
        public Dictionary<string, TelemetryFormField> Fields { get; set; }
    }

    public class TelemetryFormField
    {
        public string Name { get; set; }
        public string FormId { get; set; }
        public object Value { get; set; }
        public IEnumerable<EventUpdateEnum> UpdateOn { get; set; }
    }
}
