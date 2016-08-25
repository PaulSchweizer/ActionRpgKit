using System;
using NUnit.Framework;

namespace CharacterTests
{

    [TestFixture]
    [Category("Character.Stats")]
    public class CharacterStatsTests
    {
        Character.Stats charStats;

        [SetUp]
        public void SetUp ()                                                  
        {
            charStats = new Character.Stats(10, 0, 0);
            GameTime.time = 0f;
        }

        [Test]
        public void PrimaryAttributeTest()
        {
            Assert.AreEqual(999, (int)charStats.Body.MaxValue);
            Assert.AreEqual(0, charStats.Body.MinValue);
            Assert.AreEqual(10f, charStats.Body.Value);

            charStats.Body.AddModifier(new Character.Modifier("StrengthBuff", 10f, 10f));
            Assert.AreEqual(20f, charStats.Body.Value);

            GameTime.time = 9f;
            Assert.AreNotEqual(10f, charStats.Body.Value);

            GameTime.time = 11f;
            Assert.AreEqual(10f, charStats.Body.Value);

            charStats.Body.AddModifier(new Character.Modifier("StrengthBuff", 10f, 10f));
            charStats.Body.AddModifier(new Character.Modifier("AnotherStrengthBuff", 20f, 5f));
            Assert.AreEqual(40f, charStats.Body.Value);

            GameTime.time = 16f;
            Assert.AreEqual(20f, charStats.Body.Value);

            Assert.AreEqual(1, charStats.Body.Modifiers.Count);
            
            GameTime.time = 21f;
            Assert.AreEqual(10f, charStats.Body.Value);
            Assert.AreEqual(0, charStats.Body.Modifiers.Count);
            
            charStats.Body.Value = 123456789f;
            Assert.AreEqual(999f, charStats.Body.Value);
            
            charStats.Body.Value = -123456789f;
            Assert.AreEqual(0f, charStats.Body.Value);
        }
        
        [Test]
        public void SecondaryAttributeTest()
        {
            Assert.AreEqual(0f, charStats.Level.MinValue);
            Assert.AreEqual(99f, charStats.Level.MaxValue);
            for (int i=0; i < 100; i++)
            {
                float xp = i * i * 100;
                charStats.Experience.Value = xp;
                Assert.AreEqual(i, (int)(charStats.Level.Value));
            }

            // Console.WriteLine(Math.Max(0, Math.Min(99, 33444)));
            charStats.Level.Value = float.MaxValue;
            Assert.AreEqual(99f, charStats.Level.Value);
            
            charStats.Level.Value = float.MinValue;
            Assert.AreEqual(99f, charStats.Level.Value);
        }

        [Test]
        public void VolumeAttributeTest()
        {
            Assert.AreEqual(23, (int)(charStats.Life.Value));
            Assert.AreEqual(23, (int)(charStats.Life.MaxValue));
            Assert.AreEqual(0, (int)(charStats.Life.MinValue));

            charStats.Life.Value = 10;
            Assert.AreEqual(10, (int)(charStats.Life.Value));

            charStats.Life.Value -= 10;
            Assert.AreEqual(0, (int)(charStats.Life.Value));

            charStats.Life.Value -= 10;
            Assert.AreEqual(0, (int)(charStats.Life.Value));

            charStats.Life.Value = 999999;
            Assert.AreEqual(23, (int)(charStats.Life.Value));

            charStats.Experience.Value = 100;
            Assert.AreEqual(28, (int)(charStats.Life.MaxValue));

            charStats.Body.AddModifier(new Character.Modifier("StrengthBuff", 11, 10));
            Assert.AreEqual(32, (int)(charStats.Life.MaxValue));

            GameTime.time = 11f;
            Assert.AreEqual(28, (int)(charStats.Life.MaxValue));
        }

        [Test]
        public void PrettyRepresentationTest ()
        {
            charStats.Body.AddModifier(new Character.Modifier("StrengthBuff", 10, 10));
            charStats.Mind.AddModifier(new Character.Modifier("MindBuff", 30, 6));
            charStats.Mind.AddModifier(new Character.Modifier("MindDeBuff", -5, 20));
            GameTime.time = 5f;
            string repr = "--- Primary Attributes ------------\n" +
                          "      Body:  20 (0 - 999)\n" +
                          "                [StrengthBuff]:  10, 5/10 sec)\n" +
                          "      Mind:  25 (0 - 999)\n" +
                          "                [MindBuff]:  30, 1/6 sec)\n" +
                          "                [MindDeBuff]:  -5, 15/20 sec)\n" +
                          "      Soul:   0 (0 - 999)\n" +
                          "--- Secondary Attributes ------------\n" +
                          "     Level:   0 (0 - 99)\n" +
                          "      Life:  23 (0 - 26)\n" +
                          "     Magic:  20 (0 - 20)\n" +
                          "--- Skills ------------\n";
            string prettyRepresentation = charStats.ToString();
            Console.WriteLine(prettyRepresentation);
            Assert.AreEqual(repr, prettyRepresentation);
        }
    }
}
