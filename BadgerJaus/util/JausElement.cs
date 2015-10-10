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
using System.Linq;

using BadgerJaus.Messages;
using BadgerJaus.Messages.LocalWaypointDriver;

namespace BadgerJaus.Util
{
    public class JausElement
    {
        public const int MIN_DATA_SIZE_BYTES = 9;

        //Message Fields
        private JausUnsignedShort elementUID;
        private JausUnsignedShort previousUID;
        private JausUnsignedShort nextUID;
        private JausUnsignedShort dataSize;
        private JausByte formatField;
        private byte[] elementData;

        public JausElement()
        {
            this.configure();
        }

        public JausElement(byte[] byteArray)
            : this(byteArray, 0)
        {

        }

        public JausElement(byte[] byteArray, int index)
        {
            int indexOffset;
            this.configure();
            this.Deserialize(byteArray, index, out indexOffset);
        }

        public JausElement(JausElement e)
        {
            this.configure();
            elementUID.Value = e.getElementUID();
            previousUID.Value = e.getPreviousUID();
            nextUID.Value = e.getNextUID();
            elementData = e.getElementData();
            dataSize.Value = elementData.Length;
        }

        //Getters and Setters
        public bool setElementUID(int value)
        {
            if (value >= 65535)
            {
                Console.Error.WriteLine("Invalid UID value");
                return false;
            }
            elementUID.Value = value;
            return true;
        }

        public int getElementUID()
        {
            return (int)elementUID.Value;
        }

        public void setPreviousUID(int value)
        {
            previousUID.Value = value;
        }

        public int getPreviousUID()
        {
            return (int)previousUID.Value;
        }

        public void setNextUID(int value)
        {
            nextUID.Value = value;
        }

        public int getNextUID()
        {
            return (int)nextUID.Value;
        }

        public void setElementData(byte[] data)
        {
            elementData = new byte[data.Length];
            Array.Copy(data, 0, elementData, 0, data.Length);
            dataSize.Value = data.Length;
        }

        public byte[] getElementData()
        {
            byte[] temp = new byte[dataSize.Value];
            Array.Copy(elementData, 0, temp, 0, dataSize.Value);
            return temp;
        }

        public bool Deserialize(byte[] byteArray, out int indexOffset)
        {
            Deserialize(byteArray, 0, out indexOffset);
            return false;
        }

        public bool Deserialize(byte[] byteArray, int index, out int indexOffset)
        {
            indexOffset = index;
            if (indexOffset + MIN_DATA_SIZE_BYTES > byteArray.Length)
            {
                Console.Error.WriteLine("Not enough size for Jaus Element");
                return false;
            }
            if (!elementUID.Deserialize(byteArray, indexOffset, out indexOffset)) return false;

            if (!previousUID.Deserialize(byteArray, indexOffset, out indexOffset)) return false;

            if (!nextUID.Deserialize(byteArray, indexOffset, out indexOffset)) return false;

            if (!formatField.Deserialize(byteArray, indexOffset, out indexOffset)) return false;
            //Byte for variable-format field

            dataSize.Deserialize(byteArray, indexOffset, out indexOffset);

            elementData = new byte[dataSize.Value];
            Array.Copy(byteArray, indexOffset, elementData, 0, dataSize.Value);
            indexOffset += (int)dataSize.Value;
            return true;
        }

        public int size()
        {
            return MIN_DATA_SIZE_BYTES + (int)dataSize.Value;
        }

        public String toString()
        {
            SetLocalWaypoint temp = new SetLocalWaypoint();
            //temp.setPayloadFromJausBuffer(elementData, Message.COMMAND_CODE_SIZE_BYTES);
            return "\nElement UID: " + elementUID + "\n" +
                "Previous UID: " + previousUID + "\n" +
                "Next UID: " + nextUID + "\n" +
                "Data Size: " + dataSize + "\n" +
                temp + "\n";
        }

        public String toHexString()
        {
            String hex = elementUID.toHexString() + previousUID.toHexString()
                 + nextUID.toHexString() + dataSize.toHexString();
            String temp = "";

            for (int i = 0; i < elementData.Length; i++)
            {
                temp += String.Format("{0:X}", elementData[i]).ToUpper();
            }
            return hex + temp;
        }

        public bool Serialize(byte[] byteArray, int index, out int indexOffset)
        {
            indexOffset = index;
            if (!elementUID.Serialize(byteArray, indexOffset, out indexOffset)) return false;

            if (!previousUID.Serialize(byteArray, indexOffset, out indexOffset)) return false;

            if (!nextUID.Serialize(byteArray, indexOffset, out indexOffset)) return false;

            index++;	//Byte for variable-format field
            if (!dataSize.Serialize(byteArray, indexOffset, out indexOffset)) return false;

            Array.Copy(elementData, 0, byteArray, index, dataSize.Value);
            return true;
        }

        public bool Serialize(byte[] byteArray, out int indexOffset)
        {
            Serialize(byteArray, 0, out indexOffset);
            return false;
        }

        public void configure()
        {
            elementUID = new JausUnsignedShort();
            previousUID = new JausUnsignedShort();
            nextUID = new JausUnsignedShort();
            formatField = new JausByte();
            dataSize = new JausUnsignedShort();
            elementData = new byte[0];
        }

        public bool equals(JausElement other)
        {
            return this.getElementUID() == other.getElementUID() &&
                this.getPreviousUID() == other.getPreviousUID() &&
                this.getNextUID() == other.getNextUID() &&
                //Arrays.equals(this.getElementData(), other.getElementData());
                Enumerable.SequenceEqual(this.getElementData(), other.getElementData());
        }

    }
}