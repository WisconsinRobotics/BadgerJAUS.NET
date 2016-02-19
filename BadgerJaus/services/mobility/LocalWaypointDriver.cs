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
using BadgerJaus.Messages.LocalWaypointDriver;

using BadgerJaus.Services.Core;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Mobility
{
    public class LocalWaypointDriver : BaseService
    {
        public const String SERVICE_NAME = "LocalWaypointDriver";
        public const String SERVICE_VERSION = "1.0";
        public const String SERVICE_ID = "urn:jaus:jss:core:LocalWaypointDriver";
        public const String PARENT_SERVICE = "Management";

        private double x;
        private double y;
        private double z;

        private double roll;
        private double pitch;
        private double yaw;

        protected override string OVERRIDE_SERVICE_ID
        {
            get { return SERVICE_ID; }
        }

        public override bool IsSupported(int commandCode)
        {
            // TODO Auto-generated method stub
            return false;
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            switch (message.GetCommandCode())
            {
                case JausCommandCode.SET_LOCAL_WAYPOINT:
                    SetLocalWaypoint setLocalWaypoint = new SetLocalWaypoint();
                    setLocalWaypoint.SetFromJausMessage(message);
                    return HandleSetLocalWaypoint(setLocalWaypoint);
                case JausCommandCode.QUERY_LOCAL_WAYPOINT:
                    QueryLocalWaypoint queryLocalWaypoint = new QueryLocalWaypoint();
                    queryLocalWaypoint.SetFromJausMessage(message);
                    return HandleQueryLocalWaypoint(queryLocalWaypoint);
                default:
                    return false;
            }
        }

        public bool HandleSetLocalWaypoint(SetLocalWaypoint message)
        {
            if (message.IsFieldSet(SetLocalWaypoint.X_BIT))
                x = message.GetX();

            if (message.IsFieldSet(SetLocalWaypoint.Y_BIT))
                y = message.GetY();

            if (message.IsFieldSet(SetLocalWaypoint.Z_BIT))
                z = message.GetY();

            if (message.IsFieldSet(SetLocalWaypoint.ROLL_BIT))
                roll = message.GetRoll();

            if (message.IsFieldSet(SetLocalWaypoint.PITCH_BIT))
                pitch = message.GetPitch();

            if (message.IsFieldSet(SetLocalWaypoint.YAW_BIT))
                yaw = message.GetY();

            return true;
        }

        public bool HandleQueryLocalWaypoint(QueryLocalWaypoint message)
        {
            ReportLocalWaypoint reportLocalWaypoint = new ReportLocalWaypoint();
            reportLocalWaypoint.SetDestination(message.GetSource());
            reportLocalWaypoint.SetSource(message.GetDestination());

            if (message.IsFieldSet(ReportLocalWaypoint.X_BIT))
                reportLocalWaypoint.SetX(x);

            if (message.IsFieldSet(ReportLocalWaypoint.Y_BIT))
                reportLocalWaypoint.SetY(y);

            if (message.IsFieldSet(ReportLocalWaypoint.Z_BIT))
                reportLocalWaypoint.SetZ(z);

            if (message.IsFieldSet(ReportLocalWaypoint.ROLL_BIT))
                reportLocalWaypoint.SetRoll(roll);

            if (message.IsFieldSet(ReportLocalWaypoint.PITCH_BIT))
                reportLocalWaypoint.SetPitch(pitch);

            if (message.IsFieldSet(ReportLocalWaypoint.YAW_BIT))
                reportLocalWaypoint.SetYaw(yaw);

            Transport.SendMessage(reportLocalWaypoint);

            return true;
        }
    }
}