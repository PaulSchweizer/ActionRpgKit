using System;
using NUnit.Framework;
using ActionRpgKit.Character;

namespace ActionRpgKit.Tests.Character
{
    
    [TestFixture]
    [Category("Character.Character")]
    class CharacterTests
    {
        EnemyPool enemyPool;

        [SetUp]
        public void SetUp()
        {
            enemyPool = new EnemyPool();
            GameTime.Reset();
        }
    }
}
