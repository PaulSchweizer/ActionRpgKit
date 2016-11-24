using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Item;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;

namespace ActionRpgKit.NUnit.Tests.Character
{
    
    [TestFixture]
    [Category("Character.Character")]
    class CharacterTests
    {
        Player player;
        Enemy enemy;
        CombatSkill meleeSkill;
        MagicSkill passiveMagicSkill;
        WeaponItem sword;
        WeaponItem beretta;
        int _stateChanged;
        int _magicSkillLearned;
        int _magicSkillTriggered;
        int _combatSkillLearned;
        int _combatSkillTriggered;
        int _combatSkillUsed;

        [SetUp]
        public void SetUp()
        {
            Controller.Enemies = new List<Enemy>();
            player = new Player("Player");
            enemy = new Enemy("Enemy");
            meleeSkill = new GenericCombatSkill(id: 0,
                            name: "SwordFighting",
                            description: "Wield a sword effectively.",
                            cooldownTime: 1,
                            damage: 1,
                            maximumTargets: 1,
                            range: 1,
                            itemSequence: new int[] { });
            passiveMagicSkill = new PassiveMagicSkill(id: 0,
                                        name: "ShadowStrength",
                                        description: "A +10 Buff to the user's strength.",
                                        cooldownTime: 5,
                                        itemSequence: new int[] { },
                                        cost: 10,
                                        duration: 10,
                                        modifierValue: 10,
                                        modifiedAttributeName: "Body");
            SkillDatabase.MagicSkills = new MagicSkill[] { passiveMagicSkill };
            SkillDatabase.CombatSkills = new CombatSkill[] { meleeSkill };

            sword = new WeaponItem();
            sword.Id = 0;
            sword.Name = "Sword";
            sword.Range = 1;
            sword.Damage = 1;
            sword.Speed = 0.5f;

            beretta = new WeaponItem();
            sword.Id = 1;
            beretta.Name = "Beretta";
            beretta.Range = 10;
            beretta.Damage = 10;
            beretta.Speed = 2;

            ItemDatabase.Items = new BaseItem[] { sword, beretta};

            player.LearnCombatSkill(meleeSkill.Id);
            player.LearnMagicSkill(passiveMagicSkill.Id);
            enemy.Stats.Life.Value = 10;
            enemy.Stats.Experience.Value = 66;
            GameTime.Reset();
            player.OnCombatSkillTriggered += CombatSkillTriggered;
            enemy.OnCombatSkillTriggered += CombatSkillTriggered;
        }

        [Test]
        public void BasicsTest()
        {
            // Call ToString() to make sure it does not throw an error
            player.ToString();
            enemy.ToString();
        }

        [Test]
        public void LifeCallbackTest()
        {
            // Set the life of the enemy to 0 and expect the enemy to die
            enemy.Stats.Life.Value = -10;
            Assert.IsTrue(enemy.CurrentState is DefeatedState);
            Assert.IsTrue(enemy.IsDefeated);
        }

        [Test]
        public void StatesTest()
        {
            //     0 1 2 3 4
            //   + - - - - - 
            // 0 | P + + + E
            player.Stats.AlertnessRange.Value = 4 * 4;
            player.Position.Set(0, 0);
            enemy.Position.Set(4, 0);
            player.CurrentAttackSkill = player.CombatSkills[1];

            // Initial State
            Assert.IsTrue(player.CurrentState is IdleState);

            // It's not enough to have an Enemy in alterness range for the Player.
            // It is up to the Player to actually command the Avatar to Attack or Move.
            Controller.Update();
            Assert.IsTrue(player.CurrentState is IdleState);

            // Add Enemy and go into Chase state
            enemy.Position.Set(1, 0);
            Controller.Update();
            Assert.IsTrue(player.CurrentState is AlertState);
            Controller.Update();
            Assert.IsTrue(player.CurrentState is AlertState);
            GameTime.time += 10 - player.Alertness;
            Controller.Update();
            Assert.IsTrue(player.CurrentState is ChaseState);

            // Remove the enemy again and drop out of the chase again
            enemy.Position.Set(5, 0);
            GameTime.time += 10;
            Controller.Update();
            Assert.IsTrue(player.CurrentState is AlertState);

            // Enemy is back and the chase continues 
            //     0 1 2 3 4
            //   + - - - - - 
            // 0 | P E + + +
            enemy.Position.Set(1, 0);

            Controller.Update();
            Assert.IsTrue(player.CurrentState is AttackState);

            // Attack and get rid of the enemy
            GameTime.time += 2;
            player.AddEnemy(enemy);
            player.TargetedEnemy = enemy;
            for (int i = 1; i < 11; i++)
            {
                Controller.Update();
                Assert.AreEqual(10 - i, enemy.Stats.Life.Value);
                GameTime.time += 2;
            }
            Controller.Update();

            // All of the enemies are gone, so the Character switches back to 
            // AlertState and then to IdleState.
            Assert.AreEqual(0, player.Enemies.Count);

            // Player gathers experience for defeating the Enemy.
            Assert.AreEqual(enemy.Stats.Experience.Value, player.Stats.Experience.Value);

            Controller.Update();
            Assert.IsTrue(player.CurrentState is IdleState);

            // Reset the enemy and simulate a fleeing enemy after it has been attacked
            //     0 1 2 3 4
            //   + - - - - - 
            // 0 | P + E + +
            GameTime.time += 1;
            enemy.IsDefeated = false;
            enemy.Life.Value = 10;
            enemy.Position.Set(2, 0);
            player.AddEnemy(enemy);
            Controller.Update();
            Assert.IsTrue(player.CurrentState is IdleState);

            // Player starts walking from point A to point B 
            player.Position.Set(10, 0);
            player.IsMoving = true;
            Controller.Update();
            Assert.IsTrue(player.CurrentState is MoveState);

            // And eventually encounters an Enemy
            enemy.Position.Set(10, 0);
            Controller.Update();
            Assert.IsTrue(player.CurrentState is AlertState);
        }

