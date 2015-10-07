using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Messages;

namespace BadgerJaus.messages.manipulatortooloffset
{
    public class SetToolOffset : ReportToolOffset
    {
        protected override int CommandCode
        {
            get { return JausCommandCode.SET_TOOL_OFFSET; }
        }
    }
}
