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

using BadgerJaus.Util;

namespace BadgerJaus.Messages
{
    public enum AckNak
    {
        // AckNak Flags
        NOT_REQUIRED = 0,	// ordinal 0
        REQUIRED = 1,		// ordinal 1
        NEGATIVE_ACK = 2,	// ordinal 2
        ACKNOWLEDGE = 3	// ordinal 3
    }

    public enum Broadcast
    {
        //  Bcast Flags
        NO = 0,     //Ordinal 0
        LOCAL = 1,  //Ordinal 1
        GLOBAL = 2  //Ordinal 2
    }

    public enum DataFlag
    {
        // Data Flags
        SINGLE_DATA_PACKET = 0,	// ordinal 0   		
        FIRST_DATA_PACKET = 1,		// ordinal 1
        NORMAL_DATA_PACKET = 2,	// ordinal 2
        LAST_DATA_PACKET = 3		// ordinal 3
    }

    public enum HCFlags
    {
        // HCFlags
        NO_HEADER_COMPRESSION = 0, 		// ordinal 0
        HEADER_COMPRESSION_REQUEST = 1, // ordinal 1
        HEADER_COMPRESSION_TWO = 2,		// ordinal 2
        HEADER_COMPRESSION_ENGAGED = 3	// ordinal 3
    }

    public enum Priority
    {
        // Priority Flags
        LOW = 0,		// ordinal 0
        NORMAL = 1,		// ordinal 1
        HIGH = 2,		// ordinal 2
        SAFETY = 3		// ordinal 3
    }

    /// <summary>
    /// The message class serves as the base class for all JAUS messages. It
    /// provides a skeleton that extracts header values and information about
    /// the data payload. Child classes override specific methods that perform
    /// the actual unpacking of message fields into usable forms.
    /// </summary>
    public class Message
    {
        // The following are constants defined for the header properties in JUDP 
        public const byte JUDP_HEADER = 2;				// Version of JUDP
        public const int JUDP_HEADER_SIZE_BYTES = 1;

        // The following are constants defined for the header properties in JTCP
        public const byte JTCP_HEADER = 2;				// Version of JTCP
        public const int JTCP_HEADER_SIZE_BYTES = 1;

        public const int HEADER_SIZE_BYTES_CMP = 15;
        public const int HEADER_SIZE_BYTES_NCMP = 12;
        private int headerBaseSize = HEADER_SIZE_BYTES_NCMP; 	// Assuming No Compression 

        public const int UDP_MAX_PACKET_SIZE = 4101;   // Including JUDP Header, General Transport Header,
        // and uncompressed message payload

        public const int SEQUENCE_NUMBER_MAX = 65535;
        public const int SEQUENCE_NUMBER_SIZE_BYTES = JausBaseType.SHORT_BYTE_SIZE;

        public const int COMMAND_CODE_SIZE_BYTES = JausBaseType.SHORT_BYTE_SIZE;

        // Properties Bit Positions				
        public const int PRIORITY_BIT_POSITION = 0;
        public const int BCAST_BIT_POSITION = 2;
        public const int ACK_NAK_BIT_POSITION = 4;
        public const int DATA_FLAGS_BIT_POSITION = 6;

        // First Byte Bit Positions
        public const int MESSAGE_TYPE_BIT_POSITION = 0;
        public const int HC_FLAGS_BIT_POSITION = 6;

        // ***** JausMessage Header Fields *****
        private byte messageType = 0;
        protected JausUnsignedShort commandCode;// = new JausUnsignedShort();
        // Initial value = 0
        // Values 1 through 32 are reserved for future use.					

        // TODO: Implement header compression
        private HCFlags hcFlags;			// Header Compression Flags			
        // 0: No header compression is used
        // 1: The sender of this message is requesting 
        //    that the receiver engage in header compression
        // 2: Meaning depends upon HC Length field
        // 3: The sender is sending a message containing compressed data.

