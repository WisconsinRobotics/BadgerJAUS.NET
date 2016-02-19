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
using BadgerJaus.Messages.LocalPoseSensor;

using BadgerJaus.Services.Core;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Mobility
{
    public class LocalPoseSensor : BaseService
    {
        public const String SERVICE_NAME = "LocalPoseSensor";
        public const String SERVICE_VERSION = "1.0";
        public const String SERVICE_ID = "urn:jaus:jss:core:LocalPoseSensor";
        public const String PARENT_SERVICE = "AccessControl";

        protected double x;
        protected double y;
        protected double z;

        protected double roll;
        protected double pitch;
        protected double yaw;

        protected double xOrigin = 0;
        protected double yOrigin = 0;
        protected double zOrigin = 0;

        protected double rollOrigin = 0;
        protected double pitchOrigin = 0;
        protected double yawOrigin = 0;

        protected override string OVERRIDE_SERVICE_ID
        {
            get { return SERVICE_ID; }
        }

        public override bool IsSupported(int commandCode)
        {
            switch (commandCode)
            {
                case JausCommandCode.SET_LOCAL_POSE:
                    return true;
                case JausCommandCode.QUERY_LOCAL_POSE:
                    return true;
                default:
                    return false;
            }
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            switch (message.GetCommandCode())
            {
                case JausCommandCode.SET_LOCAL_POSE:
                    SetLocalPose setLocalPose = new SetLocalPose();
                    setLocalPose.SetFromJausMessage(message);
                    return HandleSetLocalPose(setLocalPose);
                case JausCommandCode.QUERY_LOCAL_POSE:
                    QueryLocalPose queryLocalPose = new QueryLocalPose();
                    queryLocalPose.SetFromJausMessage(message);
                    return HandleQueryLocalPose(queryLocalPose);
                default:
                    return false;
            }
        }

        public bool HandleSetLocalPose(SetLocalPose message)
        {
            if (message.IsFieldSet(SetLocalPose.X_BIT))
                xOrigin = message.GetX();

            if (message.IsFieldSet(SetLocalPose.Y_BIT))
                yOrigin = message.GetY();

            if (message.IsFieldSet(SetLocalPose.Z_BIT))
                zOrigin = message.GetY();

            if (message.IsFieldSet(SetLocalPose.ROLL_BIT))
                rollOrigin = message.GetRoll();

            if (message.IsFieldSet(SetLocalPose.PITCH_BIT))
                pitchOrigin = message.GetPitch();

            if (message.IsFieldSet(SetLocalPose.YAW_BIT))
                yawOrigin = message.GetY();

            return true;
        }

        public bool HandleQueryLocalPose(QueryLocalPose message)
        {
            ReportLocalPose reportLocalPose = new ReportLocalPose();
            reportLocalPose.SetSource(message.GetDestination());
            reportLocalPose.SetDestination(message.GetSource());

            if (message.IsFieldSet(ReportLocalPose.X_BIT))
            {
                Console.WriteLine("QueryLocalPose x bit set.");
                reportLocalPose.SetX(x);
            }
            else
            {
                Console.Error.WriteLine("Error: QueryLocalPose x bit not detected");
                Console.WriteLine(Convert.ToString(message.GetPayload()));
            }

            if (message.IsFieldSet(ReportLocalPose.Y_BIT))
                reportLocalPose.SetY(y);

            if (message.IsFieldSet(ReportLocalPose.Z_BIT))
                reportLocalPose.SetZ(z);

            if (message.IsFieldSet(ReportLocalPose.ROLL_BIT))
                reportLocalPose.SetRoll(roll);

            if (message.IsFieldSet(ReportLocalPose.PITCH_BIT))
                reportLocalPose.SetPitch(pitch);

            if (message.IsFieldSet(ReportLocalPose.YAW_BIT))
                reportLocalPose.SetYaw(yaw);

            if (message.IsFieldSet(ReportLocalPose.TS_BIT))
                reportLocalPose.SetTimestamp(0);

            reportLocalPose.SetToCurrentTime();
            Transport.SendMessage(reportLocalPose);

            return true;
        }

        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }

        public double GetYaw()
        {
            return yaw;
        }

        public double GetOriginX()
        {
            return xOrigin;
        }

        public double GetOriginY()
        {
            return yOrigin;
        }
    }
}