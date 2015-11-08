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

using BadgerJaus.Util;

namespace BadgerJaus.Messages.VelocityStateSensor
{
    public class QueryVelocityState : Message
    {
        protected JausShortPresenceVector presence;

        public const int X_BIT = 0;
        public const int Y_BIT = 1;
        public const int Z_BIT = 2;
        public const int P_RMS = 3;
        public const int ROLL_BIT = 4;
        public const int PITCH_BIT = 5;
        public const int YAW_BIT = 6;
        public const int A_RMS = 7;
        public const int TS_BIT = 8;

        protected override int CommandCode
        {
            get { return JausCommandCode.QUERY_VELOCITY_STATE; }
        }

        protected override void InitFieldData()
        {
            presence = new JausShortPresenceVector();
        }

        public bool IsFieldSet(int bit)
        {
            return presence.IsBitSet(bit);
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            return PresenceOperation(buffer, index, out indexOffset, true);
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            return PresenceOperation(buffer, index, out indexOffset, false);
        }

        private bool PresenceOperation(byte[] buffer, int index, out int indexOffset, bool set)
        {
            if (set)
                return presence.Deserialize(buffer, index, out indexOffset);
            return presence.Serialize(buffer, index, out indexOffset);
        }
    }
}