        [Test]
        public void SerializeCharacterTest()
        {
            player.Position.Set(6, 6);
            player.CombatSkillEndTimes = new List<float>() { 100 };
            BinarySerialize(player);
            var serializedPlayer = BinaryDeserialize(player);

            // Check the deserialized values
            Assert.AreEqual(player.Name, serializedPlayer.Name);
            Assert.AreEqual(player.Position.ToString(), serializedPlayer.Position.ToString());
            Assert.AreEqual(player.Stats.ToString(), serializedPlayer.Stats.ToString());
            Assert.AreEqual(player.Inventory.ToString(), serializedPlayer.Inventory.ToString());
            Assert.AreEqual(player.ToString(), serializedPlayer.ToString());
            Assert.AreNotEqual(player.CombatSkillEndTimes[0], serializedPlayer.CombatSkillEndTimes[0]);
            Assert.AreEqual(-1, serializedPlayer.CombatSkillEndTimes[0]);
            Assert.AreSame(player.IdleState, IdleState.Instance);
            Assert.AreSame(player.AlertState, AlertState.Instance);
            Assert.AreSame(player.ChaseState, ChaseState.Instance);
            Assert.AreSame(player.AttackState, AttackState.Instance);
            Assert.AreSame(player.DefeatedState, DefeatedState.Instance);

            enemy.Position.Set(6, 6);
            enemy.IsDefeated = true;
            BinarySerialize(enemy);
            var serializedEnemy = BinaryDeserialize(enemy);

            // Check the deserialized values
            Assert.AreEqual(enemy.Name, serializedEnemy.Name);
            Assert.AreEqual(enemy.Position.ToString(), serializedEnemy.Position.ToString());
            Assert.AreEqual(enemy.Stats.ToString(), serializedEnemy.Stats.ToString());
            Assert.AreEqual(enemy.Inventory.ToString(), serializedEnemy.Inventory.ToString());
            Assert.AreEqual(enemy.ToString(), serializedEnemy.ToString());
            Assert.AreNotEqual(enemy.IsDefeated, serializedEnemy.IsDefeated);
            Assert.IsFalse(serializedEnemy.IsDefeated);
        }

        private void BinarySerialize(BaseCharacter character)
        {
            var serializedFile = Path.GetTempPath() + string.Format("/__CharacterTests__{0}.bin", character.Name);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(serializedFile,
                                           FileMode.Create,
                                           FileAccess.Write,
                                           FileShare.None);
            formatter.Serialize(stream, character);
            stream.Close();
        }

        private BaseCharacter BinaryDeserialize(BaseCharacter character)
        {
            var serializedFile = Path.GetTempPath() + string.Format("/__CharacterTests__{0}.bin", character.Name);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(serializedFile,
                                           FileMode.Open,
                                           FileAccess.Read,
                                           FileShare.Read);
            BaseCharacter serializedCharacter = (BaseCharacter)formatter.Deserialize(stream);
            stream.Close();
            File.Delete(serializedFile);
            return serializedCharacter;
        }

