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

namespace BadgerJaus.Messages.PrimitiveDriver
{
    public class SetWrenchEffort : Message
    {
        public const int PROPULSIVE_LINEAR_EFFORT_X_BIT = 0;
        public const int PROPULSIVE_LINEAR_EFFORT_Y_BIT = 1;
        public const int PROPULSIVE_LINEAR_EFFORT_Z_BIT = 2;
        public const int PROPULSIVE_ROTATIONAL_EFFORT_X_BIT = 3;
        public const int PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT = 4;
        public const int PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT = 5;
        public const int RESISTIVE_LINEAR_EFFORT_X_BIT = 6;
        public const int RESISTIVE_LINEAR_EFFORT_Y_BIT = 7;
        public const int RESISTIVE_LINEAR_EFFORT_Z_BIT = 8;
        public const int RESISTIVE_ROTATIONAL_EFFORT_X_BIT = 9;
        public const int RESISTIVE_ROTATIONAL_EFFORT_Y_BIT = 10;
        public const int RESISTIVE_ROTATIONAL_EFFORT_Z_BIT = 11;

        public const int PROPULSIVE_EFFORT_MIN = -100;
        public const int PROPULSIVE_EFFORT_MAX = 100;

        public const int RESISTIVE_EFFORT_MIN = 0;
        public const int RESISTIVE_EFFORT_MAX = 100;

        JausShortPresenceVector presence;

        JausUnsignedShort propLinearX;
        JausUnsignedShort propLinearY;
        JausUnsignedShort propLinearZ;

        JausUnsignedShort propRotX;
        JausUnsignedShort propRotY;
        JausUnsignedShort propRotZ;

        JausByte resistLinearX;
        JausByte resistLinearY;
        JausByte resistLinearZ;

        JausByte resistRotX;
        JausByte resistRotY;
        JausByte resistRotZ;

        protected override int CommandCode
        {
            get { return JausCommandCode.SET_WRENCH_EFFORT; }
        }

        protected override void InitFieldData()
        {
            presence = new JausShortPresenceVector();

            propLinearX = new JausUnsignedShort();
            propLinearY = new JausUnsignedShort();
            propLinearZ = new JausUnsignedShort();

            propRotX = new JausUnsignedShort();
            propRotY = new JausUnsignedShort();
            propRotZ = new JausUnsignedShort();

            resistLinearX = new JausByte();
            resistLinearY = new JausByte();
            resistLinearZ = new JausByte();

            resistRotX = new JausByte();
            resistRotY = new JausByte();
            resistRotZ = new JausByte();
        }

        public bool isFieldSet(int bit)
        {
            return presence.isBitSet(bit);
        }

        public void SetPropulsiveLinearEffortX(double xEffort)
        {
            propLinearX.setFromDouble(xEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_LINEAR_EFFORT_X_BIT);
        }

        public void SetPropulsiveLinearEffortY(double yEffort)
        {
            propLinearY.setFromDouble(yEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_LINEAR_EFFORT_Y_BIT);
        }

        public void SetPropulsiveLinearEffortZ(double zEffort)
        {
            propLinearZ.setFromDouble(zEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_LINEAR_EFFORT_Z_BIT);
        }

        public void SetPropulsiveRotationalEffortX(double xEffort)
        {
            propRotX.setFromDouble(xEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_ROTATIONAL_EFFORT_X_BIT);
        }

        public void SetPropulsiveRotationalEffortY(double yEffort)
        {
            propRotY.setFromDouble(yEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT);
        }

        public void SetPropulsiveRotationalEffortZ(double zEffort)
        {
            propRotZ.setFromDouble(zEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT);
        }

        public void SetResistiveLinearEffortX(double xEffort)
        {
            resistLinearX.setFromDouble(xEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_LINEAR_EFFORT_X_BIT);
        }

        public void SetResistiveLinearEffortY(double yEffort)
        {
            resistLinearY.setFromDouble(yEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_LINEAR_EFFORT_Y_BIT);
        }

        public void SetResistiveLinearEffortZ(double zEffort)
        {
            resistLinearZ.setFromDouble(zEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_LINEAR_EFFORT_Z_BIT);
        }

        public void SetResistiveRotationalEffortX(double xEffort)
        {
            resistRotX.setFromDouble(xEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_ROTATIONAL_EFFORT_X_BIT);
        }

        public void SetResistiveRotationalEffortY(double yEffort)
        {
            resistRotY.setFromDouble(yEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_ROTATIONAL_EFFORT_Y_BIT);
        }

        public void SetResistiveRotationalEffortZ(double zEffort)
        {
            resistRotZ.setFromDouble(zEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_ROTATIONAL_EFFORT_Z_BIT);
        }

