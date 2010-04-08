using System;

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
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);

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

            byte[] byteArray = new byte[stream.Length];
            stream.Seek(0, System.IO.SeekOrigin.Begin);

            stream.Read(byteArray, 0, (int)stream.Length); // Make sure stream isn't too long, might want to check Length vs. int.MaxValue
            stream.Close();

            return byteArray;
        }
        #endregion

        #region Deserialize
        public static float[,] Deserialize(Byte[] input)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            stream.Write(input, 0, input.Length); // Make sure stream isn't too long, might want to check Length vs. int.MaxValue
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

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
        #endregion
    }
}
