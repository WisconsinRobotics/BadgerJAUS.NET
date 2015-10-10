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
using System.Globalization;

namespace BadgerJaus.Util
{
    public class JausTimeStamp : JausUnsignedInteger
    {

        public const int MILLISECOND_BIT_POSITION = 0;
        public const int SECOND_BIT_POSITION = 10;
        public const int MINUTE_BIT_POSITION = 16;
        public const int HOUR_BIT_POSITION = 22;
        public const int DAY_BIT_POSITION = 27;

        public JausTimeStamp() : base()
        { }
        public JausTimeStamp(JausUnsignedInteger value)
            : base(value.Value)
        {
        }

        public JausTimeStamp(long value)
            : base(value)
        {
        }

        public void setToCurrentTime()
        {
            Calendar cal = Activator.CreateInstance<Calendar>();
            //cal.setTime(cal.getTime());
            DateTime dt = new DateTime();
            cal.GetMilliseconds(dt);
            setMillisecond(dt.Millisecond);
            cal.GetSecond(dt);
            setSecond(dt.Second);
            cal.GetMinute(dt);
            setMinute(dt.Minute);
            cal.GetHour(dt);
            setHour(dt.Hour);
            cal.GetDayOfMonth(dt);
            setDay(dt.Day);
            //setMillisecond(cal.GetMilliseconds());
            //setSecond(cal.get(Calendar.SECOND));
            //setMinute(cal.get(Calendar.MINUTE));
            //setHour(cal.get(Calendar.HOUR_OF_DAY));
            //setDay(cal.get(Calendar.DATE));
        }

        public int getMillisecond()
        {
            return (int)(value >> MILLISECOND_BIT_POSITION) & 0x3FF;
        }

        public bool setMillisecond(int value)
        {
            if (value < 0 || value >= 1000)
            {
                return false;
            }

            this.value = (uint)(value << MILLISECOND_BIT_POSITION) | (this.value & 0xFFFFFC00);
            return true;
        }

        public int getSecond()
        {
            return (int)(value >> SECOND_BIT_POSITION) & 0x3F;
        }

        public bool setSecond(int value)
        {
            if (value >= 0 && value < 60)
            {
                this.value = value << SECOND_BIT_POSITION | (this.value & 0xFFFF03FF);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getMinute()
        {
            return (int)(value >> MINUTE_BIT_POSITION) & 0x3F;
        }

        public bool setMinute(int value)
        {
            if (value >= 0 && value < 60)
            {
                this.value = value << MINUTE_BIT_POSITION | (this.value & 0xFFC0FFFF);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getHour()
        {
            return (int)(value >> HOUR_BIT_POSITION) & 0x1F;
        }

        public bool setHour(int value)
        {
            if (value >= 0 && value < 24)
            {
                this.value = value << HOUR_BIT_POSITION | (this.value & 0xF83FFFFF);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getDay()
        {
            return (int)(value >> DAY_BIT_POSITION) & 0x1F;
        }

        public bool setDay(int value)
        {
            if (value >= 0 && value < 32)
            {
                this.value = value << DAY_BIT_POSITION | this.value & 0x07FFFFFF;
                // tempValue += ((long)(byteArray[index+i] & 0xFF) << i*8); // 8 bits per byte
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            string str = "";
            str += this.getDay() + ":"
                + this.getHour() + ":"
                + this.getMinute() + ":"
                + this.getSecond() + ":"
                + this.getMillisecond() + "";
            return str;
        }
    }
}