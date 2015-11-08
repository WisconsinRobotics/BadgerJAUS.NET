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

namespace BadgerJaus.Messages
{
    public class JausCommandCode
    {
        public const int NONE = 0x0000;
        public const int SET_AUTHORITY = 0x0001;
        public const int SHUTDOWN = 0x0002;
        public const int STANDBY = 0x0003;
        public const int RESUME = 0x0004;
        public const int RESET = 0x0005;
        public const int SET_EMERGENCY = 0x0006;
        public const int CLEAR_EMERGENCY = 0x0007;
        public const int CREATE_SERVICE_CONNECTION = 0x0008;
        public const int CONFIRM_SERVICE_CONNECTION = 0x0009;
        public const int ACTIVATE_SERVICE_CONNECTION = 0x000A;
        public const int SUSPEND_SERVICE_CONNECTION = 0x000B;
        public const int TERMINATE_SERVICE_CONNECTION = 0x000C;
        public const int REQUEST_CONTROL = 0x000D;
        public const int RELEASE_CONTROL = 0x000E;
        public const int CONFIRM_CONTROL = 0x000F;
        public const int REJECT_CONTROL = 0x0010;
        public const int SET_TIME = 0x0011;
        public const int CREATE_EVENT = 0x01F0;
        public const int UPDATE_EVENT = 0x01F1;
        public const int CANCEL_EVENT = 0x01F2;
        public const int CONFIRM_EVENT = 0x01F3;
        public const int REJECT_EVENT = 0x01F4;
        public const int SET_DATA_LINK_STATUS = 0x0200;
        public const int SET_DATA_LINK_SELECT = 0x0201;
        public const int SET_GLOBAL_POSE = 0x0402;
        public const int SET_LOCAL_POSE = 0x0403;
        public const int SET_WRENCH_EFFORT = 0x0405;
        public const int SET_DISCRETE_DEVICES = 0x0406;
        public const int SET_GLOBAL_VECTOR = 0x0407;
        public const int SET_LOCAL_VECTOR = 0x0408;
        public const int SET_TRAVEL_SPEED = 0x040A;
        public const int SET_GLOBAL_WAYPOINT = 0x040C;
        public const int SET_LOCAL_WAYPOINT = 0x040D;
        public const int SET_GLOBAL_PATH_SEGMENT = 0x040F;
        public const int SET_LOCAL_PATH_SEGMENT = 0x0410;
        public const int SET_GEOMAGNETIC_PROPERTY = 0x0412;
        public const int SET_VELOCITY_COMMAND = 0x0415;
        public const int SET_ACCELERATION_LIMIT = 0x0416;
        public const int SET_ELEMENT = 0x041A;
        public const int DELETE_ELEMENT = 0x041B;
        public const int CONFIRM_ELEMENT_REQUEST = 0x041C;
        public const int REJECT_ELEMENT_REQUEST = 0x041D;
        public const int EXECUTE_LIST = 0x041E;
        public const int SET_JOINT_EFFORTS = 0x0601;
        public const int SET_JOINT_POSITIONS = 0x0602;
        public const int SET_JOINT_VELOCITIES = 0x0603;
        public const int SET_TOOL_POINT = 0x0604;
        public const int SET_END_EFFECTOR_VELOCITY_STATE = 0x0606;
        public const int SET_JOINT_MOTION = 0x0607;
        public const int SET_END_EFFECTOR_PATH_MOTION = 0x0608;
        public const int SET_END_EFFECTOR_POSE = 0x0610;
        public const int SET_CAMERA_POSE = 0x0801;
        public const int SELECT_CAMERA = 0x0802;
        public const int SET_CAMERA_CAPABILITIES = 0x0805;
        public const int SET_CAMERA_FORMAT_OPTIONS = 0x0806;
        public const int REGISTER_SERVICES = 0x0B00;
        public const int SET_TOOL_OFFSET = 0x0604;

