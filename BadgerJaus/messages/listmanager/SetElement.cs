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

using BadgerJaus.Util;

namespace BadgerJaus.Messages.ListManager
{
    public class SetElement : Message
    {
        private JausByte requestID = new JausByte();
        private JausByte listSize = new JausByte();
        private LinkedList<JausElement> elements = new LinkedList<JausElement>();

        protected override int CommandCode
        {
            get { return JausCommandCode.SET_ELEMENT; }
        }

        public void SetRequestID(int value)
        {
            requestID.setValue(value);
        }

        public int GetRequestID()
        {
            return requestID.getValue();
        }

        public bool AddElement(JausElement element)
        {
            if (elements.Count >= 255) return false;

            elements.AddLast(element);
            listSize.setValue(listSize.getValue() + 1);

            return true;
        }

        protected override bool SetPayloadFromJausBuffer(byte[] buffer, int index)
        {

            if (!requestID.setFromJausBuffer(buffer, index)) return false;
            index += JausByte.SIZE_BYTES;

            if (!listSize.setFromJausBuffer(buffer, index)) return false;
            index += JausByte.SIZE_BYTES;
            Console.WriteLine("Element list size: " + listSize.getValue());
            for (int count = 0; count < listSize.getValue(); count++)
            {
                JausElement element = new JausElement();
                if (!element.setFromJausBuffer(buffer, index)) return false;
                elements.AddLast(element);
                index += element.size();
            }

            return true;
        }

        public List<JausElement> GetElements()
        {
            return new List<JausElement>(new List<JausElement>(elements).AsReadOnly());
        }
    }
}