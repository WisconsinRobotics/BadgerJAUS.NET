using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Util;

namespace BadgerJaus.Messages.VelocityStateDriver
{
    public class ReportVelocityCommand : QueryVelocityCommand
    {
        JausUnsignedInteger velocityX;
        JausUnsignedInteger velocityY;
        JausUnsignedInteger velocityZ;
        JausUnsignedShort rollRate;
        JausUnsignedShort pitchRate;
        JausUnsignedShort yawRate;

        private const double LINEAR_MIN = -327.68;
        private const double LINEAR_MAX = 327.67;

        private const double ANGULAR_MIN = -32.768;
        private const double ANGULAR_MAX = 32.767;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_VELOCITY_COMMAND; }
        }

        protected override void InitFieldData()
        {
            base.InitFieldData();
            velocityX = new JausUnsignedInteger();
            velocityY = new JausUnsignedInteger();
            velocityZ = new JausUnsignedInteger();
            rollRate = new JausUnsignedShort();
            pitchRate = new JausUnsignedShort();
            yawRate = new JausUnsignedShort();
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            base.SetPayloadFromJausBuffer(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(VELOCITY_X_BIT))
                velocityX.Deserialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(VELOCITY_Y_BIT))
                velocityY.Deserialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(VELOCITY_Z_BIT))
                velocityZ.Deserialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(ROLL_RATE_BIT))
                rollRate.Deserialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(PITCH_RATE_BIT))
                pitchRate.Deserialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(YAW_RATE_BIT))
                yawRate.Deserialize(buffer, indexOffset, out indexOffset);

            return true;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            base.PayloadToJausBuffer(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(VELOCITY_X_BIT))
                velocityX.Serialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(VELOCITY_Y_BIT))
                velocityY.Serialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(VELOCITY_Z_BIT))
                velocityZ.Serialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(ROLL_RATE_BIT))
                rollRate.Serialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(PITCH_RATE_BIT))
                pitchRate.Serialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(YAW_RATE_BIT))
                yawRate.Serialize(buffer, indexOffset, out indexOffset);

            return true;
        }

        public double VelocityX
        {
            get { return velocityX.ScaleValueToDouble(LINEAR_MIN, LINEAR_MAX); }
            set { velocityX.SetValueFromDouble(value, LINEAR_MIN, LINEAR_MAX); }
        }

        public double VelocityY
        {
            get { return velocityY.ScaleValueToDouble(LINEAR_MIN, LINEAR_MAX); }
            set { velocityY.SetValueFromDouble(value, LINEAR_MIN, LINEAR_MAX); }
        }

        public double VelocityZ
        {
            get { return velocityZ.ScaleValueToDouble(LINEAR_MIN, LINEAR_MAX); }
            set { velocityZ.SetValueFromDouble(value, LINEAR_MIN, LINEAR_MAX); }
        }

        public double RollRate
        {
            get { return rollRate.ScaleValueToDouble(ANGULAR_MIN, ANGULAR_MAX); }
            set { rollRate.SetValueFromDouble(value, ANGULAR_MIN, ANGULAR_MAX); }
        }

        public double PitchRate
        {
            get { return pitchRate.ScaleValueToDouble(ANGULAR_MIN, ANGULAR_MAX); }
            set { pitchRate.SetValueFromDouble(value, ANGULAR_MIN, ANGULAR_MAX); }
        }

        public double YawRate
        {
            get { return yawRate.ScaleValueToDouble(ANGULAR_MIN, ANGULAR_MAX); }
            set { yawRate.SetValueFromDouble(value, ANGULAR_MIN, ANGULAR_MAX); }
        }
    }
}
