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

using BadgerJaus.Services;
using BadgerJaus.Services.Core;

namespace BadgerJaus.Util
{
    public abstract class Subsystem
    {
        private JausUnsignedShort subsystemID;
        private HashSet<Node> nodeList = null;

        protected const int JAUS_PORT = 3974;

        private Transport transportService;
        private UdpClient udpClient;
        protected ConcurrentDictionary<string, JausAddressPort> jausAddrMap;
        private Timer timer;

        public Subsystem(int subsystemID)
        {
            nodeList = new HashSet<Node>();
            this.subsystemID = new JausUnsignedShort(subsystemID);
            udpClient = new UdpClient(JAUS_PORT);
            jausAddrMap = new ConcurrentDictionary<string,JausAddressPort>();
            transportService = Transport.GetTransportService(udpClient, jausAddrMap);
        }

        public int SubsystemID
        {
            get { return subsystemID.getValue(); }
            set { subsystemID.setValue(value); }
        }

        public void AddNode(Node node)
        {
            if (node == null)
                return;

            nodeList.Add(node);
            node.SetSubsystem(this);
        }

        public IEnumerable<Node> GetNodes()
        {
            return Enumerable.AsEnumerable<Node>(nodeList);
        }

        public void InitializeTimer()
        {
            long lowestSleepTime = 5000;

            foreach (Node node in nodeList)
            {
                foreach (Component component in node.GetComponents())
                {
                    foreach (BaseService service in component.GetServices())
                    {
                        if (service.SleepTime < lowestSleepTime)
                            lowestSleepTime = service.SleepTime;
                    }
                }
            }

            timer = new Timer(lowestSleepTime);
            timer.AutoReset = true;
            timer.Elapsed += Execute;
            timer.Enabled = true;
        }

        private void Execute(Object source, ElapsedEventArgs e)
        {
            foreach (Node node in nodeList)
            {
                foreach (Component component in node.GetComponents())
                {
                    foreach (BaseService service in component.GetServices())
                    {
                        service.ExecuteOnTime();
                    }
                }
            }
        }
    }
}