        [Test]
        public void BaseCharacterEventsTest()
        {
            player.OnStateChanged += new StateChangedHandler(StateChangedTest);
            enemy.OnMagicSkillLearned += new MagicSkillLearnedHandler(MagicSkillLearnedTest);
            player.OnMagicSkillTriggered += new MagicSkillTriggeredHandler(MagicSkillTriggeredTest);
            enemy.OnCombatSkillLearned += new CombatSkillLearnedHandler(CombatSkillLearnedTest);
            player.OnCombatSkillTriggered += new CombatSkillTriggeredHandler(CombatSkillTriggeredTest);
            player.OnCombatSkillUsed += new CombatSkillUsedHandler(CombatSkillUsedTest);

            // Change the State
            Assert.AreEqual(0, _stateChanged);
            player.ChangeState(player.AlertState);
            Assert.AreEqual(1, _stateChanged);
            player.ChangeState(player.IdleState);
            Assert.AreEqual(2, _stateChanged);

            // Learn a new Magic Skill
            Assert.AreEqual(0, _magicSkillLearned);
            enemy.LearnMagicSkill(passiveMagicSkill.Id);
            Assert.AreEqual(1, _magicSkillLearned);

            // Trigger a Magic Skill
            Assert.AreEqual(0, _magicSkillTriggered);
            player.TriggerMagicSkill(passiveMagicSkill.Id);
            Assert.AreEqual(1, _magicSkillTriggered);
            player.TriggerMagicSkill(passiveMagicSkill.Id);
            Assert.AreEqual(1, _magicSkillTriggered);

            // Learn a new Combat Skill
            Assert.AreEqual(0, _combatSkillLearned);
            enemy.LearnCombatSkill(meleeSkill.Id);
            Assert.AreEqual(1, _combatSkillLearned);

            // Trigger a Combat Skill
            
            // No enemy in reach
            player.TriggerCombatSkill(meleeSkill.Id);
            Assert.AreEqual(0, _combatSkillTriggered);
            Assert.AreEqual(0, _combatSkillUsed);

            // Set the enemy in reach
            player.Position.Set(0, 0);
            enemy.Position.Set(0, 0);
            player.AddEnemy(enemy);
            player.TargetedEnemy = enemy;
            player.ChangeState(player.AttackState);
            GameTime.time += 1;
            Controller.Update();
            player.TriggerCombatSkill(meleeSkill.Id);
            Assert.AreEqual(2, _combatSkillTriggered);
            Assert.AreEqual(2, _combatSkillUsed);
            
            // Not enough time has passed to trigger again
            player.TriggerCombatSkill(meleeSkill.Id);
            Assert.AreEqual(2, _combatSkillTriggered);
            Assert.AreEqual(2, _combatSkillUsed);
        }

        [Test]
        public void AdditionalAttributesTest()
        {
            // No Weapon equipped
            Console.WriteLine(player.ToString());
            Assert.AreEqual(0, player.Damage);
            Assert.AreEqual(1, player.AttackRange);
            Assert.AreEqual(1, player.AttacksPerSecond);

            // Equip the sword
            player.EquippedWeapon = sword.Id;
            Assert.AreEqual(1, player.Damage);
            Assert.AreEqual(2, player.AttackRange);
            Assert.AreEqual(2, player.AttacksPerSecond);

            // Equip the beretta
            player.EquippedWeapon = beretta.Id;
            Assert.AreEqual(10, player.Damage);
            Assert.AreEqual(11, player.AttackRange);
            Assert.AreEqual(0.5, player.AttacksPerSecond);
        }

        [Test]
        public void PlayerAttribuePointsTest()
        {
            player.Stats.Experience.Value = 0;
            player.Stats.Experience.Value = 100;
            Assert.AreEqual(10, player.AvailableAttributePoints);
            player.Stats.Experience.Value = 1000;
            Assert.AreEqual(30, player.AvailableAttributePoints);
        }

        public void StateChangedTest(ICharacter sender, IState previousState, IState newState)
        {
            _stateChanged += 1;
        }

        public void MagicSkillLearnedTest(IMagicUser sender, int skillId)
        {
            _magicSkillLearned += 1;
        }

        public void MagicSkillTriggeredTest(IMagicUser sender, int skillId)
        {
            _magicSkillTriggered += 1;
        }

        public void CombatSkillLearnedTest(IFighter sender, int skillId)
        {
            _combatSkillLearned += 1;
        }

        public void CombatSkillTriggeredTest(IFighter sender, int skillId)
        {
            _combatSkillTriggered += 1;
        }

        public void CombatSkillUsedTest(IFighter sender, int skillId)
        {
            _combatSkillUsed += 1;
        }

        private void CombatSkillTriggered(IFighter sender, int skillId)
        {
            sender.UseCombatSkill(skillId); ;
        }
    }
}
