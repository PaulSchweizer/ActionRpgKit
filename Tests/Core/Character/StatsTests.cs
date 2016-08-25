using System;
using NUnit.Framework;

namespace CharacterTests
{

    [TestFixture]
    [Category("Character.Stats")]
    public class CharacterStatsTests
    {
        Character.EnemyStats enemyStats;

        [SetUp]
        public void SetUp ()                                                  
        {
            enemyStats = new Character.EnemyStats(10, 20, 30);
            GameTime.time = 0f;
        }

        [Test]
        public void PrimaryAttributeTest()
        {

        }
    }
}
