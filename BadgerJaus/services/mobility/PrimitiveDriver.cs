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
using BadgerJaus.Messages.PrimitiveDriver;

using BadgerJaus.Services.Core;

using BadgerJaus.Util;

namespace BadgerJaus.Services.Mobility
{
    public class PrimitiveDriver : BaseService
    {
        public const string SERVICE_NAME = "PrimitiveDriver";
        public const string SERVICE_VERSION = "1.0";
        public const string PARENT_SERVICE = "Management";

        private double propLinearX;
        private double propLinearY;
        private double propLinearZ;

        private double propRotX;
        private double propRotY;
        private double propRotZ;

        private double resistLinearX;
        private double resistLinearY;
        private double resistLinearZ;

        private double resistRotX;
        private double resistRotY;
        private double resistRotZ;

        protected override string OVERRIDE_SERVICE_NAME
        {
            get { return SERVICE_NAME; }
        }

        protected override string OVERRIDE_SERVICE_FAMILY
        {
            get { return MOBILITY_SERVICE; }
        }

        public override bool IsSupported(int commandCode)
        {
            return false;
        }

        public double PropLinearX
        {
            get { return propLinearX; }
        }
        public double PropLinearY
        {
            get { return propLinearY; }
        }
        public double PropLinearZ
        {
            get { return propLinearZ; }
        }

        public double PropRotX
        {
            get { return propRotX; }
        }
        public double PropRotY
        {
            get { return propRotY; }
        }
        public double PropRotZ
        {
            get { return propRotZ; }
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            switch (message.GetCommandCode())
            {
                case JausCommandCode.QUERY_WRENCH_EFFORT:
                    QueryWrenchEffort queryEffort = new QueryWrenchEffort();
                    queryEffort.SetFromJausMessage(message);

                    return HandleQueryWrenchEffort(queryEffort);

                case JausCommandCode.SET_WRENCH_EFFORT:
                    SetWrenchEffort setEffort = new SetWrenchEffort();
                    setEffort.SetFromJausMessage(message);

                    return HandleSetWrenchEffort(setEffort);

                default:
                    return false;
            }
        }

        public bool HandleQueryWrenchEffort(QueryWrenchEffort message)
        {
            //Initialize response
            ReportWrenchEffort report = new ReportWrenchEffort();
            report.SetDestination(message.GetSource());
            report.SetSource(message.GetDestination());

            //Set requested data
            if (message.IsBitSet(SetWrenchEffort.PROPULSIVE_LINEAR_EFFORT_X_BIT))
                report.SetPropulsiveLinearEffortX(propLinearX);

            if (message.IsBitSet(SetWrenchEffort.PROPULSIVE_LINEAR_EFFORT_Y_BIT))
                report.SetPropulsiveLinearEffortY(propLinearY);

            if (message.IsBitSet(SetWrenchEffort.PROPULSIVE_LINEAR_EFFORT_Z_BIT))
                report.SetPropulsiveLinearEffortZ(propLinearZ);

            if (message.IsBitSet(SetWrenchEffort.PROPULSIVE_ROTATIONAL_EFFORT_X_BIT))
                report.SetPropulsiveRotationalEffortX(propRotX);

            if (message.IsBitSet(SetWrenchEffort.PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT))
                report.SetPropulsiveRotationalEffortY(propRotY);

            if (message.IsBitSet(SetWrenchEffort.PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT))
                report.SetPropulsiveRotationalEffortZ(propRotZ);

            if (message.IsBitSet(SetWrenchEffort.RESISTIVE_LINEAR_EFFORT_X_BIT))
                report.SetResistiveLinearEffortX(resistLinearX);

            if (message.IsBitSet(SetWrenchEffort.RESISTIVE_LINEAR_EFFORT_Y_BIT))
                report.SetResistiveLinearEffortY(resistLinearY);

            if (message.IsBitSet(SetWrenchEffort.RESISTIVE_LINEAR_EFFORT_Z_BIT))
                report.SetResistiveLinearEffortZ(resistLinearZ);

            if (message.IsBitSet(SetWrenchEffort.RESISTIVE_ROTATIONAL_EFFORT_X_BIT))
                report.SetResistiveRotationalEffortX(resistRotX);

            if (message.IsBitSet(SetWrenchEffort.RESISTIVE_ROTATIONAL_EFFORT_Y_BIT))
                report.SetResistiveRotationalEffortY(resistRotY);

            if (message.IsBitSet(SetWrenchEffort.RESISTIVE_ROTATIONAL_EFFORT_Z_BIT))
                report.SetResistiveRotationalEffortZ(resistRotZ);

            //Send out message
            Transport.SendMessage(report);

            return true;
        }

        public bool HandleSetWrenchEffort(SetWrenchEffort message)
        {
            if (message.isFieldSet(SetWrenchEffort.PROPULSIVE_LINEAR_EFFORT_X_BIT))
                propLinearX = message.GetPropulsiveLinearEffortX();

            if (message.isFieldSet(SetWrenchEffort.PROPULSIVE_LINEAR_EFFORT_Y_BIT))
                propLinearY = message.GetPropulsiveLinearEffortY();

            if (message.isFieldSet(SetWrenchEffort.PROPULSIVE_LINEAR_EFFORT_Z_BIT))
                propLinearZ = message.GetPropulsiveLinearEffortZ();

            if (message.isFieldSet(SetWrenchEffort.PROPULSIVE_ROTATIONAL_EFFORT_X_BIT))
                propRotX = message.GetPropulsiveRotationalEffortX();

            if (message.isFieldSet(SetWrenchEffort.PROPULSIVE_ROTATIONAL_EFFORT_Y_BIT))
                propRotY = message.GetPropulsiveRotationalEffortY();

            if (message.isFieldSet(SetWrenchEffort.PROPULSIVE_ROTATIONAL_EFFORT_Z_BIT))
                propRotZ = message.GetPropulsiveRotationalEffortZ();

            if (message.isFieldSet(SetWrenchEffort.RESISTIVE_LINEAR_EFFORT_X_BIT))
                resistLinearX = message.GetResistiveLinearEffortX();

            if (message.isFieldSet(SetWrenchEffort.RESISTIVE_LINEAR_EFFORT_Y_BIT))
                resistLinearY = message.GetResistiveLinearEffortY();

            if (message.isFieldSet(SetWrenchEffort.RESISTIVE_LINEAR_EFFORT_Z_BIT))
                resistLinearZ = message.GetResistiveLinearEffortZ();

            if (message.isFieldSet(SetWrenchEffort.RESISTIVE_ROTATIONAL_EFFORT_X_BIT))
                resistRotX = message.GetResistiveRotationalEffortX();

            if (message.isFieldSet(SetWrenchEffort.RESISTIVE_ROTATIONAL_EFFORT_Y_BIT))
                resistRotY = message.GetResistiveRotationalEffortY();

            if (message.isFieldSet(SetWrenchEffort.RESISTIVE_ROTATIONAL_EFFORT_Z_BIT))
                resistRotZ = message.GetResistiveRotationalEffortZ();

            return true;
        }

        public override string ToString()
        {
            return "Primitive Driver";
        }
    }
}