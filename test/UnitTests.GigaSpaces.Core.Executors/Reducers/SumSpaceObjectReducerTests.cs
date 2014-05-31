using GigaSpaces.Core.Executors;
using GigaSpaces.Core.Executors.Reducers;
using NUnit.Framework;
using UnitTests.GigaSpaces.Core.Executors.Utilities;

namespace UnitTests.GigaSpaces.Core.Executors.Reducers
{
    [TestFixture]
    public class SumSpaceObjectReducerTests
    {
        [Test]
        public void IsReducerSerializable()
        {
            SerializationUtilities.AssertObjectIsSerializable(new SumSpaceObjectReducer<SumSpaceObjectReducerTests>());
        }

        [Test]
        public void SumSAllTaskResults()
        {
            // Arrange
            var underTest = new SumSpaceObjectReducer<SumSpaceObjectReducerTests>();

            // Act
            var actual = underTest.Reduce(new SpaceTaskResultsCollection<long>
            {
                new SpaceTaskResult<long>(1, null),
                new SpaceTaskResult<long>(6, null),
                new SpaceTaskResult<long>(2, null),
            });

            // Assert
            Assert.AreEqual(9, actual);
        }
    }
}
