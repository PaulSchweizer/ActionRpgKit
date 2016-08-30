using System;
using NUnit.Framework;

namespace ActionRpgKit.Tests.Core
{
    [TestFixture]
    [Category("Core")]
    public class GameTimeTests
    {
        [SetUp]
        public void SetUp()
        {
            GameTime.Reset();
        }

        [Test]
        public void TimeTest ()
        {
            for(int i=1; i < 999; i++)
            {
                GameTime.time = i;
                Assert.AreEqual(i, GameTime.time);
                Assert.AreEqual(1, GameTime.deltaTime);
            }
            GameTime.Reset();
            Assert.AreEqual(0, GameTime.time);
            Assert.AreEqual(0, GameTime.deltaTime);
        }
    }
}
