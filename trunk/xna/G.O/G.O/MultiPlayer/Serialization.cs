using System;
using ION.GridStrategies;
using System.IO;

namespace ION
{
    public class Serializer
    {
        #region Serialize

        #region MemoryStream Write Primitives
        public static Byte[] Serialize(Boolean input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(Boolean[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (Boolean idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(Char input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);

            Byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(Char[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);
            bw.Write(input);

            Byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(Decimal input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);

            Byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(Decimal[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (Decimal idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(Double input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);
            byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(Double[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (Double idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(Int16 input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);
            Byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(Int16[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (Int16 idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(Int32 input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);
            byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(Int32[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (Int32 idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(Int64 input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);
            byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(Int64[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (Int64 idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(SByte input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);
            byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(SByte[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (SByte idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(Single input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);
            byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(Single[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (Single idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(String input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);
            byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(String[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (String idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(UInt16 input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);
            byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(UInt16[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (UInt16 idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(UInt32 input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);
            byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(UInt32[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (UInt32 idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        public static Byte[] Serialize(UInt64 input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input);
            byte[] byteArray = streamTobyteArray(ms);
            ms.Close();

            return byteArray;
        }
        public static Byte[] Serialize(UInt64[] input)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(input.Length);

            foreach (UInt64 idx in input)
            {
                bw.Write(idx);
            }

            Byte[] result = streamTobyteArray(ms);
            ms.Close();

            return result;
        }
        #endregion

        public static Byte[] Serialize(Tile[,] input, int width, int height)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(width);
            writer.Write(height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (input[i, j] is ResourceTile)
                    {
                        ResourceTile t = (ResourceTile)input[i, j];
                        writer.Write(t.charge);
                    }
                    else
                    {
                        writer.Write(0f);
                    }
                }
            }

            byte[] byteArray = streamTobyteArray(stream);

            stream.Close();

            return byteArray;
        }
        public static Byte[] Serialize(CheckedState[,] input)
        {
            MemoryStream ms = new MemoryStream();
            foreach (CheckedState tile in input)
            {
                MemoryStream tmp = tile.Serialize();
                tmp.WriteTo(ms);
                tmp.Close();
            }
            return streamTobyteArray(ms);
        }

        public static Byte[] Serialize(Serializable input)
        {
            MemoryStream stream = input.Serialize();
            Byte[] byteArray = streamTobyteArray(stream);
            stream.Close();
            return byteArray;
        }

        #endregion

        #region Deserialize

        #region MemoryStream Write Primitives
        public static Boolean DeserializeBoolean(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            Boolean result = br.ReadBoolean();
            ms.Close();

            return result;
        }
        public static Boolean DeserializeBoolean(Object[] input)
        {
            return DeserializeBoolean(DeserializeObjectArray(input));
        }
        public static Boolean[] DeserializeBooleanArray(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            Boolean[] result = new Boolean[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadBoolean();
            }
            ms.Close();

            return result;
        }
        public static Boolean[] DeserializeBooleanArray(Object[] input)
        {
            return DeserializeBooleanArray(DeserializeObjectArray(input));
        }
        public static Char DeserializeChar(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            Char result = br.ReadChar();
            ms.Close();

            return result;
        }
        public static Char DeserializeChar(Object[] input)
        {
            return DeserializeChar(DeserializeObjectArray(input));
        }
        public static Char[] DeserializeCharArray(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            Char[] result = br.ReadChars(br.ReadInt32());
            ms.Close();

            return result;
        }
        public static Char[] DeserializeCharArray(Object[] input)
        {
            return DeserializeCharArray(DeserializeObjectArray(input));
        }
        public static Decimal DeserializeDecimal(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            Decimal result = br.ReadDecimal();
            ms.Close();

            return result;
        }
        public static Decimal DeserializeDecimal(Object[] input)
        {
            return DeserializeDecimal(DeserializeObjectArray(input));
        }
        public static Decimal[] DeserializeDecimalArray(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            Decimal[] result = new Decimal[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadDecimal();
            }
            ms.Close();

            return result;
        }
        public static Decimal[] DeserializeDecimalArray(Object[] input)
        {
            return DeserializeDecimalArray(DeserializeObjectArray(input));
        }
        public static Double DeserializeDouble(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            Double result = br.ReadDouble();
            ms.Close();

            return result;
        }
        public static Double DeserializeDouble(Object[] input)
        {
            return DeserializeDouble(DeserializeObjectArray(input));
        }
        public static Double[] DeserializeDoubleArray(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            Double[] result = new Double[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadDouble();
            }
            ms.Close();

            return result;
        }
        public static Double[] DeserializeDoubleArray(Object[] input)
        {
            return DeserializeDoubleArray(DeserializeObjectArray(input));
        }
        public static Int16 DeserializeInt16(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            Int16 result = br.ReadInt16();
            ms.Close();

            return result;
        }
        public static Int16 DeserializeInt16(Object[] input)
        {
            return DeserializeInt16(DeserializeObjectArray(input));
        }
        public static Int16[] DeserializeInt16Array(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            Int16[] result = new Int16[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadInt16();
            }
            ms.Close();

            return result;
        }
        public static Int16[] DeserializeInt16Array(Object[] input)
        {
            return DeserializeInt16Array(DeserializeObjectArray(input));
        }
        public static Int32 DeserializeInt32(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            Int32 result = br.ReadInt32();
            ms.Close();

            return result;
        }
        public static Int32 DeserializeInt32(Object[] input)
        {
            return DeserializeInt32(DeserializeObjectArray(input));
        }
        public static Int32[] DeserializeInt32Array(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            Int32[] result = new Int32[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadInt32();
            }
            ms.Close();

            return result;
        }
        public static Int32[] DeserializeInt32Array(Object[] input)
        {
            return DeserializeInt32Array(DeserializeObjectArray(input));
        }
        public static Int64 DeserializeInt64(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            Int64 result = br.ReadInt64();
            ms.Close();

            return result;
        }
        public static Int64 DeserializeInt64(Object[] input)
        {
            return DeserializeInt64(DeserializeObjectArray(input));
        }
        public static Int64[] DeserializeInt64Array(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            Int64[] result = new Int64[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadInt64();
            }
            ms.Close();

            return result;
        }
        public static Int64[] DeserializeInt64Array(Object[] input)
        {
            return DeserializeInt64Array(DeserializeObjectArray(input));
        }
        public static SByte DeserializeSByte(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            SByte result = br.ReadSByte();
            ms.Close();

            return result;
        }
        public static SByte DeserializeSByte(Object[] input)
        {
            return DeserializeSByte(DeserializeObjectArray(input));
        }
        public static SByte[] DeserializeSByteArray(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            SByte[] result = new SByte[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadSByte();
            }
            ms.Close();

            return result;
        }
        public static SByte[] DeserializeSByteArray(Object[] input)
        {
            return DeserializeSByteArray(DeserializeObjectArray(input));
        }
        public static Single DeserializeSingle(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            Single result = br.ReadSingle();
            ms.Close();

            return result;
        }
        public static Single DeserializeSingle(Object[] input)
        {
            return DeserializeSingle(DeserializeObjectArray(input));
        }
        public static Single[] DeserializeSingleArray(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            Single[] result = new Single[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadSingle();
            }
            ms.Close();

            return result;
        }
        public static Single[] DeserializeSingleArray(Object[] input)
        {
            return DeserializeSingleArray(DeserializeObjectArray(input));
        }
        public static String DeserializeString(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            String result = br.ReadString();
            ms.Close();

            return result;
        }
        public static String DeserializeString(Object[] input)
        {
            return DeserializeString(DeserializeObjectArray(input));
        }
        public static String[] DeserializeStringArray(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            String[] result = new String[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadString();
            }
            ms.Close();

            return result;
        }
        public static String[] DeserializeStringArray(Object[] input)
        {
            return DeserializeStringArray(DeserializeObjectArray(input));
        }
        public static UInt16 DeserializeUInt16(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            UInt16 result = br.ReadUInt16();
            ms.Close();

            return result;
        }
        public static UInt16 DeserializeUInt16(Object[] input)
        {
            return DeserializeUInt16(DeserializeObjectArray(input));
        }
        public static UInt16[] DeserializeUInt16Array(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            UInt16[] result = new UInt16[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadUInt16();
            }
            ms.Close();

            return result;
        }
        public static UInt16[] DeserializeUInt16Array(Object[] input)
        {
            return DeserializeUInt16Array(DeserializeObjectArray(input));
        }
        public static UInt32 DeserializeUInt32(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            UInt32 result = br.ReadUInt32();
            ms.Close();

            return result;
        }
        public static UInt32 DeserializeUInt32(Object[] input)
        {
            return DeserializeUInt32(DeserializeObjectArray(input));
        }
        public static UInt32[] DeserializeUInt32Array(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            UInt32[] result = new UInt32[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadUInt32();
            }
            ms.Close();

            return result;
        }
        public static UInt32[] DeserializeUInt32Array(Object[] input)
        {
            return DeserializeUInt32Array(DeserializeObjectArray(input));
        }
        public static UInt64 DeserializeUInt64(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            UInt64 result = br.ReadUInt64();
            ms.Close();

            return result;
        }
        public static UInt64 DeserializeUInt64(Object[] input)
        {
            return DeserializeUInt64(DeserializeObjectArray(input));
        }
        public static UInt64[] DeserializeUInt64Array(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            int length = br.ReadInt32();

            UInt64[] result = new UInt64[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = br.ReadUInt64();
            }
            ms.Close();

            return result;
        }
        public static UInt64[] DeserializeUInt64Array(Object[] input)
        {
            return DeserializeUInt64Array(DeserializeObjectArray(input));
        }
        #endregion

        [Obsolete("Use Int64 version")]
        public static long DeserializeLong(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);

            long result = br.ReadInt64();
            ms.Close();

            return result;
        }
        [Obsolete("Use Int64 version")]
        public static long DeserializeLong(Object[] input)
        {
            return DeserializeLong(DeserializeObjectArray(input));
        }

        public static float[,] DeserializeFloat(Byte[] input)
        {
            MemoryStream stream = byteArrayToStream(input);
            BinaryReader reader = new BinaryReader(stream);

            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            float[,] result = new float[width, height];

            int counter = 0;
            while (stream.Position < stream.Length)
            {
                /** Calculate the indexes **/
                int i = (int)counter/width;
                int j = counter % height;
                result[i, j] = reader.ReadSingle();
                counter++;
            }

            reader.Close();
            stream.Close();
            
            return result;
        }
        public static GridStrategy DeserializeGridStrategy(Byte[] input)
        {
            System.IO.MemoryStream stream = byteArrayToStream(input);
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            String nameofStrategy = reader.ReadString();
            GridStrategy result;
            switch (nameofStrategy)
            {
                case "CreepStrategy":
                    result = new CreepStrategy();
                    break;
                case "FlowStrategy":
                    result = new FlowStrategy();
                    break;
                case "ThunderStrategy":
                    result = new ThunderStrategy();
                    break;
                default:
                    throw new NotSupportedException();
            }

            reader.Close();
            stream.Close();

            return result;
        }
        public static BallUnit DeserializeBallUnit(Byte[] input)
        {
            BallUnit bu = new BallUnit();
            bu.Deserialize(byteArrayToStream(input));
            return bu;
        }
        public static CheckedState[,] DeserializeCheckedState(Byte[] input)
        {
            MemoryStream ms = byteArrayToStream(input);
            BinaryReader br = new BinaryReader(ms);
            CheckedState[,] result = new CheckedState[3, 3];
            int counter=0;
            while (ms.Position < ms.Length)
            {
                int i = counter / 3;
                int j = counter % 3;

                CheckedState cs = new CheckedState();
                cs.CurrentState = (CheckedState.States)br.ReadInt32();
                result[i, j] = cs;

                counter++;
            }
            return result;
        }
        public static CheckedState[,] DeserializeCheckedState(Object[] input)
        {
            return DeserializeCheckedState(DeserializeObjectArray(input));
        }

        public static Byte[] DeserializeObjectArray(Object[] input)
        {
            Byte[] result = new Byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = Byte.Parse(input[i].ToString());
            }
            return result;
        }

        #endregion

        #region Helper Methods

        private static Byte[] streamTobyteArray(MemoryStream input)
        {
            byte[] bytes = new byte[input.Length];
            input.Seek(0, SeekOrigin.Begin);

            input.Read(bytes, 0, (int)input.Length); // Make sure stream isn't too long, might want to check Length vs. int.MaxValue

            return bytes;
        }
        private static MemoryStream byteArrayToStream(Byte[] input)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(input, 0, input.Length); // Make sure stream isn't too long, might want to check Length vs. int.MaxValue
            stream.Seek(0, System.IO.SeekOrigin.Begin);

            return stream;
        }

        #endregion
        
    }
}
