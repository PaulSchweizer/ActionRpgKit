using System;
using NUnit.Framework;
using ActionRpgKit.Core;

namespace ActionRpgKit.Tests.Core
{

    [TestFixture]
    [Category("Core")]
    public class PositionTests
    {
        Position position;

        [SetUp]
        public void SetUp()
        {
            position = new Position();
        }

        [Test]
        public void ManipulatePosition()
        {
            position.Set(1, 2, 3);
            Assert.AreEqual(position.X, 1);
            Assert.AreEqual(position.Y, 2);
            Assert.AreEqual(position.Z, 3);
        }
    }
 }
