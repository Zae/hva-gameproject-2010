using System.IO;

namespace ION
{
    /// <summary>
    /// All object that implement this interface can be Serialized by Serializer
    /// </summary>
    /// <seealso cref="Serializer"/>
    public interface Serializable
    {
        MemoryStream Serialize();
        void Deserialize(MemoryStream inData);
    }
}
