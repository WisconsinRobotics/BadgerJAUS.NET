using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadgerJaus.Util
{
    public abstract class JausBaseType : JausType
    {
        protected long value;

        public const int BYTE_BYTE_SIZE = 1;
        public const int SHORT_BYTE_SIZE = 2;
        public const int INT_BYTE_SIZE = 4;

        public JausBaseType()
        {
            value = 0;
        }

        public JausBaseType(int value)
        {
            this.value = value;
        }

        public JausBaseType(long value)
        {
            this.value = value;
        }

        public JausBaseType(JausBaseType other)
        {
            this.value = other.Value;
        }

        public JausBaseType(byte[] buffer)
        {
            int indexOffset;
            Deserialize(buffer, 0, out indexOffset);
        }

        public JausBaseType(byte[] buffer, int index)
        {
            int indexOffset;
            Deserialize(buffer, index, out indexOffset);
        }


        public bool Serialize(byte[] byteArray, int index, out int indexOffset)
        {
            int valueByte;

            indexOffset = index;
            if (indexOffset + SIZE_BYTES >= byteArray.Length)
                return false;

            for (valueByte = 0; valueByte < SIZE_BYTES; ++valueByte, ++indexOffset)
            {
                byteArray[indexOffset] = (byte)((value >> (valueByte * 8)) & 0xFF); // 8 bits per byte
            }

            return true;
        }

        public bool Serialize(byte[] byteArray, out int indexOffset)
        {
            return Serialize(byteArray, 0, out indexOffset);
        }

        public bool Deserialize(byte[] byteArray, int index, out int indexOffset)
        {
            int valueByte;

            indexOffset = index;
            if (indexOffset + SIZE_BYTES >= byteArray.Length)
                return false;
            value = 0;
            for (valueByte = 0; valueByte < SIZE_BYTES; ++valueByte, ++indexOffset)
            {
                value += ((long)(byteArray[indexOffset] & 0xFF) << valueByte * 8); // 8 bits per byte
            }

            return true;
        }

        public bool Deserialize(byte[] byteArray, out int indexOffset)
        {
            return Deserialize(byteArray, 0, out indexOffset);
        }

        // Real_Value = Integer_Value*Scale_Factor + Bias
        public double ScaleValueToDouble(double min, double max)
        {
            double scaleFactor = (max - min) / RANGE;
            double bias = min;

            double val = value * scaleFactor + bias;
            
            return val;
        }

        // Integer_Value = Round((Real_Value - Bias)/Scale_Factor)
        public void SetValueFromDouble(double value, double min, double max)
        {
            double scaleFactor = (max - min) / (double)RANGE;
            double bias = min;

            // limit real number between min and max
            if (value < MIN_VALUE) value = min;
            if (value > MAX_VALUE) value = max;

            // calculate rounded integer value
            this.value = (long)Math.Round((value - bias) / scaleFactor);
        }

        public bool IsBitSet(int bit)
        {
            if (bit >= (SIZE_BYTES * 8))
                return false;

            return ((value & (0x01 << bit)) != 0);
        }

        public void ToggleBit(int bit, bool set = false)
        {
            if (bit >= SIZE_BYTES * 8)
                return;

            if(set)
                value |= (uint)(0x01 << bit);
            else
                value &= ~(0x01 << bit);
        }

        public string toHexString()
        {
#warning Double check this conversion, it feels fishy/needlessly complicated
            string temp = "";
            //String temp2 = Long.toHexString(value).toUpperCase();
            string temp2 = String.Format("{0:X}", value).ToUpper();
            while (temp2.Length < SIZE_BYTES * 2) { temp2 = "0" + temp2; }
            for (int i = temp2.Length; i > 0; i -= 2)
            {
                //temp += temp2.Substring(i-2, i) + " ";
                temp += temp2.Substring(i - 2, i - (i - 2)) + " ";
            }
            return temp;
        }

        public abstract int SIZE_BYTES
        {
            get;
        }

        public abstract long MAX_VALUE
        {
            get;
        }

        public virtual long MIN_VALUE
        {
            get { return 0; }
        }

        public abstract long RANGE
        {
            get;
        }

        public long Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public static bool operator ==(JausBaseType value1, JausBaseType value2)
        {
            return value1.Value == value2.Value;
        }

        public static bool operator !=(JausBaseType value1, JausBaseType value2)
        {
            return value1.Value != value2.Value;
        }
#warning Need to fix this for real later
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            JausBaseType baseType = obj as JausBaseType;
            if (baseType == null)
                return false;

            return baseType.Value == this.Value;
        }
    }
}
