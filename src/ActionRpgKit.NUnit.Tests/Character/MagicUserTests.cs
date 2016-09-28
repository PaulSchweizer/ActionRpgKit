using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Item;

namespace ActionRpgKit.Tests.Character
{
    [TestFixture]
    [Category("Character.MagicUser")]
    public class MagicUserTests
    {

        Player player;
        Enemy enemy;
        IItem herb;
        IItem coin;

        IMagicSkill passiveMagicSkill;

        [SetUp]
        public void SetUp ()
        {
            herb = new UsableItem();
            herb.Id = 0;
            herb.Name = "Herb";
            herb.Description = "A common herb";
            coin = new UsableItem();
            coin.Id = 0;
            coin.Name = "Coin";
            coin.Description = "A gold coin";

            GameTime.Reset();
            player = new Player("John");
            enemy = new Enemy("Zombie");
            enemy.Stats.Magic.Value = 30;
            passiveMagicSkill = new PassiveMagicSkill(id: 0,
                                            name: "ShadowStrength",
                                            description: "A +10 Buff to the user's strength.",
                                            cost: 10,
                                            duration: 10,
                                            preUseTime: 10,
                                            cooldownTime: 5,
                                            modifierValue: 10,
                                            modifiedAttributeName: "Body");
            passiveMagicSkill.TriggerSequence = new IItem[] { herb };
        }

        [Test]
        public void PassiveMagicSkillTest ()
        {
            // Check the basics
            Assert.AreEqual(0, passiveMagicSkill.Id);
            Assert.AreEqual("A +10 Buff to the user's strength.", passiveMagicSkill.Description);
            Assert.AreEqual(10, passiveMagicSkill.PreUseTime);
            Assert.AreEqual("ShadowStrength (Cost: 10, Body +10 for 10 sec)", passiveMagicSkill.ToString());

            // Check if the combination matches
            Assert.IsFalse(passiveMagicSkill.Match(new IItem[] { }));
            Assert.IsFalse(passiveMagicSkill.Match(new IItem[] { coin }));
            Assert.IsFalse(passiveMagicSkill.Match(new IItem[] { herb, coin }));
            Assert.IsFalse(passiveMagicSkill.Match(new IItem[] { coin, herb }));
            Assert.IsTrue(passiveMagicSkill.Match(new IItem[] { herb }));

            // Player triggers a Skill that is not learned yet
            bool triggered = player.TriggerMagicSkill(passiveMagicSkill);
            Assert.IsFalse(triggered);

            // Player learns the Skill and triggers it
            player.LearnMagicSkill(passiveMagicSkill);
            triggered = player.TriggerMagicSkill(passiveMagicSkill);
            Assert.IsTrue(triggered);

            // Check the effect on the modified attribute
            Assert.AreEqual(10, player.Stats.Body.Value);

            // Player triggers it again right away, which is not possible due to cooldown time
            triggered = player.TriggerMagicSkill(passiveMagicSkill);
            Assert.IsFalse(triggered);

            // Enemy uses the same passive skill
            enemy.LearnMagicSkill(passiveMagicSkill);
            triggered = enemy.TriggerMagicSkill(passiveMagicSkill);
            Assert.IsTrue(triggered);

            // Advance in Time and trigger again
            GameTime.time = 5;
            triggered = player.TriggerMagicSkill(passiveMagicSkill);
            Assert.IsTrue(triggered);

            // Take the use costs into account
            GameTime.time = 10;
            triggered = player.TriggerMagicSkill(passiveMagicSkill);
            Assert.IsFalse(triggered);
        }
    }
}
