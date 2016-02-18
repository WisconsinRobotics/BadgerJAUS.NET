using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Util;

namespace BadgerJAUSSubsystem
{
    class BadgerJAUSSubsystem : JausSubsystem
    {
        const int SUBSYSTEM_ID = 10;

        BadgerJAUSSubsystem() : base(SUBSYSTEM_ID, "BadgerJAUSSubsystem")
        {

        }

        static void Main(string[] args)
        {
            BadgerJAUSSubsystem badgerJAUSSubsystem = new BadgerJAUSSubsystem();
        }
    }
}
