using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy
{
    /*
     * This class exists to allow the Trace class to use
     * composite formatting like TraceHelper does.
     */
    public static class TraceHelper
    {
        public static void WriteLine(string format, params object[] objects)
        {
            Trace.WriteLine(String.Format(format, objects));
        }

        public static void WriteLine(object toWrite)
        {
            Trace.WriteLine(toWrite);
        }
    }
}
