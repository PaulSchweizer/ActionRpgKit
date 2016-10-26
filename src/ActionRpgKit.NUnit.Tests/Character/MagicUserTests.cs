using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Item;

namespace ActionRpgKit.NUnit.Tests.Character
{
    [TestFixture]
    [Category("Character.MagicUser")]
    public class MagicUserTests
    {

        Player player;
        Enemy enemy;
        UsableItem herb;
        UsableItem coin;

        MagicSkill passiveMagicSkill;

        [SetUp]
        public void SetUp ()
        {
            herb = new UsableItem();
            herb.Id = 0;
            herb.Name = "Herb";
            herb.Description = "A common herb";
            coin = new UsableItem();
            coin.Id = 1;
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
                                            modifiedAttributeName: "Body",
                                            itemSequence: new int[] { herb.Id });
            SkillDatabase.MagicSkills = new MagicSkill[] { passiveMagicSkill };
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
            Assert.IsFalse(passiveMagicSkill.Match(new int[] { }));
            Assert.IsFalse(passiveMagicSkill.Match(new int[] { coin.Id }));
            Assert.IsFalse(passiveMagicSkill.Match(new int[] { herb.Id, coin.Id }));
            Assert.IsFalse(passiveMagicSkill.Match(new int[] { coin.Id, herb.Id }));
            Assert.IsTrue(passiveMagicSkill.Match(new int[] { herb.Id }));

            // Player triggers a Skill that is not learned yet
            bool triggered = player.TriggerMagicSkill(passiveMagicSkill.Id);
            Assert.IsFalse(triggered);

            // Player learns the Skill and triggers it
            player.LearnMagicSkill(passiveMagicSkill.Id);
            triggered = player.TriggerMagicSkill(passiveMagicSkill.Id);
            Assert.IsTrue(triggered);

            // Check the effect on the modified attribute
            Assert.AreEqual(10, player.Stats.Body.Value);

            // Player triggers it again right away, which is not possible due to cooldown time
            triggered = player.TriggerMagicSkill(passiveMagicSkill.Id);
            Assert.IsFalse(triggered);

            // Enemy uses the same passive skill
            enemy.LearnMagicSkill(passiveMagicSkill.Id);
            triggered = enemy.TriggerMagicSkill(passiveMagicSkill.Id);
            Assert.IsTrue(triggered);

            // Advance in Time and trigger again
            GameTime.time = 5;
            triggered = player.TriggerMagicSkill(passiveMagicSkill.Id);
            Assert.IsTrue(triggered);

            // Take the use costs into account
            GameTime.time = 10;
            triggered = player.TriggerMagicSkill(passiveMagicSkill.Id);
            Assert.IsFalse(triggered);
        }
    }
}
