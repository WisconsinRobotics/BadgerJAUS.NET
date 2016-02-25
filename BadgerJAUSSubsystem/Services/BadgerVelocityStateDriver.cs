using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Services.Mobility;
using BadgerJaus.Util;

namespace BadgerJAUSSubsystem.Services
{
    class BadgerVelocityStateDriver : VelocityStateDriver
    {
        protected override void Execute(Component component)
        {
            Console.Clear();
            Console.Write("\rX velocity: {0, -10} Y velocity: {1, -10}", (int)velocityX, (int)velocityY);
        }

        public override long SleepTime
        {
            get { return 100; }
        }
    }
}
