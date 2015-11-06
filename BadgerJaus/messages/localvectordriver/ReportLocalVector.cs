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

namespace BadgerJaus.Messages.LocalVectorDriver
{
    public class ReportLocalVector : QueryLocalVector
    {
        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_LOCAL_VECTOR; }
        }

        public const double MAX_SPEED = 327.67;
        public const double MIN_SPEED = 0;

        public const double MAX_Z = 35000;
        public const double MIN_Z = -10000;

        public const double MAX_HEADING_ROLL_PITCH = System.Math.PI;
        public const double MIN_HEADING_ROLL_PITCH = -System.Math.PI;

        protected JausUnsignedShort speed;
        protected JausUnsignedInteger z;
        protected JausUnsignedShort heading;
        protected JausUnsignedShort roll;
        protected JausUnsignedShort pitch;

        protected override void InitFieldData()
        {
            base.InitFieldData();
            speed = new JausUnsignedShort();
            z = new JausUnsignedInteger();
            heading = new JausUnsignedShort();
            roll = new JausUnsignedShort();
            pitch = new JausUnsignedShort();
        }

        public bool isFieldSet(int bit)
        {
            return presence.IsBitSet(bit);
        }

        public double GetSpeed()
        {
            return speed.ScaleValueToDouble(MIN_SPEED, MAX_SPEED);
        }

        public double GetZ()
        {
            return z.ScaleValueToDouble(MIN_Z, MAX_Z);
        }

        public double GetHeading()
        {
            return heading.ScaleValueToDouble(MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
        }

        public double GetRoll()
        {
            return roll.ScaleValueToDouble(MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
        }

        public double GetPitch()
        {
            return pitch.ScaleValueToDouble(MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
        }

        public void SetSpeed(double speed)
        {
            this.speed.SetValueFromDouble(speed, MIN_SPEED, MAX_SPEED);
            presence.ToggleBit(SPEED_BIT, true);
        }

        public void SetZ(double z)
        {
            this.z.SetValueFromDouble(z, MIN_Z, MAX_Z);
            presence.ToggleBit(Z_BIT, true);
        }

        public void SetHeading(double heading)
        {
            this.heading.SetValueFromDouble(heading, MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
            presence.ToggleBit(HEADING_BIT, true);
        }

        public void SetRoll(double roll)
        {
            this.roll.SetValueFromDouble(roll, MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
            presence.ToggleBit(ROLL_BIT, true);
        }

        public void SetPitch(double pitch)
        {
            this.pitch.SetValueFromDouble(pitch, MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
            presence.ToggleBit(PITCH_BIT, true);
        }

        public override int GetPayloadSize()
        {
            int size = 0;

            size += base.GetPayloadSize();

            if (presence.IsBitSet(SPEED_BIT))
                size += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(Z_BIT))
                size += JausBaseType.INT_BYTE_SIZE;

            if (presence.IsBitSet(HEADING_BIT))
                size += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(ROLL_BIT))
                size += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(PITCH_BIT))
                size += JausBaseType.SHORT_BYTE_SIZE;

            return size;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (buffer.Length < index + this.MessageSize() - base.MessageSize())
            {
                return false; // Not Enough Size
            }

            //presence.getPresenceVector().setFromJausBuffer(buffer, index);
            base.SetPayloadFromJausBuffer(buffer, indexOffset, out indexOffset);

            if (presence.IsBitSet(SPEED_BIT))
            {
                speed.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(Z_BIT))
            {
                z.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(HEADING_BIT))
            {
                heading.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(ROLL_BIT))
            {
                roll.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(PITCH_BIT))
            {
                pitch.Deserialize(buffer, indexOffset, out indexOffset);
            }

            return true;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {

            base.PayloadToJausBuffer(buffer, index, out indexOffset);

            if (presence.IsBitSet(SPEED_BIT))
            {
                if (!speed.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(Z_BIT))
            {
                if (!z.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(HEADING_BIT))
            {
                if (!heading.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(ROLL_BIT))
            {
                if (!roll.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(PITCH_BIT))
            {
                if (!pitch.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            return true;
        }
    }
}