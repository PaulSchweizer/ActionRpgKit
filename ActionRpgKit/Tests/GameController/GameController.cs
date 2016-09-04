using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;

namespace ActionRpgKit.Tests.GameController
{
    [TestFixture]
    [Category("GameController")]
    public class GameControllerTests
    {

        Player player;

        [SetUp]
        public void SetUp()
        {
            GameTime.Reset();
        }

        [Test]
        public void RunGameTest ()
        {
            CreatePlayerCharacter();
        }

        private void CreatePlayerCharacter ()
        {
            player = new Player();
            player.Name = "John";
            player.Stats.Body.Value = 20;
            player.Stats.Mind.Value = 10;
            player.Stats.Soul.Value = 5;
        }
    }
}
