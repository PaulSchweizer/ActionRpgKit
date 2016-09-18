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
        public void StatesTest()
        {
            // Initial State
            Assert.IsTrue(player.CurrentState is IdleState);

            // Add Enemy switches to alert state
            player.AddEnemy(enemy);
            player.CurrentState.UpdateState(player);
            Assert.IsTrue(player.CurrentState is AlertState);

            // No more Enemy switches to idle state
            player.RemoveEnemy(enemy);
            player.CurrentState.UpdateState(player);
            Assert.IsTrue(player.CurrentState is AlertState);

        }
    }
}
