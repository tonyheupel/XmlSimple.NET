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


        public Options()
        {
            // Set the defaults for all options here
            AttrPrefix = false;
            NoAttr = false;
        }
    }
}