        protected JausUnsignedShort dataSize;	// Size of General Transport Header + data
        // private byte HCNumber;				// Used if HCFlags != 0 range 0 - 255
        // private byte HCLength;				// Used if HCflags != 0 range 0 - 255
        private Priority priority;
        private Broadcast bCast;
        private AckNak ackNak;
        private DataFlag dataFlags;
        private JausAddress destination;
        private JausAddress source;
        // TODO: Make the following private
        protected byte[] data;
        private JausUnsignedShort sequenceNumber;
        // ******** End JausMessage Header Fields ********

        /**
         * Default Constructor.
         */
        public Message()
        {
            InitData();
            Configure();
        }

        protected void Configure()
        {
            messageType = 0;							// Transport Type of Message
            hcFlags = HCFlags.NO_HEADER_COMPRESSION;
            priority = Priority.NORMAL;
            bCast = Broadcast.NO;
            ackNak = AckNak.NOT_REQUIRED;
            dataFlags = DataFlag.SINGLE_DATA_PACKET;
            commandCode.Value = CommandCode;
        }

        private void InitData()
        {
            dataSize = new JausUnsignedShort();
            commandCode = new JausUnsignedShort();
            destination = new JausAddress();
            source = new JausAddress();
            sequenceNumber = new JausUnsignedShort();
            data = null;
            InitFieldData();
        }

        /// <summary>
        /// <c>InitFieldData</c> is used to initialize the member classes that
        /// represent the fields of a JAUS message. This method is only
        /// relevant to child classes that have payloads.
        /// </summary>
        protected virtual void InitFieldData()
        {

        }

        /// <summary>
        /// <c>UdpSize</c> provides the size of the entire message plus the
        /// UDP packet tag.
        /// </summary>
        public int UdpSize()
        {
            return JUDP_HEADER_SIZE_BYTES + MessageSize();
        }

        /**
         * Size returns the total number of bytes of the JAUS Packet (not including 
         * the JUDP Header).  Other messages should override this method in order to
         * properly set the size requirements for their particular message.  This 
         * method is primarily used to set the size of buffers.
         * 
         * @return The size of the message in bytes.
         */
        public virtual int MessageSize()
        {
            return headerBaseSize + Message.COMMAND_CODE_SIZE_BYTES + Message.SEQUENCE_NUMBER_SIZE_BYTES + GetPayloadSize();
        }

        public AckNak AckNak
        {
            get { return ackNak; }
            set { ackNak = value; }
        }

        public Broadcast BCast
        {
            get { return bCast; }
            set { bCast = value; }
        }

        public DataFlag DataFlags
        {
            get { return dataFlags; }
            set { dataFlags = value; }
        }

        public HCFlags HCFlags
        {
            get { return hcFlags; }
            set { hcFlags = (HCFlags)value; }
        }

        public byte MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        /// <summary>
        /// <c>GetPayloadSize</c> provides the size of  the data payload that
        /// a message may have. Child classes that have additional fields need
        /// to override this method to calculate sizes specific to their
        /// fields.
        /// </summary>
        /// <returns></returns>
        public virtual int GetPayloadSize()
        {
            if (data != null)
                return data.Length;
            return 0;
        }

        protected virtual int CommandCode
        {
            get { return JausCommandCode.NONE; }
        }

        public int GetCommandCode()
        {
            return (int)commandCode.Value;
        }

        public JausAddress GetDestination()
        {
            return destination;
        }

        public void SetDestination(long id)
        {
            destination.Value = id;
        }

        public void SetDestination(JausAddress address)
        {
            destination.Value = address.Value;
        }

        public JausAddress GetSource()
        {
            return source;
        }

        public void SetSource(long id)
        {
            this.source.Value = id;
        }

        public void SetSource(JausAddress address)
        {
            source.Value = address.Value;
        }

        public byte[] GetPayload()
        {
            byte[] payload = new byte[data.Length];
            Array.Copy(data, 0, payload, 0, payload.Length);
            return payload;
        }

        public bool SetPayload(byte[] payload)
        {
            if (payload == null)
                return false;
            this.data = new byte[payload.Length];
            Array.Copy(payload, 0, data, 0, payload.Length);
            return true;
        }

        public String payloadToString()
        {
            String str = "";
            for (int i = 0; i < data.Length; i++)
            {
                str += "0x" + Convert.ToString(data[i], 16).ToUpper() + " ";
            }
            return str;
        }

