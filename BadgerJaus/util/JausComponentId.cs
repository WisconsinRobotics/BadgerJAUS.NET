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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadgerJaus.util
{
    /// <summary>
    /// The legacy JAUS Reference Architecture had a set of predefined
    /// component IDs that represented specific pieces of functionality.
    /// Those IDs remain valid for usage in SAE JAUS conformant systems and
    /// this class contains the definitions for the legacy IDs.
    /// </summary>
    public class JausComponentId
    {
        public const int SUBSYSTEM_COMMANDER = 32;
        public const int SYSTEM_COMMANDER = 40;
        
        public const int COMMUNICATOR = 35;

        public const int PRIMITIVE_DRIVER = 33;
        public const int GLOBAL_VECTOR_DRIVER = 34;
        public const int GLOBAL_POSE_SENSOR = 38;
        public const int LOCAL_POSE_SENSOR = 41;
        public const int VELOCITY_STATE_SENSOR = 42;
        public const int REFLEXIVE_DRIVER = 43;
        public const int LOCAL_VECTOR_DRIVER = 44;
        public const int GLOBAL_WAYPOINT_DRIVER = 45;
        public const int LOCAL_WAYPOINT_DRIVER = 46;
        public const int GLOBAL_PATH_SEGMENT_DRIVER = 47;
        public const int LOCAL_PATH_SEGMENT_DRIVER = 48;

        public const int PRIMITIVE_MANIPULATOR = 49;
        public const int MANIPULATOR_JOINT_POSITION_SENSOR = 51;
        public const int MANIPULATOR_JOINT_VELOCITY_SENSOR = 52;
        public const int MANIPULATOR_JOINT_FORCE_TORQUE_SENSOR = 53;
        public const int MANIPULATOR_JOINT_POSITIONS_DRIVER = 54;
        public const int MANIPULATOR_END_EFFECTOR_POSE_DRIVER = 55;
        public const int MANIPULATOR_JOINT_VELOCITIES_DRIVER = 56;
        public const int MANIPULATOR_END_EFFECTOR_VELOCITY_STATE_DRIVER = 57;
        public const int MANIPULATOR_JOINT_MOVE_DRIVER = 58;
        public const int MANIPULATOR_END_EFFECTOR_DISCRETE_POSE_DRIVER = 59;

        public const int VISUAL_SENSOR = 37;
        public const int RANGE_SENSOR = 50;
        public const int WORLD_MODEL_VECTOR_KNOWLEDGE_STORE = 61;

        public const int MISSION_SPOOLER = 36;
    }
}
