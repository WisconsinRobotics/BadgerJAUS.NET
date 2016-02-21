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
using BadgerJaus.Messages.Management;
using BadgerJaus.Messages.Control;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Core
{
    public class Management : BaseService
    {
        new public const String SERVICE_NAME = "Management";
        new private const String SERVICE_ID = "urn:jaus:jss:core:Management";
        new public const String PARENT_SERVICE = "AccessControl";

        private ReportStatus reportStatus;
        private ReleaseControl releaseControl;

        private static Management managementService = null;

        public static Management CreateManagementInstance()
        {
            if (managementService == null)
                managementService = new Management();

            return managementService;
        }

        public static Management GetInstance()
        {
            return managementService;
        }

        private Management()
        {

        }

        protected override string OVERRIDE_SERVICE_NAME
        {
            get { return "Management"; }
        }

        protected override string OVERRIDE_SERVICE_FAMILY
        {
            get { return CORE_SERVICE; }
        }

        public override bool IsSupported(int commandCode)
        {
            switch (commandCode)
            {
                case JausCommandCode.SHUTDOWN:
                case JausCommandCode.STANDBY:
                case JausCommandCode.RESUME:
                case JausCommandCode.QUERY_STATUS:
                    return true;
                default:
                    return false;
            }
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            // TODO Auto-generated method stub
            switch (message.GetCommandCode())
            {
                case JausCommandCode.SHUTDOWN:
                    releaseControl = new ReleaseControl();
                    releaseControl.SetAddressFromJausMessage(message);
                    component.ComponentState = ComponentState.STATE_SHUTDOWN;

                    return AccessControl.GetInstance().ImplementsAndHandledMessage(releaseControl, component);
                case JausCommandCode.STANDBY:
                    if (component.IsController(message.GetSource()))
                        component.ComponentState = ComponentState.STATE_STANDBY;
                    return true;
                case JausCommandCode.RESUME:
                    if (component.IsController(message.GetSource()))
                        component.ComponentState = ComponentState.STATE_READY;
                    return true;
                case JausCommandCode.QUERY_STATUS:
                    reportStatus = new ReportStatus();
                    reportStatus.SetDestination(message.GetSource());
                    reportStatus.SetSource(component.JausAddress);
                    reportStatus.SetStatus((int)component.ComponentState);
                    Transport.SendMessage(reportStatus);
                    return true;
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            return "Management";
        }
    }
}