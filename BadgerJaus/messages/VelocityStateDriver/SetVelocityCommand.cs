using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadgerJaus.Messages.VelocityStateDriver
{
    public class SetVelocityCommand : ReportVelocityCommand
    {
        protected override int CommandCode
        {
            get { return JausCommandCode.SET_VELOCITY_COMMAND; }
        }
    }
}
