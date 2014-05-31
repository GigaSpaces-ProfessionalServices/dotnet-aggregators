using GigaSpaces.Core;
using GigaSpaces.Core.Executors.Tasks;
using Moq;
using NUnit.Framework;
using UnitTests.GigaSpaces.Core.Executors.Utilities;

namespace UnitTests.GigaSpaces.Core.Executors.Tasks
{
    [TestFixture]
    public class SumSpaceObjectTaskTests
    {
        [Test]
        public void IsTaskSerializable()
        {
            SerializationUtilities.AssertObjectIsSerializable(new SumSpaceObjectTask<SumSpaceObjectTaskTests>());
        }

        [Test]
        public void SumsSpaceObjectCount()
        {
            // Arrange
            var spaceProxy = new Mock<ISpaceProxy>();
            var underTest = new SumSpaceObjectTask<SumSpaceObjectTaskTests>();

            spaceProxy.Setup(m => m.ReadMultiple<SumSpaceObjectTaskTests>(It.IsAny<SqlQuery<SumSpaceObjectTaskTests>>()))
                .Returns(new[]
                {new SumSpaceObjectTaskTests(), new SumSpaceObjectTaskTests(), new SumSpaceObjectTaskTests()});

            // Act
            var actual = underTest.Execute(spaceProxy.Object, null);

            // Assert
            Assert.AreEqual(3, actual);
        }
    }
}