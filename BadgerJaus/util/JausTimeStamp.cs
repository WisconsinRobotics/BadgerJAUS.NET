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

public class JausTimeStamp : JausType
{

    public const int MILLISECOND_BIT_POSITION = 0;
    public const int SECOND_BIT_POSITION = 10;
    public const int MINUTE_BIT_POSITION = 16;
    public const int HOUR_BIT_POSITION = 22;
    public const int DAY_BIT_POSITION = 27;
    public const int SIZE_BYTES = 4;

    private JausUnsignedInteger timeStamp;

    public JausTimeStamp()
    {
        timeStamp = new JausUnsignedInteger();
    }

    public JausTimeStamp(JausUnsignedInteger value)
    {
        timeStamp = new JausUnsignedInteger(value.getValue());
    }

    public JausTimeStamp(long l)
    {
        timeStamp = new JausUnsignedInteger(l);
    }

    public JausTimeStamp(byte[] byteArray)
    {
        timeStamp = new JausUnsignedInteger(byteArray);
    }

    public JausTimeStamp(byte[] byteArray, int index)
    {
        timeStamp = new JausUnsignedInteger(byteArray, index);
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

    public JausUnsignedInteger getTimeStamp()
    {
        return new JausUnsignedInteger(timeStamp.getValue());
    }

    public void setTimeStamp(long value)
    {
        timeStamp.setValue(value);
    }

    public int getMillisecond()
    {
        long value = timeStamp.getValue();
        return (int)(value >> MILLISECOND_BIT_POSITION) & 0x3FF;
    }

    public bool setMillisecond(int value)
    {
        if (value >= 0 && value < 1000)
        {
            long tsValue = timeStamp.getValue();
            tsValue = value << MILLISECOND_BIT_POSITION | (tsValue & 0xFFFFFC00);
            timeStamp.setValue(tsValue);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getSecond()
    {
        long value = timeStamp.getValue();
        return (int)(value >> SECOND_BIT_POSITION) & 0x3F;
    }

    public bool setSecond(int value)
    {
        if (value >= 0 && value < 60)
        {
            long tsValue = timeStamp.getValue();
            tsValue = value << SECOND_BIT_POSITION | (tsValue & 0xFFFF03FF);
            timeStamp.setValue(tsValue);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getMinute()
    {
        long value = timeStamp.getValue();
        return (int)(value >> MINUTE_BIT_POSITION) & 0x3F;
    }

    public bool setMinute(int value)
    {
        if (value >= 0 && value < 60)
        {
            long tsValue = timeStamp.getValue();
            tsValue = value << MINUTE_BIT_POSITION | (tsValue & 0xFFC0FFFF);
            timeStamp.setValue(tsValue);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getHour()
    {
        long value = timeStamp.getValue();
        return (int)(value >> HOUR_BIT_POSITION) & 0x1F;
    }

    public bool setHour(int value)
    {
        if (value >= 0 && value < 24)
        {
            long tsValue = timeStamp.getValue();
            tsValue = value << HOUR_BIT_POSITION | (tsValue & 0xF83FFFFF);
            timeStamp.setValue(tsValue);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getDay()
    {
        long value = timeStamp.getValue();
        return (int)(value >> DAY_BIT_POSITION) & 0x1F;
    }

    public bool setDay(int value)
    {
        if (value >= 0 && value < 32)
        {
            long tsValue = timeStamp.getValue();
            tsValue = (long)value << DAY_BIT_POSITION | (long)(tsValue & 0x07FFFFFF);
            // tempValue += ((long)(byteArray[index+i] & 0xFF) << i*8); // 8 bits per byte
            timeStamp.setValue(tsValue);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool toJausBuffer(byte[] buffer, int index)
    {
        return timeStamp.toJausBuffer(buffer, index);
    }

    public bool toJausBuffer(byte[] byteArray)
    {
        return timeStamp.toJausBuffer(byteArray);
    }

    public bool setFromJausBuffer(byte[] buffer, int index)
    {
        return timeStamp.setFromJausBuffer(buffer, index);
    }

    public bool setFromJausBuffer(byte[] byteArray)
    {
        return timeStamp.setFromJausBuffer(byteArray);
    }

    public int size()
    {
        return timeStamp.size();
    }

    public String toHexString()
    {
        return timeStamp.toHexString();
    }

    public String toString()
    {
        String str = "";
        str += this.getDay() + ":"
            + this.getHour() + ":"
            + this.getMinute() + ":"
            + this.getSecond() + ":"
            + this.getMillisecond() + "";
        return str;
    }

}