        // JAUS Query Class Messages
        public const int QUERY_AUTHORITY = 0x2001;
        public const int QUERY_STATUS = 0x2002;
        public const int QUERY_TIMEOUT = 0x2003;
        public const int QUERY_TIME = 0x2011;
        public const int QUERY_CONTROL = 0x200D;
        public const int QUERY_EVENTS = 0x21F0;
        public const int QUERY_DATA_LINK_STATUS = 0x2200;
        public const int QUERY_SELECTED_DATA_LINK_STATUS = 0x2201;
        public const int QUERY_HEARTBEAT_PULSE = 0x2202;
        public const int QUERY_PLATFORM_SPECIFICATIONS = 0x2400;
        public const int QUERY_PLATFORM_OPERATIONAL_DATA = 0x2401;
        public const int QUERY_GLOBAL_POSE = 0x2402;
        public const int QUERY_LOCAL_POSE = 0x2403;
        public const int QUERY_VELOCITY_STATE = 0x2404;
        public const int QUERY_WRENCH_EFFORT = 0x2405;
        public const int QUERY_DISCRETE_DEVICES = 0x2406;
        public const int QUERY_GLOBAL_VECTOR = 0x2407;
        public const int QUERY_LOCAL_VECTOR = 0x2408;
        public const int QUERY_TRAVEL_SPEED = 0x240A;
        public const int QUERY_WAYPOINT_COUNT = 0x240B;
        public const int QUERY_GLOBAL_WAYPOINT = 0x240C;
        public const int QUERY_LOCAL_WAYPOINT = 0x240D;
        public const int QUERY_PATH_SEGMENT_COUNT = 0x240E;
        public const int QUERY_GLOBAL_PATH_SEGMENT = 0x240F;
        public const int QUERY_LOCAL_PATH_SEGMENT = 0x2410;
        public const int QUERY_GEOMAGNETIC_PROPERTY = 0x2412;
        public const int QUERY_VELOCITY_COMMAND = 0x2415;
        public const int QUERY_ACCELERATION_LIMIT = 0x2416;
        public const int QUERY_ELEMENT = 0x241A;
        public const int QUERY_ELEMENT_LIST = 0x241B;
        public const int QUERY_ELEMENT_COUNT = 0x241C;
        public const int QUERY_ACTIVE_ELEMENT = 0x241E;
        public const int QUERY_MANIPULATOR_SPECIFICATIONS = 0x2600;
        public const int QUERY_JOINT_EFFORTS = 0x2601;
        public const int QUERY_JOINT_POSITIONS = 0x2602;
        public const int QUERY_JOINT_VELOCITIES = 0x2603;
        public const int QUERY_TOOL_POINT = 0x2604;
        public const int QUERY_JOINT_FORCE_TORQUES = 0x2605;
        public const int QUERY_COMMANDED_END_EFFECTOR_POSE = 0x2610;
        public const int QUERY_END_EFFECTOR_POSE = 0x2615;
        public const int QUERY_CAMERA_POSE = 0x2800;
        public const int QUERY_CAMERA_COUNT = 0x2801;
        public const int QUERY_RELATIVE_OBJECT_POSITION = 0x2802;
        public const int QUERY_SELECTED_CAMERA = 0x2804;
        public const int QUERY_CAMERA_CAPABILITIES = 0x2805;
        public const int QUERY_CAMERA_FORMAT_OPTIONS = 0x2806;
        public const int QUERY_IMAGE = 0x2807;
        public const int QUERY_IDENTIFICATION = 0x2B00;
        public const int QUERY_CONFIGURATION = 0x2B01;
        public const int QUERY_SUBSYSTEM_LIST = 0x2B02;
        public const int QUERY_SERVICES = 0x2B03;

