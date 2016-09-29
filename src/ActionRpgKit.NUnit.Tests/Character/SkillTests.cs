using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Item;

namespace ActionRpgKit.Tests.Skill
{
    [TestFixture]
    [Category("Character.Skill")]
    public class SkillTests
    {

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
            coin.Id = 1;
            coin.Name = "Coin";
            coin.Description = "A gold coin";

            GameTime.Reset();
            passiveMagicSkill = new PassiveMagicSkill(id: 0,
                                            name: "ShadowStrength",
                                            description: "A +10 Buff to the user's strength.",
                                            preUseTime: 10,
                                            cooldownTime: 5,
                                            itemSequence: new IItem[] { herb },
                                            cost: 10,
                                            duration: 10,
                                            modifierValue: 10,
                                            modifiedAttributeName: "Body");
        }

        [Test]
        public void PassiveMagicSkillTest()
        {
        }
    }
}
