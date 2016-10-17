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

        UsableItem herb;
        UsableItem coin;
        MagicSkill passiveMagicSkill;
        CombatSkill meleeSkill;
        CombatSkill meleeMultiTargetsSkill;
        CombatSkill rangedSkill;

        [SetUp]
        public void SetUp ()
        {
            GameTime.Reset();

            herb = new UsableItem();
            herb.Id = 0;
            herb.Name = "Herb";
            herb.Description = "A common herb";
            coin = new UsableItem();
            coin.Id = 1;
            coin.Name = "Coin";
            coin.Description = "A gold coin";

            passiveMagicSkill = new PassiveMagicSkill(id: 0,
                                                      name: "ShadowStrength",
                                                      description: "A +10 Buff to the user's strength.",
                                                      preUseTime: 10,
                                                      cooldownTime: 5,
                                                      itemSequence: new UsableItem[] { herb },
                                                      cost: 10,
                                                      duration: 10,
                                                      modifierValue: 10,
                                                      modifiedAttributeName: "Body");
            meleeSkill = new GenericCombatSkill(id: 1,
                                        name: "SwordFighting",
                                        description: "Wield a sword effectively.",
                                        preUseTime: 1,
                                        cooldownTime: 1,
                                        damage: 1,
                                        maximumTargets: 1,
                                        range: 1,
                                        itemSequence: new UsableItem[] { });
            meleeMultiTargetsSkill = new GenericCombatSkill(id: 2,
                                                    name: "MultiHit",
                                                    description: "Wield a sword against multiple opponents.",
                                                    preUseTime: 1,
                                                    cooldownTime: 1,
                                                    damage: 1,
                                                    maximumTargets: 2,
                                                    range: 1,
                                                    itemSequence: new UsableItem[] { });
            rangedSkill = new GenericCombatSkill(id: 3,
                            name: "Longbow",
                            description: "Notch! Aim! Loose!",
                            preUseTime: 2,
                            cooldownTime: 1,
                            damage: 1,
                            maximumTargets: 1,
                            range: 10,
                            itemSequence: new UsableItem[] { });
        }

        [Test]
        public void SkillDatabaseTest()
        {
            SkillDatabase.MagicSkills = new MagicSkill[] { passiveMagicSkill };
            SkillDatabase.CombatSkills = new CombatSkill[] { meleeSkill, meleeMultiTargetsSkill, rangedSkill };

            Assert.IsNull(SkillDatabase.GetMagicSkillByName("DoesNotExist"));
            Assert.IsNull(SkillDatabase.GetCombatSkillByName("DoesNotExist"));
            Assert.AreSame(passiveMagicSkill, SkillDatabase.GetMagicSkillByName("ShadowStrength"));
            Assert.AreSame(meleeSkill, SkillDatabase.GetCombatSkillByName("SwordFighting"));
        }
    }
}
