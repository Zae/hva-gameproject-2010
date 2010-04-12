using System;
using System.IO;

namespace ION
{
    public interface Serializable
    {
        MemoryStream Serialize();
        void Deserialize(MemoryStream inData);
    }
}
