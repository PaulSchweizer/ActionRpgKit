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
            charStats = new Character.Stats(10, 0, 0);
        }

        [Test]
        public void PrimaryAttributeTest()
        {
        }
    }
}
