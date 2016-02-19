using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BadgerJaus.Util;
using BadgerJaus.Services.Mobility;

namespace BadgerJAUSSubsystem
{
    class BadgerJAUSSubsystem : JausSubsystem
    {
        const int SUBSYSTEM_ID = 10;
        const int NODE_ID = 1;
        const int COMPONENT_ID = 44;

        BadgerJAUSSubsystem() : base(SUBSYSTEM_ID, "BadgerJAUSSubsystem")
        {

        }

        static void Main(string[] args)
        {
            BadgerJAUSSubsystem badgerJAUSSubsystem = new BadgerJAUSSubsystem();
            Node node = new Node(NODE_ID);
            Component component = new Component(COMPONENT_ID);
            LocalVectorDriver localVectorDriver = new LocalVectorDriver();

            node.AddComponent(component);
            badgerJAUSSubsystem.AddNode(node);
            component.AddService(localVectorDriver);

            badgerJAUSSubsystem.InitializeTimer();
        }
    }
}
