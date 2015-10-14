using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Messages;
using BadgerJaus.Util;

namespace BadgerJaus.Services
{
    public class DiscoveredService : BaseService
    {
        public override bool IsSupported(int commandCode)
        {
            return false;
        }

        public override bool ImplementsAndHandledMessage(Message message, Component component)
        {
            return false;
        }
    }
}
