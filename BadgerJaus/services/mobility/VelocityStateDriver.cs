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
        public const string SERVICE_VERSION = "1.0";
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

        protected double accelerationX;
        protected double accelerationY;
        protected double accelerationZ;
        protected double rollAcceleration;
        protected double pitchAcceleration;
        protected double yawAcceleration;

        protected double reportAccelerationX;
        protected double reportAccelerationY;
        protected double reportAccelerationZ;
        protected double reportRollAcceleration;
        protected double reportPitchAcceleration;
        protected double reportYawAcceleration;

        QueryVelocityCommand queryVelocityCommand;
        SetVelocityCommand setVelocityCommand;

        QueryAccelerationLimit queryAccelerationLimit;
        SetAccelerationLimit setAccelerationLimit;
        
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

            accelerationX = 0;
            accelerationY = 0;
            accelerationZ = 0;
            rollAcceleration = 0;
            pitchAcceleration = 0;
            yawAcceleration = 0;

            reportAccelerationX = 0;
            reportAccelerationY = 0;
            reportAccelerationZ = 0;
            reportRollAcceleration = 0;
            reportPitchAcceleration = 0;
            reportYawAcceleration = 0;

            queryVelocityCommand = new QueryVelocityCommand();
            setVelocityCommand = new SetVelocityCommand();

            queryAccelerationLimit = new QueryAccelerationLimit();
            setAccelerationLimit = new SetAccelerationLimit();
        }

        protected override string OVERRIDE_SERVICE_NAME
        {
            get { return "VelocityStateDriver"; }
        }

        protected override string OVERRIDE_SERVICE_FAMILY
        {
            get { return MOBILITY_SERVICE; }
        }

        public override bool IsSupported(int commandCode)
        {
            switch (commandCode)
            {
                case JausCommandCode.QUERY_VELOCITY_COMMAND:
                case JausCommandCode.SET_VELOCITY_COMMAND:

                case JausCommandCode.QUERY_ACCELERATION_LIMIT:
                case JausCommandCode.SET_ACCELERATION_LIMIT:
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

                case JausCommandCode.QUERY_ACCELERATION_LIMIT:
                    queryAccelerationLimit.SetFromJausMessage(message);
                    return HandleQueryAccelerationLimit(queryAccelerationLimit);
                case JausCommandCode.SET_ACCELERATION_LIMIT:
                    return HandleSetAccelerationLimit(message, component);
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
            if (message.IsFieldSet(QueryVelocityCommand.VELOCITY_X_BIT))
                report.VelocityX = reportVelocityX;
            if (message.IsFieldSet(QueryVelocityCommand.VELOCITY_Y_BIT))
                report.VelocityY = reportVelocityY;
            if (message.IsFieldSet(QueryVelocityCommand.VELOCITY_Z_BIT))
                report.VelocityZ = reportVelocityZ;
            if (message.IsFieldSet(QueryVelocityCommand.ROLL_RATE_BIT))
                report.RollRate = reportRollRate;
            if (message.IsFieldSet(QueryVelocityCommand.PITCH_RATE_BIT))
                report.PitchRate = reportPitchRate;
            if (message.IsFieldSet(QueryVelocityCommand.YAW_RATE_BIT))
                report.YawRate = reportYawRate;


            //Send response
            Transport.SendMessage(report);

            return true;
        }

        protected bool HandleSetVelocityCommand(Message message, Component component)
        {
            if (!component.IsController(message.GetSource()))
                return true;

            setVelocityCommand.SetFromJausMessage(message);

            if (setVelocityCommand.IsFieldSet(QueryVelocityCommand.VELOCITY_X_BIT))
                velocityX = setVelocityCommand.VelocityX;
            if (setVelocityCommand.IsFieldSet(QueryVelocityCommand.VELOCITY_Y_BIT))
                velocityY = setVelocityCommand.VelocityY;
            if (setVelocityCommand.IsFieldSet(QueryVelocityCommand.VELOCITY_Y_BIT))
                velocityZ = setVelocityCommand.VelocityZ;
            if (setVelocityCommand.IsFieldSet(QueryVelocityCommand.ROLL_RATE_BIT))
                rollRate = setVelocityCommand.RollRate;
            if (setVelocityCommand.IsFieldSet(QueryVelocityCommand.PITCH_RATE_BIT))
                pitchRate = setVelocityCommand.PitchRate;
            if (setVelocityCommand.IsFieldSet(QueryVelocityCommand.YAW_RATE_BIT))
                yawRate = setVelocityCommand.YawRate;

            return true;
        }

        protected bool HandleQueryAccelerationLimit(QueryAccelerationLimit message)
        {
            //Initialize response

            ReportAccelerationLimit report = new ReportAccelerationLimit();
            report.SetDestination(message.GetSource());
            report.SetSource(message.GetDestination());

            //Set requested data
            if (message.IsFieldSet(QueryAccelerationLimit.ACCELERATION_X_BIT))
                report.AccelerationX = accelerationX;
            if (message.IsFieldSet(QueryAccelerationLimit.ACCELERATION_Y_BIT))
                report.AccelerationY = accelerationY;
            if (message.IsFieldSet(QueryAccelerationLimit.ACCELERATION_Z_BIT))
                report.AccelerationZ = accelerationZ;
            if (message.IsFieldSet(QueryAccelerationLimit.ROLL_ACCELERATION_BIT))
                report.RollAcceleration = rollAcceleration;
            if (message.IsFieldSet(QueryAccelerationLimit.PITCH_ACCELERATION_BIT))
                report.PitchAcceleration = pitchAcceleration;
            if (message.IsFieldSet(QueryAccelerationLimit.YAW_ACCELERATION_BIT))
                report.YawAcceleration = yawAcceleration;


            //Send response
            Transport.SendMessage(report);

            return true;
        }

        protected bool HandleSetAccelerationLimit(Message message, Component component)
        {
            if (!component.IsController(message.GetSource()))
                return true;

            setAccelerationLimit.SetFromJausMessage(message);

            if (setAccelerationLimit.IsFieldSet(QueryAccelerationLimit.ACCELERATION_X_BIT))
                accelerationX = setAccelerationLimit.AccelerationX;
            if (setAccelerationLimit.IsFieldSet(QueryAccelerationLimit.ACCELERATION_Y_BIT))
                accelerationY = setAccelerationLimit.AccelerationY;
            if (setAccelerationLimit.IsFieldSet(QueryAccelerationLimit.ACCELERATION_Z_BIT))
                accelerationZ = setAccelerationLimit.AccelerationZ;
            if (setAccelerationLimit.IsFieldSet(QueryAccelerationLimit.ROLL_ACCELERATION_BIT))
                rollAcceleration = setAccelerationLimit.RollAcceleration;
            if (setAccelerationLimit.IsFieldSet(QueryAccelerationLimit.PITCH_ACCELERATION_BIT))
                pitchAcceleration = setAccelerationLimit.PitchAcceleration;
            if (setAccelerationLimit.IsFieldSet(QueryAccelerationLimit.YAW_ACCELERATION_BIT))
                yawAcceleration = setAccelerationLimit.YawAcceleration;

            return true;
        }

        public override string ToString()
        {
            return "Velocity State Driver";
        }
    }
}