        public double GetPropulsiveLinearEffortX()
        {
            return propLinearX.scaleToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetPropulsiveLinearEffortY()
        {
            return propLinearY.scaleToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetPropulsiveLinearEffortZ()
        {
            return propLinearZ.scaleToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetPropulsiveRotationalEffortX()
        {
            return propRotX.scaleToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetPropulsiveRotationalEffortY()
        {
            return propRotY.scaleToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetPropulsiveRotationalEffortZ()
        {
            return propRotZ.scaleToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetResistiveLinearEffortX()
        {
            return resistLinearX.scaleToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public double GetResistiveLinearEffortY()
        {
            return resistLinearY.scaleToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public double GetResistiveLinearEffortZ()
        {
            return resistLinearZ.scaleToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public double GetResistiveRotationalEffortX()
        {
            return resistRotX.scaleToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public double GetResistiveRotationalEffortY()
        {
            return resistRotY.scaleToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public double GetResistiveRotationalEffortZ()
        {
            return resistRotZ.scaleToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public override int GetPayloadSize()
        {
            int payloadSize = 0;

            payloadSize += JausShortPresenceVector.SIZE_BYTES;

            if (presence.isBitSet(PROPULSIVE_LINEAR_EFFORT_X_BIT))
                payloadSize += JausUnsignedShort.SIZE_BYTES;

            if (presence.isBitSet(PROPULSIVE_LINEAR_EFFORT_Y_BIT))
                payloadSize += JausUnsignedShort.SIZE_BYTES;

            if (presence.isBitSet(PROPULSIVE_LINEAR_EFFORT_Z_BIT))
                payloadSize += JausUnsignedShort.SIZE_BYTES;

            if (presence.isBitSet(PROPULSIVE_ROTATIONAL_EFFORT_X_BIT))
                payloadSize += JausUnsignedShort.SIZE_BYTES;

            if (presence.isBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT))
                payloadSize += JausUnsignedShort.SIZE_BYTES;

            if (presence.isBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT))
                payloadSize += JausUnsignedShort.SIZE_BYTES;

            if (presence.isBitSet(RESISTIVE_LINEAR_EFFORT_X_BIT))
                payloadSize += JausByte.SIZE_BYTES;

            if (presence.isBitSet(RESISTIVE_LINEAR_EFFORT_Y_BIT))
                payloadSize += JausByte.SIZE_BYTES;

            if (presence.isBitSet(RESISTIVE_LINEAR_EFFORT_Z_BIT))
                payloadSize += JausByte.SIZE_BYTES;

            if (presence.isBitSet(RESISTIVE_ROTATIONAL_EFFORT_X_BIT))
                payloadSize += JausByte.SIZE_BYTES;

            if (presence.isBitSet(RESISTIVE_ROTATIONAL_EFFORT_Y_BIT))
                payloadSize += JausByte.SIZE_BYTES;

            if (presence.isBitSet(RESISTIVE_ROTATIONAL_EFFORT_Z_BIT))
                payloadSize += JausByte.SIZE_BYTES;

            return payloadSize;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index)
        {
            if (buffer.Length < index + this.MessageSize() - base.MessageSize())
            {
                Console.Error.WriteLine("Query Identification Payload Error: Not enough Size");
                return false; // Not Enough Size
            }

            presence.setFromJausBuffer(buffer, index);

            index += JausShortPresenceVector.SIZE_BYTES;

            if (presence.isBitSet(PROPULSIVE_LINEAR_EFFORT_X_BIT))
            {
                propLinearX.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PROPULSIVE_LINEAR_EFFORT_Y_BIT))
            {
                propLinearY.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PROPULSIVE_LINEAR_EFFORT_Z_BIT))
            {
                propLinearZ.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PROPULSIVE_ROTATIONAL_EFFORT_X_BIT))
            {
                propRotX.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT))
            {
                propRotY.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT))
            {
                propRotZ.setFromJausBuffer(buffer, index);
                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_LINEAR_EFFORT_X_BIT))
            {
                resistLinearX.setFromJausBuffer(buffer, index);
                index += JausByte.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_LINEAR_EFFORT_Y_BIT))
            {
                resistLinearY.setFromJausBuffer(buffer, index);
                index += JausByte.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_LINEAR_EFFORT_Z_BIT))
            {
                resistLinearZ.setFromJausBuffer(buffer, index);
                index += JausByte.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_ROTATIONAL_EFFORT_X_BIT))
            {
                resistRotX.setFromJausBuffer(buffer, index);
                index += JausByte.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_ROTATIONAL_EFFORT_Y_BIT))
            {
                resistRotY.setFromJausBuffer(buffer, index);
                index += JausByte.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_ROTATIONAL_EFFORT_Z_BIT))
            {
                resistRotZ.setFromJausBuffer(buffer, index);
                index += JausByte.SIZE_BYTES;
            }

            return true;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index)
        {
            if (!presence.toJausBuffer(buffer, index))
                return false;

            index += JausShortPresenceVector.SIZE_BYTES;

            if (presence.isBitSet(PROPULSIVE_LINEAR_EFFORT_X_BIT))
            {
                if (!propLinearX.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PROPULSIVE_LINEAR_EFFORT_Y_BIT))
            {
                if (!propLinearY.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PROPULSIVE_LINEAR_EFFORT_Z_BIT))
            {
                if (!propLinearZ.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PROPULSIVE_ROTATIONAL_EFFORT_X_BIT))
            {
                if (!propRotX.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT))
            {
                if (!propRotY.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT))
            {
                if (!propRotZ.toJausBuffer(buffer, index))
                    return false;

                index += JausUnsignedShort.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_LINEAR_EFFORT_X_BIT))
            {
                if (!resistLinearX.toJausBuffer(buffer, index))
                    return false;

                index += JausByte.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_LINEAR_EFFORT_Y_BIT))
            {
                if (!resistLinearY.toJausBuffer(buffer, index))
                    return false;

                index += JausByte.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_LINEAR_EFFORT_Z_BIT))
            {
                if (!resistLinearZ.toJausBuffer(buffer, index))
                    return false;

                index += JausByte.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_ROTATIONAL_EFFORT_X_BIT))
            {
                if (!resistRotX.toJausBuffer(buffer, index))
                    return false;

                index += JausByte.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_ROTATIONAL_EFFORT_Y_BIT))
            {
                if (!resistRotY.toJausBuffer(buffer, index))
                    return false;

                index += JausByte.SIZE_BYTES;
            }

            if (presence.isBitSet(RESISTIVE_ROTATIONAL_EFFORT_Z_BIT))
            {
                if (!resistRotZ.toJausBuffer(buffer, index))
                    return false;

                index += JausByte.SIZE_BYTES;
            }

            return true;
        }
    }
}