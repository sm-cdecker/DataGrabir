using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGrabir.App.Models
{
    public class DGConfig
    {
        public int UpdateFreq { get; set; }
        public string FormUrl { get; set; }
        public Dictionary<string, string> FormMapping { get; set; }
    }
}
