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

namespace BadgerJaus.Messages.Discovery
{
    public class ReportServices : Message
    {
        Dictionary<long, Node> nodeList;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_SERVICES; }
        }

        public override int GetPayloadSize()
        {
            int payloadSize = 0;
            if (nodeList == null)
                return 0;
            if (nodeList.Count == 0)
                return 0;
            payloadSize += JausBaseType.BYTE_BYTE_SIZE;
            foreach(Node node in nodeList.Values)
            {
                payloadSize += node.GetPayloadSize();
            }
            return payloadSize;
        }

        // Packs Message Fields into byte[] then passes array to super class
        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            JausByte nodeCount = new JausByte();
            indexOffset = index;
            nodeCount.Value = nodeList.Count;
            nodeCount.Serialize(buffer, indexOffset, out indexOffset);
            foreach (Node node in nodeList.Values)
            {
                if (!node.PayloadToJausBuffer(buffer, indexOffset, out indexOffset))
                    return false;
            }

            return true;
        }

        // Takes Super's payload, and unpacks it into Message Fields
        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            int nodeIndex;
            //int componentIndex;
            Node node;
            JausByte nodeCount = new JausByte();
            indexOffset = index;
            nodeCount.Deserialize(buffer, indexOffset, out indexOffset);
            for (nodeIndex = 0; nodeIndex < nodeCount.Value; ++nodeIndex)
            {
                node = new Node(0);
                // Need deserialize option
                //nodeID.Deserialize(buffer, indexOffset, index, out indexOffset);
                //componentID.Deserialize(buffer, index, out indexOffset);
            }

            return true;
        }

        public override String ToString()
        {
            String str = base.ToString();
            //str += "Query Type: " + queryTypes[queryType.getValue() - 1] + "\n";
            return str;
        }

        public Dictionary<long, Node> NodeList
        {
            get { return nodeList; }
            set { nodeList = value; }
        }
    }
}