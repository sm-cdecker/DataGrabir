using System.Collections.Generic;

namespace DataGrabir.App.TelemState
{
    public class TelemetryState
    {
        public TelemetryState()
        {
            this.Fields = new Dictionary<string, TelemetryFormField>();
        }
        public UpdateEvents UpdateEvent { get; set; }
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
        public ICollection<UpdateEvents> UpdateOn { get; set; }
    }
}
