using GigaSpaces.Core;
using GigaSpaces.Core.Executors.Tasks;
using Moq;
using NUnit.Framework;
using UnitTests.GigaSpaces.Core.Executors.Utilities;

namespace UnitTests.GigaSpaces.Core.Executors.Tasks
{
    [TestFixture]
    public class MinimumTaskTests
    {
        private class TestData
        {
            public int IntegerNumber { get; set; }
        }

        [Test]
        public void TaskIsSerializable()
        {
            SerializationUtilities.AssertObjectIsSerializable(new MinimumTask<TestData, int>(d => d.IntegerNumber));
        }

        [Test]
        public void MimimumIsSelectedWhenFirstInList()
        {
            // Arrange
            var underTest = new MinimumTask<TestData, int>(d => d.IntegerNumber);
            var mockedSpaceProxy = new Mock<ISpaceProxy>();
            mockedSpaceProxy.Setup(m => m.ReadMultiple<TestData>(It.IsAny<SqlQuery<TestData>>()))
                .Returns(new[] { new TestData { IntegerNumber = 2 }, new TestData { IntegerNumber = 4 } });

            // Act
            var actual = underTest.Execute(mockedSpaceProxy.Object, null);

            // Assert
            Assert.AreEqual(2, actual);
        }

        [Test]
        public void MinimumIsSelectedWhenLastInList()
        {
            // Arrange
            var underTest = new MinimumTask<TestData, int>(d => d.IntegerNumber);
            var mockedSpaceProxy = new Mock<ISpaceProxy>();
            mockedSpaceProxy.Setup(m => m.ReadMultiple<TestData>(It.IsAny<SqlQuery<TestData>>()))
                .Returns(new[] { new TestData { IntegerNumber = 5 }, new TestData { IntegerNumber = 2 } });

            // Act
            var actual = underTest.Execute(mockedSpaceProxy.Object, null);

            // Assert
            Assert.AreEqual(2, actual);
        }
    }
}