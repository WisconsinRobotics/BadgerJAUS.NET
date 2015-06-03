using System;

public class ClientService : Service
{
    private bool authorized;
    private Component jausComponent;
    private JausAddress sourceAddr;
    private JausAddress destAddr;
    public const int SLEEP_TIME = 5000;

    public ClientService(JausAddress source, JausAddress dest)
    {
        this.sourceAddr = source;
        this.destAddr = dest;
    }

    public int GetState()
    {
        return 0;
    }

    public Service GetParentService()
    {
        return null;
    }

    public JausServiceSignature GetServiceSignature()
    {
        return null;
    }

    public bool IsSupported(int commandCode)
    {
        return false;
    }

    public bool ImplementsAndHandledMessage(Message message)
    {
        switch (message.GetCommandCode())
        { 
            case CommandCode.CONFIRM_CONTROL:
                this.authorized = true;
                break;
        }
        return false;
    }

    public long Execute()
    {
        if (!this.authorized)
        {
            RequestControl rc = new RequestControl();
            rc.setSource(this.sourceAddr);
            rc.setDestination(this.destAddr);
            Transport.SendMessage(rc);
            return SLEEP_TIME;
        }

        else
        {
            SetLocalVector sv = new SetLocalVector();
            sv.SetSpeed(100);
            sv.SetHeading(Math.PI);
            sv.setSource(this.sourceAddr);
            sv.setDestination(this.destAddr);
            Transport.SendMessage(sv);
            Console.WriteLine("Called");

            return SLEEP_TIME;
        }
    }

    public String GetServiceID()
    {
        return null;
    }

    public int GetMajorVersion()
    {
        return 0;
    }

    public int GetMinorVersion()
    {
        return 0;
    }

    public void SetComponent(Component component)
    {
        this.jausComponent = component;
    }

    public bool isAuthorized()
    {
        return this.authorized;
    }
}
