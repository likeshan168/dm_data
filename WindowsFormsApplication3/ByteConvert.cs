using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class ByteConvert
    {
        public static int byteArrayToInt(byte[] byteArray)
        {
            int value = 0;
            value = (int)((byteArray[0] << 24) & 0xFF000000)
                + ((byteArray[1] << 16) & 0xFF0000)
                + ((byteArray[2] << 8) & 0xFF00)
                + (byteArray[3] & 0xFF);
            return value;
        }

        public static byte[] intToByteArray(int intNum)
        {
            byte[] byteArray = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                byteArray[i] = (byte)((intNum >> (24 - 8 * i)) & 0xFF);
            }
            return byteArray;
        }

        public static byte[] shortToByteArray(short shortNum)
        {
            byte[] byteArray = new byte[2];
            for (int i = 0; i < 2; i++)
            {
                byteArray[i] = (byte)((shortNum >> (8 - 8 * i)) & 0xFF);
            }
            return byteArray;
        }

        public static short byteArrayToShort(byte[] byteArray)
        {
            short value;
            value = (short)(((byteArray[0] << 8) & 0xFF00) + (byteArray[1] & 0xFF));
            return value;
        }


    }
}
