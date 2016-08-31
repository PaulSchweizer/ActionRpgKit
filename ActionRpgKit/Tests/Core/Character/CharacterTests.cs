using System;
using NUnit.Framework;
using Character;
using Character.Skill;

namespace ActionRpgKit.Tests.Core.Character
{
    [TestFixture]
    [Category("Character.Character")]
    public class CharacterTests
    {

        ICharacter player;
        ICharacter enemy;

        PassiveSkill passiveSkill;

        [SetUp]
        public void SetUp ()
        {
            GameTime.Reset();
            player = new Player("John");
            enemy = new Enemy("Zombie");
            passiveSkill = new PassiveSkill(id: 0,
                                            name: "ShadowStrength",
                                            description: "A Description",
                                            cost: 10,
                                            duration: 10,
                                            preUseTime: 10,
                                            cooldownTime: 5,
                                            modifierValue: 10,
                                            modifiedAttributeName: "Body");
            enemy.Stats.Magic.Value = 30;
        }

        [Test]
        public void UsingSkillTest ()
        {
            // Player triggers a Skill that is not learned yet
            bool triggered = player.TriggerSkill(passiveSkill);
            Assert.IsFalse(triggered);

            // Player learns the Skill and triggers it
            player.LearnSkill(passiveSkill);
            triggered = player.TriggerSkill(passiveSkill);
            Assert.IsTrue(triggered);

            // Player triggers it again right away, which is not possible due to cooldown time
            triggered = player.TriggerSkill(passiveSkill);
            Assert.IsFalse(triggered);
            
            // Enemy uses the same passive skill
            enemy.LearnSkill(passiveSkill);
            triggered = enemy.TriggerSkill(passiveSkill);
            Assert.IsTrue(triggered);

            // Advance in Time and trigger again
            GameTime.time = 5;
            Console.WriteLine(player.Stats.Magic);
            triggered = player.TriggerSkill(passiveSkill);
            Assert.IsTrue(triggered);
            
            // Take the use costs into account
            GameTime.time = 10;
            triggered = player.TriggerSkill(passiveSkill);
            Assert.IsFalse(triggered);
        }
        
        [Test]
        public void PassiveSkillTest ()
        {
            player.LearnSkill(passiveSkill);
            player.TriggerSkill(passiveSkill);
            Assert.AreEqual(33, player.Stats.Body.Value);
        }
    }
}
