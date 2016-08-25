using System;
using NUnit.Framework;

namespace CharacterTests
{

    [TestFixture]
    [Category("Character.Stats")]
    public class CharacterStatsTests
    {
        Character.Stats charStats;

        [SetUp]
        public void SetUp ()                                                  
        {
            charStats = new Character.Stats();
        }

        [Test]
        public void PrimaryAttributeTest()
        {
            Assert.AreEqual(charStats, 0);
        }
    }
}
