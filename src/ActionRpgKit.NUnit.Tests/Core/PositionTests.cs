using System;
using NUnit.Framework;
using ActionRpgKit.Core;

namespace ActionRpgKit.Tests.Core
{

    [TestFixture]
    [Category("Core")]
    public class PositionTests
    {
        Position position1;
        Position position2;

        [SetUp]
        public void SetUp()
        {
            position1 = new Position();
            position2 = new Position();
        }

        [Test]
        public void ManipulatePositionTest()
        {
            position1.Set(1, 2);
            Assert.AreEqual(position1.X, 1);
            Assert.AreEqual(position1.Y, 2);
        }

        [Test]
        public void DistanceTest()
        {
            position1.Set(0, 0);
            position2.Set(1.23456789f, 0);
            Assert.AreEqual((float)Math.Pow(1.23456789f, 2), position1.SquaredDistanceTo(position2));
            position2.Set(-1.23456789f, 0);
            Assert.AreEqual((float)Math.Pow(1.23456789f, 2), position2.SquaredDistanceTo(position1));
            Assert.AreEqual(new int[]{0, 0}, position1.Get());
            Assert.AreEqual("0, 0", position1.ToString());
        }
    }
 }
