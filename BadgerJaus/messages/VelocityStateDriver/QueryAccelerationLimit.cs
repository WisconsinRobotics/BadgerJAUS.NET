using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Util;

namespace BadgerJaus.Messages.VelocityStateDriver
{
    public class QueryAccelerationLimit : Message
    {
        JausBytePresenceVector presenceVector;
        JausByte commandType;

        public const int SET_MAXIMUM_ALLOWED = 1;
        public const int SET_MINIMUM_ALLOWED = 2;

        public const int ACCELERATION_X_BIT = 0;
        public const int ACCELERATION_Y_BIT = 1;
        public const int ACCELERATION_Z_BIT = 2;
        public const int ROLL_ACCELERATION_BIT = 3;
        public const int PITCH_ACCELERATION_BIT = 4;
        public const int YAW_ACCELERATION_BIT = 5;

        protected override int CommandCode
        {
            get { return JausCommandCode.QUERY_ACCELERATION_LIMIT; }
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
