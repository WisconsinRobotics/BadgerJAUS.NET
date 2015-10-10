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

namespace BadgerJaus.Messages.LocalWaypointDriver
{
    public class ReportLocalWaypoint : QueryLocalWaypoint
    {
        JausUnsignedInteger x;
        JausUnsignedInteger y;
        JausUnsignedInteger z;

        JausUnsignedShort roll;
        JausUnsignedShort pitch;
        JausUnsignedShort yaw;

        JausUnsignedShort waypointTolerance;
        JausUnsignedInteger pathTolerance;


        public const int X_BIT = 0;
        public const int Y_BIT = 1;
        public const int Z_BIT = 2;
        public const int ROLL_BIT = 3;
        public const int PITCH_BIT = 4;
        public const int YAW_BIT = 5;
        public const int WT_BIT = 6;
        public const int PT_BIT = 7;

        private const int POSE_MIN = -100000;
        private const int POSE_MAX = 100000;

        private const double ORIENT_MIN = -System.Math.PI;
        private const double ORIENT_MAX = System.Math.PI;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_LOCAL_WAYPOINT; }
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
            waypointTolerance = new JausUnsignedShort();
            pathTolerance = new JausUnsignedInteger();
        }

        public void SetX(double xValue)
        {
            x.setFromDouble(xValue, POSE_MIN, POSE_MAX);
            presence.ToggleBit(X_BIT, true);
        }

        public void SetY(double yValue)
        {
            y.setFromDouble(yValue, POSE_MIN, POSE_MAX);
            presence.ToggleBit(Y_BIT, true);
        }

        public void SetZ(double zValue)
        {
            z.setFromDouble(zValue, POSE_MIN, POSE_MAX);
            presence.ToggleBit(Z_BIT, true);
        }

        public void SetRoll(double rollValue)
        {
            roll.SetValueFromDouble(rollValue, ORIENT_MIN, ORIENT_MAX);
            presence.ToggleBit(ROLL_BIT, true);
        }

        public void SetPitch(double pitchValue)
        {
            pitch.SetValueFromDouble(pitchValue, ORIENT_MIN, ORIENT_MAX);
            presence.ToggleBit(PITCH_BIT, true);
        }

        public void SetYaw(double yawValue)
        {
            yaw.SetValueFromDouble(yawValue, ORIENT_MIN, ORIENT_MAX);
            presence.ToggleBit(YAW_BIT, true);
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

            if (presence.IsBitSet(ROLL_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(PITCH_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(YAW_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(WT_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(PT_BIT))
                payloadSize += JausBaseType.INT_BYTE_SIZE;

            return payloadSize;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            bool status;

            status = base.PayloadToJausBuffer(buffer, index, out indexOffset);
            if (!status)
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

            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            base.SetPayloadFromJausBuffer(buffer, index, out indexOffset);

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

            return true;
        }
    }
}