        // JAUS Inform Class Messages
        public const int REPORT_AUTHORITY = 0x4001;
        public const int REPORT_STATUS = 0x4002;
        public const int REPORT_TIMEOUT = 0x4003;
        public const int REPORT_TIME = 0x4011;
        public const int REPORT_CONTROL = 0x400D;
        public const int REPORT_EVENTS = 0x41F0;
        public const int EVENT = 0x41F1;
        public const int REPORT_DATA_LINK_STATUS = 0x4200;
        public const int REPORT_SELECTED_DATA_LINK_STATUS = 0x4201;
        public const int REPORT_HEARTBEAT_PULSE = 0x4202;
        public const int REPORT_PLATFORM_SPECIFICATIONS = 0x4400;
        public const int REPORT_PLATFORM_OPERATIONAL_DATA = 0x4401;
        public const int REPORT_GLOBAL_POSE = 0x4402;
        public const int REPORT_LOCAL_POSE = 0x4403;
        public const int REPORT_VELOCITY_STATE = 0x4404;
        public const int REPORT_WRENCH_EFFORT = 0x4405;
        public const int REPORT_DISCRETE_DEVICES = 0x4406;
        public const int REPORT_GLOBAL_VECTOR = 0x4407;
        public const int REPORT_LOCAL_VECTOR = 0x4408;
        public const int REPORT_TRAVEL_SPEED = 0x440A;
        public const int REPORT_WAYPOINT_COUNT = 0x440B;
        public const int REPORT_GLOBAL_WAYPOINT = 0x440C;
        public const int REPORT_LOCAL_WAYPOINT = 0x440D;
        public const int REPORT_PATH_SEGMENT_COUNT = 0x440E;
        public const int REPORT_GLOBAL_PATH_SEGMENT = 0x440F;
        public const int REPORT_LOCAL_PATH_SEGMENT = 0x4410;
        public const int REPORT_GEOMAGNETIC_PROPERTY = 0x4412;
        public const int REPORT_VELOCITY_COMMAND = 0x4415;
        public const int REPORT_ACCELERATION_LIMIT = 0x4416;
        public const int REPORT_ELEMENT = 0x441A;
        public const int REPORT_ELEMENT_LIST = 0x441B;
        public const int REPORT_ELEMENT_COUNT = 0x441C;
        public const int REPORT_ACTIVE_ELEMENT = 0x441E;
        public const int REPORT_MANIPULATOR_SPECIFICATIONS = 0x4600;
        public const int REPORT_JOINT_EFFORTS = 0x4601;
        public const int REPORT_JOINT_POSITIONS = 0x4602;
        public const int REPORT_JOINT_VELOCITIES = 0x4603;
        public const int REPORT_TOOL_POINT = 0x4604;
        public const int REPORT_JOINT_FORCE_TORQUES = 0x4605;
        public const int REPORT_COMMANDED_END_EFFECTOR_POSE = 0x4610;
        public const int REPORT_END_EFFECTOR_POSE = 0x4615;
        public const int REPORT_CAMERA_POSE = 0x4800;
        public const int REPORT_CAMERA_COUNT = 0x4801;
        public const int REPORT_RELATIVE_OBJECT_POSITION = 0x4802;
        public const int REPORT_SELECTED_CAMERA = 0x4804;
        public const int REPORT_CAMERA_CAPABILITIES = 0x4805;
        public const int REPORT_CAMERA_FORMAT_OPTIONS = 0x4806;
        public const int REPORT_IMAGE = 0x4807;
        public const int REPORT_IDENTIFICATION = 0x4B00;
        public const int REPORT_CONFIGURATION = 0x4B01;
        public const int REPORT_SUBSYSTEM_LIST = 0x4B02;
        public const int REPORT_SERVICES = 0x4B03;

        //@ Deprecated public const int CONFIGURATION_CHANGED_EVENT_SETUP 				= 0xD6A8;
        //@ Deprecated public const int CONFIGURATION_CHANGED_EVENT_NOTIFICATION 		= 0xD8A8;

        // Message Type Enum
        /*	
            public enum MessageType {
		
                COMMAND, QUERY, INFORM	
            }*/

        //private static HashMap<Integer, String> commandCodes = null;
        private static Dictionary<Int32, String> commandCodes = null;

