using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadgerJaus.Messages;

namespace BadgerJaus.messages.manipulatortooloffset
{
    public class ReportToolOffset : Message
    {
        private const int X_MIN = -15;
        private const int X_MAX = 15;
        private const int Y_MIN = -15;
        private const int Y_MAX = 15;
        private const int Z_MIN = -15;
        private const int Z_MAX = 15;

        private JausUnsignedInteger pointx;
        private JausUnsignedInteger pointy;
        private JausUnsignedInteger pointz;

        protected override int CommandCode
        {
            get
            {
                return JausCommandCode.REPORT_TOOL_OFFSET;
            }
        }

        public void SetPointX(double value)
        {
            pointx.setFromDouble(value, X_MIN, X_MAX);
        }

        public void SetPointY(double value)
        {
            pointy.setFromDouble(value, Y_MIN, Y_MAX);
        }

        public void SetPointZ(double value)
        {
            pointz.setFromDouble(value, Z_MIN, Z_MAX);
        }

        public double GetPointX()
        {
            return pointx.getValue();
        }

        public double GetPointY()
        {
            return pointy.getValue();
        }

        public double GetPointZ()
        {
            return pointz.getValue();
        }

        public override int GetPayloadSize()
        {
            return JausUnsignedInteger.SIZE_BYTES + JausUnsignedInteger.SIZE_BYTES + JausUnsignedInteger.SIZE_BYTES;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!pointx.toJausBuffer(buffer, indexOffset))
                return false;
            indexOffset += JausUnsignedInteger.SIZE_BYTES;

            if (!pointy.toJausBuffer(buffer, indexOffset))
                return false;
            indexOffset += JausUnsignedInteger.SIZE_BYTES;

            if (!pointz.toJausBuffer(buffer, indexOffset))
                return false;
            indexOffset += JausUnsignedInteger.SIZE_BYTES;

            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (buffer.Length < index + this.MessageSize() - base.MessageSize())
            {
                return false; // Not Enough Size
            }

            //presence.getPresenceVector().setFromJausBuffer(buffer, index);
            base.SetPayloadFromJausBuffer(buffer, index, out indexOffset);

            pointx.setFromJausBuffer(buffer, indexOffset);
            indexOffset += JausUnsignedInteger.SIZE_BYTES;

            pointy.setFromJausBuffer(buffer, indexOffset);
            indexOffset += JausUnsignedInteger.SIZE_BYTES;

            pointz.setFromJausBuffer(buffer, indexOffset);
            indexOffset += JausUnsignedInteger.SIZE_BYTES;

            return true;
        }
    }
}
