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
using System.Collections.Generic;

using BadgerJaus.Util;

namespace BadgerJaus.Messages.JointPositionsDriver
{
    public class ReportJointPosition : Message
    {
        public const double REVOLUTE_MIN = -8 * System.Math.PI;
        public const double REVOLUTE_MAX = 8 * System.Math.PI;

        public const double PRISMATIC_MIN = -10;
        public const double PRISMATIC_MAX = 10;

        List<JausUnsignedInteger> prismaticList;
        List<JausUnsignedInteger> revoluteList;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_JOINT_POSITIONS; }
        }

        protected override void InitFieldData()
        {
            prismaticList = new List<JausUnsignedInteger>();
            revoluteList = new List<JausUnsignedInteger>();
        }

        public void AddRevolute(double value)
        {
            JausUnsignedInteger revolute = new JausUnsignedInteger();
            revolute.setFromDouble(value, REVOLUTE_MIN, REVOLUTE_MAX);
            revoluteList.Add(revolute);
        }

        public List<JausUnsignedInteger> RevoluteList
        {
            get { return revoluteList; }
        }

        public void AddPrismatic(double value)
        {
            JausUnsignedInteger prismatic = new JausUnsignedInteger();
            prismatic.setFromDouble(value, PRISMATIC_MIN, PRISMATIC_MAX);
            prismaticList.Add(prismatic);
        }

        public List<JausUnsignedInteger> PrismaticList
        {
            get { return prismaticList; }
        }

        public override int GetPayloadSize()
        {
            int listCount = (prismaticList.Count > revoluteList.Count) ? prismaticList.Count : revoluteList.Count;
            return JausBaseType.BYTE_BYTE_SIZE + JausBaseType.INT_BYTE_SIZE * listCount;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            int listCount;
            int arrayIndex;
            JausByte arrayCount;
            JausUnsignedInteger blankInteger = new JausUnsignedInteger(0);
            indexOffset = index;
            listCount = (prismaticList.Count > revoluteList.Count) ? prismaticList.Count : revoluteList.Count;
            arrayCount = new JausByte(listCount);
            arrayCount.Serialize(buffer, indexOffset, out indexOffset);
            for (arrayIndex = 0; arrayIndex < listCount; ++arrayIndex)
            {
                if (arrayIndex < revoluteList.Count)
                {
                    revoluteList[arrayIndex].Serialize(buffer, indexOffset, out indexOffset);
                }
                else
                {
                    blankInteger.Serialize(buffer, indexOffset, out indexOffset);
                }

                if (arrayIndex < prismaticList.Count)
                {
                    prismaticList[arrayIndex].Serialize(buffer, indexOffset, out indexOffset);
                }
                else
                {
                    blankInteger.Serialize(buffer, indexOffset, out indexOffset);
                }
            }

            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            JausByte arrayCount = new JausByte();
            JausUnsignedInteger revolute;
            JausUnsignedInteger prismatic;
            int arrayIndex;
            indexOffset = index;
            arrayCount.Deserialize(buffer, indexOffset, out indexOffset);
            for (arrayIndex = 0; arrayIndex < arrayCount.Value; ++arrayIndex)
            {
                revolute = new JausUnsignedInteger();
                revolute.Deserialize(buffer, indexOffset, out indexOffset);
                revoluteList.Add(revolute);
                prismatic = new JausUnsignedInteger();
                prismatic.Deserialize(buffer, indexOffset, out indexOffset);
                prismaticList.Add(prismatic);
            }

            return true;
        }
    }
}