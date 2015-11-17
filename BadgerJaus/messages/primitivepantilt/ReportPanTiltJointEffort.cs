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

namespace BadgerJaus.Messages.primitivepantilt
{
    public class ReportPanTiltJointEffort : QueryPanTiltJointEffort
    {
        JausUnsignedInteger Joint1Effort;
        JausUnsignedInteger Joint2Effort;

        public const int LOWER_LIMIT = -100;
        public const int UPPER_LIMIT = 100;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_PAN_TILT_JOINT_EFFORT; }
        }

        protected override void InitFieldData()
        {
            Joint1Effort = new JausUnsignedInteger();
            Joint2Effort = new JausUnsignedInteger();
        }

        public void setJoint1Effort(double joint1)
        {
            Joint1Effort.setFromDouble(joint1, LOWER_LIMIT, UPPER_LIMIT);
        }

        public void setJoint2Effort(double joint2)
        {
            Joint2Effort.setFromDouble(joint2, LOWER_LIMIT, UPPER_LIMIT);
        }

        public double getJoint1Effort()
        {
            return Joint1Effort.ScaleValueToDouble(LOWER_LIMIT, UPPER_LIMIT);
        }

        public double getJoint2Effort()
        {
            return Joint2Effort.ScaleValueToDouble(LOWER_LIMIT, UPPER_LIMIT);
        }

        public override int GetPayloadSize()
        {
            return JausBaseType.INT_BYTE_SIZE * 2;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if(!Joint1Effort.Deserialize(buffer, indexOffset, out indexOffset))
            {
                return false;
            }
            if (!Joint2Effort.Deserialize(buffer, indexOffset, out indexOffset))
            {
                return false;
            }
            return true;
        }
        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!Joint1Effort.Serialize(buffer, indexOffset, out indexOffset))
            {
                return false;
            }
            if (!Joint2Effort.Serialize(buffer, indexOffset, out indexOffset))
            {
                return false;
            }
            return true;
        }
    }
}
