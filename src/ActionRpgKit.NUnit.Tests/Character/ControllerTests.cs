using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using System.Collections.Generic;

namespace ActionRpgKit.NUnit.Tests.Character
{
    [TestFixture]
    [Category("Character")]
    class ControllerTests
    {
        Player player;
        Enemy[] enemies;

        [SetUp]
        public void SetUp()
        {
            Controller.Enemies = new List<Enemy>();
            player = new Player("Player");
            enemies = new Enemy[1000];
            for (int i=0; i < 1000; i++)
            {
                enemies[i] = new Enemy(string.Format("Enemy{0}", i));
                enemies[i].Position.Set(i, 0);
            }
        }

        [Test]
        public void RegisteringTest()
        {
            player.Stats.AlertnessRange.Value = 10*10;
            Assert.AreEqual(1000, Controller.Enemies.Count);
            Controller.Update();
            Assert.AreEqual(11, player.Enemies.Count);
        }

    }
}
