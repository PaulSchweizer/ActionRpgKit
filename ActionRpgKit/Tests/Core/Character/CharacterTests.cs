using NUnit.Framework;
using Character;
using Character.Skill;

namespace CharacterTests
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
            GameTime.time = 0;
            player = new BaseCharacter("John");
            enemy = new BaseCharacter("Zombie");
            passiveSkill = new PassiveSkill("ShadowStrength",
                                            "A Description",
                                            cost: 20,
                                            preUseTime: 20,
                                            cooldownTime: 20);
        }

        [Test]
        public void UsingSkillTest ()
        {
            // Trigger a Skill
            player.LearnSkill(passiveSkill);
            bool triggered = player.TriggerSkill(passiveSkill);
            Assert.IsTrue(triggered);
            triggered = player.TriggerSkill(passiveSkill);
            Assert.IsFalse(triggered);
            
            // Enemy uses the passive skill
            triggered = enemy.TriggerSkill(passiveSkill);
            Assert.IsFalse(triggered);
            enemy.LearnSkill(passiveSkill);
            Assert.IsTrue(triggered);

            // Advance in Time
            GameTime.time = 21;
            triggered = player.TriggerSkill(passiveSkill);
            Assert.IsTrue(triggered);
        }
    }
}
