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
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BadgerJaus.Messages;
using BadgerJaus.Messages.Control;
using BadgerJaus.Messages.Management;
using BadgerJaus.Messages.LocalVectorDriver;
using BadgerJaus.Services;
using BadgerJaus.Util;

namespace BadgerJausUnitTests
{
    [TestClass]
    public class MessageSerializationTest
    {
        [TestMethod]
        public void ConfirmControlSerializationTest()
        {
            ConfirmControl sendConfirmControl;
            ConfirmControl recConfirmControl;
            byte[] buffer;
            int indexOffset;

            sendConfirmControl = new ConfirmControl();
            sendConfirmControl.SetSource(100);
            sendConfirmControl.SetDestination(101);
            sendConfirmControl.SetResponseCode(ConfirmControl.CONTROL_ACCEPTED);

            buffer = new byte[Message.UDP_MAX_PACKET_SIZE];
            sendConfirmControl.ToJausUdpBuffer(buffer, out indexOffset);

            recConfirmControl = new ConfirmControl();
            recConfirmControl.SetFromJausUdpBuffer(buffer);

            Assert.AreEqual(sendConfirmControl.GetSource().Value, recConfirmControl.GetSource().Value);
            Assert.AreEqual(sendConfirmControl.GetDestination().Value, recConfirmControl.GetDestination().Value);
            Assert.AreEqual(sendConfirmControl.GetResponseCode(), recConfirmControl.GetResponseCode());
        }

        public void SetLocalVectorSerializationTest()
        {
            SetLocalVector sendSetLocalVector;
            SetLocalVector receiveSetLocalVector;
        }

        [TestMethod]
        public void ReportStatusSerializationTest()
        {
            ReportStatus sendStatus;
            ReportStatus receiveStatus;
            byte[] buffer;
            int indexOffset;
            
            sendStatus = new ReportStatus();
            sendStatus.SetSource(100);
            sendStatus.SetDestination(200);
            sendStatus.SetStatus((int) ComponentState.STATE_READY);

            buffer = new byte[Message.UDP_MAX_PACKET_SIZE];
            sendStatus.ToJausUdpBuffer(buffer, out indexOffset);

            receiveStatus = new ReportStatus();
            receiveStatus.SetFromJausUdpBuffer(buffer);

            Assert.AreEqual(sendStatus.GetSource().Value, receiveStatus.GetSource().Value);
            Assert.AreEqual(sendStatus.GetDestination().Value, receiveStatus.GetDestination().Value);
            Assert.AreEqual(sendStatus.GetStatus(), receiveStatus.GetStatus());
        }
    }
}
