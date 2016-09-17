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
        }

        [Test]
        public void RunGameDemo()
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
            KillAllRats();
            storyline.CheckProgress();
            Console.WriteLine(storyline.ToString());
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
            Console.WriteLine(MainController.Player.ToString());
        }

        private void StartStoryline()
        {
            storyline.Start();
            Console.WriteLine(storyline.ToString());
        }

        private void KillAllRats()
        {
            Console.WriteLine("Player fights the 10 rats.");
            GetRidOfRatsObjective.rats = 0;

            return;
            Enemy[] rats = new Enemy[10];
            for(int i=0; i<10; i++)
            {
                rats[i] = new Enemy("Rat");
                rats[i].Stats.Life.Value = 1;
            }

            for (int i = 0; i < 10; i++)
            {
                MainController.Player.AddEnemy(rats[i]);
                
            }
            
        }

        private void GatherHerbs()
        {
            Console.WriteLine("Player runs around and gathers the 10 herbs");
            MainController.Player.Inventory.AddItem(ItemDatabase.GetItemByName("Herb"), 10);
            Console.WriteLine(MainController.Player.ToString());
        }

        private void Separator()
        {
            Console.WriteLine("\n////////////////////////////////////////////\n");
        }
    }
}
