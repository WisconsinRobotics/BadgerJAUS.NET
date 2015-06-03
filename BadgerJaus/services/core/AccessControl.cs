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
using BadgerJaus.Messages.Control;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Core
{
    public class AccessControl : BaseService
    {
        public const String SERVICE_NAME = "AccessControl";
        public const String SERVICE_VERSION = "1.0";
        public const String SERVICE_ID = "urn:jaus:jss:core:AccessControl";
        public const String PARENT_SERVICE = "Events";

        public const int DEFAULT_AUTHORITY_CODE = 10;

        ControlState controlState;
        JausAddress controller;
        int authorityCode;

        ConfirmControl confirmControl;
        RejectControl rejectControl;
        ReportAuthority reportAuthority;
        ReportControl reportControl;

        RequestControl requestControl = new RequestControl();

        public AccessControl()
            : base()
        {
            controlState = ControlState.STATE_NOT_CONTROLLED;
            controller = new JausAddress();
            authorityCode = DEFAULT_AUTHORITY_CODE;
        }

        /*
        public override int GetState()
        {
            // TODO Auto-generated method stub
            return 0;
        }
        */

        public override bool IsSupported(int commandCode)
        {
            switch(commandCode)
            {
                case JausCommandCode.QUERY_CONTROL:
                case JausCommandCode.RELEASE_CONTROL:
                case JausCommandCode.REQUEST_CONTROL:
                case JausCommandCode.QUERY_AUTHORITY:
                case JausCommandCode.SET_AUTHORITY:
                case JausCommandCode.QUERY_TIMEOUT:
                    return true;
                default:
                    return false;
            }
        }

        public override bool ImplementsAndHandledMessage(Message message)
        {
            switch (message.GetCommandCode())
            {
                case JausCommandCode.QUERY_CONTROL:
                    reportControl = new ReportControl();
                    reportControl.SetController(controller);
                    reportControl.SetAuthorityCode(authorityCode);
                    reportControl.SetDestination(message.GetSource());
                    reportControl.SetSource(jausAddress);
                    Transport.SendMessage(reportControl);
                    return true;

                case JausCommandCode.RELEASE_CONTROL:
                    rejectControl = new RejectControl();
                    rejectControl.SetResponseCode(RejectControl.CONTROL_RELEASED);
                    rejectControl.SetDestination(message.GetSource());
                    rejectControl.SetSource(jausAddress);
                    if (controlState == ControlState.STATE_CONTROLLED)
                    {
                        if (controller.equals(message.GetSource()))
                        {
                            controller.setId(0);
                            authorityCode = DEFAULT_AUTHORITY_CODE;
                            controlState = ControlState.STATE_NOT_CONTROLLED;
                        }
                        else
                        {
                            rejectControl.SetResponseCode(RejectControl.NOT_AVAILABLE);
                        }
                    }

                    Transport.SendMessage(rejectControl);
                    return true;

                case JausCommandCode.REQUEST_CONTROL:
                    requestControl.SetFromJausMessage(message);
                    switch (controlState)
                    {
                        case ControlState.STATE_CONTROL_NOT_AVAILABLE:
                            confirmControl = new ConfirmControl();
                            confirmControl.SetResponseCode(ConfirmControl.NOT_AVAILABLE);
                            confirmControl.SetDestination(requestControl.GetSource());
                            confirmControl.SetSource(jausAddress);
                            Transport.SendMessage(confirmControl);
                            break;
                        case ControlState.STATE_CONTROLLED:
                            if (controller.equals(requestControl.GetSource()))
                            {
                                if (requestControl.GetAuthorityCode() <= DEFAULT_AUTHORITY_CODE)
                                {
                                    confirmControl = new ConfirmControl();
                                    confirmControl.SetDestination(requestControl.GetSource());
                                    confirmControl.SetResponseCode(ConfirmControl.CONTROL_ACCEPTED);
                                    confirmControl.SetSource(message.GetDestination());
                                    Transport.SendMessage(confirmControl);
                                }
                                else
                                {
                                    rejectControl = new RejectControl();
                                    rejectControl.SetResponseCode(RejectControl.CONTROL_RELEASED);
                                    controlState = ControlState.STATE_NOT_CONTROLLED;
                                    Transport.SendMessage(rejectControl);
                                }
                            }
                            else
                            {
                                confirmControl = new ConfirmControl();
                                confirmControl.SetDestination(requestControl.GetSource());
                                confirmControl.SetSource(jausAddress);
                                if (requestControl.GetAuthorityCode() < authorityCode)
                                {
                                    rejectControl = new RejectControl();
                                    rejectControl.SetResponseCode(RejectControl.CONTROL_RELEASED);
                                    rejectControl.SetDestination(controller);
                                    rejectControl.SetSource(requestControl.GetDestination());
                                    Transport.SendMessage(rejectControl);
                                    confirmControl.SetResponseCode(ConfirmControl.CONTROL_ACCEPTED);
                                    controller.setId(requestControl.GetSource().getId());
                                    authorityCode = requestControl.GetAuthorityCode();
                                }
                                else
                                {
                                    confirmControl.SetResponseCode(ConfirmControl.INSUFFICIENT_AUTHORITY);
                                }

                                Transport.SendMessage(confirmControl);
                            }

                            break;
                        case ControlState.STATE_NOT_CONTROLLED:
                            confirmControl = new ConfirmControl();
                            confirmControl.SetDestination(requestControl.GetSource());
                            if (requestControl.GetAuthorityCode() <= DEFAULT_AUTHORITY_CODE)
                            {
                                confirmControl.SetResponseCode(ConfirmControl.CONTROL_ACCEPTED);
                                authorityCode = requestControl.GetAuthorityCode();
                                controller.setId(requestControl.GetSource().getId());
                                controlState = ControlState.STATE_CONTROLLED;
                            }
                            else
                            {
                                confirmControl.SetResponseCode(ConfirmControl.INSUFFICIENT_AUTHORITY);
                            }

                            Transport.SendMessage(confirmControl);
                            break;
                    }

                    return true;
                case JausCommandCode.QUERY_AUTHORITY:
                    reportAuthority = new ReportAuthority();
                    reportAuthority.SetAuthorityCode(authorityCode);
                    reportAuthority.SetDestination(message.GetSource());
                    //reportAuthority.setSource(message.getDestination());
                    reportAuthority.SetSource(jausAddress);
                    Transport.SendMessage(reportAuthority);
                    return true;
                case JausCommandCode.SET_AUTHORITY:
                    return true;
                case JausCommandCode.QUERY_TIMEOUT:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsController(JausAddress address)
        {
            if (controlState != ControlState.STATE_CONTROLLED)
                return false;

            if (controller.getId() != address.getId())
                return false;

            return true;
        }
    }
}