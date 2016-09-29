using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Item;

namespace ActionRpgKit.Tests.Character
{
    [TestFixture]
    [Category("Character.Fighter")]
    class FighterTests
    {
        Player player;
        Enemy enemy1;
        Enemy enemy2;
        Enemy enemy3;

        ICombatSkill meleeSkill;
        ICombatSkill meleeMultiTargetsSkill;

        [SetUp]
        public void SetUp()
        {
            GameTime.Reset();
            player = new Player("John");
            enemy1 = new Enemy("Zombie");
            enemy2 = new Enemy("AxeZombie");
            enemy3 = new Enemy("SwampZombie");

            enemy1.Stats.Life.Value = 10;
            enemy2.Stats.Life.Value = 10;
            enemy3.Stats.Life.Value = 10;

            meleeSkill = new MeleeSkill(id: 1,
                                        name: "SwordFighting",
                                        description: "Wield a sword effectively.",
                                        preUseTime: 1,
                                        cooldownTime: 1,
                                        damage: 1,
                                        maximumTargets: 1,
                                        itemSequence: new IItem[]{});
            meleeMultiTargetsSkill = new MeleeSkill(id: 1,
                                            name: "MultiHit",
                                            description: "Wield a sword against multiple opponents.",
                                            preUseTime: 1,
                                            cooldownTime: 1,
                                            damage: 1,
                                            maximumTargets: 2,
                                            itemSequence: new IItem[]{});
        }

        [Test]
        public void SimpleMeeSkillTest()
        {
            // Prepare the enemy by learning the meleeSkill
            enemy1.LearnCombatSkill(meleeSkill);
            Assert.IsTrue(enemy1.CombatSkills.Contains(meleeSkill));

            // Add the Player as an enemy
            enemy1.AddEnemy(player);
            Assert.IsTrue(enemy1.Enemies.Contains(player));

            // Attack the Player until his health runs out
            for (int i = 0; i < 20; i++)
            {
                enemy1.TriggerCombatSkill(meleeSkill);
                Assert.AreEqual(20 - (i + 1), player.Life);
                GameTime.time += 1;
            }
        }

        [Test]
        public void MultiTargetsSkillTest()
        {
            // Prepare the player by learning the multiMeleeSkill
            player.LearnCombatSkill(meleeMultiTargetsSkill);

            // Add the enemies
            player.AddEnemy(enemy3);
            player.AddEnemy(enemy2);
            player.AddEnemy(enemy1);

            // Attack the Enemy until his health runs out
            for (int i = 0; i < 10; i++)
            {
                player.TriggerCombatSkill(meleeMultiTargetsSkill);
                Assert.AreEqual(10 - (i + 1), enemy1.Life);
                Assert.AreEqual(10 - (i + 1), enemy2.Life);
                Assert.AreEqual(10, enemy3.Life);
                GameTime.time += 1;
            }
        }
    }
}
