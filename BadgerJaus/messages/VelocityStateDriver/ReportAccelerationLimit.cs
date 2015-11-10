/*
 * Copyright (c) 2015, Wisconsin Robotics
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * * Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 * * Neither the name of Wisconsin Robotics nor the
 *   names of its contributors may be used to endorse or promote products
 *   derived from this software without specific prior written permission.
 *   
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL WISCONSIN ROBOTICS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
            set 
            {
                presenceVector.ToggleBit(ACCELERATION_X_BIT, true);
                accelerationX.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX); 
            }
        }

        public double AccelerationY
        {
            get { return accelerationY.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set 
            {
                presenceVector.ToggleBit(ACCELERATION_Y_BIT, true);
                accelerationY.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX); 
            }
        }

        public double AccelerationZ
        {
            get { return accelerationZ.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set 
            {
                presenceVector.ToggleBit(ACCELERATION_Z_BIT, true);
                accelerationZ.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX);
            }
        }

        public double RollAcceleration
        {
            get { return rollAcceleration.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set 
            {
                presenceVector.ToggleBit(ROLL_ACCELERATION_BIT, true);
                rollAcceleration.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX);
            }
        }

        public double PitchAcceleration
        {
            get { return pitchAcceleration.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set 
            {
                presenceVector.ToggleBit(PITCH_ACCELERATION_BIT, true);
                pitchAcceleration.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX);
            }
        }

        public double YawAcceleration
        {
            get { return yawAcceleration.ScaleValueToDouble(ACCELERATION_MIN, ACCELERATION_MAX); }
            set 
            {
                presenceVector.ToggleBit(YAW_ACCELERATION_BIT, true);
                yawAcceleration.SetValueFromDouble(value, ACCELERATION_MIN, ACCELERATION_MAX); 
            }
        }
    }
}
