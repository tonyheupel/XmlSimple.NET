using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSimple
{
    public class Options
    {
        public bool AttrPrefix { get; set; }
        public bool NoAttr { get; set; }
        public object ForceArray { get; set; } // Can be bool or array of tags to force arrays for


        public Options()
        {
            // Set the defaults for all options here
            AttrPrefix = false;
            NoAttr = false;
            ForceArray = false;  // Defaults to true in Ruby, but not natural I think
        }
    }
}
