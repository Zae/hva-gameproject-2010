using System.IO;

namespace ION
{
    /// <summary>
    /// All object that implement this interface can be Serialized by Serializer
    /// </summary>
    /// <seealso cref="Serializer"/>
    public interface Serializable
    {
        /// <summary>
        /// Serialize your custom object into a bytearray through the <see cref="Serializer">Serializer</see>
        /// </summary>
        /// <returns>A Memorystream with the to be serialized data.</returns>
        MemoryStream Serialize();
        /// <summary>
        /// Deserialize your custom object into a bytearray through the <see cref="Serializer">Serializer</see>
        /// </summary>
        /// <param name="inData">A Memorystream with the to be deserialized data.</param>
        void Deserialize(MemoryStream inData);
    }
}
