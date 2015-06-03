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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using BadgerJaus.Messages;

using BadgerJaus.Util;

namespace BadgerJaus.Services
{
    public abstract class BaseService : Service
    {
        protected JausAddress jausAddress = null;
        protected Component component = null;
        protected JausServiceSignature jausServiceSignature = null;
        protected Service parentService = null;

        public const int MAJOR_VERSION = 1;
        public const int MINOR_VERSION = 0;

        public const int DEFAULT_SLEEP_TIME = 5000;

        private Stopwatch executionStopwatch;

        public BaseService()
        {
            executionStopwatch = new Stopwatch();
            executionStopwatch.Start();
        }

        protected virtual void Execute()
        {
            return;
        }

        public Service GetParentService()
        {
            return parentService;
        }

        public JausServiceSignature GetServiceSignature()
        {
            return jausServiceSignature;
        }

        public void SetComponent(Component component)
        {
            this.component = component;
            this.jausAddress = component.GetAddress();
        }

        public int GetMajorVersion()
        {
            return MAJOR_VERSION;
        }

        public int GetMinorVersion()
        {
            return MINOR_VERSION;
        }

        public String GetServiceID()
        {
            return null;
        }

        public abstract bool IsSupported(int commandCode);
        public abstract bool ImplementsAndHandledMessage(Message message);

        public void ExecuteOnTime()
        {
            long elapsedTime;
            
            elapsedTime = executionStopwatch.ElapsedMilliseconds;

            if (elapsedTime >= SleepTime && component.GetState() == COMPONENT_STATE.STATE_READY)
                Execute();

            executionStopwatch.Reset();
            executionStopwatch.Start();
        }

        public virtual long SleepTime
        {
            get { return DEFAULT_SLEEP_TIME; }
        }
    }
}
