using System;
using ION.GridStrategies;
using System.IO;

namespace ION
{
    public class Serializer
    {
        #region Serialize

        public static Byte[] Serialize(String[] input)
        {
            throw new NotImplementedException();
        }
        public static Byte[] Serialize(float[] input)
        {
            throw new NotImplementedException();
        }
        public static Byte[] Serialize(float[,] input)
        {
            throw new NotImplementedException();
        }
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
        public static Byte[] Serialize(Serializable input)
        {
            MemoryStream stream = input.Serialize();
            Byte[] byteArray = streamTobyteArray(stream);
            stream.Close();
            return byteArray;
        }

        #endregion

        #region Deserialize

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
                case "BleedStrategy":
                    result = new BleedStrategy();
                    break;
                case "CreepStrategy":
                    result = new CreepStrategy();
                    break;
                case "FlowStrategy":
                    result = new FlowStrategy();
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
