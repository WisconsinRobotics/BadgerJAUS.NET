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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadgerJaus.Util;

namespace BadgerJaus.Messages.PanTiltJointVelocityDriver
{
    public class ReportCommandedPanTiltJointVelocity : Message
    {
        JausUnsignedInteger Joint1velocity;
        JausUnsignedInteger Joint2velocity;

        public const double MIN_LIMIT = -10 * Math.PI;
        public const double MAX_LIMIT = 10 * Math.PI;

        protected override int CommandCode
        {
            get
            {
                return JausCommandCode.REPORT_COMMANDED_PAN_TILT_JOINT_VELOCITY;
            }
        }

        protected override void InitFieldData()
        {
            Joint1velocity = new JausUnsignedInteger();
            Joint2velocity = new JausUnsignedInteger();
        }

        public void SetJoint1velocity(double velocity1)
        {
            Joint1velocity.SetValueFromDouble(velocity1, MIN_LIMIT, MAX_LIMIT);
        }

        public void SetJoint2velocity(double velocity2)
        {
            Joint2velocity.SetValueFromDouble(velocity2, MIN_LIMIT, MAX_LIMIT);
        }

        public double GetJoint1velocity(double velocity1)
        {
            return Joint1velocity.ScaleValueToDouble(MIN_LIMIT, MAX_LIMIT);
        }

        public double GetJoint2velocity(double velocity2)
        {
            return Joint2velocity.ScaleValueToDouble(MIN_LIMIT, MAX_LIMIT);
        }

        public override int GetPayloadSize()
        {
            return JausBaseType.INT_BYTE_SIZE * 2;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!Joint1velocity.Deserialize(buffer, indexOffset, out indexOffset))
                return false;
            if (!Joint2velocity.Deserialize(buffer, indexOffset, out indexOffset))
                return false;
            return true;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!Joint1velocity.Serialize(buffer, indexOffset, out indexOffset))
                return false;
            if (!Joint2velocity.Serialize(buffer, indexOffset, out indexOffset))
                return false;
            return true;
        }
    }
}
