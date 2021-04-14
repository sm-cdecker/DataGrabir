using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGrabir.App.Models
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
        public Dictionary<string, TelemetryFormField> Fields { get; set; }
    }
}
