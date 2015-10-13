using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Util;
namespace BadgerJaus.Messages.JointPositionsDriver
{
    class ReportJointPosition : QueryJointPosition
    {
        //consts
        private const int REVOLUTE = 1;
        private const int PRISMATIC = 2;

        //-8pi rad to +8pi rad
        private const int REV_MIN = -8;
        private const int REV_MAX = 8;
        //-10meters to +10meters
        private const int PRIS_MIN = -10;
        private const int PRIS_MAX = 10;
        
        private int index;

        private enum Index { REVOLUTE=1, PRISMATIC };

        private JausUnsignedInteger jointPos;

        //First in the message is the commandcode
        protected override int CommandCode
        {
            get
            {
                return JausCommandCode.REPORT_JOINT_POSITIONS;
            }
        }
        
        //No matter what setting will be double
        public void SetJointPos(double value)
        {
            if (index == (int)Index.REVOLUTE) {
                jointPos.SetValueFromDouble(value, REV_MIN, REV_MAX);
            }
            else
            {
                jointPos.SetValueFromDouble(value, PRIS_MIN, PRIS_MAX);
            }
        }

        public double GetJointPos()
        {
            if (index == (int)Index.REVOLUTE)
            {
                return jointPos.ScaleValueToDouble(REV_MIN, REV_MAX);
            }
            else
            {
                return jointPos.ScaleValueToDouble(PRIS_MIN, PRIS_MAX);
            }
        }

        public override int GetPayloadSize()
        {
 	         return JausBaseType.INT_BYTE_SIZE;
        }

        //Putting payload to byte array (Jaus buffer)
        protected override bool PayloadToJausBuffer(byte[] data, int index, out int indexOffset)
        {
            if (!jointPos.Serialize(data, index, out indexOffset))
                return false;

            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            return jointPos.Deserialize(buffer, index, out indexOffset);
        }

    }
}
