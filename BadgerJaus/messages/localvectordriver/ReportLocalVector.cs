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
            return presence.isBitSet(bit);
        }

        public double GetSpeed()
        {
            return speed.scaleToDouble(MIN_SPEED, MAX_SPEED);
        }

        public double GetZ()
        {
            return z.scaleToDouble(MIN_Z, MAX_Z);
        }

        public double GetHeading()
        {
            return heading.scaleToDouble(MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
        }

        public double GetRoll()
        {
            return roll.scaleToDouble(MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
        }

        public double GetPitch()
        {
            return pitch.scaleToDouble(MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
        }

        public void SetSpeed(double speed)
        {
            this.speed.setFromDouble(speed, MIN_SPEED, MAX_SPEED);
            presence.setBit(SPEED_BIT);
        }

        public void SetZ(double z)
        {
            this.z.setFromDouble(z, MIN_Z, MAX_Z);
            presence.setBit(Z_BIT);
        }

        public void SetHeading(double heading)
        {
            this.heading.setFromDouble(heading, MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
            presence.setBit(HEADING_BIT);
        }

        public void SetRoll(double roll)
        {
            this.roll.setFromDouble(roll, MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
            presence.setBit(ROLL_BIT);
        }

        public void SetPitch(double pitch)
        {
            this.pitch.setFromDouble(pitch, MIN_HEADING_ROLL_PITCH, MAX_HEADING_ROLL_PITCH);
            presence.setBit(PITCH_BIT);
        }

        public override int GetPayloadSize()
        {
            int size = 0;

            size += base.GetPayloadSize();

            if (presence.isBitSet(SPEED_BIT))
                size += JausUnsignedShort.SIZE_BYTES;

            if (presence.isBitSet(Z_BIT))
                size += JausUnsignedInteger.SIZE_BYTES;

            if (presence.isBitSet(HEADING_BIT))
                size += JausUnsignedShort.SIZE_BYTES;

            if (presence.isBitSet(ROLL_BIT))
                size += JausUnsignedShort.SIZE_BYTES;

            if (presence.isBitSet(PITCH_BIT))
                size += JausUnsignedShort.SIZE_BYTES;

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
            base.SetPayloadFromJausBuffer(buffer, index, out indexOffset);

            if (presence.isBitSet(SPEED_BIT))
            {
                speed.setFromJausBuffer(buffer, indexOffset);
                indexOffset += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(Z_BIT))
            {
                z.setFromJausBuffer(buffer, indexOffset);
                indexOffset += JausUnsignedInteger.SIZE_BYTES;
            }

            if (presence.isBitSet(HEADING_BIT))
            {
                heading.setFromJausBuffer(buffer, indexOffset);
                indexOffset += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(ROLL_BIT))
            {
                roll.setFromJausBuffer(buffer, indexOffset);
                indexOffset += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PITCH_BIT))
            {
                pitch.setFromJausBuffer(buffer, indexOffset);
                indexOffset += JausUnsignedShort.SIZE_BYTES;
            }

            return true;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!presence.toJausBuffer(buffer, index))
                return false;

            base.PayloadToJausBuffer(buffer, index, out indexOffset);

            if (presence.isBitSet(SPEED_BIT))
            {
                if (!speed.toJausBuffer(buffer, indexOffset))
                    return false;

                indexOffset += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(Z_BIT))
            {
                if (!z.toJausBuffer(buffer, indexOffset))
                    return false;

                indexOffset += JausUnsignedInteger.SIZE_BYTES;
            }

            if (presence.isBitSet(HEADING_BIT))
            {
                if (!heading.toJausBuffer(buffer, indexOffset))
                    return false;

                indexOffset += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(ROLL_BIT))
            {
                if (!roll.toJausBuffer(buffer, indexOffset))
                    return false;

                indexOffset += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PITCH_BIT))
            {
                if (!pitch.toJausBuffer(buffer, indexOffset))
                    return false;

                indexOffset += JausUnsignedShort.SIZE_BYTES;
            }

            return true;
        }
    }
}