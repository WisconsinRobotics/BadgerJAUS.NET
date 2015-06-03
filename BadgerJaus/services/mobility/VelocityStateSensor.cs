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

using BadgerJaus.Messages;
using BadgerJaus.Messages.VelocityStateSensor;

using BadgerJaus.Services.Core;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Mobility
{
    public class VelocityStateSensor : BaseService
    {
        public const String SERVICE_NAME = "VelocityStateSensor";
        public const String SERVICE_VERSION = "1.0";
        public const String SERVICE_ID = "urn:jaus:jss:core:VelocityStateSensor";
        public const String PARENT_SERVICE = "Events";

        protected double xVelocity = 0;
        protected double yVelocity = 0;
        protected double zVelocity = 0;

        protected double yawRate = 0;

        public override bool IsSupported(int commandCode)
        {
            // TODO Auto-generated method stub
            return false;
        }

        public override bool ImplementsAndHandledMessage(Message message)
        {
            if (message.GetCommandCode() != JausCommandCode.QUERY_VELOCITY_STATE) return false;

            QueryVelocityState queryVelocityState = new QueryVelocityState();
            queryVelocityState.SetFromJausMessage(message);

            return HandleQueryVelocityState(queryVelocityState);
        }

        public bool HandleQueryVelocityState(QueryVelocityState message)
        {
            ReportVelocityState reportVelocityState = new ReportVelocityState();
            reportVelocityState.SetDestination(message.GetSource());
            reportVelocityState.SetSource(message.GetDestination());
            //if(message.IsFieldSet(QueryVelocityState.))
            reportVelocityState.SetX(xVelocity);
            reportVelocityState.SetYaw(yawRate);
            reportVelocityState.SetToCurrentTime();

            Transport.SendMessage(reportVelocityState);

            return true;
        }
    }
}