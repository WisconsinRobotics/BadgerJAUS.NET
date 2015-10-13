using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Messages;
using BadgerJaus.Util;

namespace BadgerJaus.Messages.JointPositionsDriver
{
    class SetJointPosition : ReportJointPosition
    {
        protected override int CommandCode
        {
            get
            {
                return JausCommandCode.SET_JOINT_POSITIONS;
            }
        }
    }
}
