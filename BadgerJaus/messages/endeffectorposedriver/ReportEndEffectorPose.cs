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

namespace BadgerJaus.Messages.EndEffectorPoseDriver
{
    public class ReportEndEffectorPose : Message
    {
        JausUnsignedInteger ToolPointCoordinateX;
        JausUnsignedInteger ToolPointCoordinateY;
        JausUnsignedInteger ToolPointCoordinateZ;
        JausUnsignedInteger dComponentOfUnitQuaternionQ;
        JausUnsignedInteger aComponentOfUnitQuaternionQ;
        JausUnsignedInteger bComponentOfUnitQuaternionQ;
        JausUnsignedInteger cComponentOfUnitQuaternionQ;

        public const int COORDINATE_MIN = -30;
        public const int COORDINATE_MAX = 30;
        public const int COMPONENT_MIN = -1;
        public const int COMPONENT_MAX = 1;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_END_EFFECTOR_POSE; }
        }

        protected override void InitFieldData()
        {
            ToolPointCoordinateX = new JausUnsignedInteger();
            ToolPointCoordinateY = new JausUnsignedInteger();
            ToolPointCoordinateZ = new JausUnsignedInteger();
            dComponentOfUnitQuaternionQ = new JausUnsignedInteger();
            aComponentOfUnitQuaternionQ = new JausUnsignedInteger();
            bComponentOfUnitQuaternionQ = new JausUnsignedInteger();
            cComponentOfUnitQuaternionQ = new JausUnsignedInteger();
        }

        public void SetToolPointCoordinateX(double CoordinateX)
        {
            ToolPointCoordinateX.setFromDouble(CoordinateX, COORDINATE_MIN, COORDINATE_MAX);
        }

        public void SetToolPointCoordinateY(double CoordinateY)
        {
            ToolPointCoordinateY.setFromDouble(CoordinateY, COORDINATE_MIN, COORDINATE_MAX);
        }

        public void SetToolPointCoordinateZ(double CoordinateZ)
        {
            ToolPointCoordinateZ.setFromDouble(CoordinateZ, COORDINATE_MIN, COORDINATE_MAX);
        }

        public void SetdComponentOfUnitQuaternionQ(double dComponent)
        {
            dComponentOfUnitQuaternionQ.setFromDouble(dComponent, COMPONENT_MIN, COMPONENT_MAX);
        }

        public void SetaComponentOfUnitQuaternionQ(double aComponent)
        {
            aComponentOfUnitQuaternionQ.setFromDouble(aComponent, COMPONENT_MIN, COMPONENT_MAX);
        }

        public void SetbComponentOfUnitQuaternionQ(double bComponent)
        {
            bComponentOfUnitQuaternionQ.setFromDouble(bComponent, COMPONENT_MIN, COMPONENT_MAX);
        }

        public void SetcComponentOfUnitQuaternionQ(double cComponent)
        {
            cComponentOfUnitQuaternionQ.setFromDouble(cComponent, COMPONENT_MIN, COMPONENT_MAX);
        }

        public double GetToolPointCoordinateX()
        {
            return ToolPointCoordinateX.ScaleValueToDouble(COORDINATE_MIN, COORDINATE_MAX);
        }

        public double GetToolPointCoordinateY()
        {
            return ToolPointCoordinateY.ScaleValueToDouble(COORDINATE_MIN, COORDINATE_MAX);
        }

        public double GetToolPointCoordinateZ()
        {
            return ToolPointCoordinateZ.ScaleValueToDouble(COORDINATE_MIN, COORDINATE_MAX);
        }

        public double GetdComponentOfUnitQuaternionQ()
        {
            return dComponentOfUnitQuaternionQ.ScaleValueToDouble(COMPONENT_MIN, COMPONENT_MAX);
        }

        public double GetaComponentOfUnitQuaternionQ()
        {
            return aComponentOfUnitQuaternionQ.ScaleValueToDouble(COMPONENT_MIN, COMPONENT_MAX);
        }

        public double GetbComponentOfUnitQuaternionQ()
        {
            return bComponentOfUnitQuaternionQ.ScaleValueToDouble(COMPONENT_MIN, COMPONENT_MAX);
        }

        public double GetcComponentOfUnitQuaternionQ()
        {
            return cComponentOfUnitQuaternionQ.ScaleValueToDouble(COMPONENT_MIN, COMPONENT_MAX);
        }

        public override int GetPayloadSize()
        {
            return JausBaseType.INT_BYTE_SIZE * 7;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            if (!ToolPointCoordinateX.Deserialize(buffer, index, out indexOffset))
                return false;

            if (!ToolPointCoordinateY.Deserialize(buffer, indexOffset, out indexOffset))
                return false;

            if (!ToolPointCoordinateZ.Deserialize(buffer, indexOffset, out indexOffset))
                return false;

            if (!dComponentOfUnitQuaternionQ.Deserialize(buffer, indexOffset, out indexOffset))
                return false;

            if (!aComponentOfUnitQuaternionQ.Deserialize(buffer, indexOffset, out indexOffset))
                return false;

            if (!bComponentOfUnitQuaternionQ.Deserialize(buffer, indexOffset, out indexOffset))
                return false;

            if (!cComponentOfUnitQuaternionQ.Deserialize(buffer, indexOffset, out indexOffset))
                return false;

            return true;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            if (!ToolPointCoordinateX.Serialize(buffer, index, out indexOffset))
                return false;

            if (!ToolPointCoordinateY.Serialize(buffer, indexOffset, out indexOffset))
                return false;

            if (!ToolPointCoordinateZ.Serialize(buffer, indexOffset, out indexOffset))
                return false;

            if (!dComponentOfUnitQuaternionQ.Serialize(buffer, indexOffset, out indexOffset))
                return false;

            if (!aComponentOfUnitQuaternionQ.Serialize(buffer, indexOffset, out indexOffset))
                return false;

            if (!bComponentOfUnitQuaternionQ.Serialize(buffer, indexOffset, out indexOffset))
                return false;

            if (!cComponentOfUnitQuaternionQ.Serialize(buffer, indexOffset, out indexOffset))
                return false;

            return true;
        }

        // This message provides the receiver with the current pose of the end effector. The message data for this message are identical to ID 0610h:SetEndEffectorPose
        // JausCommandCode.REPORT_END_EFFECTOR_POSE;
    }
}
