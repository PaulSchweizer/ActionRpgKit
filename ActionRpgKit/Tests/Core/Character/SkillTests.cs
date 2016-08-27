using System;
using NUnit.Framework;
using Character.Skill;

namespace CharacterTests
{
    [TestFixture]
    [Category("Character.Skill")]
    class SkillTests
    {

        PassiveSkill passiveA;
        PassiveSkill passiveB;
        PassiveSkill passiveC;

        [SetUp]
        public void SetUp()
        {
            passiveA = new PassiveSkill("PassiveSkillA");
            passiveB = new PassiveSkill("PassiveSkillB",
                                        "Description for B",
                                        cost: 20,
                                        preUseTime: 20,
                                        cooldownTime: 20);
            passiveC = new PassiveSkill(0,
                                        "PassiveSkillC",
                                        "Description for C",
                                        30,
                                        30,
                                        30);
            GameTime.time = 0f;
        }

        [Test]
        public void SkillUseTest()
        {
            bool triggered = passiveA.TriggerSkill();
            Assert.True(triggered);
            Assert.False(passiveA.CanBeUsed);

            GameTime.time = 11f;
            Assert.True(passiveA.CanBeUsed);
            triggered = passiveA.TriggerSkill();
            Assert.False(triggered);
        }
    }
}
