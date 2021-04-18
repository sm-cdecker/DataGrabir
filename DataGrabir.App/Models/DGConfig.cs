using System.Collections.Generic;
using DataGrabir.App.TelemState;

namespace DataGrabir.App.Models
{
    public class DGConfig
    {
        public int UpdateFreq { get; set; }
        public string FormUrl { get; set; }
        public bool PostTelemetryToConsole { get; set; }
        public Dictionary<string, ConfFormField> FormMapping { get; set; }
    }

    public class ConfFormField {
        public ICollection<UpdateEvents> SendOn { get; set; }
        public string FormId { get; set; }
    }

}
