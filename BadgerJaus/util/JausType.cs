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
/**
 * Specifies an object that can be marshaled for packaging in a JausMessage.
 * 
 *  JausTypes form the basic building blocks for a JAUS message.  This makes it 
 *  easy to set values for a JAUS message and then have them converted to a byte 
 *  array for passage across the network.  JausTypes also allow for the 
 *  decoding of messages when they come in from a network.  JausTypes provide 
 *  abstraction from having to think about low level details such as byte order, 
 *  low level data structures and complex composition of these data structures. 
 * 
 * 
 * @author Patrick Blesi
 * */

using System;

public interface JausType {

	/**
	 * Populates {@code byteArray} starting at index {@code index} with the data
	 * represented by this JausType. The bool return variable indicates 
	 * success or failure of the operation.   
	 * 
	 * @param byteArray The byteArray to be populated with data
	 * @param index The index into the byteArray where the data should be placed
	 * @return Success or failure of populating the byteArray
	 * 
	 * @see #toJausBuffer(byte[])
	 */
	bool Serialize(byte[] byteArray, int index, out int indexOffset);
	
	/**
	 * This is a shorthand method for {@code toJausBuffer(byte[], 0)}, providing
	 * a shorthand from having to specify population at the beginning of the 
	 * array.  
	 * 
	 * @param byteArray The byteArray to be populated with data
	 * @return Success  or failure of populating the byteArray
	 * 
	 * @see #toJausBuffer(byte[], int)
	 */
	bool Serialize(byte[] byteArray, out int indexOffset);
	
	/**
	 * Decodes {@code byteArray} populating the data structures of this JausType
	 * with the respective data.  This method sets the values and properties of
	 * a JausType from a byteArray with this data encoded.  
	 * 
	 * @param byteArray Holds the data to be decoded
	 * @param index Specifies the index into byteArray where the encoded data 
	 * begins
	 * @return Success or failure of decoding the byteArray
	 * 
	 * @see #setFromJausBuffer(byte[])
	 */
	bool Deserialize(byte[] byteArray, int index, out int indexOffset);
	
	/**
	 * This is a shorthand method for {@code setFromJausBuffer(byte[], 0)}, 
	 * providing a shorthand from having to specify decoding at the beginning 
	 * of the array.  
	 * 
	 * @param byteArray The byteArray containing the encoded data
	 * @return Success or failure of decoding the byteArray
	 * 
	 * @see #setFromJausBuffer(byte[], int)
	 */
	bool Deserialize(byte[] byteArray, out int indexOffset);

    double ScaleValueToDouble(double min, double max);

    void SetValueFromDouble(double value, double min, double max);

    bool IsBitSet(int bit);

    void ToggleBit(int bit, bool set = false);
	
	/**
	 * Provides a string to view the marshaled data.  An example would be:
	 * 
	 * 
	 * @return a string representing the marshaled data of this JausType
	 */
	String toHexString();
}
