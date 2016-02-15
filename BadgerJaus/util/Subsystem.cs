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
using System.Linq;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Timers;
using System.Net;
using System.Threading;

using BadgerJaus.Services;
using BadgerJaus.Services.Core;

namespace BadgerJaus.Util
{
    public class Subsystem
    {
        protected JausUnsignedShort subsystemID;
        protected Dictionary<long, Node> nodeList = null;
        protected IPEndPoint networkAddress = null;
        protected string identification;

        public const int JAUS_PORT = 3974;

        public Subsystem(int subsystemID, int port)
        {
            nodeList = new Dictionary<long, Node>();
            this.subsystemID = new JausUnsignedShort(subsystemID);
            networkAddress = new IPEndPoint(IPAddress.Any, port);
        }

        public Subsystem(int subSystemID, IPEndPoint networkAddress)
        {
            nodeList = new Dictionary<long, Node>();
            this.subsystemID = new JausUnsignedShort(subsystemID);
            this.networkAddress = new IPEndPoint(networkAddress.Address, networkAddress.Port);
        }

        public int SubsystemID
        {
            get { return (int)subsystemID.Value; }
            set { subsystemID.Value = value; }
        }

        public void AddNode(Node node)
        {
            if (node == null)
                return;

            nodeList.Add(node.NodeID, node);
            node.SetSubsystem(this);
        }

        public Dictionary<long, Node> NodeList
        {
            get { return nodeList; }
        }

        public IPEndPoint NetworkAddress
        {
            get { return networkAddress; }
        }

        public string Identification
        {
            get { return identification; }
            set { identification = value; }
        }

        public void UpdateNodList(Dictionary<long, Node> updatedList)
        {
            foreach(KeyValuePair<long, Node> entry in updatedList)
            {
                Node existingNode;
                if(!nodeList.TryGetValue(entry.Key, out existingNode))
                {
                    nodeList.Add(entry.Key, entry.Value);
                    continue;
                }

                existingNode.UpdateComponents(entry.Value.ComponentList);
            }
        }

        public override string ToString()
        {
            return identification;
        }
    }
}