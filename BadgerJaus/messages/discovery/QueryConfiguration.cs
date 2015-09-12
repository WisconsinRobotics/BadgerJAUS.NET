using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Messages;
using BadgerJaus.Util;

namespace BadgerJaus.messages.discovery
{
    public class QueryConfiguration : Message
    {
        public const int SUBSYSTEM_CONFIGURATION = 2;
        public const int NODE_CONFIGURATION = 3;

        JausByte queryType;

        protected override int CommandCode
        {
            get { return JausCommandCode.QUERY_IDENTIFICATION; }
        }

        protected override void InitFieldData()
        {
            queryType = new JausByte(SUBSYSTEM_CONFIGURATION);
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!queryType.toJausBuffer(buffer, indexOffset)) return false;
            indexOffset += JausByte.SIZE_BYTES;

            return true;
        }

        // Takes Super's payload, and unpacks it into Message Fields
        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            queryType.setValue(buffer[indexOffset]);
            indexOffset += JausByte.SIZE_BYTES;
            return true;
        }
    }
}
