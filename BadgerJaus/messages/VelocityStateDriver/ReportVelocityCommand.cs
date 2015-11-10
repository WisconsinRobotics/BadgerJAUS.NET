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
            set
            {
                presenceVector.ToggleBit(VELOCITY_X_BIT, true);
                velocityX.SetValueFromDouble(value, LINEAR_MIN, LINEAR_MAX); 
            }
        }

        public double VelocityY
        {
            get { return velocityY.ScaleValueToDouble(LINEAR_MIN, LINEAR_MAX); }
            set 
            {
                presenceVector.ToggleBit(VELOCITY_Y_BIT, true);
                velocityY.SetValueFromDouble(value, LINEAR_MIN, LINEAR_MAX);
            }
        }

        public double VelocityZ
        {
            get { return velocityZ.ScaleValueToDouble(LINEAR_MIN, LINEAR_MAX); }
            set
            {
                presenceVector.ToggleBit(VELOCITY_Z_BIT, true);
                velocityZ.SetValueFromDouble(value, LINEAR_MIN, LINEAR_MAX); 
            }
        }

        public double RollRate
        {
            get { return rollRate.ScaleValueToDouble(ANGULAR_MIN, ANGULAR_MAX); }
            set 
            {
                presenceVector.ToggleBit(ROLL_RATE_BIT, true);
                rollRate.SetValueFromDouble(value, ANGULAR_MIN, ANGULAR_MAX); 
            }
        }

        public double PitchRate
        {
            get { return pitchRate.ScaleValueToDouble(ANGULAR_MIN, ANGULAR_MAX); }
            set
            {
                presenceVector.ToggleBit(PITCH_RATE_BIT, true);
                pitchRate.SetValueFromDouble(value, ANGULAR_MIN, ANGULAR_MAX);
            }
        }

        public double YawRate
        {
            get { return yawRate.ScaleValueToDouble(ANGULAR_MIN, ANGULAR_MAX); }
            set 
            {
                presenceVector.ToggleBit(YAW_RATE_BIT, true);
                yawRate.SetValueFromDouble(value, ANGULAR_MIN, ANGULAR_MAX); 
            }
        }
    }
}
