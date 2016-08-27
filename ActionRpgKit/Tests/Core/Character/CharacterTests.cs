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

        [SetUp]
        public void SetUp ()
        {
            player = new Player("John");
        }

        [Test]
        public void UsingSkillTest ()
        {
            PassiveSkill skill = new PassiveSkill("ShadowStrength");
            player.LearnSkill(skill);
            player.UseSkill(player.Skills[0]);
        }
    }
}
