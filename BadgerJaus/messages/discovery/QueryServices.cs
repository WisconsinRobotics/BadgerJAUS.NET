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
    public class QueryServices : Message
    {
        private const int MAX_DATA_SIZE_BYTES = JausByte.SIZE_BYTES * 2;

        JausByte nodeID;
        JausByte componentID;

        protected override int CommandCode
        {
            get { return JausCommandCode.QUERY_SERVICES; }
        }

        protected override void InitFieldData()
        {
            nodeID = new JausByte();
            componentID = new JausByte();
        }

        // Getters and Setters
        public int GetNodeID()
        {
            return nodeID.getValue();
        }

        public int GetComponentID()
        {
            return componentID.getValue();
        }

        public void SetNodeID(int nodeID)
        {
            if (nodeID > 0 && nodeID < 256)
                this.nodeID.setValue(nodeID);
        }

        public void SetComponentID(int componentID)
        {
            if (componentID > 0 && componentID < 256)
                this.componentID.setValue(componentID);
        }

        public override int GetPayloadSize()
        {
            return MAX_DATA_SIZE_BYTES;
        }

        // Packs Message Fields into byte[] then passes array to super class
        protected override bool PayloadToJausBuffer(byte[] buffer, int index)
        {
            if (!nodeID.toJausBuffer(buffer, index))
            {
                return false; // Failed to add query type to buffer;
            }

            index += JausByte.SIZE_BYTES;

            if (!componentID.toJausBuffer(buffer, index))
            {
                return false; // Failed to add query type to buffer;
            }

            return true;
        }

        // Takes Super's payload, and unpacks it into Message Fields
        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index)
        {
            if (buffer.Length < index + GetPayloadSize())
            {
                Console.Error.WriteLine("Query Services Payload Error: Not enough Size.");
                Console.Error.WriteLine("Index: " + index);
                return false; // Not Enough Size
            }

            nodeID.setValue(buffer[index]);
            componentID.setValue(buffer[index + JausByte.SIZE_BYTES]);
            return true;
        }

        public override String toString()
        {
            String str = base.toString();
            //str += "Query Type: " + queryTypes[queryType.getValue() - 1] + "\n";
            return str;
        }
    }
}