using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnitTests.GigaSpaces.Core.Executors.Utilities
{
    public static class SerializationUtilities
    {
        public static void AssertObjectIsSerializable(object underTest)
        {
            var formatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, underTest);
            }
        }
    }
}