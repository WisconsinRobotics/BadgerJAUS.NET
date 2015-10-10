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
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.IO;

using BadgerJaus.Messages;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Core
{
    public class SendThread
    {
        BlockingCollection<Message> sendMessage = null;
        ConcurrentDictionary<String, IPEndPoint> jausAddrMap = null;
        UdpClient socket = null;
        int maxJausRecvSize = Message.UDP_MAX_PACKET_SIZE + 100; // (100 = buffer)
        short sequenceNumber = 0;

        public SendThread(BlockingCollection<Message> sendMessage, ConcurrentDictionary<String, IPEndPoint> jausAddrMap, UdpClient socket)
        {
            this.sendMessage = sendMessage;
            this.jausAddrMap = jausAddrMap;
            this.socket = socket;
        }

        public void run()
        {
            // TODO Auto-generated method stub
            IPEndPoint currentDest = null;
            if (sendMessage == null) return;
            if (jausAddrMap == null) return;
            if (socket == null) return;
            IPEndPoint dest = null;
            byte[] sendbuffer = new byte[maxJausRecvSize];
            int indexOffset;

            while (true)
            {
                //Console.WriteLine("Preparing to get message to send.\n");
                Message message = null;
                try
                {
                    message = sendMessage.Take();
                }
                catch (ThreadInterruptedException e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine(e.ToString());
                }
                if (message == null)
                {
                    continue;
                }
                message.SetSequenceNumber(sequenceNumber);
                ++sequenceNumber;

                if (message.GetDestination() == null)
                {
                    Console.Error.WriteLine("No destination set for this message: " + message.GetCommandCode());
                    continue;
                }

                jausAddrMap.TryGetValue(message.GetDestination().toHexString(), out dest);

                if (dest == null)
                {
                    Console.Error.WriteLine("Error: this address is not recognized: ");
                    Console.Error.WriteLine(message.GetDestination().toHexString());
                    continue;
                }

                if (currentDest == null || currentDest != dest)
                {
                    //System.Net.IPEndPoint endPoint = new System.Net.IPEndPoint(dest.ipAddr, dest.port);
                    socket.Connect(dest);
                }
                else if (currentDest != dest)
                {
                    socket.Close();
                    socket.Connect(dest);
                }
                currentDest = dest;

                if (!message.ToJausUdpBuffer(sendbuffer, out indexOffset))
                {
                    Console.Error.WriteLine("Failed to convert message.");
                }

                try
                {
                    socket.Send(sendbuffer, message.UdpSize());

                }
                catch (IOException e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine(e.ToString());
                }
            }
        }

    }
}