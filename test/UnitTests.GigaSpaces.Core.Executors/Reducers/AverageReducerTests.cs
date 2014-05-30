using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GigaSpaces.Core.Executors;
using GigaSpaces.Core.Executors.Reducers;
using GigaSpaces.Core.Executors.Tasks;
using NUnit.Framework;
using UnitTests.GigaSpaces.Core.Executors.Tasks;

namespace UnitTests.GigaSpaces.Core.Executors.Reducers
{
    [TestFixture]
    public class AverageReducerTests
    {
        [Test]
        public void TheReducerIsSerializable()
        {
            var formatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, new AverageReducer<TestData, int>(o => o.IntegerProperty));
            }
        }

        [Test]
        public void CorrectlyAveragesTaskResult()
        {
            // Arrange
            var underTest = new AverageReducer<TestData, int>(o => o.IntegerProperty);

            // Act
            var actual = underTest.Reduce(new SpaceTaskResultsCollection<long>()
            {
                new SpaceTaskResult<long>(1, null),
                new SpaceTaskResult<long>(2, null),
                new SpaceTaskResult<long>(3, null),
            });

            // Assert
            Assert.AreEqual(2, actual);
        }

        private class TestData
        {
            public int IntegerProperty { get; set; }
        }
    }

}