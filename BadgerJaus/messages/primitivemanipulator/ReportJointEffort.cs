using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadgerJaus.Messages;

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
            effort.setFromDouble(value, EFFORT_MIN, EFFORT_MAX);
        }

        public int GetEffort()
        {
            return effort.getValue();
        }

        public override int GetPayloadSize()
        {
            return JausUnsignedShort.SIZE_BYTES;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!effort.toJausBuffer(buffer, indexOffset)) 
                return false;
            indexOffset += JausUnsignedShort.SIZE_BYTES;

            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            bool status;

            status = effort.setFromJausBuffer(buffer, index);
            indexOffset = index + JausUnsignedShort.SIZE_BYTES;
            if (!status)
                indexOffset = index;
            return status;
        }
    }
}
