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

using BadgerJaus.Util;

namespace BadgerJaus.Messages.VelocityStateDriver
{
    public class QueryVelocityCommand : Message
    {
        protected JausBytePresenceVector presenceVector;
        JausByte commandType;

        public const int SET_CURRENT = 0;
        public const int SET_MAXIMUM_ALLOWED = 1;
        public const int SET_MINIMUM_ALLOWED = 2;
        public const int SET_DEFAULT = 3;

        public const int VELOCITY_X_BIT = 0;
        public const int VELOCITY_Y_BIT = 1;
        public const int VELOCITY_Z_BIT = 2;
        public const int ROLL_RATE_BIT = 3;
        public const int PITCH_RATE_BIT = 4;
        public const int YAW_RATE_BIT = 5;

        protected override int CommandCode
        {
            get { return JausCommandCode.QUERY_VELOCITY_COMMAND; }
        }

        protected override void InitFieldData()
        {
            presenceVector = new JausBytePresenceVector();
            commandType = new JausByte();
        }

        public bool IsFieldSet(int bit)
        {
            return presenceVector.IsBitSet(bit);
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!PresenceOperation(buffer, indexOffset, out indexOffset, true))
                return false;

            return commandType.Deserialize(buffer, indexOffset, out indexOffset);
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!PresenceOperation(buffer, indexOffset, out indexOffset, false))
                return false;
            return commandCode.Serialize(buffer, indexOffset, out indexOffset);
        }

        private bool PresenceOperation(byte[] buffer, int index, out int indexOffset, bool set)
        {
            if (set)
                return presenceVector.Deserialize(buffer, index, out indexOffset);
            return presenceVector.Serialize(buffer, index, out indexOffset);
        }

        public int CommandType
        {
            get { return (int)commandType.Value; }
            set { commandType.Value = value; }
        }
    }
}
