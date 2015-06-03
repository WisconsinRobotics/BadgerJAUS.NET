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
using System.Collections;
using System.Linq;

namespace BadgerJaus.Util
{
    public class Node
    {
        private HashSet<Component> componentList;
        private JausByte nodeID;
        private Subsystem jausSubsystem;

        public Node(int nodeID)
        {
            componentList = new HashSet<Component>();
            this.nodeID = new JausByte(nodeID);
        }

        public void AddComponent(Component component)
        {
            componentList.Add(component);
            component.SetNode(this);
        }

        public IEnumerable<Component> GetComponents()
        {
            return Enumerable.AsEnumerable<Component>(componentList);
        }

        public int NodeID
        {
            get { return nodeID.getValue(); }
            set { nodeID.setValue(value); }
        }

        public bool PayloadToJausBuffer(byte[] buffer, int index)
        {
            int bytesWritten = 0;
            nodeID.toJausBuffer(buffer, index);
            bytesWritten += JausByte.SIZE_BYTES;

            foreach (Component component in componentList)
            {
                int componentSize = component.GetPayloadSize();
                if (!component.PayloadToJausBuffer(buffer, index + bytesWritten))
                    return false;
                bytesWritten += componentSize;
            }

            return true;
        }

        public int GetPayloadSize()
        {
            int totalSize = JausByte.SIZE_BYTES;
            foreach (Component component in componentList)
            {
                totalSize += component.GetPayloadSize();
            }

            return totalSize;
        }

        public void SetSubsystem(Subsystem subsystem)
        {
            jausSubsystem = subsystem;
        }

        public Subsystem GetSubsystem()
        {
            return jausSubsystem;
        }
    }
}