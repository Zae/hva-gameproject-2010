using System;
using System.IO;

namespace ION
{
    public abstract class Serializable
    {
        public virtual MemoryStream Serialize();
        public virtual void Deserialize(MemoryStream inData);
    }
}
