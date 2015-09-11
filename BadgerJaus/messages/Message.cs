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
        public const int SEQUENCE_NUMBER_SIZE_BYTES = JausUnsignedShort.SIZE_BYTES;

        public const int COMMAND_CODE_SIZE_BYTES = JausUnsignedShort.SIZE_BYTES;

        // Properties Bit Positions				
        public const int PRIORITY_BIT_POSITION = 0;
        public const int BCAST_BIT_POSITION = 2;
        public const int ACK_NAK_BIT_POSITION = 4;
        public const int DATA_FLAGS_BIT_POSITION = 6;

        // First Byte Bit Positions
        public const int MESSAGE_TYPE_BIT_POSITION = 0;
        public const int HC_FLAGS_BIT_POSITION = 6;

        // ***** JausMessage Header Fields *****
        private byte messageTypeHCflags;	// First byte of JAUS Header see below bytes for details
        private byte messageType = 0;
        protected JausUnsignedShort commandCode;// = new JausUnsignedShort();
        // Initial value = 0
        // Values 1 through 32 are reserved for future use.					

        // TODO: Implement header compression
        private HCFlagsClass.HCFlags HCflags;			// Header Compression Flags			
        // 0: No header compression is used
        // 1: The sender of this message is requesting 
        //    that the receiver engage in header compression
        // 2: Meaning depends upon HC Length field
        // 3: The sender is sending a message containing compressed data.

        protected JausUnsignedShort dataSize;	// Size of General Transport Header + data
        // private byte HCNumber;				// Used if HCFlags != 0 range 0 - 255
        // private byte HCLength;				// Used if HCflags != 0 range 0 - 255
        private byte properties;			// JAUS packet properties
        private PriorityClass.Priority priority;
        private BroadcastClass.Broadcast bCast;
        private AckNakClass.AckNak ackNak;
        private DataFlagClass.DataFlag dataFlags;
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
            setMessageType(0);							// Transport Type of Message
            setHCflags(HCFlagsClass.value(HCFlagsClass.HCFlags.NO_HEADER_COMPRESSION));
            setPriority(PriorityClass.value(PriorityClass.Priority.NORMAL));
            setBcast(BroadcastClass.value(BroadcastClass.Broadcast.NO));
            setAckNak(AckNakClass.value(AckNakClass.AckNak.NOT_REQUIRED));
            setDataFlag(DataFlagClass.value(DataFlagClass.DataFlag.SINGLE_DATA_PACKET));
            commandCode.setValue(CommandCode);
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

        public byte getMessageTypeHCflags()
        {
            return messageTypeHCflags;
        }

        public void setMessageTypeHCflags(byte mtHCflags)
        {
            this.messageTypeHCflags = mtHCflags;
            this.setMessageType(this.getMessageType());
            this.setHCflags(this.getHCflags());
        }

        public int getMessageType()
        {
            return (messageTypeHCflags >> MESSAGE_TYPE_BIT_POSITION) & 0x3F;
        }

        //Message type is currently not anything except 0 in the spec.
        public void setMessageType(int value)
        {
            //		this.messageType = CommandCode.convert(value);
            messageTypeHCflags = (byte)((messageType << MESSAGE_TYPE_BIT_POSITION) | (messageTypeHCflags & 0xC0));
        }

        public int getHCflags()
        {
            // return this.HCflags;
            return (messageTypeHCflags >> HC_FLAGS_BIT_POSITION) & 0x03;
        }

        public void setHCflags(int value)
        {
            this.HCflags = HCFlagsClass.convert(value);
            messageTypeHCflags = (byte)((HCFlagsClass.value(this.HCflags) << HC_FLAGS_BIT_POSITION) | (messageTypeHCflags & 0x3F));
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

        public byte getProperties()
        {
            return properties;
        }

        public void setProperties(byte properties)
        {
            this.properties = properties;
            // Inorder to set enum types, these must be called
            this.setPriority(this.getPriority());
            this.setBcast(this.getBcast());
            this.setAckNak(this.getAckNak());
            this.setDataFlag(this.getDataFlag());
        }


        public int getPriority()
        {
            return (properties >> PRIORITY_BIT_POSITION) & 0x03;
            // return this.priority.ordinal();
        }

        public void setPriority(int priority)
        {
            this.priority = PriorityClass.convert(priority);
            properties = (byte)((PriorityClass.value(this.priority) << PRIORITY_BIT_POSITION) | (properties & 0xFC));
        }

        public int getBcast()
        {
            return (properties >> BCAST_BIT_POSITION) & 0x03;
            // return bCast.ordinal();
        }

        public void setBcast(int bcast)
        {
            this.bCast = BroadcastClass.convert(bcast);
            properties = (byte)((BroadcastClass.value(this.bCast) << BCAST_BIT_POSITION) | (properties & 0xF3));
        }

        public int getAckNak()
        {
            return (properties >> ACK_NAK_BIT_POSITION) & 0x03;
            // return this.ackNak.ordinal();
        }

        public void setAckNak(int ackNak)
        {
            this.ackNak = AckNakClass.convert(ackNak);
            properties = (byte)(AckNakClass.value(this.ackNak) << ACK_NAK_BIT_POSITION | (properties & 0xCF));
        }

        public int getDataFlag()
        {
            return (properties >> DATA_FLAGS_BIT_POSITION) & 0x03;
            // return this.dataFlags.ordinal();
        }

        public void setDataFlag(int dflag)
        {
            this.dataFlags = DataFlagClass.convert(dflag);
            properties = (byte)((DataFlagClass.value(this.dataFlags) << DATA_FLAGS_BIT_POSITION) | (properties & 0x3F));
        }

        protected virtual int CommandCode
        {
            get { return JausCommandCode.NONE; }
        }

        public int GetCommandCode()
        {
            return commandCode.getValue();
        }

        public JausAddress GetDestination()
        {
            return destination;
        }

        public void SetDestination(int id)
        {
            destination.setId(id);
        }

        public void SetDestination(JausAddress address)
        {
            destination.setId(address.getId());
        }

        public JausAddress GetSource()
        {
            return source;
        }

        public void SetSource(int id)
        {
            this.source.setId(id);
        }

        public void SetSource(JausAddress address)
        {
            source.setId(address.getId());
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
            if (this.data != null)
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
            
            payloadSize = dataSize.getValue() - HEADER_SIZE_BYTES_NCMP - COMMAND_CODE_SIZE_BYTES - SEQUENCE_NUMBER_SIZE_BYTES;
            data = new byte[payloadSize];
            Array.Copy(buffer, index, data, 0, payloadSize);

            indexOffset = index + payloadSize;
            return true;
        }

        public int GetSequenceNumber()
        {
            return sequenceNumber.getValue();
        }

        public void SetSequenceNumber(int sequenceNumber)
        {
            this.sequenceNumber.setValue(sequenceNumber);
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
            dataSize.setValue(jausMessage.MessageSize());
        }

        public void SetAddressFromJausMessage(Message jausMessage)
        {
            SetDestination(jausMessage.GetDestination().getId());
            SetSource(jausMessage.GetSource().getId());
        }

        public int SetFromJausBuffer(byte[] buffer, int index = 0)
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
            if (this.getDataFlag() != DataFlagClass.value(DataFlagClass.DataFlag.SINGLE_DATA_PACKET))
            {
                Console.Error.WriteLine("Receiving Multiple Packets Per Message Not Supported");
                return -1;
            }

            // TODO: Implement Ack/Nak
            if (this.getAckNak() != AckNakClass.value(AckNakClass.AckNak.NOT_REQUIRED))
            {
                return -1;
            }

            if(CommandCode == JausCommandCode.NONE)
            {
                if (!commandCode.setFromJausBuffer(buffer, index))
                {
                    return -1; // setCommandCodeFromJausBuffer failed
                }
            }
            else
            {
                JausUnsignedShort bufferCommandCode = new JausUnsignedShort(buffer, index);
                if (bufferCommandCode.getValue() != CommandCode)
                    return -1;
            }

            index += COMMAND_CODE_SIZE_BYTES;

            SetPayloadFromJausBuffer(buffer, index, out index);

            int dataSize = GetPayloadSize();
            if (dataSize < 0 || dataSize > 10000)
            {
                return -1;
            }

            if (!sequenceNumber.setFromJausBuffer(buffer, index))
            {
                return -1; // setSequenceNumberFromJausBuffer failed
            }

            return index + JausUnsignedShort.SIZE_BYTES;
        }

        // Takes the header and data byte array and packs them into a data buffer
        // This method needs to be overridden for all subclasses of JausMessage to reflect the correct pack routine
        public bool ToJausBuffer(byte[] buffer, int index = 0)
        {
            // TODO: Implement Compression
            if (this.getHCflags() != HCFlagsClass.value(HCFlagsClass.HCFlags.NO_HEADER_COMPRESSION))
            {
                Console.Error.WriteLine("Sending Compression Not Supported");
                return false;
            }

            // TODO: Implement Ack/Nak
            if (this.getAckNak() != AckNakClass.value(AckNakClass.AckNak.NOT_REQUIRED))
            {
                Console.Error.WriteLine("Sending Ack/Nak Not Supported");
                return false;
            }

            if (buffer.Length < (index + GetPayloadSize() + Message.SEQUENCE_NUMBER_SIZE_BYTES))
            {
                Console.Error.WriteLine("Error in toJausBuffer: Not Enough Size");
                return false; // Not Enough Size	
            }
            if (!HeaderToJausBuffer(buffer, index))
            {
                Console.Error.WriteLine("ToJausBuffer Failed");
                return false; //headerToJausBuffer failed
            }

            index += headerBaseSize;

            if (!commandCode.toJausBuffer(buffer, index)) return false;
            index += JausUnsignedShort.SIZE_BYTES;

            PayloadToJausBuffer(buffer, index, out index);
            //Console.WriteLine("Claimed payload size: " + GetPayloadSize());

            //Console.WriteLine("Sequence index position: " + index);

            if (!sequenceNumber.toJausBuffer(buffer, index))
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
            index = SetFromJausBuffer(buffer, index);

            return index;
        }

        public int SetFromJausUdpBuffer(byte[] buffer)
        {
            return SetFromJausUdpBuffer(buffer, 0);
        }

        public bool ToJausUdpBuffer(byte[] buffer, int index)
        {
            if (buffer.Length < (index + JUDP_HEADER_SIZE_BYTES))
            {
                Console.Error.WriteLine("ToJausUdpBuffer Failed: Not Enough Size");
                return false; // Not Enough Size
            }

            buffer[index] = Message.JUDP_HEADER;

            index += Message.JUDP_HEADER_SIZE_BYTES;
            return ToJausBuffer(buffer, index);
        }


        // Overloaded method to accept a buffer and pack this message into its UDP form
        public bool ToJausUdpBuffer(byte[] buffer)
        {
            return ToJausUdpBuffer(buffer, 0);
        }

        // This private method takes a buffer at the given index and unpacks it into the header properties and data fields
        // This method is called whenever a message is unpacked to pull the header properties
        private bool SetHeaderFromJausBuffer(byte[] buffer, int index)
        {
            if (buffer.Length < index + headerBaseSize)
                return false; // Not Enough Size

            this.setMessageTypeHCflags(buffer[index]);

            // TODO: Implement Compression
            if (this.getHCflags() != HCFlagsClass.value(HCFlagsClass.HCFlags.NO_HEADER_COMPRESSION))
            {
                Console.Error.WriteLine("Compression Not Supported");
                return false;
            }

            // int data1 = buffer[index+2];
            dataSize.setFromJausBuffer(buffer, index + 1);

            // No Compression Assumed
            this.setProperties(buffer[index + 3]);

            destination.setComponent(buffer[index + 4]);
            destination.setNode(buffer[index + 5]);
            destination.setSubsystem(new JausUnsignedShort(buffer, index + 6).getValue());

            source.setComponent(buffer[index + 8]);
            source.setNode(buffer[index + 9]);
            source.setSubsystem(new JausUnsignedShort(buffer, index + 10).getValue());

            return true;
        }

        // This private method packs the header properties and data fields into the provided buffer at the given index
        // This method is called whenever a message is packed to put the header properties
        private bool HeaderToJausBuffer(byte[] buffer, int index)
        {
            if (buffer.Length < index + headerBaseSize)
                return false; // Not Enough Size

            buffer[index] = this.getMessageTypeHCflags();			// Message type

            //Console.WriteLine("Size: " + size());

            dataSize.setValue(MessageSize());
            dataSize.toJausBuffer(buffer, index + 1);

            //buffer[index + 3] = 0;								// HC number, not needed
            //buffer[index + 4] = 0;								// HC Length, not needed

            buffer[index + 3] = this.getProperties();				// Message Properties

            buffer[index + 4] = (byte)destination.getComponent();	// Destination component
            buffer[index + 5] = (byte)destination.getNode();		// Destination node	
            new JausUnsignedShort(destination.getSubsystem()).toJausBuffer(buffer, index + 6);

            buffer[index + 8] = (byte)source.getComponent();		// Source component
            buffer[index + 9] = (byte)source.getNode();				// Source node
            new JausUnsignedShort(source.getSubsystem()).toJausBuffer(buffer, index + 10);

            return true;
        }

        public virtual string toString()
        {
            string str = "Command Code: " + JausCommandCode.Lookup(commandCode.getValue()) + " (0x" + Convert.ToString(commandCode.getValue(), 16).ToUpper() + ") " + "\n";
            return str;
        }

        public String headerToString()
        {
            String str = "";
            // str += "JUDP Version: " + JausMessage.JUDP_HEADER + "\n";
            str += "Message Type: " + this.messageType + "\n";
            str += "HC flags: " + this.HCflags + "\n";
            str += "Data Size: " + this.dataSize.getValue() + "\n";
            // str += "HC Number: " + this.getHCnumber() + "\n";
            // str += "HC Length: " + this.getHCLength() + "\n";
            str += "Priority: " + this.priority + "\n";
            str += "Broadcast: " + this.bCast + "\n";
            str += "Ack/Nak: " + this.ackNak + "\n";
            str += "Data Flags: " + this.dataFlags + "\n";
            str += "Destination: " + this.destination.toString() + "\n";
            str += "Source: " + this.source.toString() + "\n";
            //str += "Command Code: " + commandCodeJT + " (0x" + Integer.toString(this.commandCodeJT.COMMAND_CODE(), 16).toUpperCase() + ") " + "\n";
            str += "Data: " + this.payloadToString() + "\n";
            str += "Sequence Number: " + sequenceNumber.getValue() + "\n";

            return str;
        }

    }
}