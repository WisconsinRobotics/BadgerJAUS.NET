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

using BadgerJaus.Messages;
using BadgerJaus.Messages.VelocityStateDriver;
using BadgerJaus.Services.Core;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Mobility
{
    public class VelocityStateDriver : BaseService
    {
        public const string SERVICE_NAME = "VelocityStateDriver";
        public const string SERVICE_VERSION = "1.0";
        public const string SERVICE_ID = "urn:jaus:jss:mobility:VelocityStateDriver";
        public const string PARENT_SERVICE = "Management";

        protected double velocityX;
        protected double velocityY;
        protected double velocityZ;
        protected double rollRate;
        protected double pitchRate;
        protected double yawRate;

        protected double reportVelocityX;
        protected double reportVelocityY;
        protected double reportVelocityZ;
        protected double reportRollRate;
        protected double reportPitchRate;
        protected double reportYawRate;

        QueryVelocityCommand queryVelocityCommand;
        SetVelocityCommand setVelocityCommand;
        
        public VelocityStateDriver()
        {
            velocityX = 0;
            velocityY = 0;
            velocityZ = 0;
            rollRate = 0;
            pitchRate = 0;
            yawRate = 0;

            reportVelocityX = 0;
            reportVelocityY = 0;
            reportVelocityZ = 0;
            reportRollRate = 0;
            reportPitchRate = 0;
            reportYawRate = 0;

            queryVelocityCommand = new QueryVelocityCommand();
            setVelocityCommand = new SetVelocityCommand();
        }

        public override bool IsSupported(int commandCode)
        {
            switch (commandCode)
            {
                case JausCommandCode.QUERY_VELOCITY_COMMAND:
                case JausCommandCode.SET_VELOCITY_COMMAND:
                    return true;
                default:
                    return false;
            }
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            switch (message.GetCommandCode())
            {
                case JausCommandCode.QUERY_VELOCITY_COMMAND:
                    queryVelocityCommand.SetFromJausMessage(message);
                    return HandleQueryVelocityCommand(queryVelocityCommand);
                case JausCommandCode.SET_VELOCITY_COMMAND:
                    return HandleSetVelocityCommand(message, component);

                default:
                    return false;
            }
        }

        protected bool HandleQueryVelocityCommand(QueryVelocityCommand message)
        {
            //Initialize response

            ReportVelocityCommand report = new ReportVelocityCommand();
            report.SetDestination(message.GetSource());
            report.SetSource(message.GetDestination());

            //Set requested data
            report.VelocityX = velocityX;
            report.VelocityY = velocityY;
            report.VelocityZ = velocityZ;
            report.RollRate = rollRate;
            report.PitchRate = pitchRate;
            report.YawRate = yawRate;


            //Send response
            Transport.SendMessage(report);

            return true;
        }

        protected bool HandleSetVelocityCommand(Message message, Component component)
        {
            if (!component.IsController(message.GetSource()))
                return true;

            setVelocityCommand.SetFromJausMessage(message);

            velocityX = setVelocityCommand.VelocityX;
            velocityY = setVelocityCommand.VelocityY;
            velocityZ = setVelocityCommand.VelocityZ;
            rollRate = setVelocityCommand.RollRate;
            pitchRate = setVelocityCommand.PitchRate;
            yawRate = setVelocityCommand.YawRate;

            return true;
        }
    }
}
