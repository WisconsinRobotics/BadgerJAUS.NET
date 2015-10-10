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

namespace BadgerJaus.Messages.Management
{
    public class ReportStatus : Message
    {
        JausByte status;
        JausUnsignedInteger reserved;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_STATUS; }
        }

        protected override void InitFieldData()
        {
            status = new JausByte();
            reserved = new JausUnsignedInteger();
        }

        public void SetStatus(int status)
        {
            this.status.Value = status;
        }

        public int GetStatus()
        {
            return (int)status.Value;
        }

        public override int GetPayloadSize()
        {
            return JausBaseType.BYTE_BYTE_SIZE + JausBaseType.INT_BYTE_SIZE;
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!status.Serialize(buffer, indexOffset, out indexOffset))
            {
                return false;
            }

            return reserved.Serialize(buffer, indexOffset, out indexOffset);
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (buffer.Length < index + this.MessageSize() - base.MessageSize())
            {
                //Console.Error.WriteLine("Report Status Payload Error: Not enough Size");
                return false; // Not Enough Size
            }

            status.Deserialize(buffer, indexOffset, out indexOffset);
            reserved.Deserialize(buffer, indexOffset, out indexOffset);

            return true;
        }
    }
}