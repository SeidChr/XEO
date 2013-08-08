using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEO.Core.Default
{
    [Export(typeof(IEnvironment))]
    internal class Environment : IEnvironment
    {
        public Environment()
        {
            WriteToConsole = false;
        }

        public bool WriteToConsole
        {
            get;
            set;
        }
    }
}
