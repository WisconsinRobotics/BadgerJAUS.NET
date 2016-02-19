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
using System.Diagnostics;

using BadgerJaus.Services;
using BadgerJaus.Services.Core;

namespace BadgerJaus.Util
{
    public enum ComponentState
    {
        STATE_INIT = 0,
        STATE_READY = 1,
        STATE_STANDBY = 2,
        STATE_SHUTDOWN = 3,
        STATE_FAILURE = 4,
        STATE_EMERGENCY = 5
    };

    public enum ControlState
    {
        STATE_CONTROL_NOT_AVAILABLE,
        STATE_NOT_CONTROLLED,
        STATE_CONTROLLED
    };

    public class Component
    {
        private JausByte componentID;
        private List<Service> serviceList;
        private Node jausNode;
        private JausAddress jausAddress;
        private JausByte instanceID;

        ControlState controlState;
        ComponentState componentState;
        JausAddress controller;
        int authorityCode;

        public Component(int componentID)
        {
            this.componentID = new JausByte(componentID);
            serviceList = new List<Service>();
            jausAddress = new JausAddress();
            jausAddress.setComponent(componentID);
            instanceID = new JausByte(0);
            controlState = ControlState.STATE_NOT_CONTROLLED;
            componentState = ComponentState.STATE_INIT;
            controller = new JausAddress();
            authorityCode = AccessControl.DEFAULT_AUTHORITY_CODE;
        }

        public int ComponentID
        {
            get { return (int)componentID.Value; }
            set {
                componentID.Value = value;
                jausAddress.setComponent(value);
            }
        }

        public void AddService(Service service)
        {
            serviceList.Add(service);
        }

        public List<Service> Services
        {
            get { return serviceList; }
            set { serviceList = value; }
        }

        public ControlState ControlState
        {
            get { return controlState; }
            set { controlState = value; }
        }

        public ComponentState ComponentState
        {
            get { return componentState; }
            set { componentState = value; }
        }

        public JausAddress Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        public int AuthorityCode
        {
            get { return authorityCode; }
            set { authorityCode = value; }
        }

        public bool Serialize(byte[] buffer, int index, out int indexOffset, bool getServices = true)
        {
            bool status;
            JausByte serviceCount = new JausByte(serviceList.Count);
            indexOffset = index;
            status = componentID.Serialize(buffer, indexOffset, out indexOffset);
            instanceID.Serialize(buffer, indexOffset, out indexOffset);

            if (!getServices)
                return status;
            
            serviceCount.Serialize(buffer, indexOffset, out indexOffset);

            foreach(Service service in serviceList)
            {
                service.Serialize(buffer, indexOffset, out indexOffset);
            }

            return true;
        }

        public bool Deserialize(byte[] buffer, int index, out int indexOffset)
        {
            JausByte serviceCount = new JausByte();
            DiscoveredService discoveredService;
            int serviceIndex;
            indexOffset = index;

            componentID.Deserialize(buffer, indexOffset, out indexOffset);
            instanceID.Deserialize(buffer, indexOffset, out indexOffset);
            serviceCount.Deserialize(buffer, indexOffset, out indexOffset);

            if (serviceCount.Value == 0)
                return true;

            for (serviceIndex = 0; serviceIndex < serviceCount.Value; ++serviceIndex )
            {
                discoveredService = new DiscoveredService();
                discoveredService.Deserialize(buffer, indexOffset, out indexOffset);
                serviceList.Add(discoveredService);
            }

            return true;
        }

        public int GetPayloadSize()
        {
            int totalSize = JausBaseType.BYTE_BYTE_SIZE;
            foreach (Service service in serviceList)
            {
                totalSize += service.Size();
            }

            return totalSize;
        }

        public void SetNode(Node node)
        {
            Subsystem subsystem;
            jausNode = node;
            jausAddress.setNode(jausNode.NodeID);
            subsystem = jausNode.GetSubsystem();
            if (subsystem == null)
                return;
            jausAddress.setSubsystem(subsystem.SubsystemID);
        }

        public Node GetNode()
        {
            return jausNode;
        }

        public JausAddress JausAddress
        {
            get { return jausAddress; }
        }

        public void SetSubsystemAddress(int subsystemID)
        {
            jausAddress.setSubsystem(subsystemID);
        }

        public bool IsController(JausAddress address)
        {
            return address == controller;
        }
    }
}