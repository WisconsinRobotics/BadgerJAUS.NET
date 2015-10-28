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
    public abstract class Subsystem
    {
        private JausUnsignedShort subsystemID;
        private List<Node> nodeList = null;

        protected const int JAUS_PORT = 3974;

        private UdpClient udpClient;
        protected ConcurrentDictionary<long, IPEndPoint> jausAddrMap;
        private System.Timers.Timer timer;

        protected static Transport transportService;
        //protected static Event eventService --not yet implemented
        protected static AccessControl accessControlService;
        protected static Management managementService;
        //protected static Time timeService --not yet implemented
        protected static Liveness livenessService;
        protected static Discovery discoveryService;

        public Subsystem(int subsystemID, int port = JAUS_PORT)
        {
            nodeList = new List<Node>();
            this.subsystemID = new JausUnsignedShort(subsystemID);
            udpClient = new UdpClient(port);
            jausAddrMap = new ConcurrentDictionary<long, IPEndPoint>();
            transportService = Transport.CreateTransportInstance(udpClient, jausAddrMap);
            accessControlService = AccessControl.CreateAccessControlInstance();
            managementService = Management.CreateManagementInstance();
            livenessService = Liveness.CreateLivenessInstance();
            discoveryService = Discovery.CreateDiscoveryInstance(this);
            transportService.AddSubsystem(this);
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

            nodeList.Add(node);
            node.SetSubsystem(this);
        }

        public List<Node> NodeList
        {
            get { return nodeList; }
        }

        public void InitializeTimer()
        {
            long lowestSleepTime = 5000;
            Thread transportThread = new Thread(transportService.run);
            transportThread.Start();

            foreach (Node node in nodeList)
            {
                foreach (Component component in node.ComponentList)
                {
                    foreach (BaseService service in component.GetServices())
                    {
                        if (service.SleepTime < lowestSleepTime)
                            lowestSleepTime = service.SleepTime;
                    }
                }
            }

            timer = new System.Timers.Timer(lowestSleepTime);
            timer.AutoReset = true;
            timer.Elapsed += Execute;
            timer.Enabled = true;
        }

        private void Execute(Object source, ElapsedEventArgs e)
        {
            bool firstComponent = true;
            
            foreach (Node node in nodeList)
            {
                foreach (Component component in node.ComponentList)
                {
                    if (firstComponent)
                    {
                        firstComponent = false;
                        livenessService.ExecuteOnTime(component);
                        discoveryService.ExecuteOnTime(component);
                    }
                    foreach (BaseService service in component.GetServices())
                    {
                        service.ExecuteOnTime(component);
                    }
                }
            }
        }
    }
}