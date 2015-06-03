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
using System.Text;
using System.Linq;

using BadgerJaus.Services;
using BadgerJaus.Services.Core;

namespace BadgerJaus.Util
{
    public class Component
    {
        private JausByte componentID;
        private HashSet<Service> serviceList;
        private Node jausNode;
        private JausAddress jausAddress;

        Management managementService = null;
        AccessControl accessControlService = null;

        public Component(int componentID)
        {
            this.componentID = new JausByte(componentID);
            serviceList = new HashSet<Service>();
            jausAddress = null;
        }

        public int ComponentID
        {
            get { return componentID.getValue(); }
            set { componentID.setValue(value); }
        }

        public void AddService(Service service)
        {
            if(service is AccessControl)
            {
                accessControlService = (AccessControl)service;
            }

            if(service is Management)
            {
                accessControlService = (AccessControl)service;
                managementService = (Management)service;
            }

            serviceList.Add(service);
            service.SetComponent(this);
        }

        public IEnumerable<Service> GetServices()
        {
            return Enumerable.AsEnumerable<Service>(serviceList);
        }

        public bool PayloadToJausBuffer(byte[] buffer, int index)
        {
            int bytesWritten = 0;
            JausByte version = new JausByte();
            componentID.toJausBuffer(buffer, index);
            bytesWritten += JausByte.SIZE_BYTES;

            foreach (Service service in serviceList)
            {
                String serviceID = service.GetServiceID();
                Array.Copy(getBytes(serviceID), 0, buffer, index + bytesWritten, serviceID.Length);
                bytesWritten += serviceID.Length;
                version.setValue(service.GetMajorVersion());
                version.toJausBuffer(buffer, index + bytesWritten);
                bytesWritten += JausByte.SIZE_BYTES;
                version.setValue(service.GetMinorVersion());
                version.toJausBuffer(buffer, index + bytesWritten);
                bytesWritten += JausByte.SIZE_BYTES;
            }

            return true;
        }

        public int GetPayloadSize()
        {
            int totalSize = JausByte.SIZE_BYTES;
            foreach (Service service in serviceList)
            {
                String serviceID = service.GetServiceID();
                totalSize += serviceID.Length;
                totalSize += JausByte.SIZE_BYTES;
                totalSize += JausByte.SIZE_BYTES;
            }

            return totalSize;
        }

        public void SetNode(Node node)
        {
            jausNode = node;
        }

        public Node GetNode()
        {
            return jausNode;
        }

        public JausAddress GetAddress()
        {
            if(jausAddress == null)
            {
                jausAddress = new JausAddress(jausNode.GetSubsystem().SubsystemID, jausNode.NodeID, componentID.getValue());
            }

            return jausAddress;
        }

        public void SetSubsystemAddress(int subsystemID)
        {
            jausAddress.setSubsystem(subsystemID);
        }

        public void SetNodeAddress(int nodeID)
        {
            jausAddress.setNode(nodeID);
        }

        public byte[] getBytes(String str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public COMPONENT_STATE GetState()
        {
            if(managementService == null)
            {
                return COMPONENT_STATE.STATE_SHUTDOWN;
            }

            return managementService.CurrentState;
        }

        public bool IsController(JausAddress address)
        {
            if (accessControlService == null)
                return false;

            return accessControlService.IsController(address);
        }
    }
}