using System;
using NUnit.Framework;
using ActionRpgKit.Character;
using ActionRpgKit.Core;

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
            // Retrieve an enemy and release it back to the pool
            var enemy = enemyPool.Acquire();
            enemyPool.Release(enemy);
            enemyPool.Acquire();
            Assert.AreEqual(1, enemyPool.Size);
            
            // Retrieve more enemies than the pool has to offer, will increase the size.
            for(int i=0; i < 2; i++)
            {
                enemyPool.Acquire();
            }
            Assert.AreEqual(2, enemyPool.Size);
            
        }
    }
}
