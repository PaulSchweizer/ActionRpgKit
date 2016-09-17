using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;

namespace ActionRpgKit.Tests.Character
{
    [TestFixture]
    [Category("Character.Character")]
    class CharacterTests
    {
        Player player;
        Enemy enemy;

        [SetUp]
        public void SetUp()
        {
            player = new Player();
            enemy = new Enemy();
            GameTime.Reset();
        }

        [Test]
        public void InitialStateTest()
        {
            Assert.IsTrue(player.CurrentState is IdleState);
        }
    }
}
