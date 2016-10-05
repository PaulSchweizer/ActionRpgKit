using System;
using NUnit.Framework;
using ActionRpgKit.Character;

namespace ActionRpgKit.Tests.Character
{
    
    [TestFixture]
    [Category("Character.Character")]
    class EnemyPoolTests
    {
        EnemyPool enemyPool;

        [SetUp]
        public void SetUp()
        {
            enemyPool = new EnemyPool(size: 1);
            GameTime.Reset();
        }
        
        [Test]
        public void TestSize()
        {
            // Retrieve more enemies than the pool has to offer, will increase the size.
            for(int i=0; i < 2; i++)
            {
                enemyPool.Acquire();
            }
            Assert.AreEqual(2, enemyPool.size);
        }
    }
}
