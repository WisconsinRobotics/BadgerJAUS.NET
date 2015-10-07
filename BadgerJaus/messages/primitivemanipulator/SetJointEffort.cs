using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Messages;

namespace BadgerJaus.messages.primitivemanipulator
{
    public class SetJointEffort : ReportJointEffort
    {
        protected override int CommandCode
        {
            get { return JausCommandCode.SET_JOINT_EFFORT; }
        }
    }
}
