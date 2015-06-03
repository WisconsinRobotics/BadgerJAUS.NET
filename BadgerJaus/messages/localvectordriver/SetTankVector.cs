using System;

public class SetTankVector : Message
{
    private const int PAYLOAD_SIZE = 4;
    private short leftSpeed;
    private short rightSpeed;

	public SetTankVector() : base()
	{
        this.SetCommandCode(CommandCode.SET_TANT_VECTOR);
	}

    public SetTankVector(Message message) : base(message)
    {
        this.SetCommandCode(CommandCode.SET_TANT_VECTOR);
    }

    protected override void InitData()
    {
        base.InitData();
    }

    protected override bool setPayloadFromJausBuffer(byte[] data, int index)
    {
        if (!base.setPayloadFromJausBuffer(data, index))
        {
            Console.Error.WriteLine("Command Code Failed");
            return false;
        }
        if (data.Length != PAYLOAD_SIZE)
        {
            Console.Error.WriteLine("Payload is incorrect size");
            return false;
        }

        byte[] leftDataArray = new byte[sizeof(short)];
        byte[] rightDataArray = new byte[sizeof(short)];
        for (int i = 0; i < leftDataArray.Length; i++)
        {
            leftDataArray[i] = data[i];
        }

        for (int i = 0; i < rightDataArray.Length; i++)
        {
            rightDataArray[i] = data[i + leftDataArray.Length];
        }

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(leftDataArray);
            Array.Reverse(rightDataArray);
        }

        this.leftSpeed = BitConverter.ToInt16(leftDataArray, 0);
        this.rightSpeed = BitConverter.ToInt16(rightDataArray, 0);
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

        BitConverter.GetBytes(this.leftSpeed).CopyTo(data, 0);
        BitConverter.GetBytes(this.rightSpeed).CopyTo(data, sizeof(short));
        return true;
    }

    public override int GetPayloadSize()
    {
        return PAYLOAD_SIZE;
    }
}