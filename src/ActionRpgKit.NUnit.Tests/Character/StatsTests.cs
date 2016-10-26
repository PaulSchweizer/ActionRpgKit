using System;
using NUnit.Framework;
using ActionRpgKit.Character.Stats;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace ActionRpgKit.NUnit.Tests.Character
{
    [TestFixture]
    [Category("Character.Stats")]
    public class StatsTests
    {

        PlayerStats _playerStats;
        EnemyStats _enemyStats;

        [SetUp]
        public void SetUp()
        {
            _playerStats = new PlayerStats();
            _playerStats.Body.Value = 10;
            _playerStats.Mind.Value = 10;
            _playerStats.Soul.Value = 10;
            _playerStats.Experience.Value = 100;
            _playerStats.Life.Value = 10;
            _playerStats.Magic.Value = 10;
            _playerStats.AlertnessRange.Value = 10;
            _playerStats.AttackRange.Value = 10;

            _enemyStats = new EnemyStats();
            _enemyStats.Body.Value = 10;
            _enemyStats.Mind.Value = 10;
            _enemyStats.Soul.Value = 10;
            _enemyStats.Experience.Value = 100;
            _enemyStats.Level.Value = 1;
            _enemyStats.Life.Value = 10;
            _enemyStats.Magic.Value = 10;
            _enemyStats.AlertnessRange.Value = 10;
            _enemyStats.AttackRange.Value = 10;
            _enemyStats.MagicRegenerationRate.Value = 1.01f;

            Console.WriteLine(_playerStats.ToString());
        }

        [Test]
        public void SerializeStatsTest()
        {

            BinarySerialize(_playerStats);
            var serializedPlayerStats = (PlayerStats)BinaryDeserialize(_playerStats);
            StatsTest(serializedPlayerStats);

            // Test that the Secondary and Volume Attributes are connected properly
            Assert.AreEqual(28, serializedPlayerStats.Life.MaxValue);
            Assert.AreEqual(1, serializedPlayerStats.Level.Value);
            serializedPlayerStats.Body.Value = 0;
            serializedPlayerStats.Experience.Value = 0;
            Assert.AreEqual(20, serializedPlayerStats.Life.MaxValue);
            Assert.AreEqual(0, serializedPlayerStats.Level.Value);

            // Test that the dictionary implementation has been restored
            Assert.AreSame(serializedPlayerStats.Body, serializedPlayerStats.Dict["Body"]);

            // Test the EnemyStats
            BinarySerialize(_enemyStats);
            var serializedEnemyStats = (EnemyStats)BinaryDeserialize(_enemyStats);
            StatsTest(serializedEnemyStats);
        }

        private void StatsTest(BaseStats stats)
        {
            Assert.AreEqual(10, stats.Body.Value);
            Assert.AreEqual(10, stats.Mind.Value);
            Assert.AreEqual(10, stats.Soul.Value);
            Assert.AreEqual(100, stats.Experience.Value);
            Assert.AreEqual(1, stats.Level.Value);
            Assert.AreEqual(10, stats.Life.Value);
            Assert.AreEqual(10, stats.Magic.Value);
            Assert.AreEqual(1.01f, stats.MagicRegenerationRate.Value);
            Assert.AreEqual(10, stats.AlertnessRange.Value);
            Assert.AreEqual(10, stats.AttackRange.Value);
        }

        private void BinarySerialize(BaseStats stats)
        {
            var serializedFile = Path.GetTempPath() + string.Format("/__StatsTest__.bin");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(serializedFile,
                                           FileMode.Create,
                                           FileAccess.Write,
                                           FileShare.None);
            formatter.Serialize(stream, stats);
            stream.Close();
        }

        private BaseStats BinaryDeserialize(BaseStats stats)
        {
            var serializedFile = Path.GetTempPath() + string.Format("/__StatsTest__.bin");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(serializedFile,
                                    FileMode.Open,
                                    FileAccess.Read,
                                    FileShare.Read);
            BaseStats serializedStats = (BaseStats)formatter.Deserialize(stream);
            stream.Close();
            File.Delete(serializedFile);
            return serializedStats;
        }
    }
}
