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
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using BadgerJaus.Messages;
using BadgerJaus.Messages.Discovery;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Core
{
    public class Discovery : BaseService
    {
        public const String SERVICE_NAME = "Discovery";
        public const String SERVICE_VERSION = "1.0";
        public const String SERVICE_ID = "urn:jaus:jss:core:Discovery";
        public const String PARENT_SERVICE = "Events";
        // Discovery Service States
        public enum DiscoveryState { IDLE_STATE, BROADCAST_STATE, SERVICE_STATE, CONFIGURATION_STATE };

        QueryIdentification qID = new QueryIdentification();
        QueryServices queryServices = new QueryServices();
        ReportServices reportServices;

        Subsystem subsystem = null;
        ConcurrentDictionary<long, Subsystem> discoveredSubsystems;

        DiscoveryState clientDiscoveryState;
        DiscoveryState serverDiscoveryState;

        long sleepTime = BaseService.DEFAULT_SLEEP_TIME;
        bool performDiscovery;

        private static Discovery discoveryService = null;

        public static Discovery CreateDiscoveryInstance(Subsystem subsystem)
        {
            if (discoveryService == null)
                discoveryService = new Discovery(subsystem);

            return discoveryService;
        }

        public static Discovery GetInstance()
        {
            return discoveryService;
        }

        private Discovery(Subsystem subsystem)
        {
            this.subsystem = subsystem;
            clientDiscoveryState = DiscoveryState.IDLE_STATE;
            discoveredSubsystems = new ConcurrentDictionary<long, Subsystem>();
            performDiscovery = false;
        }

        public override bool IsSupported(int commandCode)
        {
            switch (commandCode)
            {
                case JausCommandCode.QUERY_IDENTIFICATION:
                case JausCommandCode.QUERY_SERVICES:
                case JausCommandCode.REPORT_IDENTIFICATION:
                case JausCommandCode.REPORT_SERVICES:
                    return true;
                default:
                    return false;
            }
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            switch (message.GetCommandCode())
            {
                case JausCommandCode.QUERY_IDENTIFICATION:
                    ReportIdentification reportIdentification = new ReportIdentification();
                    reportIdentification.SetSource(subsystem.SubsystemID);
                    reportIdentification.SetDestination(message.GetSource());
                    reportIdentification.Identification = subsystem.Identification;
                    Transport.SendMessage(reportIdentification);
                    return true;
                case JausCommandCode.QUERY_SERVICES:
                    queryServices.SetFromJausMessage(message);
                    return HandleQueryServices(queryServices);
                case JausCommandCode.REPORT_IDENTIFICATION:
                    return HandleReportIdentification(message);
                case JausCommandCode.REPORT_SERVICES:
                    return HandleReportServices(message);
                default:
                    return false;
            }
        }

        public void InitiateDiscovery()
        {
            performDiscovery = true;
            sleepTime = 0;
        }

        protected override void Execute(Component component)
        {
            JausAddress destination = new JausAddress();
            destination.setSubsystem(0);
            destination.setNode(0);
            destination.setComponent(0);
            qID.SetDestination(destination);
            qID.SetSource(component.JausAddress);
            Console.WriteLine(qID.GetDestination().toHexString());
            // TODO Auto-generated method stub
            if (clientDiscoveryState == DiscoveryState.BROADCAST_STATE)
            {
                //long currentTime = System.nanoTime();
                Transport.SendMessage(qID);
            }
        }

        public override long SleepTime
        {
            get { return sleepTime; }
        }

        private bool HandleReportIdentification(Message message)
        {
            int remoteSubsystemID = message.GetSource().SubsystemID;
            Subsystem remoteSubsystem;
            QueryServices queryServices;
            JausAddress remoteAddress;
            Node remoteNode;
            Component remoteComponent;
            ReportIdentification reportIdentification = new ReportIdentification();
            reportIdentification.SetFromJausMessage(message);

            discoveredSubsystems.TryGetValue(remoteSubsystemID, out remoteSubsystem);
            remoteSubsystem.Identification = reportIdentification.Identification;
            queryServices = new QueryServices();
            remoteAddress = new JausAddress(remoteSubsystemID, 255, 255);
            queryServices.SetDestination(remoteAddress);
            queryServices.SetSource(reportIdentification.GetDestination());
            remoteNode = new Node(255);
            remoteComponent = new Component(255);
            remoteNode.AddComponent(remoteComponent);
            queryServices.AddNode(remoteNode);

            Transport.SendMessage(queryServices);

            return true;
        }

        private bool HandleQueryServices(QueryServices message)
        {
            List<Node> queryNodes;
            Dictionary<long, Node> subsystemNodes = subsystem.NodeList;
            reportServices = new ReportServices();

            queryNodes = message.NodeList;

            foreach(Node node in queryNodes)
            {
                if(node.NodeID == 255)
                {
                    reportServices.NodeList = subsystemNodes;
                    break;
                }
            }

            reportServices.SetSource(subsystemNodes[0].ComponentList[0].JausAddress);
            reportServices.SetDestination(message.GetSource());
            Transport.SendMessage(reportServices);

            return true;
        }

        private bool HandleReportServices(Message message)
        {
            int remoteSubsystemID = message.GetSource().SubsystemID;
            Subsystem remoteSubsystem;
            ReportServices reportServices = new ReportServices();

            reportServices.SetFromJausMessage(message);
            discoveredSubsystems.TryGetValue(remoteSubsystemID, out remoteSubsystem);
            remoteSubsystem.UpdateNodList(reportServices.NodeList);

            return true;
        }

        public ConcurrentDictionary<long, Subsystem> DiscoveredSubsystems
        {
            get { return discoveredSubsystems; }
        }

        public void AddRemoteSubsystem(long remoteSubsystemID, Subsystem remoteSubsystem)
        {
            discoveredSubsystems.AddOrUpdate(remoteSubsystemID, remoteSubsystem, (key, oldValue) => remoteSubsystem);
        }

        public void SetDiscoveryState(DiscoveryState state)
        {
            clientDiscoveryState = state;
        }
    }
}