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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Timers;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;

using BadgerJaus.Services;
using BadgerJaus.Services.Core;

namespace BadgerJaus.Util
{
    public class Subsystem : INotifyPropertyChanged
    {
        protected JausUnsignedShort subsystemID;
        protected Dictionary<long, Node> nodeDictionary;
        protected ObservableCollection<Node> observableNodes;
        protected IPEndPoint networkAddress = null;
        protected string identification;

        public const int JAUS_PORT = 3974;

        public event PropertyChangedEventHandler PropertyChanged;

        public Subsystem(int subsystemID, int port)
        {
            InitializeData(subsystemID);
            networkAddress = new IPEndPoint(IPAddress.Any, port);
        }

        public Subsystem(int subsystemID, IPEndPoint networkAddress)
        {
            InitializeData(subsystemID);
            this.networkAddress = new IPEndPoint(networkAddress.Address, networkAddress.Port);
        }

        private void InitializeData(int subsystemID)
        {
            nodeDictionary = new Dictionary<long, Node>();
            observableNodes = new ObservableCollection<Node>();
            this.subsystemID = new JausUnsignedShort(subsystemID);
        }

        public int SubsystemID
        {
            get { return (int)subsystemID.Value; }
            set { subsystemID.Value = value; }
        }

        public void AddNode(Node node)
        {
            Node knownNode;
            if (node == null)
                return;

            if(NodeDictionary.TryGetValue(node.NodeID, out knownNode))
            {
                nodeDictionary.Remove(node.NodeID);
                observableNodes.Remove(knownNode);
            }
            nodeDictionary.Add(node.NodeID, node);
            observableNodes.Add(node);

            node.SetSubsystem(this);
        }

        public Dictionary<long, Node> NodeDictionary
        {
            get { return nodeDictionary; }
        }

        public IEnumerable<Node> NodeList
        {
            get { return nodeDictionary.Values; }
        }

        public ObservableCollection<Node> ObservableNodes
        {
            get { return observableNodes; }
        }

        public IPEndPoint NetworkAddress
        {
            get { return networkAddress; }
            set
            {
                if(value != null)
                {
                    networkAddress.Address = value.Address;
                    networkAddress.Port = value.Port;
                }
                    
            }
        }

        public string Identification
        {
            get { return identification; }
            set
            {
                if (identification == value)
                    return;
                identification = value;
                NotifyPropertyChanged("Identification");
            }
        }

        public void UpdateNodList(Dictionary<long, Node> updatedList)
        {
            foreach(KeyValuePair<long, Node> entry in updatedList)
            {
                Node existingNode;
                if(!nodeDictionary.TryGetValue(entry.Key, out existingNode))
                {
                    AddNode(entry.Value);
                    continue;
                }

                existingNode.UpdateComponents(entry.Value.ComponentDictionary);
            }
        }

        public override string ToString()
        {
            return identification;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}