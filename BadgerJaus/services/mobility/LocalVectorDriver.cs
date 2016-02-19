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
using System.Threading;

using BadgerJaus.Messages;
using BadgerJaus.Messages.LocalVectorDriver;
using BadgerJaus.Services.Core;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Mobility
{
    public class LocalVectorDriver : BaseService
    {
        public const String SERVICE_NAME = "PrimitiveDriver";
        public const String SERVICE_VERSION = "1.0";
        public const String SERVICE_ID = "urn:jaus:jss:mobility:LocalVectorDriver";
        public const String PARENT_SERVICE = "Management";

        protected double speed;
        protected double z;
        protected double heading;
        protected double roll;
        protected double pitch;

        protected double reportSpeed;
        protected double reportZ;
        protected double reportHeading;
        protected double reportRoll;
        protected double reportPitch;

        QueryLocalVector queryVector;
        SetLocalVector setVector;

        public LocalVectorDriver()
        {
            speed = 0;
            z = 0;
            heading = 0;
            roll = 0;
            pitch = 0;

            reportSpeed = 0;
            reportZ = 0;
            reportHeading = 0;
            reportRoll = 0;
            reportPitch = 0;

            queryVector = new QueryLocalVector();
            setVector = new SetLocalVector();
        }

        protected override string OVERRIDE_SERVICE_ID
        {
            get { return SERVICE_ID; }
        }

        public override bool IsSupported(int commandCode)
        {
            switch (commandCode)
            {
                case JausCommandCode.QUERY_LOCAL_VECTOR:
                case JausCommandCode.SET_LOCAL_VECTOR:
                    return true;
                default:
                    return false;
            }
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            switch (message.GetCommandCode())
            {
                case JausCommandCode.QUERY_LOCAL_VECTOR:
                    queryVector.SetFromJausMessage(message);
                    return HandleQueryLocalVector(queryVector);
                case JausCommandCode.SET_LOCAL_VECTOR:
                    return HandleSetLocalVector(message, component);

                default:
                    return false;
            }
        }

        protected bool HandleQueryLocalVector(QueryLocalVector message)
        {
            //Initialize response

            ReportLocalVector report = new ReportLocalVector();
            report.SetDestination(message.GetSource());
            report.SetSource(message.GetDestination());

            //Set requested data
            if (message.IsBitSet(SetLocalVector.SPEED_BIT))
                report.SetSpeed(reportSpeed);

            if (message.IsBitSet(SetLocalVector.Z_BIT))
                report.SetZ(reportZ);

            if (message.IsBitSet(SetLocalVector.HEADING_BIT))
                report.SetHeading(reportHeading);

            if (message.IsBitSet(SetLocalVector.ROLL_BIT))
                report.SetRoll(reportRoll);

            if (message.IsBitSet(SetLocalVector.PITCH_BIT))
                report.SetPitch(reportPitch);

            //Send response
            Transport.SendMessage(report);

            return true;
        }

        protected bool HandleSetLocalVector(Message message, Component component)
        {
            if (!component.IsController(message.GetSource()))
                return true;

            setVector.SetFromJausMessage(message);

            if (setVector.isFieldSet(SetLocalVector.SPEED_BIT))
                Interlocked.Exchange(ref speed, setVector.GetSpeed());

            if (setVector.isFieldSet(SetLocalVector.Z_BIT))
                Interlocked.Exchange(ref z, setVector.GetZ());

            if (setVector.isFieldSet(SetLocalVector.HEADING_BIT))
                Interlocked.Exchange(ref heading, setVector.GetHeading());

            if (setVector.isFieldSet(SetLocalVector.ROLL_BIT))
                Interlocked.Exchange(ref roll, setVector.GetRoll());

            if (setVector.isFieldSet(SetLocalVector.PITCH_BIT))
                Interlocked.Exchange(ref pitch, setVector.GetPitch());

            return true;
        }
    }
}