        public static String Lookup(int code)
        {
            if (commandCodes == null)
            {
                commandCodes = new Dictionary<Int32, String>();
                commandCodes.Add(NONE, "NONE");
                commandCodes.Add(SET_AUTHORITY, "SET_AUTHORITY");
                commandCodes.Add(SHUTDOWN, "SHUTDOWN");
                commandCodes.Add(STANDBY, "STANDBY");
                commandCodes.Add(RESUME, "RESUME");
                commandCodes.Add(RESET, "RESET");
                commandCodes.Add(SET_EMERGENCY, "SET_EMERGENCY");
                commandCodes.Add(CLEAR_EMERGENCY, "CLEAR_EMERGENCY");
                commandCodes.Add(CREATE_SERVICE_CONNECTION, "CREATE_SERVICE_CONNECTION");
                commandCodes.Add(CONFIRM_SERVICE_CONNECTION, "CONFIRM_SERVICE_CONNECTION");
                commandCodes.Add(ACTIVATE_SERVICE_CONNECTION, "ACTIVATE_SERVICE_CONNECTION");
                commandCodes.Add(SUSPEND_SERVICE_CONNECTION, "SUSPEND_SERVICE_CONNECTION");
                commandCodes.Add(TERMINATE_SERVICE_CONNECTION, "TERMINATE_SERVICE_CONNECTION");
                commandCodes.Add(REQUEST_CONTROL, "REQUEST_CONTROL");
                commandCodes.Add(RELEASE_CONTROL, "RELEASE_CONTROL");
                commandCodes.Add(CONFIRM_CONTROL, "CONFIRM_CONTROL");
                commandCodes.Add(REJECT_CONTROL, "REJECT_CONTROL");
                commandCodes.Add(SET_TIME, "SET_TIME");
                commandCodes.Add(CREATE_EVENT, "CREATE_EVENT");
                commandCodes.Add(UPDATE_EVENT, "UPDATE_EVENT");
                commandCodes.Add(CANCEL_EVENT, "CANCEL_EVENT");
                commandCodes.Add(CONFIRM_EVENT, "CONFIRM_EVENT");
                commandCodes.Add(REJECT_EVENT, "REJECT_EVENT");
                commandCodes.Add(SET_DATA_LINK_STATUS, "SET_DATA_LINK_STATUS");
                commandCodes.Add(SET_DATA_LINK_SELECT, "SET_DATA_LINK_SELECT");
                commandCodes.Add(SET_GLOBAL_POSE, "SET_GLOBAL_POSE");
                commandCodes.Add(SET_LOCAL_POSE, "SET_LOCAL_POSE");
                commandCodes.Add(SET_WRENCH_EFFORT, "SET_WRENCH_EFFORT");
                commandCodes.Add(SET_DISCRETE_DEVICES, "SET_DISCRETE_DEVICES");
                commandCodes.Add(SET_GLOBAL_VECTOR, "SET_GLOBAL_VECTOR");
                commandCodes.Add(SET_LOCAL_VECTOR, "SET_LOCAL_VECTOR");
                commandCodes.Add(SET_TRAVEL_SPEED, "SET_TRAVEL_SPEED");
                commandCodes.Add(SET_GLOBAL_WAYPOINT, "SET_GLOBAL_WAYPOINT");
                commandCodes.Add(SET_LOCAL_WAYPOINT, "SET_LOCAL_WAYPOINT");
                commandCodes.Add(SET_GLOBAL_PATH_SEGMENT, "SET_GLOBAL_PATH_SEGMENT");
                commandCodes.Add(SET_GEOMAGNETIC_PROPERTY, "SET_GEOMAGNETIC_PROPERTY");
                commandCodes.Add(SET_VELOCITY_COMMAND, "SET_VELOCITY_COMMAND");
                commandCodes.Add(SET_ACCELERATION_LIMIT, "SET_ACCELERATION_LIMIT");
                commandCodes.Add(SET_LOCAL_PATH_SEGMENT, "SET_LOCAL_PATH_SEGMENT");
                commandCodes.Add(SET_ELEMENT, "SET_ELEMENT");
                commandCodes.Add(DELETE_ELEMENT, "DELETE_ELEMENT");
                commandCodes.Add(CONFIRM_ELEMENT_REQUEST, "CONFIRM_ELEMENT_REQUEST");
                commandCodes.Add(REJECT_ELEMENT_REQUEST, "REJECT_ELEMENT_REQUEST");
                commandCodes.Add(EXECUTE_LIST, "EXECUTE_LIST");
                commandCodes.Add(SET_JOINT_EFFORTS, "SET_JOINT_EFFORTS");
                commandCodes.Add(SET_JOINT_POSITIONS, "SET_JOINT_POSITIONS");
                commandCodes.Add(SET_JOINT_VELOCITIES, "SET_JOINT_VELOCITIES");
                commandCodes.Add(SET_TOOL_POINT, "SET_TOOL_POINT");
                commandCodes.Add(SET_END_EFFECTOR_POSE, "SET_END_EFFECTOR_POSE");
                commandCodes.Add(SET_END_EFFECTOR_VELOCITY_STATE, "SET_END_EFFECTOR_VELOCITY_STATE");
                commandCodes.Add(SET_JOINT_MOTION, "SET_JOINT_MOTION");
                commandCodes.Add(SET_END_EFFECTOR_PATH_MOTION, "SET_END_EFFECTOR_PATH_MOTION");
                commandCodes.Add(SET_CAMERA_POSE, "SET_CAMERA_POSE");
                commandCodes.Add(SELECT_CAMERA, "SELECT_CAMERA");
                commandCodes.Add(SET_CAMERA_CAPABILITIES, "SET_CAMERA_CAPABILITIES");
                commandCodes.Add(SET_CAMERA_FORMAT_OPTIONS, "SET_CAMERA_FORMAT_OPTIONS");
                commandCodes.Add(REGISTER_SERVICES, "REGISTER_SERVICES");

                // JAUS Query Class Messages
                commandCodes.Add(QUERY_AUTHORITY, "QUERY_AUTHORITY");
                commandCodes.Add(QUERY_STATUS, "QUERY_STATUS");
                commandCodes.Add(QUERY_TIMEOUT, "QUERY_TIMEOUT");
                commandCodes.Add(QUERY_TIME, "QUERY_TIME");
                commandCodes.Add(QUERY_CONTROL, "QUERY_CONTROL");
                commandCodes.Add(QUERY_EVENTS, "QUERY_EVENTS");
                commandCodes.Add(QUERY_DATA_LINK_STATUS, "QUERY_DATA_LINK_STATUS");
                commandCodes.Add(QUERY_SELECTED_DATA_LINK_STATUS, "QUERY_SELECTED_DATA_LINK_STATUS");
                commandCodes.Add(QUERY_HEARTBEAT_PULSE, "QUERY_HEARTBEAT_PULSE");
                commandCodes.Add(QUERY_PLATFORM_SPECIFICATIONS, "QUERY_PLATFORM_SPECIFICATIONS");
                commandCodes.Add(QUERY_PLATFORM_OPERATIONAL_DATA, "QUERY_PLATFORM_OPERATIONAL_DATA");
                commandCodes.Add(QUERY_GLOBAL_POSE, "QUERY_GLOBAL_POSE");
                commandCodes.Add(QUERY_LOCAL_POSE, "QUERY_LOCAL_POSE");
                commandCodes.Add(QUERY_VELOCITY_STATE, "QUERY_VELOCITY_STATE");
                commandCodes.Add(QUERY_WRENCH_EFFORT, "QUERY_WRENCH_EFFORT");
                commandCodes.Add(QUERY_DISCRETE_DEVICES, "QUERY_DISCRETE_DEVICES");
                commandCodes.Add(QUERY_GLOBAL_VECTOR, "QUERY_GLOBAL_VECTOR");
                commandCodes.Add(QUERY_LOCAL_VECTOR, "QUERY_LOCAL_VECTOR");
                //commandCodes.Add(QUERY_LOCAL_VECTOR, "QUERY_LOCAL_VECTOR");
                commandCodes.Add(QUERY_WAYPOINT_COUNT, "QUERY_WAYPOINT_COUNT");
                commandCodes.Add(QUERY_GLOBAL_WAYPOINT, "QUERY_GLOBAL_WAYPOINT");
                commandCodes.Add(QUERY_LOCAL_WAYPOINT, "QUERY_LOCAL_WAYPOINT");
                commandCodes.Add(QUERY_PATH_SEGMENT_COUNT, "QUERY_PATH_SEGMENT_COUNT");
                commandCodes.Add(QUERY_GLOBAL_PATH_SEGMENT, "QUERY_GLOBAL_PATH_SEGMENT");
                commandCodes.Add(QUERY_LOCAL_PATH_SEGMENT, "QUERY_LOCAL_PATH_SEGMENT");
                commandCodes.Add(QUERY_GEOMAGNETIC_PROPERTY, "QUERY_GEOMAGNETIC_PROPERTY");
                commandCodes.Add(QUERY_VELOCITY_COMMAND, "QUERY_VELOCITY_COMMAND");
                commandCodes.Add(QUERY_ACCELERATION_LIMIT, "QUERY_ACCELERATION_LIMIT");
                commandCodes.Add(QUERY_ELEMENT, "QUERY_ELEMENT");
                commandCodes.Add(QUERY_ELEMENT_LIST, "QUERY_ELEMENT_LIST");
                commandCodes.Add(QUERY_ELEMENT_COUNT, "QUERY_ELEMENT_COUNT");
                commandCodes.Add(QUERY_ACTIVE_ELEMENT, "QUERY_ACTIVE_ELEMENT");
                commandCodes.Add(QUERY_MANIPULATOR_SPECIFICATIONS, "QUERY_MANIPULATOR_SPECIFICATIONS");
                commandCodes.Add(QUERY_JOINT_EFFORTS, "QUERY_JOINT_EFFORTS");
                commandCodes.Add(QUERY_JOINT_POSITIONS, "QUERY_JOINT_POSITIONS");
                commandCodes.Add(QUERY_JOINT_VELOCITIES, "QUERY_JOINT_VELOCITIES");
                commandCodes.Add(QUERY_TOOL_POINT, "QUERY_TOOL_POINT");
                commandCodes.Add(QUERY_JOINT_FORCE_TORQUES, "QUERY_JOINT_FORCE_TORQUES");
                commandCodes.Add(QUERY_CAMERA_POSE, "QUERY_CAMERA_POSE");
                commandCodes.Add(QUERY_CAMERA_COUNT, "QUERY_CAMERA_COUNT");
                commandCodes.Add(QUERY_RELATIVE_OBJECT_POSITION, "QUERY_RELATIVE_OBJECT_POSITION");
                commandCodes.Add(QUERY_SELECTED_CAMERA, "QUERY_SELECTED_CAMERA");
                commandCodes.Add(QUERY_CAMERA_CAPABILITIES, "QUERY_CAMERA_CAPABILITIES");
                commandCodes.Add(QUERY_CAMERA_FORMAT_OPTIONS, "QUERY_CAMERA_FORMAT_OPTIONS");
                commandCodes.Add(QUERY_IMAGE, "QUERY_IMAGE");
                commandCodes.Add(QUERY_IDENTIFICATION, "QUERY_IDENTIFICATION");
                commandCodes.Add(QUERY_CONFIGURATION, "QUERY_CONFIGURATION");
                commandCodes.Add(QUERY_SUBSYSTEM_LIST, "QUERY_SUBSYSTEM_LIST");
                commandCodes.Add(QUERY_SERVICES, "QUERY_SERVICES");

                // JAUS Inform Class Messages
                commandCodes.Add(REPORT_AUTHORITY, "REPORT_AUTHORITY");
                commandCodes.Add(REPORT_STATUS, "REPORT_STATUS");
                commandCodes.Add(REPORT_TIMEOUT, "REPORT_TIMEOUT");
                commandCodes.Add(REPORT_TIME, "REPORT_TIME");
                commandCodes.Add(REPORT_CONTROL, "REPORT_CONTROL");
                commandCodes.Add(REPORT_EVENTS, "REPORT_EVENTS");
                commandCodes.Add(EVENT, "EVENT");
                commandCodes.Add(REPORT_DATA_LINK_STATUS, "REPORT_DATA_LINK_STATUS");
                commandCodes.Add(REPORT_SELECTED_DATA_LINK_STATUS, "REPORT_SELECTED_DATA_LINK_STATUS");
                commandCodes.Add(REPORT_HEARTBEAT_PULSE, "REPORT_HEARTBEAT_PULSE");
                commandCodes.Add(REPORT_PLATFORM_SPECIFICATIONS, "REPORT_PLATFORM_SPECIFICATIONS");
                commandCodes.Add(REPORT_PLATFORM_OPERATIONAL_DATA, "REPORT_PLATFORM_OPERATIONAL_DATA");
                commandCodes.Add(REPORT_GLOBAL_POSE, "REPORT_GLOBAL_POSE");
                commandCodes.Add(REPORT_LOCAL_POSE, "REPORT_LOCAL_POSE");
                commandCodes.Add(REPORT_VELOCITY_STATE, "REPORT_VELOCITY_STATE");
                commandCodes.Add(REPORT_WRENCH_EFFORT, "REPORT_WRENCH_EFFORT");
                commandCodes.Add(REPORT_DISCRETE_DEVICES, "REPORT_DISCRETE_DEVICES");
                commandCodes.Add(REPORT_GLOBAL_VECTOR, "REPORT_GLOBAL_VECTOR");
                commandCodes.Add(REPORT_LOCAL_VECTOR, "REPORT_LOCAL_VECTOR");
                commandCodes.Add(REPORT_TRAVEL_SPEED, "REPORT_TRAVEL_SPEED");
                commandCodes.Add(REPORT_WAYPOINT_COUNT, "REPORT_WAYPOINT_COUNT");
                commandCodes.Add(REPORT_GLOBAL_WAYPOINT, "REPORT_GLOBAL_WAYPOINT");
                commandCodes.Add(REPORT_LOCAL_WAYPOINT, "REPORT_LOCAL_WAYPOINT");
                commandCodes.Add(REPORT_PATH_SEGMENT_COUNT, "REPORT_PATH_SEGMENT_COUNT");
                commandCodes.Add(REPORT_GLOBAL_PATH_SEGMENT, "REPORT_GLOBAL_PATH_SEGMENT");
                commandCodes.Add(REPORT_LOCAL_PATH_SEGMENT, "REPORT_LOCAL_PATH_SEGMENT");
                commandCodes.Add(REPORT_GEOMAGNETIC_PROPERTY, "REPORT_GEOMAGNETIC_PROPERTY");
                commandCodes.Add(REPORT_VELOCITY_COMMAND, "REPORT_VELOCITY_COMMAND");
                commandCodes.Add(REPORT_ACCELERATION_LIMIT, "REPORT_ACCELERATION_LIMIT");
                commandCodes.Add(REPORT_ELEMENT, "REPORT_ELEMENT");
                commandCodes.Add(REPORT_ELEMENT_LIST, "REPORT_ELEMENT_LIST");
                commandCodes.Add(REPORT_ELEMENT_COUNT, "REPORT_ELEMENT_COUNT");
                commandCodes.Add(REPORT_ACTIVE_ELEMENT, "REPORT_ACTIVE_ELEMENT");
                commandCodes.Add(REPORT_MANIPULATOR_SPECIFICATIONS, "REPORT_MANIPULATOR_SPECIFICATIONS");
                commandCodes.Add(REPORT_JOINT_EFFORTS, "REPORT_JOINT_EFFORTS");
                commandCodes.Add(REPORT_JOINT_POSITIONS, "REPORT_JOINT_POSITIONS");
                commandCodes.Add(REPORT_JOINT_VELOCITIES, "REPORT_JOINT_VELOCITIES");
                commandCodes.Add(REPORT_TOOL_POINT, "REPORT_TOOL_POINT");
                commandCodes.Add(REPORT_JOINT_FORCE_TORQUES, "REPORT_JOINT_FORCE_TORQUES");
                commandCodes.Add(REPORT_CAMERA_POSE, "REPORT_CAMERA_POSE");
                commandCodes.Add(REPORT_CAMERA_COUNT, "REPORT_CAMERA_COUNT");
                commandCodes.Add(REPORT_RELATIVE_OBJECT_POSITION, "REPORT_RELATIVE_OBJECT_POSITION");
                commandCodes.Add(REPORT_SELECTED_CAMERA, "REPORT_SELECTED_CAMERA");
                commandCodes.Add(REPORT_CAMERA_CAPABILITIES, "REPORT_CAMERA_CAPABILITIES");
                commandCodes.Add(REPORT_CAMERA_FORMAT_OPTIONS, "REPORT_CAMERA_FORMAT_OPTIONS");
                commandCodes.Add(REPORT_IMAGE, "REPORT_IMAGE");
                commandCodes.Add(REPORT_IDENTIFICATION, "REPORT_IDENTIFICATION");
                commandCodes.Add(REPORT_CONFIGURATION, "REPORT_CONFIGURATION");
                commandCodes.Add(REPORT_SUBSYSTEM_LIST, "REPORT_SUBSYSTEM_LIST");
                commandCodes.Add(REPORT_SERVICES, "REPORT_SERVICES");
            }

            String output;
            commandCodes.TryGetValue(code, out output);
            return output;
        }
    }
}