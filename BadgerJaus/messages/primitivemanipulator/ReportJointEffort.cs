using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Messages;
using BadgerJaus.Util;

namespace BadgerJaus.messages.primitivemanipulator
{
    public class ReportJointEffort : Message
    {
        private const double EFFORT_MIN = -100;
        private const double EFFORT_MAX = 100;

        private JausUnsignedShort effort;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_JOINT_EFFORT; }
        }

        public void SetEffort(double value)
        {
            effort.SetValueFromDouble(value, EFFORT_MIN, EFFORT_MAX);
        }

        public int GetEffort()
        {
            return (int)effort.Value;
        }

        public override int GetPayloadSize()
        {
            return JausBaseType.SHORT_BYTE_SIZE;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            if (!effort.Serialize(buffer, index, out indexOffset)) 
                return false;

            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            return effort.Deserialize(buffer, index, out indexOffset);
        }
    }
}
