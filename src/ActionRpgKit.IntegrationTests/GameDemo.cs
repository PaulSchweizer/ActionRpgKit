using System;
using NUnit.Framework;
using ActionRpgKit.Item;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Story;

namespace ActionRpgKit.Tests.IntegrationTests
{

    [TestFixture]
    [Category("GameDemo")]
    public class GameDemo
    {

        Storyline storyline = new GameStoryline();

        [SetUp]
        public void SetUp()
        {
            GameTime.Reset();
            System.IO.File.WriteAllText(@"C:\Users\Paul\Desktop\INTEGRATIONTEST.txt", "");
        }

        private void LogToFile(string text)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Paul\Desktop\INTEGRATIONTEST.txt", true))
            {
                file.WriteLine(text);
            }
            Console.WriteLine(text);
        }

        [Test]
        public void RunGameDemo()
        {
            InitItemDatabase();
            InitSkillDatabase();
            Separator();
            LogToFile("New Game started");
            Separator();
            CreatePlayerCharacter();
            Separator();
            StartStoryline();
            Separator();
            KillAllRats();
            storyline.CheckProgress();
            LogToFile(storyline.ToString());
            Separator();
            GatherHerbs();
            storyline.CheckProgress();
            storyline.CheckProgress();
            Separator();
        }

        private void InitItemDatabase()
        {
            IItem herb = new UsableItem();
            herb.Id = 0;
            herb.Name = "Herb";
            herb.Description = "A common herb";
            ItemDatabase.Items = new IItem[] { herb };
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

        private void CreatePlayerCharacter()
        {
            MainController.CreatePlayer();
            MainController.Player.Name = "John";
            MainController.Player.Stats.Body.Value = 20;
            MainController.Player.Stats.Mind.Value = 10;
            MainController.Player.Stats.Soul.Value = 5;
            MainController.Player.Stats.Life.Reset();
            MainController.Player.Stats.Magic.Reset();
            MainController.Player.LearnCombatSkill(SkillDatabase.GetCombatSkillByName("SwordFighting"));
            MainController.Player.LearnMagicSkill(SkillDatabase.GetMagicSkillByName("ShadowStrength"));
            LogToFile(MainController.Player.ToString());
        }

        private void StartStoryline()
        {
            storyline.Start();
            LogToFile(storyline.ToString());
        }

        private void KillAllRats()
        {
            LogToFile("Player fights the 10 rats.");
            GetRidOfRatsObjective.rats = 0;            
        }

        private void GatherHerbs()
        {
            LogToFile("Player runs around and gathers the 10 herbs");
            MainController.Player.Inventory.AddItem(ItemDatabase.GetItemByName("Herb"), 10);
            LogToFile(MainController.Player.ToString());
        }

        private void Separator()
        {
            LogToFile("\n# ////////////////////////////////////////////\n");
        }
    }
}
