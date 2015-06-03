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
using System;

namespace BadgerJaus.Messages.LocalPoseSensor
{
    public class SetLocalPose : Message
    {
        JausShortPresenceVector presenceVector;

        JausUnsignedInteger x;
        JausUnsignedInteger y;
        JausUnsignedInteger z;
        JausUnsignedInteger positionRMS;

        JausUnsignedShort roll;
        JausUnsignedShort pitch;
        JausUnsignedShort yaw;
        JausUnsignedShort attitudeRMS;

        JausTimeStamp timeStamp;

        public const int X_BIT = 0;
        public const int Y_BIT = 1;
        public const int Z_BIT = 2;
        public const int P_RMS = 3;
        public const int ROLL_BIT = 4;
        public const int PITCH_BIT = 5;
        public const int YAW_BIT = 6;
        public const int A_RMS = 7;
        public const int TS_BIT = 8;

        private const int POSE_MIN = -100000;
        private const int POSE_MAX = 100000;

        private const double ORIENT_MIN = -Math.PI;
        private const double ORIENT_MAX = Math.PI;

        protected override int CommandCode
        {
            get { return JausCommandCode.SET_LOCAL_POSE; }
        }

        protected override void InitFieldData()
        {
            presenceVector = new JausShortPresenceVector();
            x = new JausUnsignedInteger();
            y = new JausUnsignedInteger();
            z = new JausUnsignedInteger();
            positionRMS = new JausUnsignedInteger();
            roll = new JausUnsignedShort();
            pitch = new JausUnsignedShort();
            yaw = new JausUnsignedShort();
            attitudeRMS = new JausUnsignedShort();
            timeStamp = new JausTimeStamp();
        }

        public bool IsFieldSet(int bit)
        {
            return presenceVector.isBitSet(bit);
        }

        public void SetX(double xValue)
        {
            x.setFromDouble(xValue, POSE_MIN, POSE_MAX);
            presenceVector.setBit(X_BIT);
        }

        public void SetY(double yValue)
        {
            y.setFromDouble(yValue, POSE_MIN, POSE_MAX);
            presenceVector.setBit(Y_BIT);
        }

        public void SetZ(double zValue)
        {
            z.setFromDouble(zValue, POSE_MIN, POSE_MAX);
            presenceVector.setBit(Z_BIT);
        }

        public void SetRoll(double rollValue)
        {
            roll.setFromDouble(rollValue, ORIENT_MIN, ORIENT_MAX);
            presenceVector.setBit(ROLL_BIT);
        }

        public void SetPitch(double pitchValue)
        {
            pitch.setFromDouble(pitchValue, ORIENT_MIN, ORIENT_MAX);
            presenceVector.setBit(PITCH_BIT);
        }

        public void SetYaw(double yawValue)
        {
            yaw.setFromDouble(yawValue, ORIENT_MIN, ORIENT_MAX);
            presenceVector.setBit(YAW_BIT);
        }

        public void SetTimestamp(int timeValue)
        {
            //timeStamp.setValue(timeValue);
            presenceVector.setBit(TS_BIT);
        }

        public double GetX()
        {
            return x.scaleToDouble(POSE_MIN, POSE_MAX);
        }

        public double GetY()
        {
            return y.scaleToDouble(POSE_MIN, POSE_MAX);
        }

        public double GetZ()
        {
            return z.scaleToDouble(POSE_MIN, POSE_MAX);
        }

        public double GetRoll()
        {
            return roll.scaleToDouble(ORIENT_MIN, ORIENT_MAX);
        }

        public double GetPitch()
        {
            return pitch.scaleToDouble(ORIENT_MIN, ORIENT_MAX);
        }

        public double GetYaw()
        {
            return yaw.scaleToDouble(ORIENT_MIN, ORIENT_MAX);
        }

        public override int GetPayloadSize()
        {
            int payloadSize = 0;
            payloadSize += JausShortPresenceVector.SIZE_BYTES;

            if (presenceVector.isBitSet(X_BIT))
                payloadSize += JausUnsignedInteger.SIZE_BYTES;

            if (presenceVector.isBitSet(Y_BIT))
                payloadSize += JausUnsignedInteger.SIZE_BYTES;

            if (presenceVector.isBitSet(Z_BIT))
                payloadSize += JausUnsignedInteger.SIZE_BYTES;

            if (presenceVector.isBitSet(YAW_BIT))
                payloadSize += JausUnsignedShort.SIZE_BYTES;

            if (presenceVector.isBitSet(TS_BIT))
                payloadSize += JausTimeStamp.SIZE_BYTES;

            return payloadSize;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index)
        {
            if (!presenceVector.toJausBuffer(buffer, index))
                return false;

            index += JausShortPresenceVector.SIZE_BYTES;

            if (presenceVector.isBitSet(X_BIT))
            {
                if (!x.toJausBuffer(buffer, index))
                    return false;
                index += JausUnsignedInteger.SIZE_BYTES;
            }

            if (presenceVector.isBitSet(Y_BIT))
            {
                if (!y.toJausBuffer(buffer, index))
                    return false;
                index += JausUnsignedInteger.SIZE_BYTES;
            }

            if (presenceVector.isBitSet(Z_BIT))
            {
                if (!z.toJausBuffer(buffer, index))
                    return false;
                index += JausUnsignedInteger.SIZE_BYTES;
            }

            if (presenceVector.isBitSet(YAW_BIT))
            {
                if (!y.toJausBuffer(buffer, index))
                    return false;
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presenceVector.isBitSet(TS_BIT))
            {
                if (!timeStamp.toJausBuffer(buffer, index))
                    return false;
                index += JausTimeStamp.SIZE_BYTES;
            }

            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index)
        {
            if (buffer.Length < index + this.MessageSize() - base.MessageSize())
            {
                Console.Error.WriteLine("Query Identification Payload Error: Not enough Size");
                return false; // Not Enough Size
            }

            presenceVector.setFromJausBuffer(buffer, index);

            index += JausShortPresenceVector.SIZE_BYTES;

            if (presenceVector.isBitSet(X_BIT))
            {
                x.setFromJausBuffer(buffer, index);
                index += JausUnsignedInteger.SIZE_BYTES;
            }

            if (presenceVector.isBitSet(Y_BIT))
            {
                y.setFromJausBuffer(buffer, index);
                index += JausUnsignedInteger.SIZE_BYTES;
            }

            if (presenceVector.isBitSet(Z_BIT))
            {
                z.setFromJausBuffer(buffer, index);
                index += JausUnsignedInteger.SIZE_BYTES;
            }

            if (presenceVector.isBitSet(YAW_BIT))
            {
                yaw.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presenceVector.isBitSet(TS_BIT))
            {
                timeStamp.setFromJausBuffer(buffer, index);
                index += JausTimeStamp.SIZE_BYTES;
            }

            return true;
        }

        public void SetToCurrentTime()
        {
            presenceVector.setBit(TS_BIT);
            timeStamp.setToCurrentTime();
        }
    }
}