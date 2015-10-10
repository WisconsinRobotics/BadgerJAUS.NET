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
    public class JausAddress : JausUnsignedInteger
    {
        private const int SUBSYSTEM_BIT_POSITION = 16;
        private const int NODE_BIT_POSITION = 8;
        private const int COMPONENT_BIT_POSITION = 0;

        /** 
         * Indicates the invalid component value, 0
         */
        public const int INVALID_COMPONENT = 0;

        /**
         * Indicates the invalid node value, 0
         */
        public const int INVALID_NODE = 0;

        /**
         * Indicates the invalid subsystem value, 0
         */
        public const int INVALID_SUBSYSTEM = 0;

        /**
         * Indicates the component broadcast value, 255
         */
        public const int BROADCAST_COMPONENT = 255;

        /**
         * Indicates the node broadcast value, 255
         */
        public const int BROADCAST_NODE = 255;

        /**
         * Indicates the subsystem broadcast value, 65535
         */
        public const int BROADCAST_SUBSYSTEM = 65535;

        /*
        public const int NODE_MANAGER_COMPONENT = 1;
        public const int PRIMARY_NODE_MANAGER_NODE = 1;
        */

        /**
         * Indicates the size, 4, in bytes of a JAUS Address 
         */
        public const int SIZE = 4;

        /**
         * Creates the empty JAUS Address 0.0.0. This is not a valid address, in the 
         * sense of a valid address for a JAUS component.  After generating an 
         * address using this constructor one should use the set 
         * methods to specify a valid subsystem, node, and component. 
         *  
         */
        public JausAddress()
        {
            value = 0;
        }

        /**
         * Creates a JAUS Address using the unique identifier id received by making 
         * a call to another JausAddress's {@link #getId()} method.  This can be a 
         * shorthand for creating a default JausAddress and then calling the 
         * {@link #setId(int)} method.
         *   
         * @param id a unique integer which determines the subystem, node, and component
         *
         * @see		#setId(int)
         * @see 	#getId()
         */
        public JausAddress(int id)
        {
            this.value = id;
        }

        /**
         * Creates a JAUS Address with the same subsystem, node, and component as 
         * {@code address}. 
         * 
         * @param address the JausAddress to be cloned
         * 
         * @see 	#equals(Object)
         */
        public JausAddress(JausAddress address)
        {
            value = address.Value;
        }

        /**
         * Creates a JAUS Address with the specified {@code subsystem}, {@code node}, 
         * and {@code component}.  
         * 
         * @param subsystem The subsystem of this JausAddress
         * @param node The node of this JausAddress
         * @param component The Component of this JausAddress
         */
        public JausAddress(int subsystem, int node, int component)
        {
            setSubsystem(subsystem);
            setNode(node);
            setComponent(component);
        }

        /**
         * Returns the subsystem associated with this JausAddress. Valid ranges for 
         * a subsystem include 0 through 65535.   
         * 
         * @return The subsystem identifier associated with this address.
         * 
         * @see #setSubsystem(int)
         */
        public int getSubsystem()
        {
            return (int)value >> SUBSYSTEM_BIT_POSITION;
        }

        /**
         * Returns the node associated with this JausAddress. Valid ranges for a 
         * node include 0 through 255.
         * 
         * @return The node identifier associated with this address.
         * 
         * @see #setNode(int)
         */
        public int getNode()
        {
            return ((int)value >> NODE_BIT_POSITION) & 0xFF;
        }

        /**
         * Returns the component associated with this JausAddress. Valid ranges for
         * a component include 0 through 255.
         * 
         * @return The component identifier associated with this address.
         * 
         * @see #setComponent(int)
         */
        public int getComponent()
        {
            return ((int)value >> COMPONENT_BIT_POSITION) & 0xFF;
        }

        /**
         * Sets the subsystem associated with this JausAddress. Valid ranges for 
         * a subsystem include 0 through 65535.  Values outside the valid range will 
         * be bit masked to a value within the valid range. 
         * 
         * @param subsystem The subsystem identifier to be associated with this 
         * 					address.
         * 
         * @see #getSubsystem() 
         */
        public void setSubsystem(int subsystem)
        {
            if (subsystem > -1 && subsystem < 65536)
            {
                value = (uint)(subsystem << SUBSYSTEM_BIT_POSITION) | (value & 0xFFFF);
            }
        }

        /**
         * Sets the node associated with this JausAddress. Valid ranges for a 
         * node include 0 through 255. Values outside the valid range will 
         * be bit masked to a value within the valid range.
         * 
         * @param node The node identifier to be associated with this address.
         * 
         * @see #getNode()
         */
        public void setNode(int node)
        {
            if (node > -1 && node < 256)
            {
                value = (node << NODE_BIT_POSITION) | (int)(value & 0xFFFF00FF);
            }
        }

        /**
         * Sets the component associated with this JausAddress. Valid ranges for
         * a component include 0 through 255. Values outside the valid range will 
         * be bit masked to a value within the valid range.
         * 
         * @return The component identifier to be associated with this address.
         * 
         * @see #getComponent()
         */
        public void setComponent(int component)
        {
            if (component > -1 && component < 256)
            {
                value = (component << COMPONENT_BIT_POSITION) | (int)(value & 0xFFFFFF00);
            }
        }

        /**
         * 
         * @return True if this address does not contain zeros or
         * broadcast values (i.e. 255, 65535).
         * 
         */
        public bool isValid()
        {
            if (this.getComponent() != INVALID_COMPONENT && this.getComponent() != BROADCAST_COMPONENT &&
                 this.getNode() != INVALID_NODE && this.getNode() != BROADCAST_NODE &&
                 this.getSubsystem() != 0 && this.getSubsystem() != BROADCAST_SUBSYSTEM)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public String toHexString()
        {
            return (new JausByte(this.getComponent())).toHexString() +
            " " + (new JausByte(this.getNode())).toHexString() +
            " " + (new JausUnsignedShort(this.getSubsystem())).toHexString();
        }

        public int size()
        {
            return SIZE;
        }

        public override string ToString()
        {
            return "" + getSubsystem() + "." + getNode() + "." + getComponent();
        }

        public static String IDToHexString(int id)
        {
            return (new JausAddress(id)).toHexString();
        }
    }
}