using System;

public class SetArcVector : Message
{
    private const int PAYLOAD_SIZE = 6;
    private short velocity;
    private int turnValue;

	public SetArcVector() : base()
	{
        this.commandCode.setValue(CommandCode.SET_ARC_VECTOR);
	}

    public SetArcVector(Message message) : base(message)
    {
        this.commandCode.setValue(CommandCode.SET_ARC_VECTOR);
    }

    protected override void InitData()
    {
        base.InitData();
    }

    protected override bool setPayloadFromJausBuffer(byte[] data, int index)
    {
        if(!base.setPayloadFromJausBuffer(data, index))
        {
            Console.Error.WriteLine("Command Code Failed");
            return false;
        }
        if (data.Length != PAYLOAD_SIZE)
        {
            Console.Error.WriteLine("Payload is incorrect size");
            return false;
        }

        byte[] shortDataArray = new byte[sizeof(short)];
        byte[] intDataArray = new byte[sizeof(int)];
        for (int i = 0; i < sizeof(short); i++)
        {
            shortDataArray[i] = data[i];
        }

        for (int i = 0; i < sizeof(int); i++)
        {
            intDataArray[i] = data[i + shortDataArray.Length];
        }

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(shortDataArray);
            Array.Reverse(intDataArray);
        }

        this.velocity = BitConverter.ToInt16(shortDataArray, 0);
        this.turnValue = BitConverter.ToInt32(intDataArray, 0);
        return true;
    }

    protected override bool payloadToJausBuffer(byte[] data, int index)
    {
        if (!base.payloadToJausBuffer(data, index))
        {
            Console.Error.WriteLine("Command Code Failed");
            return false;
        }

        if (data.Length != PAYLOAD_SIZE)
        {
            Console.Error.WriteLine("Buffer is incorrect size, can't put payloadToJausBuffer");
            return false;
        }

        BitConverter.GetBytes(this.velocity).CopyTo(data, 0);
        BitConverter.GetBytes(this.turnValue).CopyTo(data, sizeof(short));
        return true;
    }

    public override int GetPayloadSize()
    {
        return PAYLOAD_SIZE;
    }
}
