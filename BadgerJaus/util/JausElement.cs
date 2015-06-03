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
    public class JausElement : JausType
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
            this.configure();
            this.setFromJausBuffer(byteArray, index);
        }

        public JausElement(JausElement e)
        {
            this.configure();
            elementUID.setValue(e.getElementUID());
            previousUID.setValue(e.getPreviousUID());
            nextUID.setValue(e.getNextUID());
            elementData = e.getElementData();
            dataSize.setValue(elementData.Length);
        }

        //Getters and Setters
        public bool setElementUID(int value)
        {
            if (value >= 65535)
            {
                Console.Error.WriteLine("Invalid UID value");
                return false;
            }
            elementUID.setValue(value);
            return true;
        }

        public int getElementUID()
        {
            return elementUID.getValue();
        }

        public void setPreviousUID(int value)
        {
            previousUID.setValue(value);
        }

        public int getPreviousUID()
        {
            return previousUID.getValue();
        }

        public void setNextUID(int value)
        {
            nextUID.setValue(value);
        }

        public int getNextUID()
        {
            return nextUID.getValue();
        }

        public void setElementData(byte[] data)
        {
            elementData = new byte[data.Length];
            Array.Copy(data, 0, elementData, 0, data.Length);
            dataSize.setValue(data.Length);
        }

        public byte[] getElementData()
        {
            byte[] temp = new byte[dataSize.getValue()];
            Array.Copy(elementData, 0, temp, 0, dataSize.getValue());
            return temp;
        }

        public bool setFromJausBuffer(byte[] byteArray)
        {
            setFromJausBuffer(byteArray, 0);
            return false;
        }

        public bool setFromJausBuffer(byte[] byteArray, int index)
        {
            if (index + MIN_DATA_SIZE_BYTES > byteArray.Length)
            {
                Console.Error.WriteLine("Not enough size for Jaus Element");
                return false;
            }
            if (!elementUID.setFromJausBuffer(byteArray, index)) return false;
            index += JausUnsignedShort.SIZE_BYTES;

            if (!previousUID.setFromJausBuffer(byteArray, index)) return false;
            index += JausUnsignedShort.SIZE_BYTES;

            if (!nextUID.setFromJausBuffer(byteArray, index)) return false;
            index += JausUnsignedShort.SIZE_BYTES;

            if (!formatField.setFromJausBuffer(byteArray, index)) return false;
            index += JausByte.SIZE_BYTES;	//Byte for variable-format field

            dataSize.setFromJausBuffer(byteArray, index);
            index += JausUnsignedShort.SIZE_BYTES;

            elementData = new byte[dataSize.getValue()];
            Array.Copy(byteArray, index, elementData, 0, dataSize.getValue());
            return true;
        }

        public int size()
        {
            return MIN_DATA_SIZE_BYTES + dataSize.getValue();
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

        public bool toJausBuffer(byte[] byteArray, int index)
        {
            if (!elementUID.toJausBuffer(byteArray, index)) return false;
            index += JausUnsignedShort.SIZE_BYTES;

            if (!previousUID.toJausBuffer(byteArray, index)) return false;
            index += JausUnsignedShort.SIZE_BYTES;

            if (!nextUID.toJausBuffer(byteArray, index)) return false;
            index += JausUnsignedShort.SIZE_BYTES;

            index++;	//Byte for variable-format field
            if (!dataSize.toJausBuffer(byteArray, index)) return false;
            index += JausUnsignedShort.SIZE_BYTES;

            Array.Copy(elementData, 0, byteArray, index, dataSize.getValue());
            return true;
        }

        public bool toJausBuffer(byte[] byteArray)
        {
            toJausBuffer(byteArray, 0);
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