        /// <summary>
        /// This method is intended to be overridden by child classes that
        /// encode their respective fields into the byte buffer in the order
        /// detailed in the specification. The base implementation handles
        /// copying of already existing buffers when dealing with a generic or
        /// not yet differentiated message instance.
        /// </summary>
        /// <param name="data">Array to copy data to.</param>
        /// <param name="index">Offset into array to start copying to.</param>
        /// <returns></returns>
        protected virtual bool PayloadToJausBuffer(byte[] data, int index, out int indexOffset)
        {
            indexOffset = index;
            if (this.data == null)
                this.data = new byte[data.Length];
            Array.Copy(this.data, 0, data, index, this.data.Length);
            indexOffset += this.data.Length;
            return true;
        }

        protected virtual bool SetPayloadFromJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            int payloadSize;

            // The data size field includes the size of the entire message
            // including the header and sequence number but not the JUDP
            // tag. To get just the size of the data payload we need to
            // subtract all that out.
            
            payloadSize = (int)dataSize.Value - HEADER_SIZE_BYTES_NCMP - COMMAND_CODE_SIZE_BYTES - SEQUENCE_NUMBER_SIZE_BYTES;
            data = new byte[payloadSize];
            Array.Copy(buffer, index, data, 0, payloadSize);

            indexOffset = index + payloadSize;
            return true;
        }

        public int GetSequenceNumber()
        {
            return (int)sequenceNumber.Value;
        }

        public void SetSequenceNumber(int sequenceNumber)
        {
            this.sequenceNumber.Value = sequenceNumber;
        }

        public void SetFromJausMessage(Message jausMessage)
        {
            int indexOffset;
            SetHeaderFromJausMessage(jausMessage);
            SetPayloadFromJausBuffer(jausMessage.GetPayload(), 0, out indexOffset);
        }

        private void SetHeaderFromJausMessage(Message jausMessage)
        {
            SetAddressFromJausMessage(jausMessage);
            dataSize.Value = jausMessage.MessageSize();
        }

        public void SetAddressFromJausMessage(Message jausMessage)
        {
            SetDestination(jausMessage.GetDestination().Value);
            SetSource(jausMessage.GetSource().Value);
        }

        public int Deserialize(byte[] buffer, int index = 0)
        {
            if (!SetHeaderFromJausBuffer(buffer, index))
            {
                //Console.Error.WriteLine("setFromJausBuffer Failed.  Set header from Jause Buffer"); 
                Console.Error.WriteLine("setFromJausBuffer Failed");
                return -1; // setHeaderFromJausBuffer failed
            }

            // Store Data Byte Array
            index += headerBaseSize;

            // TODO: Implement Multiple Packets Per Message
            if (dataFlags != DataFlag.SINGLE_DATA_PACKET)
            {
                Console.Error.WriteLine("Receiving Multiple Packets Per Message Not Supported");
                return -1;
            }

            // TODO: Implement Ack/Nak
            if (ackNak != AckNak.NOT_REQUIRED)
            {
                return -1;
            }

            if(CommandCode == JausCommandCode.NONE)
            {
                if (!commandCode.Deserialize(buffer, index, out index))
                {
                    return -1; // setCommandCodeFromJausBuffer failed
                }
            }
            else
            {
                JausUnsignedShort bufferCommandCode = new JausUnsignedShort(buffer, index);
                if (bufferCommandCode.Value != CommandCode)
                    return -1;
            }

            index += COMMAND_CODE_SIZE_BYTES;

            SetPayloadFromJausBuffer(buffer, index, out index);

            int dataSize = GetPayloadSize();
            if (dataSize < 0 || dataSize > 10000)
            {
                return -1;
            }

            if (!sequenceNumber.Deserialize(buffer, index, out index))
            {
                return -1; // setSequenceNumberFromJausBuffer failed
            }

            return index;
        }

        public bool ToJausBuffer(byte[] buffer, out int indexOffset)
        {
            return ToJausBuffer(buffer, 0, out indexOffset);
        }

