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
using System.Threading;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;

using BadgerJaus.Messages;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Core
{
    public sealed class Transport : BaseService
    {
        public const String SERVICE_NAME = "Transport";
        public const String SERVICE_VERSION = "1.0";
        public const String SERVICE_ID = "urn:jaus:jss:core:Transport";
        public const String PARENT_SERVICE = "none";

        private static Transport transportService = null;

        private static SendThread sendThread;
        private static ReceiveThread receiveThread;
        private static BlockingCollection<Message> sendMessageQueue;
        private static BlockingCollection<ReceivedPacket> receivedMessageQueue;
        private static ConcurrentDictionary<string, IPEndPoint> sendAddrs;

        private LinkedList<Subsystem> subsystems = null;

        public static Transport CreateTransportInstance(UdpClient socket, ConcurrentDictionary<string, IPEndPoint> jausAddrMap)
        {
            if (transportService == null)
                transportService = new Transport(socket, jausAddrMap);

            return transportService;
        }

        public static Transport GetTransportService()
        {
            return transportService;
        }

        private Transport(UdpClient socket, ConcurrentDictionary<String, IPEndPoint> jausAddrMap)
        {
            sendMessageQueue = new BlockingCollection<Message>();
            receivedMessageQueue = new BlockingCollection<ReceivedPacket>();
            sendThread = new SendThread(sendMessageQueue, jausAddrMap, socket);
            receiveThread = new ReceiveThread(receivedMessageQueue, socket);
            sendAddrs = jausAddrMap;
            subsystems = new LinkedList<Subsystem>();
        }

        public void AddRemoteAddress(String hostAddress, IPEndPoint ipEndpoint)
        {
            sendAddrs.AddOrUpdate(hostAddress, ipEndpoint, (key, oldValue) => ipEndpoint);
        }

        public void AddSubsystem(Subsystem subsystem)
        {
            if (subsystem == null)
                return;
            
            subsystems.AddLast(subsystem);
        }

        public static void SendMessage(Message jausMessage)
        {
            //Console.WriteLine("Adding message to queue.");
            sendMessageQueue.Add(jausMessage);
        }

        public override bool IsSupported(int commandCode)
        {
            return false;
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            return false;
        }

        public void run()
        {
            // TODO Auto-generated method stub
            Message message = null;
            bool messageHandled;
            ReceivedPacket receivedPacket;
            byte[] buffer;
            Thread sendThreadT = new Thread(Transport.sendThread.run);
            Thread receiveThreadT = new Thread(Transport.receiveThread.run);

            sendThreadT.Start();
            receiveThreadT.Start();
            while (true)
            {
                messageHandled = false;
                try
                {
                    receivedPacket = receivedMessageQueue.Take();
                    buffer = receivedPacket.Buffer;
                    message = new Message();
                    message.SetFromJausUdpBuffer(buffer);
                    String from = message.GetSource().toHexString();
                    JausAddress destination = message.GetDestination();
                    if (!sendAddrs.ContainsKey(from))
                    {
                        sendAddrs.TryAdd(from, receivedPacket.SourceAddr);
                    }

                    foreach (Subsystem subsystem in subsystems)
                    {
                        if (subsystem.SubsystemID != destination.getSubsystem())
                            continue;

                        foreach (Node node in subsystem.GetNodes())
                        {
                            if (node.NodeID != destination.getNode())
                                continue;

                            foreach (Component component in node.ComponentList)
                            {
                                Discovery.GetInstance().ImplementsAndHandledMessage(message, component);
                                if (component.ComponentID != destination.getComponent())
                                    continue;

                                Liveness.GetInstance().ImplementsAndHandledMessage(message, component);
                                AccessControl.GetInstance().ImplementsAndHandledMessage(message, component);
                                Management.GetInstance().ImplementsAndHandledMessage(message, component);
                                Liveness.GetInstance().ImplementsAndHandledMessage(message, component);

                                foreach (Service service in component.GetServices())
                                {
                                    if (service.ImplementsAndHandledMessage(message, component))
                                    {
                                        messageHandled = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (!messageHandled)
                    {
                        //Handle error case
                    }
                }
                catch (ThreadInterruptedException e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}