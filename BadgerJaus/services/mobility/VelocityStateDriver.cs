using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using BadgerJaus.Messages;
using BadgerJaus.Messages.VelocityStateDriver;
using BadgerJaus.Services.Core;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Mobility
{
    public class VelocityStateDriver : BaseService
    {
        public const String SERVICE_NAME = "VelocityStateDriver";
        public const String SERVICE_VERSION = "1.0";
        public const String SERVICE_ID = "urn:jaus:jss:mobility:VelocityStateDriver";
        public const String PARENT_SERVICE = "Management";

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
