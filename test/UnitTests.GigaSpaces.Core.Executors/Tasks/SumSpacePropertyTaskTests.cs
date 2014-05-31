using GigaSpaces.Core;
using GigaSpaces.Core.Executors.Tasks;
using Moq;
using NUnit.Framework;
using UnitTests.GigaSpaces.Core.Executors.Utilities;

namespace UnitTests.GigaSpaces.Core.Executors.Tasks
{
    [TestFixture]
    public class SumSpacePropertyTaskTests
    {
        [Test]
        public void TaskIsSerializable()
        {
            SerializationUtilities.AssertObjectIsSerializable(new SumSpacePropertyTask<TestData, int>(d => d.IntegerProperty));
        }

        [Test]
        public void SumsAllData()
        {
            // Arrange
            var underTest = new SumSpacePropertyTask<TestData, int>(d => d.IntegerProperty);
            var mockedSpaceProxy = new Mock<ISpaceProxy>();
            mockedSpaceProxy.Setup(m => m.ReadMultiple<TestData>(It.IsAny<SqlQuery<TestData>>()))
                .Returns(new[] { new TestData { IntegerProperty = 2 }, new TestData { IntegerProperty = 5 } });

            // Act
            var actual = underTest.Execute(mockedSpaceProxy.Object, null);

            // Assert
            Assert.AreEqual(7, actual);
        }

        private class TestData
        {
            public int IntegerProperty { get; set; }
        }
    }
}
