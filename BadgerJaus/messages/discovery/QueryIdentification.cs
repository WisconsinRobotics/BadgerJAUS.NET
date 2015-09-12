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

namespace BadgerJaus.Messages.Discovery
{
    public class QueryIdentification : Message
    {
        /*
         * The query identification command was changed to 2B00h from
         * D2A4h from the 3.2 specification.
         */

        // Spec Defined Constants

        // Query Type Values
        // 0, 5 - 255 Reserved
        public const int SYSTEM_IDENTIFICATION = 1;
        public const int SUBSYSTEM_IDENTIFICATION = 2;
        public const int NODE_IDENTIFICATION = 3;
        public const int COMPONENT_IDENTIFICATION = 4;

        private String[] queryTypes =
		{
		 "SYSTEM_IDENTIFICATION",
		 "SUBSYSTEM_IDENTIFICATION",
		 "NODE_IDENTIFICATION",
		 "COMPONENT_IDENTIFICATION"
		 };




        // Message Fields
        protected JausByte queryType;

        protected override int CommandCode
        {
            get { return JausCommandCode.QUERY_IDENTIFICATION; }
        }

        protected override void InitFieldData()
        {
            queryType = new JausByte(QueryIdentification.SYSTEM_IDENTIFICATION);
        }

        // Getters and Setters
        public int getQueryType()
        {
            return queryType.getValue();
        }

        public void setQueryType(int queryType)
        {
            if (queryType > 0 && queryType < 5)
                this.queryType.setValue(queryType);
        }

        public override int GetPayloadSize()
        {
            return JausByte.SIZE_BYTES;
        }

        // Packs Message Fields into byte[] then passes array to super class
        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!queryType.toJausBuffer(buffer, indexOffset)) return false;
            indexOffset += JausByte.SIZE_BYTES;

            return true;
        }

        // Takes Super's payload, and unpacks it into Message Fields
        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            queryType.setValue(buffer[indexOffset]);
            indexOffset += JausByte.SIZE_BYTES;
            return true;
        }

        public override String toString()
        {
            String str = base.toString();
            str += "Query Type: " + queryTypes[queryType.getValue() - 1] + "\n";
            return str;
        }
    }
}