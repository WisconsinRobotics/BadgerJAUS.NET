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
namespace BadgerJaus.Messages.PrimitiveManipulator
{
    public class ReportJointPosition : Message
    {
        private static int REVOLUTE_MIN = (-8) * (System.Convert.ToInt32(System.Math.PI) / 180);
        private static int REVOLUTE_MAX = (8) * (System.Convert.ToInt32(System.Math.PI) / 180);
        private JausUnsignedInteger revolute;

        private static int PRISMATIC_MIN = -10;
        private static int PRISMATIC_MAX = 10;
        private JausUnsignedInteger prismatic;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_JOINT_POSITIONS; }
        }

        public void SetRevolute(int value)
        {
            revolute.setFromDouble(value, REVOLUTE_MIN, REVOLUTE_MAX);
        }

        public int GetRevolute()
        {
            return System.Convert.ToInt32(revolute.getValue());
        }

        public void SetPrismatic(int value)
        {
            prismatic.setFromDouble(value, PRISMATIC_MIN, PRISMATIC_MAX);
        }

        public int GetPrismatic()
        {
            return System.Convert.ToInt32(prismatic.getValue());
        }

        public override int GetPayloadSize()
        {
            return JausUnsignedInteger.SIZE_BYTES;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!revolute.toJausBuffer(buffer, indexOffset)) return false;
            indexOffset += JausUnsignedInteger.SIZE_BYTES;
            if (!prismatic.toJausBuffer(buffer, indexOffset)) return false;
            indexOffset += JausUnsignedInteger.SIZE_BYTES;

            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!revolute.setFromJausBuffer(buffer, indexOffset)) return false;
            indexOffset += JausUnsignedInteger.SIZE_BYTES;
            if (!prismatic.setFromJausBuffer(buffer, indexOffset)) return false;
            indexOffset += JausUnsignedInteger.SIZE_BYTES;
            
            return true;
        }
    }
}