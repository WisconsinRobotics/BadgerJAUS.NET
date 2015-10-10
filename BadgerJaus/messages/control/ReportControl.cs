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

namespace BadgerJaus.Messages.Control
{
    public class ReportControl : Message
    {
        // Size of payload not including Message Code
        private const int MAX_DATA_SIZE_BYTES = JausAddress.SIZE + JausBaseType.BYTE_BYTE_SIZE;


        // Message Fields
        JausByte authorityCode;
        JausAddress controller;

        protected override int CommandCode
        {
            get { return JausCommandCode.REPORT_CONTROL; }
        }

        protected override void InitFieldData()
        {
            authorityCode = new JausByte();
            controller = new JausAddress();
        }

        // Getters and Setters
        public int GetAuthorityCode()
        {
            return (int)authorityCode.Value;
        }

        public void SetAuthorityCode(int authorityCode)
        {
            this.authorityCode.Value = authorityCode;
        }

        public JausAddress GetController()
        {
            return controller;
        }

        public void SetController(JausAddress controller)
        {
            this.controller.setSubsystem(controller.getSubsystem());
            this.controller.setNode(controller.getNode());
            this.controller.setComponent(controller.getComponent());
        }

        public override int GetPayloadSize()
        {
            return MAX_DATA_SIZE_BYTES;
        }

        // Packs Message Fields into byte[] then passes array to super class
        protected override bool PayloadToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!controller.Serialize(buffer, indexOffset, out indexOffset))
            {
                return false;
            }

            if (!authorityCode.Serialize(buffer, indexOffset, out indexOffset))
            {
                return false;
            }

            return true;
        }

        // Takes Super's payload, and unpacks it into Message Fields
        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            controller.Deserialize(buffer, indexOffset, out indexOffset);

            authorityCode.Deserialize(buffer, indexOffset, out indexOffset);

            return true;
        }

        public override String ToString()
        {
            String str = base.ToString();
            //str += "Query Type: " + queryTypes[queryType.getValue() - 1] + "\n";
            return str;
        }
    }
}