        // Takes the header and data byte array and packs them into a data buffer
        // This method needs to be overridden for all subclasses of JausMessage to reflect the correct pack routine
        public bool ToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            // TODO: Implement Compression
            if (hcFlags != HCFlags.NO_HEADER_COMPRESSION)
            {
                Console.Error.WriteLine("Sending Compression Not Supported");
                return false;
            }

            // TODO: Implement Ack/Nak
            if (ackNak != AckNak.NOT_REQUIRED)
            {
                Console.Error.WriteLine("Sending Ack/Nak Not Supported");
                return false;
            }

            if (buffer.Length < (indexOffset + GetPayloadSize() + Message.SEQUENCE_NUMBER_SIZE_BYTES))
            {
                Console.Error.WriteLine("Error in toJausBuffer: Not Enough Size");
                return false; // Not Enough Size	
            }
            if (!HeaderToJausBuffer(buffer, indexOffset, out indexOffset))
            {
                Console.Error.WriteLine("ToJausBuffer Failed");
                return false; //headerToJausBuffer failed
            }

            if (!commandCode.Serialize(buffer, indexOffset, out indexOffset)) return false;

            PayloadToJausBuffer(buffer, indexOffset, out indexOffset);
            //Console.WriteLine("Claimed payload size: " + GetPayloadSize());

            //Console.WriteLine("Sequence index position: " + index);

            if (!sequenceNumber.Serialize(buffer, indexOffset, out indexOffset))
            {
                Console.Error.WriteLine("Failed to write sequence number to buffer");
                return false; //headerToJausBuffer failed
            }

