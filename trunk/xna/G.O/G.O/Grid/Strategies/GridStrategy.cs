using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ION.GridStrategies
{
    public abstract class GridStrategy : Serializable
    {
        public string name;
        public int speed = 0;

        public GridStrategy()
        {
            name = "GridStrategy";
        }

        public abstract void reset();

        public abstract void draw();

        public abstract void update(int ellapsed);

        public abstract void increaseSpeed();
        public abstract void decreaseSpeed();

        public MemoryStream Serialize()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(this.name);
            writer.Close();
            return stream;
        }
        public void Deserialize(MemoryStream inData)
        {
            throw new NotImplementedException();
        }
    }
}
