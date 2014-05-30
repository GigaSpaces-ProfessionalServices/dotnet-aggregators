using GigaSpaces.Core;
using GigaSpaces.Core.Executors.Tasks;
using Moq;
using NUnit.Framework;
using UnitTests.GigaSpaces.Core.Executors.Utilities;

namespace UnitTests.GigaSpaces.Core.Executors.Tasks
{
    [TestFixture]
    public class MaximumTaskTests
    {
        private class TestData
        {
            public int IntegerProperty { get; set; }
        }

        [Test]
        public void TaskIsSerializable()
        {
            SerializationUtilities.AssertObjectIsSerializable(new MaximumTask<TestData, int>(d => d.IntegerProperty));
        }

        [Test]
        public void MaximumIsSelectedWhenFirstInList()
        {
            // Arrange
            var underTest = new MaximumTask<TestData, int>(d => d.IntegerProperty);
            var mockedSpaceProxy = new Mock<ISpaceProxy>();
            mockedSpaceProxy.Setup(m => m.ReadMultiple<TestData>(It.IsAny<SqlQuery<TestData>>()))
                .Returns(new[] {new TestData {IntegerProperty = 5}, new TestData {IntegerProperty = 2}});

            // Act
            var actual = underTest.Execute(mockedSpaceProxy.Object, null);

            // Assert
            Assert.AreEqual(5, actual);
        }

        [Test]
        public void MaximumIsSelectedWhenLastInList()
        {
            // Arrange
            var underTest = new MaximumTask<TestData, int>(d => d.IntegerProperty);
            var mockedSpaceProxy = new Mock<ISpaceProxy>();
            mockedSpaceProxy.Setup(m => m.ReadMultiple<TestData>(It.IsAny<SqlQuery<TestData>>()))
                .Returns(new[] { new TestData { IntegerProperty = 2 }, new TestData { IntegerProperty = 5 } });

            // Act
            var actual = underTest.Execute(mockedSpaceProxy.Object, null);

            // Assert
            Assert.AreEqual(5, actual);
        }

        [Test]
        public void SubZeroValuesActuallyModifyResult()
        {
            // Arrange
            var underTest = new MaximumTask<TestData, int>(d => d.IntegerProperty);
            var mockedSpaceProxy = new Mock<ISpaceProxy>();
            mockedSpaceProxy.Setup(m => m.ReadMultiple<TestData>(It.IsAny<SqlQuery<TestData>>()))
                .Returns(new[] { new TestData { IntegerProperty = -2 }, new TestData { IntegerProperty = -5 } });

            // Act
            var actual = underTest.Execute(mockedSpaceProxy.Object, null);

            // Assert
            Assert.AreEqual(-2, actual); 
        }
    }
}