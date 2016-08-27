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

        PassiveSkill passiveSkill;

        [SetUp]
        public void SetUp ()
        {
            player = new Player("John");
            passiveSkill = new PassiveSkill("ShadowStrength",
                                            "Description for B",
                                            cost: 20,
                                            preUseTime: 20,
                                            cooldownTime: 20);
        }

        [Test]
        public void UsingSkillTest ()
        {
            player.LearnSkill(passiveSkill);
            player.TriggerSkill(passiveSkill);

        }
    }
}
