using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Util;

namespace BadgerJaus.Messages.VelocityStateDriver
{
    public class ReportAccelerationLimit : QueryAccelerationLimit
    {
        JausUnsignedInteger accelerationX;
        JausUnsignedInteger accelerationY;
        JausUnsignedInteger accelerationZ;
        JausUnsignedInteger rollAcceleration;
        JausUnsignedInteger pitchAcceleration;
        JausUnsignedInteger yawAcceleration;

        private const double ACCELERATION_MIN = -1310.68;
        private const double ACCELERATION_MAX = 1310.68;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_ACCELERATION_LIMIT; }
        }

        protected override void InitFieldData()
        {
            base.InitFieldData();
            accelerationX = new JausUnsignedInteger();
            accelerationY = new JausUnsignedInteger();
            accelerationZ = new JausUnsignedInteger();
            rollAcceleration = new JausUnsignedInteger();
            pitchAcceleration = new JausUnsignedInteger();
            yawAcceleration = new JausUnsignedInteger();
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            base.SetPayloadFromJausBuffer(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(ACCELERATION_X_BIT))
                accelerationX.Deserialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(ACCELERATION_Y_BIT))
                accelerationY.Deserialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(ACCELERATION_Z_BIT))
                accelerationZ.Deserialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(ROLL_ACCELERATION_BIT))
                rollAcceleration.Deserialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(PITCH_ACCELERATION_BIT))
                pitchAcceleration.Deserialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(YAW_ACCELERATION_BIT))
                yawAcceleration.Deserialize(buffer, indexOffset, out indexOffset);

            return true;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            base.PayloadToJausBuffer(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(ACCELERATION_X_BIT))
                accelerationX.Serialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(ACCELERATION_Y_BIT))
                accelerationY.Serialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(ACCELERATION_Z_BIT))
                accelerationZ.Serialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(ROLL_ACCELERATION_BIT))
                rollAcceleration.Serialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(PITCH_ACCELERATION_BIT))
                pitchAcceleration.Serialize(buffer, indexOffset, out indexOffset);
            if (IsFieldSet(YAW_ACCELERATION_BIT))
                yawAcceleration.Serialize(buffer, indexOffset, out indexOffset);

            return true;
        }

        public double AccelerationX
        {
            get { return accelerationX.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set { accelerationX.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX); }
        }

        public double AccelerationY
        {
            get { return accelerationY.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set { accelerationY.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX); }
        }

        public double AccelerationZ
        {
            get { return accelerationZ.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set { accelerationZ.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX); }
        }

        public double RollAcceleration
        {
            get { return rollAcceleration.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set { rollAcceleration.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX); }
        }

        public double PitchAcceleration
        {
            get { return pitchAcceleration.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set { pitchAcceleration.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX); }
        }

        public double YawAcceleration
        {
            get { return yawAcceleration.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set { yawAcceleration.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX); }
        }
    }
}
