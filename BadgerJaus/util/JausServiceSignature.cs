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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace BadgerJaus.Util
{
    public class JausServiceSignature
    {

        // Values for Service identification (See RegisterServicesMessage)
        private string uri;
        private JausByte majorVersionNumber;
        private JausByte minorVersionNumber;

        // Constructors
        public JausServiceSignature()
        {

            uri = "";
            majorVersionNumber = new JausByte();
            minorVersionNumber = new JausByte();

        }

        public JausServiceSignature(byte[] buffer, int index)
            : this()
        {
            int indexOffset;
            this.Deserialize(buffer, index, out indexOffset);

        }

        public JausServiceSignature(String uri, int majorVN, int minorVN)
        {

            this.uri = uri;
            this.majorVersionNumber = new JausByte(majorVN);
            this.minorVersionNumber = new JausByte(minorVN);

        }

        // Getters and Setters

        // URI
        public string URI
        {
            get { return uri; }
            set { uri = value; }
        }

        // Major Version Number
        public int MajorVersion
        {
            get { return (int)majorVersionNumber.Value; }
            set { majorVersionNumber.Value = value; }
        }

        public int MinorVersion
        {
            get { return (int)minorVersionNumber.Value; }
            set { minorVersionNumber.Value = value; }
        }

        // JausType Methods

        public bool Serialize(byte[] byteArray, out int indexOffset)
        {

            return this.Serialize(byteArray, 0, out indexOffset);
        }

        private static byte[] getBytes(String str)
        {
            byte[] bytes = new byte[str.Length + sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static String getString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new String(chars);
        }

        public bool Serialize(byte[] byteArray, int index, out int indexOffset)
        {
            Encoding enc = Encoding.UTF8;
            JausByte signatureLength = new JausByte(uri.Length);
            indexOffset = index;

            signatureLength.Serialize(byteArray, indexOffset, out indexOffset);

            Array.Copy(enc.GetBytes(uri), 0, byteArray, indexOffset, uri.Length);
            indexOffset += uri.Length;

            this.majorVersionNumber.Serialize(byteArray, indexOffset, out indexOffset);

            this.minorVersionNumber.Serialize(byteArray, indexOffset, out indexOffset);

            return true;
        }

        public bool Deserialize(byte[] byteArray, out int indexOffset)
        {

            return this.Deserialize(byteArray, 0, out indexOffset);
        }

        public bool Deserialize(byte[] byteArray, int index, out int indexOffset)
        {

            int uriLength;
            indexOffset = index;

            uriLength = (int)(new JausByte(byteArray, indexOffset).Value);
            indexOffset += JausBaseType.BYTE_BYTE_SIZE;

            uri = Encoding.UTF8.GetString(byteArray, indexOffset, uriLength);
            indexOffset += uriLength;

            this.majorVersionNumber.Deserialize(byteArray, indexOffset, out indexOffset);

            this.minorVersionNumber.Deserialize(byteArray, indexOffset, out indexOffset);

            return true;
        }

        public int size()
        {
            return JausBaseType.BYTE_BYTE_SIZE + uri.Length + JausBaseType.BYTE_BYTE_SIZE + JausBaseType.BYTE_BYTE_SIZE;
        }

        public String toHexString()
        {

            byte[] tmpByteArray = getBytes(uri);
            JausByte tmpJB = new JausByte();

            String temp = "";
            temp += (new JausByte(uri.Length).toHexString());
            for (int i = 0; i < tmpByteArray.Length; i++)
            {
                tmpJB.Value = tmpByteArray[i];
                temp += tmpJB.toHexString();
            }
            temp += this.majorVersionNumber.toHexString();
            temp += this.minorVersionNumber.toHexString();
            return temp;
        }

        public String toString()
        {

            String temp;
            temp = "id = " + uri + " " +
                    "version = " + this.majorVersionNumber.Value + "." +
                    this.minorVersionNumber.Value + "\n";
            return temp;

        }

    }
}