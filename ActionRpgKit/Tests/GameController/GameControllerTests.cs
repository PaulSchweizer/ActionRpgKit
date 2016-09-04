using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Skill;

namespace ActionRpgKit.Tests.GameController
{
    [TestFixture]
    [Category("GameController")]
    public class GameControllerTests
    {

        Player player;

        ICombatSkill meleeSkill;
        IMagicSkill passiveMagicSkill;

        [SetUp]
        public void SetUp()
        {
            GameTime.Reset();

            meleeSkill = new MeleeSkill(id: 1,
                                        name: "SwordFighting",
                                        description: "How to wield a sword.",
                                        preUseTime: 1,
                                        cooldownTime: 1,
                                        damage: 1,
                                        maximumTargets: 1);
            passiveMagicSkill = new PassiveMagicSkill(id: 0,
                                           name: "ShadowStrength",
                                           description: "A +10 Buff to the user's strength.",
                                           cost: 10,
                                           duration: 10,
                                           preUseTime: 10,
                                           cooldownTime: 5,
                                           modifierValue: 10,
                                           modifiedAttributeName: "Body");
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
            player.Stats.Body.Reset();
            player.Stats.Life.Reset();
            player.Stats.Magic.Reset();
            player.LearnCombatSkill(meleeSkill);
            player.LearnMagicSkill(passiveMagicSkill);
            Console.WriteLine(player.ToString());
        }
    }
}
