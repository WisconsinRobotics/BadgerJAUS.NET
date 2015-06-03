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

namespace BadgerJaus.Messages.PrimitiveDriver
{
    public class QueryWrenchEffort : Message
    {
        JausShortPresenceVector presence;

        protected override int CommandCode
        {
            get { return JausCommandCode.QUERY_WRENCH_EFFORT; }
        }

        protected override void InitFieldData()
        {
            presence = new JausShortPresenceVector();
        }

        public bool isBitSet(int bit)
        {
            return presence.isBitSet(bit);
        }

        public void SetField(int bit)
        {
            presence.setBit(bit);
        }

        public override int GetPayloadSize()
        {
            return JausShortPresenceVector.SIZE_BYTES;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index)
        {
            if (buffer.Length < index + GetPayloadSize())
            {
                Console.Error.WriteLine("Query Identification Payload Error: Not enough Size");
                return false; // Not Enough Size
            }

            return presence.setFromJausBuffer(buffer, index);
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index)
        {
            return presence.toJausBuffer(buffer, index);
        }
    }
}