using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GigaSpaces.Core;
using GigaSpaces.Core.Executors;
using GigaSpaces.Core.Executors.Reducers;
using Moq;
using NUnit.Framework;
using UnitTests.GigaSpaces.Core.Executors.Utilities;

namespace UnitTests.GigaSpaces.Core.Executors.Reducers
{
    [TestFixture]
    public class SumReducerTests
    {
        [Test]
        public void TaskIsSerializable()
        {
            SerializationUtilities.AssertObjectIsSerializable(new SumReducer<TestData, int>(d => d.IntegerProperty));
        }

        [Test]
        public void SumsAllData()
        {
            // Arrange
            var underTest = new SumReducer<TestData, int>(d => d.IntegerProperty);
            var mockedSpaceProxy = new Mock<ISpaceProxy>();
            mockedSpaceProxy.Setup(m => m.ReadMultiple<TestData>(It.IsAny<SqlQuery<TestData>>()))
                .Returns(new[] { new TestData { IntegerProperty = 2 }, new TestData { IntegerProperty = 5 } });

            // Act
            var actual = underTest.Reduce(new SpaceTaskResultsCollection<long>()
            {
                new SpaceTaskResult<long>(2, null),
                new SpaceTaskResult<long>(5, null),
            });

            // Assert
            Assert.AreEqual(7, actual);
        }

        private class TestData
        {
            public int IntegerProperty { get; set; }
        }
    }
}
