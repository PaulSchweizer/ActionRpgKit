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

        [SetUp]
        public void SetUp()
        {
            EnemyPool.Initialize(size: 1);
            GameTime.Reset();
        }
        
        [Test]
        public void TestSize()
        {
            // Retrieve an enemy and release it back to the pool
            var enemy = EnemyPool.Acquire();
            EnemyPool.Release(enemy);
            Assert.AreEqual(1, EnemyPool.Size);

            // Retrieve more enemies than the pool has to offer will 
            // increase the size.
            for(int i=0; i < 2; i++)
            {
                EnemyPool.Acquire();
            }
            Assert.AreEqual(2, EnemyPool.Size);
            
        }
    }
}
