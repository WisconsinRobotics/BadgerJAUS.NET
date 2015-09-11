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

namespace BadgerJaus.Messages.Control
{
    public class ReportAuthority : Message
    {
        // Size of payload not including Message Code
        private const int MAX_DATA_SIZE_BYTES = JausByte.SIZE_BYTES;


        // Message Fields
        JausByte authorityCode;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_AUTHORITY; }
        }

        protected override void InitFieldData()
        {
            authorityCode = new JausByte();
        }

        // Getters and Setters
        public int GetAuthorityCode()
        {
            return authorityCode.getValue();
        }

        public void SetAuthorityCode(int queryType)
        {
            this.authorityCode.setValue(queryType);
        }

        public override int GetPayloadSize()
        {
            return MAX_DATA_SIZE_BYTES;
        }

        // Packs Message Fields into byte[] then passes array to super class
        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!authorityCode.toJausBuffer(buffer, indexOffset))
            {
                return false;
            }
            indexOffset += JausByte.SIZE_BYTES;

            return true;
        }

        // Takes Super's payload, and unpacks it into Message Fields
        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            authorityCode.setFromJausBuffer(buffer, indexOffset);
            indexOffset = JausByte.SIZE_BYTES;

            return true;
        }

        // reports the size of the "Current" message (based on which fields are set)
        // could return different values is payload size is variable for given message
        // will return static size if message size is static
        public override int MessageSize()
        {
            return base.MessageSize() + ReportAuthority.MAX_DATA_SIZE_BYTES;
        }

        public override String toString()
        {
            String str = base.toString();
            //str += "Query Type: " + queryTypes[queryType.getValue() - 1] + "\n";
            return str;
        }
    }
}