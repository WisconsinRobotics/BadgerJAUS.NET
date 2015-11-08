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

namespace BadgerJaus.Messages.VelocityStateSensor
{
    public class ReportVelocityState : QueryVelocityState
    {
        JausUnsignedInteger x;
        JausUnsignedInteger y;
        JausUnsignedInteger z;

        JausUnsignedShort roll;
        JausUnsignedShort pitch;
        JausUnsignedShort yaw;

        JausUnsignedShort waypointTolerance;
        JausUnsignedInteger pathTolerance;

        JausTimeStamp timeStamp;

        private const double POSE_MIN = -327.68;
        private const double POSE_MAX = 327.67;

        private const double ORIENT_MIN = -32.768;
        private const double ORIENT_MAX = 32.767;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_VELOCITY_STATE; }
        }

        protected override void InitFieldData()
        {
            base.InitFieldData();
            x = new JausUnsignedInteger();
            y = new JausUnsignedInteger();
            z = new JausUnsignedInteger();
            roll = new JausUnsignedShort();
            pitch = new JausUnsignedShort();
            yaw = new JausUnsignedShort();
            pathTolerance = new JausUnsignedInteger();
            timeStamp = new JausTimeStamp();
        }

        public void SetX(double xValue)
        {
            x.SetValueFromDouble(xValue, POSE_MIN, POSE_MAX);
            presence.setBit(X_BIT);
        }

        public void SetY(double yValue)
        {
            y.SetValueFromDouble(yValue, POSE_MIN, POSE_MAX);
            presence.setBit(Y_BIT);
        }

        public void SetZ(double zValue)
        {
            z.setFromDouble(zValue, POSE_MIN, POSE_MAX);
            presence.setBit(Z_BIT);
        }

        public void SetRoll(double rollValue)
        {
            roll.SetValueFromDouble(rollValue, ORIENT_MIN, ORIENT_MAX);
            presence.setBit(ROLL_BIT);
        }

        public void SetPitch(double pitchValue)
        {
            pitch.SetValueFromDouble(pitchValue, ORIENT_MIN, ORIENT_MAX);
            presence.setBit(PITCH_BIT);
        }

        public void SetYaw(double yawValue)
        {
            yaw.SetValueFromDouble(yawValue, ORIENT_MIN, ORIENT_MAX);
            presence.setBit(YAW_BIT);
        }

        public void SetTimestamp(int timeValue)
        {
            //timeStamp.setValue(timeValue);
            presence.setBit(TS_BIT);
        }

        public double GetX()
        {
            return x.ScaleValueToDouble(POSE_MIN, POSE_MAX);
        }

        public double GetY()
        {
            return y.ScaleValueToDouble(POSE_MIN, POSE_MAX);
        }

        public double GetZ()
        {
            return z.ScaleValueToDouble(POSE_MIN, POSE_MAX);
        }

        public double GetRoll()
        {
            return roll.ScaleValueToDouble(ORIENT_MIN, ORIENT_MAX);
        }

        public double GetPitch()
        {
            return pitch.ScaleValueToDouble(ORIENT_MIN, ORIENT_MAX);
        }

        public double GetYaw()
        {
            return yaw.ScaleValueToDouble(ORIENT_MIN, ORIENT_MAX);
        }

        public override int GetPayloadSize()
        {
            int payloadSize = 0;
            payloadSize += base.GetPayloadSize();

            if (presence.IsBitSet(X_BIT))
                payloadSize += JausBaseType.INT_BYTE_SIZE;

            if (presence.IsBitSet(Y_BIT))
                payloadSize += JausBaseType.INT_BYTE_SIZE;

            if (presence.IsBitSet(Z_BIT))
                payloadSize += JausBaseType.INT_BYTE_SIZE;

            if (presence.IsBitSet(YAW_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(TS_BIT))
                payloadSize += JausBaseType.INT_BYTE_SIZE;

            return payloadSize;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;

            if(!base.PayloadToJausBuffer(buffer, indexOffset, out indexOffset))
                return false;

            if (presence.IsBitSet(X_BIT))
            {
                if (!x.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(Y_BIT))
            {
                if (!y.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(Z_BIT))
            {
                if (!z.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(YAW_BIT))
            {
                if (!y.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(TS_BIT))
            {
                if (!timeStamp.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            if (!base.SetPayloadFromJausBuffer(buffer, index, out indexOffset))
                return false;

            if (presence.IsBitSet(X_BIT))
            {
                x.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(Y_BIT))
            {
                y.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(Z_BIT))
            {
                z.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(YAW_BIT))
            {
                yaw.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(TS_BIT))
            {
                timeStamp.Deserialize(buffer, indexOffset, out indexOffset);
            }

            return true;
        }

        public void SetToCurrentTime()
        {
            presence.setBit(TS_BIT);
            timeStamp.setToCurrentTime();
        }
    }
}