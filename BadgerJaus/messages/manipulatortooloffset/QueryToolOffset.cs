﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadgerJaus.Messages;

namespace BadgerJaus.messages.manipulatortooloffset
{
    public class QueryToolOffset : Message
    {
        protected override int CommandCode
        {
            get { return JausCommandCode.QUERY_TOOL_OFFSET; }
        }
    }
}
