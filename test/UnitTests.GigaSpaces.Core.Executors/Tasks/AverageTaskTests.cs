using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GigaSpaces.Core;
using GigaSpaces.Core.Executors.Tasks;
using Moq;
using NUnit.Framework;
using UnitTests.GigaSpaces.Core.Executors.Utilities;

namespace UnitTests.GigaSpaces.Core.Executors.Tasks
{
    [TestFixture]
    public class AverageTaskTests
    {
        [Test]
        public void TheTaskIsSerializable()
        {
            SerializationUtilities.AssertObjectIsSerializable(new AverageTask<TestObject, int>(o => o.IntegerProperty));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsWhenExpressionIsForField()
        {
            new AverageTask<TestObject, int>(o => o.Field);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsWhenExpressionIsForMethod()
        {
            new AverageTask<TestObject, int>(o => o.AMethod());
        }

        [Test]
        public void CalculatesAverageCorrectly()
        {
            // Arrange
            var mockedSpaceProxy = new Mock<ISpaceProxy>();
            var underTest = new AverageTask<TestObject, int>(o => o.IntegerProperty);

            mockedSpaceProxy.Setup(m => m.ReadMultiple<TestObject>(It.IsAny<SqlQuery<TestObject>>()))
                .Returns(new[] {new TestObject {IntegerProperty = 1}, new TestObject {IntegerProperty = 2}, new TestObject {IntegerProperty = 3}});

            // Act
            var actual = underTest.Execute(mockedSpaceProxy.Object, null);

            // Assert
            Assert.AreEqual(2, actual);
        }          
        
        [Test]
        public void MaxIntegersDoNotThrowDuringSum()
        {
            // Arrange
            var mockedSpaceProxy = new Mock<ISpaceProxy>();
            var underTest = new AverageTask<TestObject, int>(o => o.IntegerProperty);

            mockedSpaceProxy.Setup(m => m.ReadMultiple<TestObject>(It.IsAny<SqlQuery<TestObject>>()))
                .Returns(new[] {new TestObject {IntegerProperty = int.MaxValue}, new TestObject {IntegerProperty = int.MaxValue }, new TestObject {IntegerProperty = int.MaxValue}});

            // Act
            var actual = underTest.Execute(mockedSpaceProxy.Object, null);

            // Assert
            Assert.AreEqual(int.MaxValue, actual);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void NonIntegralTypesreturnZero()
        {
            // Arrange
            var mockedSpaceProxy = new Mock<ISpaceProxy>();
            var underTest = new AverageTask<TestObject, string>(o => o.StringProperty);

            mockedSpaceProxy.Setup(m => m.ReadMultiple<TestObject>(It.IsAny<SqlQuery<TestObject>>()))
                .Returns(new[] { new TestObject { StringProperty = "foo" }, new TestObject { StringProperty = "bar" } });

            // Act
            underTest.Execute(mockedSpaceProxy.Object, null);
        }

        private class TestObject
        {
            public int IntegerProperty { get; set; }

            public string StringProperty { get; set; }

            public int AMethod()
            {
                return 0;
            }

            public int Field;
        }
    }
}
