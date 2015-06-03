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

namespace BadgerJaus.Util
{
    public class JausBytePresenceVector : JausByte
    {
        public static int getPresenceFromBuffer(byte[] buffer, int index)
        {
            if (index >= buffer.Length)
                return 0;

            return buffer[index];
        }

        public JausBytePresenceVector() : base()
        {
        }

        public JausBytePresenceVector(byte[] byteArray) : base(byteArray)
        {
        }

        public JausBytePresenceVector(byte[] byteArray, int index) : base(byteArray, index)
        {
        }

        public bool isBitSet(int bit)
        {
            return ((value & (0x01 << bit)) > 0);
        }

        public bool setBit(int bit)
        {
            if (SIZE_BYTES * 8 < bit) // 8 bits per byte
                return false;
            else
            {
                value = value | (0x01 << bit);
                return true;
            }
        }

        public bool clearBit(int bit)
        {
            if (SIZE_BYTES * 8 < bit)
                return false;
            else
            {
                value = value & ~(0x01 << bit);
                return true;
            }
        }

        new public String toString()
        {
            String str = "";
            for (int i = this.size() * 8 - 1; i >= 0; i--)
            {
                if (this.isBitSet(i))
                {
                    str += "1";
                }
                else
                {
                    str += "0";
                }
            }
            return str;
        }
    }
}