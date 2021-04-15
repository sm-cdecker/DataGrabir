using System.Collections.Generic;
using DataGrabir.App.TelemState;

namespace DataGrabir.App.Models
{
    public class DGConfig
    {
        public int UpdateFreq { get; set; }
        public string FormUrl { get; set; }
        public Dictionary<string, ConfFormField> FormMapping { get; set; }
    }

    public class ConfFormField {
        public UpdateEvents UpdateOn { get; set; }
        public string FormId { get; set; }
    }

}
