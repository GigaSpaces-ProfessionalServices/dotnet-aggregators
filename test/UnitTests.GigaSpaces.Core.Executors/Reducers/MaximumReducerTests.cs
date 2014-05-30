using GigaSpaces.Core.Executors;
using GigaSpaces.Core.Executors.Reducers;
using NUnit.Framework;
using UnitTests.GigaSpaces.Core.Executors.Utilities;

namespace UnitTests.GigaSpaces.Core.Executors.Reducers
{
    [TestFixture]
    public class MaximumReducerTests
    {
        [Test]
        public void ReducerIsSerializable()
        {
            SerializationUtilities.AssertObjectIsSerializable(new MaximumReducer<TestData, int>(d => d.IntegerProperty));
        }

        [Test]
        public void MaximumIsSelectedWhenFirstInList()
        {
            // Arrange
            var underTest = new MaximumReducer<TestData, int>(d => d.IntegerProperty);

            // Act
            var actual = underTest.Reduce(new SpaceTaskResultsCollection<long>()
            {
                new SpaceTaskResult<long>(5, null),
                new SpaceTaskResult<long>(2, null),
                new SpaceTaskResult<long>(3, null),
            });

            // Assert
            Assert.AreEqual(5, actual);
        }

        [Test]
        public void MaximumIsSelectedWhenLastInList()
        {
            // Arrange
            var underTest = new MaximumReducer<TestData, int>(d => d.IntegerProperty);

            // Act
            var actual = underTest.Reduce(new SpaceTaskResultsCollection<long>()
            {
                new SpaceTaskResult<long>(2, null),
                new SpaceTaskResult<long>(2, null),
                new SpaceTaskResult<long>(5, null),
            });

            // Assert
            Assert.AreEqual(5, actual);
        }


        [Test]
        public void SubZeroValuesActuallyModifyResult()
        {
            // Arrange
            var underTest = new MaximumReducer<TestData, int>(d => d.IntegerProperty);

            // Act
            var actual = underTest.Reduce(new SpaceTaskResultsCollection<long>()
            {
                new SpaceTaskResult<long>(-2, null),
                new SpaceTaskResult<long>(-5, null),
            });

            // Assert
            Assert.AreEqual(-2, actual);
        }

        private class TestData
        {
            public int IntegerProperty { get; set; }
        }
    }
}