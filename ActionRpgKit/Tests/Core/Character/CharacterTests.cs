using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Core.Character;
using ActionRpgKit.Core.Character.Skill;

namespace ActionRpgKit.Tests.Core.Character
{
    [TestFixture]
    [Category("Character.Character")]
    public class CharacterTests
    {

        Player player;
        Enemy enemy;

        IMagicSkill passiveMagicSkill;
        ICombatSkill meleeSkill;
        ICombatSkill meleeMultiTargetsSkill;

        [SetUp]
        public void SetUp ()
        {
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
            meleeSkill = new MeleeSkill(id: 1,
                                        name: "SwordFighting",
                                        description: "How to wield a sword.",
                                        preUseTime: 1,
                                        cooldownTime: 1,
                                        damage: 1,
                                        maximumTargets: 1);
            meleeMultiTargetsSkill = new MeleeSkill(id: 1,
                                            name: "MultiHit",
                                            description: "How to wield a sword against multiple opponents.",
                                            preUseTime: 1,
                                            cooldownTime: 1,
                                            damage: 1,
                                            maximumTargets: 10);
        }

        [Test]
        public void PassiveMagicSkillTest ()
        {
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
            Console.WriteLine(player.Stats.Magic);
            triggered = player.TriggerMagicSkill(passiveMagicSkill);
            Assert.IsTrue(triggered);

            // Take the use costs into account
            GameTime.time = 10;
            triggered = player.TriggerMagicSkill(passiveMagicSkill);
            Assert.IsFalse(triggered);
        }

        [Test]
        public void SimpleMeeSkillTest ()
        {
            // Prepare the enemy by learning the meleeSkill
            enemy.LearnCombatSkill(meleeSkill);
            Assert.IsTrue(enemy.CombatSkills.Contains(meleeSkill));

            // Add the Player as an enemy
            enemy.AddEnemy(player);
            Assert.IsTrue(enemy.Enemies.Contains(player));

            // Attack the Player until his health runs out
            for (int i=0; i < 20; i++)
            {
                enemy.TriggerCombatSkill(meleeSkill);
                Assert.AreEqual(20 - (i+1), player.Life);
                GameTime.time += 1;
            }
        }
        
        [Test]
        public void MultiTargetsSkillTest ()
        {
            // Prepare the enemy by learning the meleeSkill
            enemy.LearnCombatSkill(meleeMultiTargetsSkill);
            Assert.IsTrue(enemy.CombatSkills.Contains(meleeMultiTargetsSkill));
        }
    }
}
