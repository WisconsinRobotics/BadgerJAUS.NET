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
using System.Threading;

using BadgerJaus.Messages;
using BadgerJaus.Messages.Liveness;
using BadgerJaus.Messages.Discovery;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Core
{
    //this class will be used to test for liveness between a source and a destination.
    //if the source is the server, the destination is the robot.
    //if the source is the robot, the destination is the server. 
    public class Liveness : BaseService
    {
        private const long SLEEP_TIME = 4900; //how long we will sleep
        private const long TIMEOUT_LENGTH = 5000000001L;	//actual timeout length in nanosecond
        private const long TIMEOUT_LENGTH_MILLI = 5000; //TIMEOUT in millisecond

        private ConcurrentDictionary<long, LivenessTime> targetAddressMap;

        private static Liveness livenessService = null;

        public static Liveness CreateLivenessInstance()
        {
            if (livenessService == null)
                livenessService = new Liveness();

            return livenessService;
        }

        public static Liveness GetInstance()
        {
            return livenessService;
        }

        private Liveness()
        {
            //if (this.stopwatch == null) this.stopwatch = new Stopwatch();
            this.targetAddressMap = new ConcurrentDictionary<long, LivenessTime>();
        }

        protected override string OVERRIDE_SERVICE_NAME
        {
            get { return "Liveness"; }
        }

        protected override string OVERRIDE_SERVICE_FAMILY
        {
            get { return CORE_SERVICE; }
        }

        //tells us if the destination is live
        public bool IsLive(JausAddress checkAddress)
        {
            LivenessTime checkTime;
            this.targetAddressMap.TryGetValue(checkAddress.Value, out checkTime);
            if (checkTime == null)
                return false;

            long lastReceivedTime = checkTime.getTime();
            if (DateTime.Now.Millisecond * 1000000 - lastReceivedTime > TIMEOUT_LENGTH_MILLI)
                return false;
            return true;
        }

        private void SendQuery(Component component)
        {
            foreach (LivenessTime currentLivenessTime in this.targetAddressMap.Values)
            {
                QueryHeartbeatPulse queryHeartBeat = new QueryHeartbeatPulse();
                JausAddress destinationAddress = currentLivenessTime.getAddress();
                queryHeartBeat.SetSource(component.JausAddress);
                queryHeartBeat.SetDestination(destinationAddress);

                Transport.SendMessage(queryHeartBeat);
            }
        }

        public bool AddLivenessAddress(JausAddress destination)
        {
            JausAddress target = new JausAddress(destination);
            LivenessTime timeToAdd = new LivenessTime(DateTime.Now.Millisecond * 1000000, target);
            LivenessTime tempTime = this.targetAddressMap.GetOrAdd(destination.Value, timeToAdd);
            if (tempTime == null)
                return true;
            return false;
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            switch (message.GetCommandCode())
            {
                case JausCommandCode.REPORT_HEARTBEAT_PULSE:
                    LivenessTime time;
                    this.targetAddressMap.TryGetValue(message.GetSource().Value, out time);
                    if (time == null) return true;
                    time.setTime(DateTime.Now.Millisecond * 1000000);
                    return true;
                case JausCommandCode.QUERY_HEARTBEAT_PULSE:
                    ReportHeartbeatPulse reportHeartBeat = new ReportHeartbeatPulse();
                    reportHeartBeat.SetDestination(message.GetSource());
                    reportHeartBeat.SetSource(component.JausAddress);
                    Transport.SendMessage(reportHeartBeat);
                    return true;
                default:
                    return false;
            }
        }

        protected override void Execute(Component component)
        {
            if (this.targetAddressMap.Count == 0)
                return;
            SendQuery(component);
        }

        public override bool IsSupported(int commandCode)
        {
            switch (commandCode)
            {
                case JausCommandCode.REPORT_HEARTBEAT_PULSE:
                case JausCommandCode.QUERY_HEARTBEAT_PULSE:
                    return true;
                default:
                    return false;
            }
        }

        public ConcurrentDictionary<long, LivenessTime> GetAddresses()
        {
            return this.targetAddressMap;
        }

        public void ClearAddresses()
        {
            this.targetAddressMap.Clear();
        }

        public override long SleepTime
        {
            get { return SLEEP_TIME; }
        }

        public override string ToString()
        {
            return "Liveness";
        }
    }
}