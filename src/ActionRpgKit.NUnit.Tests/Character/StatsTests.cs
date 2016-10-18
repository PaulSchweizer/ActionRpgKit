using System;
using NUnit.Framework;
using ActionRpgKit.Character.Stats;
using ActionRpgKit.Character.Attribute;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace ActionRpgKit.Tests.Character
{
    [TestFixture]
    [Category("Character.Stats")]
    public class StatsTests
    {

        PlayerStats _playerStats;

        [SetUp]
        public void SetUp()
        {
            _playerStats = new PlayerStats();
            _playerStats.Body.Value = 10;
            _playerStats.Mind.Value = 10;
            _playerStats.Soul.Value = 10;
        }

        [Test]
        public void SerializePlayerStatsTest()
        {
            BinarySerialize(_playerStats);
            var serializedPlayerStats = (PlayerStats)BinaryDeserialize(_playerStats);
            Console.WriteLine(serializedPlayerStats.ToString());
            PlayerStatsTest(serializedPlayerStats);
        }

        private void PlayerStatsTest(PlayerStats playerStats)
        {
            Assert.AreEqual(10, playerStats.Body.Value);
        }

        private void BinarySerialize(BaseStats stats)
        {
            var serializedFile = Path.GetTempPath() + string.Format("/__StatsTest__.bin");
            Console.WriteLine(serializedFile);
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
