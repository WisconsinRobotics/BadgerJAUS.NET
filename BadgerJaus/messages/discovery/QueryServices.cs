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
    public class QueryServices : Message
    {
        //private const int MAX_DATA_SIZE_BYTES = JausBaseType.BYTE_BYTE_SIZE * 2;
        List<Node> nodeList;

        protected override int CommandCode
        {
            get { return JausCommandCode.QUERY_SERVICES; }
        }

        protected override void InitFieldData()
        {
            nodeList = new List<Node>();
        }

        // Getters and Setters
        public void AddNode(Node node)
        {
            nodeList.Add(node);
        }

        public override int GetPayloadSize()
        {
            int size = 1;
            foreach(Node node in nodeList)
            {
                size += node.GetPayloadSize();
            }
            return size;
        }

        // Packs Message Fields into byte[] then passes array to super class
        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            JausByte nodeCount = new JausByte(nodeList.Count);
            indexOffset = index;
            nodeCount.Serialize(buffer, indexOffset, out indexOffset);
            
            foreach(Node node in nodeList)
            {
                if (!node.PayloadToJausBuffer(buffer, indexOffset, out indexOffset, false))
                    return false;
            }

            return true;
        }

        // Takes Super's payload, and unpacks it into Message Fields
        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            JausByte nodeCount = new JausByte();
            JausByte componentCount = new JausByte();
            JausByte nodeID;
            JausByte componentID;
            int nodeIndex;
            int componentIndex;
            indexOffset = index;
            Node node;
            Component component;
            
            nodeCount.Deserialize(buffer, indexOffset, out indexOffset);
            nodeID = new JausByte();
            componentID = new JausByte();
            for (nodeIndex = 0; nodeIndex < nodeCount.Value; ++nodeIndex )
            {
#warning Unpacking is not yet implemented
                nodeID.Deserialize(buffer, indexOffset, out indexOffset);
                node = new Node((int)nodeID.Value);
                if (nodeID.Value == 255)
                    break;
                componentCount.Deserialize(buffer, indexOffset, out indexOffset);
                for(componentIndex = 0; componentIndex < componentCount.Value; ++componentIndex)
                {
                    componentID.Deserialize(buffer, indexOffset, out indexOffset);
                    component = new Component((int)componentID.Value);
                    if (componentID.Value == 255)
                        break;
                }
            }
            
            return true;
        }

        public List<Node> NodeList
        {
            get { return nodeList; }
        }

        public override String ToString()
        {
            String str = base.ToString();
            //str += "Query Type: " + queryTypes[queryType.getValue() - 1] + "\n";
            return str;
        }
    }
}