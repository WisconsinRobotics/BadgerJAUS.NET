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

public class JausUnsignedShort : JausType
{
	private const double RANGE = 65535.0; // NOTE: This range is incorrect, but is as defined by Jaus
	public const int MAX_VALUE = 65535;
	public const int MIN_VALUE = 0;
	
	public const int SIZE_BYTES = 2;
	protected int value;

	public JausUnsignedShort()
	{
		value = 0;
	}

	public JausUnsignedShort(int value)
	{
		setValue(value);
	}
	
	public JausUnsignedShort(JausUnsignedShort other)
	{
		setValue(other.getValue());
	}

	public JausUnsignedShort(byte[] byteArray)
	{
		value = fromJausBuffer(byteArray, 0);
	}

	public JausUnsignedShort(byte[] byteArray, int index)
	{
		value = fromJausBuffer(byteArray, index);
	}
	
	public bool toJausBuffer(byte[] byteArray)
	{
		return toJausBuffer(this.value, byteArray, 0);
	}
	
	public bool toJausBuffer(byte[] byteArray, int index)
	{
		return toJausBuffer(this.value, byteArray, index);
	}

	public bool setFromJausBuffer(byte[] byteArray)
	{
		return setFromJausBuffer(byteArray, 0);
	}
	
	public bool setFromJausBuffer(byte[] byteArray, int index)
	{
		if(byteArray.Length - index < SIZE_BYTES)
			return false;
		else
		{
			value = fromJausBuffer(byteArray, index);
			return true;
		}
	}	
	
	public int getValue()
	{
		return value;
	}
	
	public void setValue(int value)
	{
		if(value < 0) 
			value = 0;
		else 
			this.value = value;
	}
	public int size()
	{
		return SIZE_BYTES;
	}

	// Takes the input double and the scale range and stores the coresponding scaled integer in value
	// This is to support Jaus Scaled Integers
	// Real_Value = Integer_Value*Scale_Factor + Bias
	public double scaleToDouble(double min, double max)
	{
		//BUG: What to do when max < min
		//limit value between min and max Integer values
		if(value > MAX_VALUE) value = MAX_VALUE;
		if(value < MIN_VALUE) value = MIN_VALUE;
		
		double scaleFactor = (max-min)/RANGE;
		double bias = min;
	    
		return value*scaleFactor + bias;
	}

	// Takes the input double and the scale range and stores the coresponding scaled integer in value
	// This is to support Jaus Scaled Integers
	// Integer_Value = Round((Real_Value ??? Bias)/Scale_Factor)
	public void setFromDouble(double value, double min, double max)
	{
		//BUG: What to do when max < min
		double scaleFactor = (max-min)/RANGE;
		double bias = min;
		
		// limit real number between min and max
		if(value < min) value = min;
		if(value > max) value = max;
				
		// calculate rounded integer value
		this.value = (int) Math.Round((value - bias)/scaleFactor);
	}
	
	public static bool toJausBuffer(int value, byte[] byteArray)
	{
		return toJausBuffer(value, byteArray, 0);
	}
	
	public static bool toJausBuffer(int value, byte[] byteArray, int index)
	{
		if(byteArray.Length < index + SIZE_BYTES)
			return false; //not enough size
		
		for(int i=0; i < SIZE_BYTES; i++)
		{
			byteArray[index+i] = (byte)((value >> (i*8)) & 0xFF); // 8 bits per byte
		}
		return true;
	}

	public static int fromJausBuffer(byte[] byteArray)
	{
		return fromJausBuffer(byteArray, 0);
	}
	
	public static int fromJausBuffer(byte[] byteArray, int index)
	{
		int tempValue = 0;
		int arrayUpperBounds = byteArray.Length-index;
		
		// NOTE: Should not get an input array not equal in size to a Integer
		//       If this occurs, the higher order bytes will be left as zeros 
		for(int i = 0; i < SIZE_BYTES && i < arrayUpperBounds; i++)
		{
			tempValue = tempValue | ((byteArray[index+i] & 0xFF) << i*8); // 8 bits per byte
		}
		
		return tempValue;
	}
	
	public String toHexString()
	{
        return (value & 0xFF).ToString("x");
	}
		
	public String toString()
	{
        return Convert.ToString(this.value);

	}
}
