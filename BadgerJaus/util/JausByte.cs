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
    public class JausByte : JausType
    {
        private const double RANGE = 255.0;
        public const int MAX_VALUE = 255;
        public const int MIN_VALUE = 0;
        public const int SIZE_BYTES = 1;

        protected int value;

        public JausByte()
        {
            value = 0;
        }

        public JausByte(int value)
        {
            setValue(value);
        }

        public JausByte(byte value)
        {
            this.value = (int)(value & 0xFF);
        }

        public JausByte(byte[] byteArray)
        {
            value = fromJausBuffer(byteArray, 0);
        }

        public JausByte(byte[] byteArray, int index)
        {
            value = fromJausBuffer(byteArray, index);
        }

        public bool toJausBuffer(byte[] byteArray)
        {
            return toJausBuffer(byteArray, 0);
        }

        public bool toJausBuffer(byte[] byteArray, int index)
        {
            return JausByte.toJausBuffer(this.value, byteArray, index);
        }

        public int getValue()
        {
            return value;
        }

        public void setValue(int value)
        {
            this.value = (int)(value & 0xFF);
        }


        public static int fromJausBuffer(byte[] byteArray)
        {
            if (byteArray.Length == 0) //Zero-Length array
                return 0;
            else
                return byteArray[0] & 0xFF;
        }

        public static int fromJausBuffer(byte[] byteArray, int index)
        {
            int arrayUpperBound = byteArray.Length - index;
            if (arrayUpperBound > 0) // if not Zero-Length array
                return byteArray[index] & 0xFF;
            else
                return 0;
        }

        public static int fromJausBuffer(byte byteArray)
        {
            return byteArray & 0xFF;
        }

        public bool setFromJausBuffer(byte[] byteArray)
        {
            return setFromJausBuffer(byteArray, 0);
        }

        public bool setFromJausBuffer(byte[] byteArray, int index)
        {
            if (byteArray.Length - index < SIZE_BYTES)
                return false;
            else
            {
                value = fromJausBuffer(byteArray, index);
                return true;
            }
        }

        public void setFromJausBuffer(byte byteArray)
        {
            value = fromJausBuffer(byteArray);
        }

        public int size()
        {
            return SIZE_BYTES;
        }

        // Takes the input double and the scale range and stores the coresponding scaled Int32 in value
        // This is to support Jaus Scaled Int32s
        // Real_Value = Integer_Value*Scale_Factor + Bias
        public double scaleToDouble(double min, double max)
        {
            //BUG: What to do when max < min

            //limit value between min and max Integer values
            if (value > MAX_VALUE) value = MAX_VALUE;
            if (value < MIN_VALUE) value = MIN_VALUE;

            double scaleFactor = (max - min) / RANGE;
            double bias = min;

            return value * scaleFactor + bias;
        }

        // Takes the value inside the object and returns it as a double in the range
        // This is to support Jaus Scaled Integers
        // Integer_Value = Round((Real_Value - Bias)/Scale_Factor)
        public void setFromDouble(double value, double min, double max)
        {
            //limit value between min and max Byte values
            double scaleFactor = (max - min) / RANGE;
            double bias = min;

            // limit real number between min and max
            if (value < min) value = min;
            if (value > max) value = max;

            // calculate rounded integer value
            this.value = (int)Math.Round((value - bias) / scaleFactor);
        }

        public static bool toJausBuffer(int value, byte[] byteArray)
        {
            return toJausBuffer(value, byteArray, 0);
        }

        public static bool toJausBuffer(int value, byte[] byteArray, int index)
        {
            if (byteArray.Length - index >= SIZE_BYTES)
            {
                byteArray[index] = (byte)(value & 0xFF);
                return true;
            }
            else return false;
        }

        public String toString()
        {
            return Convert.ToString(this.value);
        }

        public String toHexString()
        {
            return value.ToString("X");
        }
    }
}