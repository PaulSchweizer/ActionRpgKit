using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Story;
using ActionRpgKit.Tests.Story;

namespace ActionRpgKit.Tests.GameController
{
    [TestFixture]
    [Category("GameController")]
    public class GameControllerTests
    {

        Player player;
        Storyline storyline = new GameStoryline();

        private void Separator()
        {
            Console.WriteLine("////////////////////////////////////////////");
        }

        [SetUp]
        public void SetUp()
        {
            GameTime.Reset();
        }

        [Test]
        public void RunGameTest ()
        {
            InitItemDatabase();
            InitSkillDatabase();
            Separator();
            Console.WriteLine("New Game started");
            Separator();
            CreatePlayerCharacter();
            Separator();
            StartStoryline();
            Separator();
            storyline.CurrentChapter.ToString();
            Separator();
            Console.WriteLine("Gather some of the herbs");
            
        }

        private void InitItemDatabase()
        {
            // IItem herb = new Item();
            // ItemDatabase.Items = new IItem[] { herb };
        }

        private void InitSkillDatabase()
        {
            IMagicSkill passiveMagicSkill = new PassiveMagicSkill(id: 0,
                                                name: "ShadowStrength",
                                                description: "A +10 Buff to the user's strength.",
                                                cost: 10,
                                                duration: 10,
                                                preUseTime: 10,
                                                cooldownTime: 5,
                                                modifierValue: 10,
                                                modifiedAttributeName: "Body");
            ICombatSkill meleeSkill = new MeleeSkill(id: 0,
                                          name: "SwordFighting",
                                          description: "How to wield a sword.",
                                          preUseTime: 1,
                                          cooldownTime: 1,
                                          damage: 1,
                                          maximumTargets: 1);
            SkillDatabase.CombatSkills = new ICombatSkill[] { meleeSkill };
            SkillDatabase.MagicSkills = new IMagicSkill[] { passiveMagicSkill };
        }

        private void CreatePlayerCharacter ()
        {
            player = new Player();
            player.Name = "John";
            player.Stats.Body.Value = 20;
            player.Stats.Mind.Value = 10;
            player.Stats.Soul.Value = 5;
            player.Stats.Life.Reset();
            player.Stats.Magic.Reset();
            player.LearnCombatSkill(SkillDatabase.GetCombatSkillByName("SwordFighting"));
            player.LearnMagicSkill(SkillDatabase.GetMagicSkillByName("ShadowStrength"));
            Console.WriteLine(player.ToString());
        }

        private void StartStoryline ()
        {
            storyline.Start();
            Console.WriteLine(storyline.CurrentChapter.ToString());
        }
    }
}
