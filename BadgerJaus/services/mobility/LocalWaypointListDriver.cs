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
using System.Collections.ObjectModel;

using BadgerJaus.Messages;
using BadgerJaus.Messages.LocalWaypointDriver;
using BadgerJaus.Messages.ListManager;
using BadgerJaus.Messages.LocalVectorDriver;
using BadgerJaus.Messages.Driver;

using BadgerJaus.Services.Core;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Mobility
{
    public class LocalWaypointListDriver : BaseService
    {
        public const String SERVICE_VERSION = "1.0";
        public const String PARENT_SERVICE = "ListManager";

        protected LinkedList<WaypointElement> waypointList = new LinkedList<WaypointElement>();
        protected WaypointElement currentWaypointElement = null;
        protected int waypointIndex = -1;
        private JausByte requestID = new JausByte();

        protected LocalPoseSensor localPoseSensor = null;

        protected double xOrigin = 0;
        protected double yOrigin = 0;
        protected double zOrigin = 0;

        protected double rollOrigin = 0;
        protected double pitchOrigin = 0;
        protected double yawOrigin = 0;

        protected double speed;

        protected double desiredSpeed;

        public LocalWaypointListDriver(LocalPoseSensor localPoseSensor)
        {
            this.localPoseSensor = localPoseSensor;
        }

        protected override string OVERRIDE_SERVICE_NAME
        {
            get { return "LocalWaypointListDriver"; }
        }

        protected override string OVERRIDE_SERVICE_FAMILY
        {
            get { return MOBILITY_SERVICE; }
        }

        public override bool IsSupported(int commandCode)
        {
            // TODO Auto-generated method stub
            return false;
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            Message returnMessage = null;
            bool handled = true;
            switch (message.GetCommandCode())
            {
                case JausCommandCode.SET_LOCAL_WAYPOINT:
                    SetLocalWaypoint setLocalWaypoint = new SetLocalWaypoint();
                    setLocalWaypoint.SetFromJausMessage(message);
                    return HandleSetLocalWaypoint(setLocalWaypoint);
                case JausCommandCode.SET_ELEMENT:
                    SetElement setElement = new SetElement();
                    setElement.SetFromJausMessage(message);
                    handled = HandleSetElement(setElement);
                    break;
                case JausCommandCode.REPORT_ELEMENT_LIST:
                    ReportElementList reportElementList = new ReportElementList();
                    reportElementList.SetRequestID((int)requestID.Value);
                    returnMessage = reportElementList;
                    break;
                case JausCommandCode.QUERY_ACTIVE_ELEMENT:
                    return HandleQueryActiveElement(message);
                case JausCommandCode.QUERY_ELEMENT_COUNT:
                    ReportElementCount reportElementCount = new ReportElementCount();
                    reportElementCount.SetElementCount(waypointList.Count);
                    returnMessage = reportElementCount;
                    break;
                case JausCommandCode.EXECUTE_LIST:
                    if (!ExecuteList())	//Must be overridden somehow
                        return true;
                    waypointIndex = 0;
                    currentWaypointElement = waypointList.First.Value;
                    break;
                case JausCommandCode.QUERY_LOCAL_WAYPOINT:
                    QueryLocalWaypoint queryLocalWaypoint = new QueryLocalWaypoint();
                    queryLocalWaypoint.SetFromJausMessage(message);
                    return HandleQueryLocalWaypoint(queryLocalWaypoint);
                case JausCommandCode.QUERY_TRAVEL_SPEED:
                    return HandleQueryTravelSpeed(message);
                case JausCommandCode.SET_TRAVEL_SPEED:
                    SetTravelSpeed setTravelSpeed = new SetTravelSpeed();
                    setTravelSpeed.SetFromJausMessage(message);
                    return HandleSetTravelSpeed(setTravelSpeed);
                default:
                    handled = false;
                    break;
            }

            if (returnMessage != null)
            {
                returnMessage.SetSource(message.GetDestination());
                returnMessage.SetDestination(message.GetSource());
                Transport.SendMessage(returnMessage);
            }

            return handled;
        }

        public bool HandleSetLocalWaypoint(SetLocalWaypoint message)
        {
            if (message.IsFieldSet(SetLocalWaypoint.X_BIT))
                xOrigin = message.GetX();

            if (message.IsFieldSet(SetLocalWaypoint.Y_BIT))
                yOrigin = message.GetY();

            if (message.IsFieldSet(SetLocalWaypoint.Z_BIT))
                zOrigin = message.GetY();

            if (message.IsFieldSet(SetLocalWaypoint.ROLL_BIT))
                rollOrigin = message.GetRoll();

            if (message.IsFieldSet(SetLocalWaypoint.PITCH_BIT))
                pitchOrigin = message.GetPitch();

            if (message.IsFieldSet(SetLocalWaypoint.YAW_BIT))
                yawOrigin = message.GetY();

            return true;
        }

        public bool HandleSetElement(SetElement message)
        {
#warning Completely broken
            requestID.Value = message.GetRequestID();
            List<JausElement> elements = message.GetElements();
            JausUnsignedShort elementCommandCode = new JausUnsignedShort();
            foreach (JausElement element in elements)
            {
                byte[] data = element.getElementData();
                //elementCommandCode.Deserialize(data);
                if (!(elementCommandCode.Value == JausCommandCode.SET_LOCAL_WAYPOINT))
                    continue;


                //Commented out DUE to visibility modifier
                //SetLocalWaypoint setLocalWaypoint = new SetLocalWaypoint();
                //setLocalWaypoint.setPayloadFromJausBuffer(data, JausBaseType.SHORT_BYTE_SIZE);

                //WaypointElement waypoint = new WaypointElement(element, setLocalWaypoint);
                //waypointList.AddLast(waypoint);
                //Console.WriteLine("Adding element waypoint, current count: " + waypointList.Count);
            }

            ConfirmElementRequest confirmElementRequest = new ConfirmElementRequest();
            confirmElementRequest.SetSource(message.GetDestination());
            confirmElementRequest.SetDestination(message.GetSource());
            confirmElementRequest.SetRequestID(message.GetRequestID());

            Transport.SendMessage(confirmElementRequest);

            return true;
        }

        public bool HandleQueryLocalWaypoint(QueryLocalWaypoint message)
        {
            ReportLocalWaypoint reportLocalWaypoint = new ReportLocalWaypoint();
            reportLocalWaypoint.SetDestination(message.GetSource());
            reportLocalWaypoint.SetSource(message.GetDestination());

            WaypointElement waypointElement = currentWaypointElement;
            if (waypointElement == null) return true;

            if (message.IsFieldSet(ReportLocalWaypoint.X_BIT))
                reportLocalWaypoint.SetX(waypointElement.GetX());

            if (message.IsFieldSet(ReportLocalWaypoint.Y_BIT))
                reportLocalWaypoint.SetY(waypointElement.GetY());

            /*
            if(message.IsFieldSet(ReportLocalWaypoint.Z_BIT))
                reportLocalWaypoint.SetZ(z);
		
            if(message.IsFieldSet(ReportLocalWaypoint.ROLL_BIT))
                reportLocalWaypoint.SetRoll(roll);
		
            if(message.IsFieldSet(ReportLocalWaypoint.PITCH_BIT))
                reportLocalWaypoint.SetPitch(pitch);
            */
            if (message.IsFieldSet(ReportLocalWaypoint.YAW_BIT))
                reportLocalWaypoint.SetYaw(waypointElement.GetYaw());

            Transport.SendMessage(reportLocalWaypoint);

            return true;
        }

        public bool HandleQueryTravelSpeed(Message message)
        {
            ReportTravelSpeed reportTravelSpeed = new ReportTravelSpeed();
            reportTravelSpeed.SetDestination(message.GetSource());
            reportTravelSpeed.SetSource(message.GetDestination());
            reportTravelSpeed.SetSpeed(speed);

            Transport.SendMessage(reportTravelSpeed);

            return true;
        }

        public bool HandleSetTravelSpeed(SetTravelSpeed message)
        {
            desiredSpeed = message.GetSpeed();
            return true;
        }

        public bool HandleQueryActiveElement(Message message)
        {
            ReportActiveElement reportActiveElement = new ReportActiveElement();
            if (currentWaypointElement != null)
                reportActiveElement.SetElementID(currentWaypointElement.element.getElementUID());

            return true;
        }

        public bool ExecuteList()
        {
            waypointIndex = 0;

            currentWaypointElement = waypointList.First.Value;

            return true;
        }

        public WaypointElement IncrementList()
        {
            int currentIndex = Interlocked.Increment(ref waypointIndex);
            if (currentIndex >= waypointList.Count)
                return null;

            LinkedList<WaypointElement>.Enumerator itr = waypointList.GetEnumerator();
            int count = 0;
            while (count <= currentIndex + 1)
            {
                count++;
                itr.MoveNext();
            }

            WaypointElement currentWaypoint = itr.Current;
            currentWaypointElement = currentWaypoint;

            return currentWaypoint;
        }

        public void SetActiveWaypoint(int index)
        {

            waypointIndex = index;
            LinkedList<WaypointElement>.Enumerator itr = waypointList.GetEnumerator();
            int count = 0;
            while (count <= index + 1)
            {
                count++;
                itr.MoveNext();
            }
            currentWaypointElement = itr.Current;
        }

        public ReadOnlyCollection<WaypointElement> GetWaypoints()
        {
            //return Collections.unmodifiableList(waypointList);
            List<WaypointElement> li = new List<WaypointElement>();
            LinkedList<WaypointElement>.Enumerator itr = waypointList.GetEnumerator();
            while (!itr.MoveNext()) li.Add(itr.Current);
            return li.AsReadOnly();
        }

        public WaypointElement GetCurrentWaypoint()
        {
            return currentWaypointElement;
        }

        public override string ToString()
        {
            return "Local Waypoint List Driver";
        }
    }
}