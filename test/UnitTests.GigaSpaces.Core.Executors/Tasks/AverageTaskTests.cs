using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GigaSpaces.Core.Executors.Tasks;
using NUnit.Framework;

namespace UnitTests.GigaSpaces.Core.Executors.Tasks
{
    [TestFixture]
    public class AverageTaskTests
    {
        [Test]
        public void TheTaskIsSerializable()
        {
            var formatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, new AverageTask());
            }
        }
    }
}
