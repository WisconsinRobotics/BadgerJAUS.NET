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
    public class JausUnsignedInteger : JausBaseType
    {
        public JausUnsignedInteger() : base()
        {
        }

        public JausUnsignedInteger(long value)
        {
            this.value = value;
        }

        public JausUnsignedInteger(byte[] byteArray)
        {
            int indexOffset;
            Deserialize(byteArray, 0, out indexOffset);
        }

        public JausUnsignedInteger(byte[] byteArray, int index)
        {
            int indexOffset;
            Deserialize(byteArray, index, out indexOffset);
        }

        public override int SIZE_BYTES
        {
            get { return 4; }
        }

        public override long MAX_VALUE
        {
            get { return 4294967295L; }
        }

        public override long RANGE
        {
            get { return 4294967295L; }
        }

        // Takes the input double and the scale range and stores the coresponding scaled integer in value
        // This is to support Jaus Scaled Integers
        // Integer_Value = Round((Real_Value - Bias)/Scale_Factor)
        public void setFromDouble(double value, double min, double max)
        {
            //BUG: What to do when max < min
            double scaleFactor = (max - min) / RANGE;
            double bias = min;

            // limit real number between min and max
            if (value < min) value = min;
            if (value > max) value = max;

            // calculate rounded integer value
            this.value = (long)Math.Round((value - bias) / scaleFactor);
        }

        public override string ToString()
        {
            return Convert.ToString(this.value);
        }
    }
}