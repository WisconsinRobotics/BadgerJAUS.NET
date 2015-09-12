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
using System.Text;

using BadgerJaus.Util;

namespace BadgerJaus.Messages.Discovery
{
    public class ReportIdentification : QueryIdentification
    {
        public const int SYSTEM_IDENTIFICATION = 1;
        public const int SUBSYSTEM_IDENTIFICATION = 2;
        public const int NODE_IDENTIFICATION = 3;
        public const int COMPONENT_IDENTIFICATION = 4;

        private JausUnsignedShort type;
        private string identification;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_IDENTIFICATION; }
        }

        public string Identification
        {
            get { return identification; }
            set { identification = value; }
        }

        public override int GetPayloadSize()
        {
            return JausByte.SIZE_BYTES + JausUnsignedShort.SIZE_BYTES + JausByte.SIZE_BYTES + identification.Length;
        }

        protected override void InitFieldData()
        {
            base.InitFieldData();
            type = new JausUnsignedShort();
            identification = "";
        }

        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            base.PayloadToJausBuffer(buffer, index, out indexOffset);

            if (!type.toJausBuffer(buffer, indexOffset)) return false;

            indexOffset += JausUnsignedShort.SIZE_BYTES;

            JausByte identificationCountField = new JausByte(identification.Length);

            if (!identificationCountField.toJausBuffer(buffer, indexOffset)) return false;

            indexOffset += JausByte.SIZE_BYTES;

            if (indexOffset + identification.Length > buffer.Length)
                return false;

            Encoding enc = Encoding.UTF8;
            Array.Copy(enc.GetBytes(identification), 0, buffer, indexOffset, identification.Length);
            indexOffset += identification.Length;

            return true;
        }

        // Takes Super's payload, and unpacks it into Message Fields
        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            JausByte identificationCountField = new JausByte();
            int identificationLength;

            base.SetPayloadFromJausBuffer(buffer, index, out indexOffset);
            type.setFromJausBuffer(buffer, indexOffset);
            indexOffset += JausUnsignedShort.SIZE_BYTES;
            identificationCountField.setFromJausBuffer(buffer, indexOffset);
            indexOffset += JausByte.SIZE_BYTES;
            identificationLength = identificationCountField.getValue();

            if (identificationLength <= 0)
                return true;

            if (indexOffset + identificationLength > buffer.Length)
                return false;

            identification = Encoding.UTF8.GetString(buffer, indexOffset, identificationLength);
            indexOffset += identificationLength;

            return true;
        }
    }
}