using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Item;

namespace ActionRpgKit.Tests.Character
{
    
    [TestFixture]
    [Category("Character.Character")]
    class CharacterTests
    {
        Player player;
        Enemy enemy;
        ICombatSkill meleeSkill;
        int _stateChanged;

        [SetUp]
        public void RunBeforeAnyTests()
        {
            player = new Player();
            enemy = new Enemy();
            meleeSkill = new MeleeCombatSkill(id: 1,
                            name: "SwordFighting",
                            description: "Wield a sword effectively.",
                            preUseTime: 1,
                            cooldownTime: 1,
                            damage: 1,
                            maximumTargets: 1,
                            range: 1,
                            itemSequence: new IItem[] {});
            player.LearnCombatSkill(meleeSkill);
            enemy.Stats.Life.Value = 10;
            GameTime.Reset();
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
            Assert.IsTrue(enemy.CurrentState is DyingState);
            Assert.IsTrue(enemy.IsDead);
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
            player.CurrentAttackSkill = player.CombatSkills[0];

            // Initial State
            Assert.IsTrue(player.CurrentState is IdleState);

            // Add Enemy switches to alert state
            player.AddEnemy(enemy);
            player.Update();
            Assert.IsTrue(player.CurrentState is AlertState);

            // No more Enemy switches to idle state
            player.RemoveEnemy(enemy);
            player.Update();
            Assert.IsTrue(player.CurrentState is IdleState);

            // Add Enemy and go into Chase state
            player.AddEnemy(enemy);
            player.Update();
            Assert.IsTrue(player.CurrentState is AlertState);
            player.Update();
            Assert.IsTrue(player.CurrentState is ChaseState);

            //     0 1 2 3 4
            //   + - - - - - 
            // 0 | + + P + E
            player.Position.Set(2, 0);
            player.Update();
            Assert.IsTrue(player.CurrentState is ChaseState);

            //     0 1 2 3 4
            //   + - - - - - 
            // 0 | + + + P E
            player.Position.Set(3, 0);
            player.Update();
            Assert.IsTrue(player.CurrentState is AttackState);

            // Attack and get rid of the enemy
            for (int i = 1; i < 11; i ++)
            {
                GameTime.time += 1;
                player.Update();
                Assert.AreEqual(10 - i, enemy.Stats.Life.Value);
                enemy.Update();
                Assert.AreEqual(1, enemy.Enemies.Count);
            }

            // All of the enemies are gone, so the Character switches back to 
            // AlertState and then to IdleState.
            Assert.AreEqual(0, player.Enemies.Count);
            player.Update();
            Assert.IsTrue(player.CurrentState is AlertState);
            player.Update();
            Assert.IsTrue(player.CurrentState is IdleState);

            // Reset the enemy and simulate a fleeing enemy after it has been attacked
            //     0 1 2 3 4
            //   + - - - - - 
            // 0 | + + E P +
            GameTime.time += 1;
            enemy.IsDead = false;
            enemy.Life = 10;
            enemy.Position.Set(2, 0);
            player.AddEnemy(enemy);
            player.Update();
            Assert.IsTrue(player.CurrentState is AlertState);
            player.Update();
            Assert.IsTrue(player.CurrentState is ChaseState);
            player.Update();
            Assert.IsTrue(player.CurrentState is AttackState);
            player.Update();

            // Enemy flees
            //     0 1 2 3 4
            //   + - - - - - 
            // 0 | + E + P +
            enemy.Position.Set(1, 0);
            player.Update();
            Assert.IsTrue(player.CurrentState is ChaseState);

            // Player chases after the enemy
            //     0 1 2 3 4
            //   + - - - - - 
            // 0 | + E P + +
            player.Position.Set(2, 0);
            player.Update();
            Assert.IsTrue(player.CurrentState is AttackState);

            // Enemy is out of AlertnessRange so we eventually return to Idle
            //     0 1 2 3 4 5
            //   + - - - - - - 
            // 0 | E + + + P
            player.Position.Set(4, 0);
            enemy.Position.Set(0, 0);
            player.Stats.AlertnessRange.Value = 1;
            player.Update();
            Assert.IsTrue(player.CurrentState is ChaseState);
            player.Update();
            Assert.IsTrue(player.CurrentState is AlertState);
            player.Update();
            Assert.IsTrue(player.CurrentState is IdleState);
        }

        [Test]
        public void ICharacterEventsTest()
        {
            player.OnStateChanged += new StateChangedHandler(StateChangedTest);
            Assert.AreEqual(0, _stateChanged);
            player.ChangeState(player._alertState);
            Assert.AreEqual(1, _stateChanged);
            player.ChangeState(player._idleState);
            Assert.AreEqual(2, _stateChanged);
        }

        public void StateChangedTest(ICharacter sender, IState previousState, IState newState)
        {
            _stateChanged += 1;
        }
    }
}
