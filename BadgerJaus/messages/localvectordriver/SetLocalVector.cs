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

using BadgerJaus.Util;

namespace BadgerJaus.Messages.LocalVectorDriver
{
    public class SetLocalVector : Message
    {
        public const int SPEED_BIT = 0;
        public const int Z_BIT = 1;
        public const int HEADING_BIT = 2;
        public const int ROLL_BIT = 3;
        public const int PITCH_BIT = 4;

        public const double MAX_SPEED = 327.67;
        public const double MIN_SPEED = 0;

        public const double MAX_Z = 35000;
        public const double MIN_Z = -10000;

        public const double MAX_HEADING_ROLL_PITCH = Math.PI;
        public const double MIN_HEADING_ROLL_PITCH = -Math.PI;

        JausBytePresenceVector presence;

        JausUnsignedShort speed;
        JausUnsignedInteger z;
        JausUnsignedShort heading;
        JausUnsignedShort roll;
        JausUnsignedShort pitch;

        protected override int CommandCode
        {
            get { return JausCommandCode.SET_LOCAL_VECTOR; }
        }

        protected override void InitFieldData()
        {
            presence = new JausBytePresenceVector();
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

            size += JausBytePresenceVector.SIZE_BYTES;

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

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index)
        {
            if (buffer.Length < index + this.MessageSize() - base.MessageSize())
            {
                Console.Error.WriteLine("Query Identification Payload Error: Not enough Size");
                return false; // Not Enough Size
            }

            //presence.getPresenceVector().setFromJausBuffer(buffer, index);
            presence.setFromJausBuffer(buffer, index);
            index += JausBytePresenceVector.SIZE_BYTES;

            if (presence.isBitSet(SPEED_BIT))
            {
                speed.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(Z_BIT))
            {
                z.setFromJausBuffer(buffer, index);
                index += JausUnsignedInteger.SIZE_BYTES;
            }

            if (presence.isBitSet(HEADING_BIT))
            {
                heading.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(ROLL_BIT))
            {
                roll.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PITCH_BIT))
            {
                pitch.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            return true;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index)
        {
            if (!presence.toJausBuffer(buffer, index))
                return false;

            //index += JausShortPresenceVector.SIZE_BYTES;
            index += JausBytePresenceVector.SIZE_BYTES;

            if (presence.isBitSet(SPEED_BIT))
            {
                if (!speed.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(Z_BIT))
            {
                if (!z.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedInteger.SIZE_BYTES;
            }

            if (presence.isBitSet(HEADING_BIT))
            {
                if (!heading.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(ROLL_BIT))
            {
                if (!roll.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PITCH_BIT))
            {
                if (!pitch.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedShort.SIZE_BYTES;
            }

            return true;
        }
    }
}