            return true;
        }

        // This function takes a byte array and attempts to extract a JAUS
        // message from it. It also attempts to find where the message it is
        // extracting ends, so that if the array has multiple messages the
        // array can be split appropriately.

        public int SetFromJausUdpBuffer(byte[] buffer, int index)
        {
            if (buffer == null)
                return -1;

            if (buffer.Length <= index)
                return -1;

            if (buffer[index] != Message.JUDP_HEADER)
            {
                return -1; // improper JUDP header / incompatible version
            }

            index += JUDP_HEADER_SIZE_BYTES;
            index = Deserialize(buffer, index);

            return index;
        }

        public int SetFromJausUdpBuffer(byte[] buffer)
        {
            return SetFromJausUdpBuffer(buffer, 0);
        }

        public bool ToJausUdpBuffer(byte[] buffer, int index, out int indexOffset)
        {
            indexOffset = index;
            if (buffer.Length < (index + JUDP_HEADER_SIZE_BYTES))
            {
                return false; // Not Enough Size
            }

            buffer[indexOffset] = Message.JUDP_HEADER;

            indexOffset += Message.JUDP_HEADER_SIZE_BYTES;
            return ToJausBuffer(buffer, indexOffset, out indexOffset);
        }


        // Overloaded method to accept a buffer and pack this message into its UDP form
        public bool ToJausUdpBuffer(byte[] buffer, out int indexOffset)
        {
            return ToJausUdpBuffer(buffer, 0, out indexOffset);
        }

        // This private method takes a buffer at the given index and unpacks it into the header properties and data fields
        // This method is called whenever a message is unpacked to pull the header properties
        private bool SetHeaderFromJausBuffer(byte[] buffer, int index)
        {
            int indexOffset;
            byte properties;
            byte messageTypeHCflags;
            if (buffer.Length < index + headerBaseSize)
                return false; // Not Enough Size

            messageTypeHCflags = buffer[index];
            messageType = (byte)((messageTypeHCflags >> MESSAGE_TYPE_BIT_POSITION) & 0x3F);
            hcFlags = (HCFlags)((messageTypeHCflags >> HC_FLAGS_BIT_POSITION) & 0x3);
            

            // TODO: Implement Compression
            if (hcFlags != HCFlags.NO_HEADER_COMPRESSION)
            {
                Console.Error.WriteLine("Compression Not Supported");
                return false;
            }

            // int data1 = buffer[index+2];
            dataSize.Deserialize(buffer, index + 1, out indexOffset);

            // No Compression Assumed
            properties = buffer[index + 3];
            ackNak = (AckNak)((properties >> ACK_NAK_BIT_POSITION) & 0x03);
            bCast = (Broadcast)((properties >> BCAST_BIT_POSITION) & 0x03);
            dataFlags = (DataFlag)((properties >> DATA_FLAGS_BIT_POSITION) & 0x03);
            priority = (Priority)((properties >> PRIORITY_BIT_POSITION) & 0x3);

            destination.setComponent(buffer[index + 4]);
            destination.setNode(buffer[index + 5]);
            destination.setSubsystem((int)(new JausUnsignedShort(buffer, index + 6).Value));

            source.setComponent(buffer[index + 8]);
            source.setNode(buffer[index + 9]);
            source.setSubsystem((int)(new JausUnsignedShort(buffer, index + 10).Value));

            return true;
        }

        // This private method packs the header properties and data fields into the provided buffer at the given index
        // This method is called whenever a message is packed to put the header properties
        private bool HeaderToJausBuffer(byte[] buffer, int index, out int indexOffset)
        {
            byte properties = 0;
            byte messageTypeHCflags = messageType;
            indexOffset = index;
            if (buffer.Length < index + headerBaseSize)
                return false; // Not Enough Size

            messageTypeHCflags = (byte)(((byte)hcFlags << HC_FLAGS_BIT_POSITION) | (messageTypeHCflags & 0x3F));
            buffer[indexOffset] = messageTypeHCflags;			// Message type
            indexOffset += 1;
            //Console.WriteLine("Size: " + size());

            dataSize.Value = MessageSize();
            dataSize.Serialize(buffer, indexOffset, out indexOffset);

            //buffer[index + 3] = 0;								// HC number, not needed
            //buffer[index + 4] = 0;								// HC Length, not needed

            properties = (byte)(((byte)ackNak << ACK_NAK_BIT_POSITION) | properties);
            properties = (byte)(((byte)bCast << BCAST_BIT_POSITION) | properties);
            properties = (byte)(((byte)dataFlags << DATA_FLAGS_BIT_POSITION) | properties);
            properties = (byte)(((byte)priority << PRIORITY_BIT_POSITION) | properties);
            buffer[indexOffset] = properties;				// Message Properties
            indexOffset += 1;

            buffer[indexOffset] = (byte)destination.getComponent();	// Destination component
            indexOffset += 1;
            buffer[indexOffset] = (byte)destination.getNode();		// Destination node	
            indexOffset += 1;
            new JausUnsignedShort(destination.getSubsystem()).Serialize(buffer, indexOffset, out indexOffset);

            buffer[indexOffset] = (byte)source.getComponent();		// Source component
            indexOffset += 1;
            buffer[indexOffset] = (byte)source.getNode();				// Source node
            indexOffset += 1;
            new JausUnsignedShort(source.getSubsystem()).Serialize(buffer, indexOffset, out indexOffset);

            return true;
        }

        public override string ToString()
        {
            string str = "Command Code: " + JausCommandCode.Lookup((int)(commandCode.Value)) + " (0x" + Convert.ToString(commandCode.Value, 16).ToUpper() + ") " + "\n";
            return str;
        }

        public String headerToString()
        {
            String str = "";
            // str += "JUDP Version: " + JausMessage.JUDP_HEADER + "\n";
            str += "Message Type: " + this.messageType + "\n";
            str += "HC flags: " + this.hcFlags + "\n";
            str += "Data Size: " + this.dataSize.Value + "\n";
            // str += "HC Number: " + this.getHCnumber() + "\n";
            // str += "HC Length: " + this.getHCLength() + "\n";
            str += "Priority: " + this.priority + "\n";
            str += "Broadcast: " + this.bCast + "\n";
            str += "Ack/Nak: " + this.ackNak + "\n";
            str += "Data Flags: " + this.dataFlags + "\n";
            str += "Destination: " + this.destination.ToString() + "\n";
            str += "Source: " + this.source.ToString() + "\n";
            //str += "Command Code: " + commandCodeJT + " (0x" + Integer.toString(this.commandCodeJT.COMMAND_CODE(), 16).toUpperCase() + ") " + "\n";
            str += "Data: " + this.payloadToString() + "\n";
            str += "Sequence Number: " + sequenceNumber.Value + "\n";

            return str;
        }

    }
}