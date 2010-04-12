using System;
using System.IO;

namespace ION
{
    public abstract class Serializable
    {
        public virtual MemoryStream Serialize()
        {
            return new MemoryStream();
        }
        public virtual void Deserialize(MemoryStream inData)
        {

        }
    }
}
