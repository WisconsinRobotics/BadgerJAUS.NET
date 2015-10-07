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
    public class JausServiceSignature : JausType
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

            this.setFromJausBuffer(buffer, index);

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
            get { return majorVersionNumber.getValue(); }
            set { majorVersionNumber.setValue(value); }
        }

        public int MinorVersion
        {
            get { return minorVersionNumber.getValue(); }
            set { minorVersionNumber.setValue(value); }
        }

        // JausType Methods

        public bool toJausBuffer(byte[] byteArray)
        {

            return this.toJausBuffer(byteArray, 0);
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

        public bool toJausBuffer(byte[] byteArray, int index)
        {
            Encoding enc = Encoding.UTF8;
            JausByte signatureLength = new JausByte(uri.Length);

            signatureLength.toJausBuffer(byteArray, index);
            index += JausByte.SIZE_BYTES;

            Array.Copy(enc.GetBytes(uri), 0, byteArray, index, uri.Length);
            index += uri.Length;

            this.majorVersionNumber.toJausBuffer(byteArray, index);
            index += JausByte.SIZE_BYTES;

            this.minorVersionNumber.toJausBuffer(byteArray, index);
            index += JausByte.SIZE_BYTES;

            return true;
        }

        public bool setFromJausBuffer(byte[] byteArray)
        {

            return this.setFromJausBuffer(byteArray, 0);
        }

        public bool setFromJausBuffer(byte[] byteArray, int index)
        {

            int uriLength;

            uriLength = (new JausByte(byteArray, index).getValue());
            index += JausByte.SIZE_BYTES;

            uri = Encoding.UTF8.GetString(byteArray, index, uriLength);
            index += uriLength;

            this.majorVersionNumber.setFromJausBuffer(byteArray, index);
            index += JausByte.SIZE_BYTES;

            this.minorVersionNumber.setFromJausBuffer(byteArray, index);
            index += JausByte.SIZE_BYTES;

            return true;
        }

        public int size()
        {
            return JausByte.SIZE_BYTES + uri.Length + JausByte.SIZE_BYTES + JausByte.SIZE_BYTES;
        }

        public String toHexString()
        {

            byte[] tmpByteArray = getBytes(uri);
            JausByte tmpJB = new JausByte();

            String temp = "";
            temp += (new JausByte(uri.Length).toHexString());
            for (int i = 0; i < tmpByteArray.Length; i++)
            {
                tmpJB.setValue((int)tmpByteArray[i]);
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
                    "version = " + this.majorVersionNumber.getValue() + "." +
                    this.minorVersionNumber.getValue() + "\n";
            return temp;

        }

    }
}