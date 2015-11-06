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

using BadgerJaus.Util;

namespace BadgerJaus.Messages.JointVelocity
{
    public class ReportJointVelocity : QueryJointVelocity
    {
        List<JausUnsignedInteger> jointVelocityRevolute;
        List<JausUnsignedInteger> jointVelocityPrismatic;

        public const double MAX_REVOLUTE = 10 * Math.PI;
        public const double MIN_REVOLUTE = -10 * Math.PI;
        public const int MAX_PRISMATIC = 5;
        public const int MIN_PRISMATIC = -5;

        
        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_JOINT_VELOCITIES; }
        }

        protected override void InitFieldData()
        {
            jointVelocityRevolute = new List<JausUnsignedInteger>();
            jointVelocityPrismatic = new List<JausUnsignedInteger>();
        }

        public List<JausUnsignedInteger> RevoluteList()
        {
            return jointVelocityRevolute;
        }

        public List<JausUnsignedInteger> PrismaticList()
        {
            return jointVelocityPrismatic;
        }

        public void AddRevoluteVelocity(double value)
        {
            JausUnsignedInteger revolute = new JausUnsignedInteger();
            revolute.SetValueFromDouble(value, MIN_REVOLUTE, MAX_REVOLUTE);
            jointVelocityRevolute.Add(revolute);
        }

        public void AddPrismaticVelocity(double value)
        {
            JausUnsignedInteger prismatic = new JausUnsignedInteger();
            prismatic.SetValueFromDouble(value, MIN_PRISMATIC, MAX_PRISMATIC);
            jointVelocityPrismatic.Add(prismatic);
        }

        public override int GetPayloadSize()
        {
            int listCount = (jointVelocityPrismatic.Count > jointVelocityRevolute.Count) ? jointVelocityPrismatic.Count : jointVelocityRevolute.Count;
            return JausBaseType.BYTE_BYTE_SIZE + JausBaseType.INT_BYTE_SIZE * listCount;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            JausUnsignedInteger zero = new JausUnsignedInteger(0);
            indexOffset = index;
            int listCount = (jointVelocityPrismatic.Count > jointVelocityRevolute.Count) ? jointVelocityPrismatic.Count : jointVelocityRevolute.Count;
            JausByte arrayCount = new JausByte(listCount);
            arrayCount.Serialize(buffer, indexOffset, out indexOffset);

            for (int i = 0; i < listCount; i++)
            {
                if (i < jointVelocityRevolute.Count)
                {
                    jointVelocityRevolute[i].Serialize(buffer, indexOffset, out indexOffset);
                }
                else
                {
                    zero.Serialize(buffer, indexOffset, out indexOffset);
                }
                if (i < jointVelocityPrismatic.Count)
                {
                    jointVelocityPrismatic[i].Serialize(buffer, indexOffset, out indexOffset);
                }
                else
                {
                    zero.Serialize(buffer, indexOffset, out indexOffset);
                }
            }
            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            JausByte arrayCount = new JausByte();
            indexOffset = index;
            arrayCount.Deserialize(buffer, indexOffset, out indexOffset);
            JausUnsignedInteger revolute = new JausUnsignedInteger();
            JausUnsignedInteger prismatic = new JausUnsignedInteger();
            for (int i = 0; i < arrayCount.Value; i++)
            {
                revolute.Deserialize(buffer, indexOffset, out indexOffset);
                jointVelocityRevolute.Add(revolute);
                prismatic.Deserialize(buffer, indexOffset, out indexOffset);
                jointVelocityPrismatic.Add(prismatic);
            }
            return true;
        }

    }
}
