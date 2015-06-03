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
using System.Collections.Generic;

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
        public enum DiscoveryState { STATE_INIT, STATE_READY };

        QueryIdentification qID = new QueryIdentification();
        QueryServices queryServices = new QueryServices();
        ReportServices reportServices;

        Subsystem subsystem = null;

        DiscoveryState discoveryState;
        //long lastCheck = System.nanoTime();

        public Discovery(Subsystem subsystem)
        {
            this.subsystem = subsystem;
            discoveryState = DiscoveryState.STATE_INIT;
        }

        public int GetState()
        {
            return (int)discoveryState;
        }

        public override bool IsSupported(int commandCode)
        {
            // TODO Auto-generated method stub
            return false;
        }

        public override bool ImplementsAndHandledMessage(Message message)
        {
            switch (message.GetCommandCode())
            {
                case JausCommandCode.REPORT_IDENTIFICATION:
                    return HandleReportIdentification(message);
                case JausCommandCode.QUERY_SERVICES:
                    queryServices.SetFromJausMessage(message);
                    return HandleQueryServices(queryServices);
                default:
                    return false;
            }
        }

        protected override void Execute()
        {
            //HACK: This is primarily to deal with competition.
            JausAddress destination = new JausAddress();
            destination.setSubsystem(42);
            destination.setNode(1);
            destination.setComponent(1);
            JausAddress source = new JausAddress();
            source.setComponent(1);
            source.setNode(1);
            source.setSubsystem(114);
            qID.SetDestination(destination);
            qID.SetSource(source);
            Console.WriteLine(qID.GetDestination().toHexString());
            // TODO Auto-generated method stub
            if (discoveryState == DiscoveryState.STATE_INIT)
            {
                //long currentTime = System.nanoTime();
                Transport.SendMessage(qID);
            }
        }

        private bool HandleReportIdentification(Message message)
        {
            discoveryState = DiscoveryState.STATE_READY;
            //JausAddress address = message.getSource();
            //Transport.AddReceiver(address.toString());
            return true;
        }

        private bool HandleQueryServices(QueryServices message)
        {
            JausAddress target = message.GetDestination();
            if (target.getSubsystem() != subsystem.SubsystemID)
            {
                Console.WriteLine("Wrong subsystemID: " + target.getSubsystem());
                return true;
            }

            reportServices = new ReportServices();
            //Set<Node> nodeList = subsystem.GetNodes();
            IEnumerable<Node> nodeItr = subsystem.GetNodes();
            foreach (Node node in nodeItr)
            {
                if (node.NodeID == target.getNode())
                {
                    if (target.getComponent() == 1)
                    {
                        reportServices.SetNode(node);
                        reportServices.SetDestination(message.GetSource());
                        reportServices.SetSource(target);
                        Transport.SendMessage(reportServices);
                        break;
                    }
                    else
                    {
                        //TODO: Implement single component service handling later
                    }
                }
            }

            return true;
        }
    }
}