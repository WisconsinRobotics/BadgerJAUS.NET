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
namespace BadgerJaus.Messages.PrimitiveDriver
{
    public class ReportWrenchEffort : QueryWrenchEffort
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
            get { return JausCommandCode.REPORT_WRENCH_EFFORT; }
        }

        protected override void InitFieldData()
        {
            base.InitFieldData();

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
            return presence.IsBitSet(bit);
        }

        public void SetPropulsiveLinearEffortX(double xEffort)
        {
            propLinearX.SetValueFromDouble(xEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_LINEAR_EFFORT_X_BIT);
        }

        public void SetPropulsiveLinearEffortY(double yEffort)
        {
            propLinearY.SetValueFromDouble(yEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_LINEAR_EFFORT_Y_BIT);
        }

        public void SetPropulsiveLinearEffortZ(double zEffort)
        {
            propLinearZ.SetValueFromDouble(zEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_LINEAR_EFFORT_Z_BIT);
        }

        public void SetPropulsiveRotationalEffortX(double xEffort)
        {
            propRotX.SetValueFromDouble(xEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_ROTATIONAL_EFFORT_X_BIT);
        }

        public void SetPropulsiveRotationalEffortY(double yEffort)
        {
            propRotY.SetValueFromDouble(yEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT);
        }

        public void SetPropulsiveRotationalEffortZ(double zEffort)
        {
            propRotZ.SetValueFromDouble(zEffort, PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
            presence.setBit(PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT);
        }

        public void SetResistiveLinearEffortX(double xEffort)
        {
            resistLinearX.SetValueFromDouble(xEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_LINEAR_EFFORT_X_BIT);
        }

        public void SetResistiveLinearEffortY(double yEffort)
        {
            resistLinearY.SetValueFromDouble(yEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_LINEAR_EFFORT_Y_BIT);
        }

        public void SetResistiveLinearEffortZ(double zEffort)
        {
            resistLinearZ.SetValueFromDouble(zEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_LINEAR_EFFORT_Z_BIT);
        }

        public void SetResistiveRotationalEffortX(double xEffort)
        {
            resistRotX.SetValueFromDouble(xEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_ROTATIONAL_EFFORT_X_BIT);
        }

        public void SetResistiveRotationalEffortY(double yEffort)
        {
            resistRotY.SetValueFromDouble(yEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_ROTATIONAL_EFFORT_Y_BIT);
        }

        public void SetResistiveRotationalEffortZ(double zEffort)
        {
            resistRotZ.SetValueFromDouble(zEffort, RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
            presence.setBit(RESISTIVE_ROTATIONAL_EFFORT_Z_BIT);
        }

        public double GetPropulsiveLinearEffortX()
        {
            return propLinearX.ScaleValueToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetPropulsiveLinearEffortY()
        {
            return propLinearY.ScaleValueToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetPropulsiveLinearEffortZ()
        {
            return propLinearZ.ScaleValueToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetPropulsiveRotationalEffortX()
        {
            return propRotX.ScaleValueToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetPropulsiveRotationalEffortY()
        {
            return propRotY.ScaleValueToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetPropulsiveRotationalEffortZ()
        {
            return propRotZ.ScaleValueToDouble(PROPULSIVE_EFFORT_MIN, PROPULSIVE_EFFORT_MAX);
        }

        public double GetResistiveLinearEffortX()
        {
            return resistLinearX.ScaleValueToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public double GetResistiveLinearEffortY()
        {
            return resistLinearY.ScaleValueToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public double GetResistiveLinearEffortZ()
        {
            return resistLinearZ.ScaleValueToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public double GetResistiveRotationalEffortX()
        {
            return resistRotX.ScaleValueToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public double GetResistiveRotationalEffortY()
        {
            return resistRotY.ScaleValueToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public double GetResistiveRotationalEffortZ()
        {
            return resistRotZ.ScaleValueToDouble(RESISTIVE_EFFORT_MIN, RESISTIVE_EFFORT_MAX);
        }

        public override int GetPayloadSize()
        {
            int payloadSize = 0;

            payloadSize += base.GetPayloadSize();

            if (presence.IsBitSet(PROPULSIVE_LINEAR_EFFORT_X_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(PROPULSIVE_LINEAR_EFFORT_Y_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(PROPULSIVE_LINEAR_EFFORT_Z_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(PROPULSIVE_ROTATIONAL_EFFORT_X_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT))
                payloadSize += JausBaseType.SHORT_BYTE_SIZE;

            if (presence.IsBitSet(RESISTIVE_LINEAR_EFFORT_X_BIT))
                payloadSize += JausBaseType.BYTE_BYTE_SIZE;

            if (presence.IsBitSet(RESISTIVE_LINEAR_EFFORT_Y_BIT))
                payloadSize += JausBaseType.BYTE_BYTE_SIZE;

            if (presence.IsBitSet(RESISTIVE_LINEAR_EFFORT_Z_BIT))
                payloadSize += JausBaseType.BYTE_BYTE_SIZE;

            if (presence.IsBitSet(RESISTIVE_ROTATIONAL_EFFORT_X_BIT))
                payloadSize += JausBaseType.BYTE_BYTE_SIZE;

            if (presence.IsBitSet(RESISTIVE_ROTATIONAL_EFFORT_Y_BIT))
                payloadSize += JausBaseType.BYTE_BYTE_SIZE;

            if (presence.IsBitSet(RESISTIVE_ROTATIONAL_EFFORT_Z_BIT))
                payloadSize += JausBaseType.BYTE_BYTE_SIZE;

            return payloadSize;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            base.SetPayloadFromJausBuffer(buffer, index, out indexOffset);

            if (presence.IsBitSet(PROPULSIVE_LINEAR_EFFORT_X_BIT))
            {
                propLinearX.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(PROPULSIVE_LINEAR_EFFORT_Y_BIT))
            {
                propLinearY.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(PROPULSIVE_LINEAR_EFFORT_Z_BIT))
            {
                propLinearZ.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(PROPULSIVE_ROTATIONAL_EFFORT_X_BIT))
            {
                propRotX.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT))
            {
                propRotY.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT))
            {
                propRotZ.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(RESISTIVE_LINEAR_EFFORT_X_BIT))
            {
                resistLinearX.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(RESISTIVE_LINEAR_EFFORT_Y_BIT))
            {
                resistLinearY.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(RESISTIVE_LINEAR_EFFORT_Z_BIT))
            {
                resistLinearZ.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(RESISTIVE_ROTATIONAL_EFFORT_X_BIT))
            {
                resistRotX.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(RESISTIVE_ROTATIONAL_EFFORT_Y_BIT))
            {
                resistRotY.Deserialize(buffer, indexOffset, out indexOffset);
            }

            if (presence.IsBitSet(RESISTIVE_ROTATIONAL_EFFORT_Z_BIT))
            {
                resistRotZ.Deserialize(buffer, indexOffset, out indexOffset);
            }

            return true;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            base.PayloadToJausBuffer(buffer, index, out indexOffset);

            if (presence.IsBitSet(PROPULSIVE_LINEAR_EFFORT_X_BIT))
            {
                if (!propLinearX.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(PROPULSIVE_LINEAR_EFFORT_Y_BIT))
            {
                if (!propLinearY.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(PROPULSIVE_LINEAR_EFFORT_Z_BIT))
            {
                if (!propLinearZ.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(PROPULSIVE_ROTATIONAL_EFFORT_X_BIT))
            {
                if (!propRotX.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT))
            {
                if (!propRotY.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT))
            {
                if (!propRotZ.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(RESISTIVE_LINEAR_EFFORT_X_BIT))
            {
                if (!resistLinearX.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(RESISTIVE_LINEAR_EFFORT_Y_BIT))
            {
                if (!resistLinearY.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(RESISTIVE_LINEAR_EFFORT_Z_BIT))
            {
                if (!resistLinearZ.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(RESISTIVE_ROTATIONAL_EFFORT_X_BIT))
            {
                if (!resistRotX.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(RESISTIVE_ROTATIONAL_EFFORT_Y_BIT))
            {
                if (!resistRotY.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            if (presence.IsBitSet(RESISTIVE_ROTATIONAL_EFFORT_Z_BIT))
            {
                if (!resistRotZ.Serialize(buffer, indexOffset, out indexOffset))
                    return false;
            }

            return true;
        }
    }
}