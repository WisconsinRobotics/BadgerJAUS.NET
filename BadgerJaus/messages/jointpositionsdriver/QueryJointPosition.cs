using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Messages;

namespace BadgerJaus.Messages.JointPositionsDriver
{
    public class QueryJointPosition : Message
    {
        protected override int CommandCode
        {
            get
            {
                return JausCommandCode.QUERY_JOINT_POSITIONS;
            }
        }


    }
}
