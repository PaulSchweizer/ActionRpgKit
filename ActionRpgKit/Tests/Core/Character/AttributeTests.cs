using System;
using NUnit.Framework;
using Character.Attribute;

namespace CharacterTests
{
    [TestFixture]
    [Category("Character.Attribute")]
    public class AttributeTests
    {
        PrimaryAttribute Body;
        PrimaryAttribute Experience;
        PrimaryAttribute MagicRegenerationRate;
        SecondaryAttribute Level;
        VolumeAttribute Life;
        VolumeAttribute Magic;

        [SetUp]
        public void SetUp ()
        {
            GameTime.Reset();
            Body = new PrimaryAttribute("Body", 0, 999, 10);
            Experience = new PrimaryAttribute("Experience");
            MagicRegenerationRate = new PrimaryAttribute("MagicRegenerationRate", 0, 99, 1);
            Level = new SecondaryAttribute("Level",
                        x => (int)(Math.Sqrt(x[0].Value / 100)) * 1f,
                        new IAttribute[] { Experience }, 0, 99);
            Life = new VolumeAttribute("Life",
                         x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f,
                         new IAttribute[] { Level, Body }, 0, 999);
            Magic = new VolumeAttribute("Magic",
                    x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f,
                    new IAttribute[] { Level, Body }, 0, 999);
        }

        [Test]
        public void PrimaryAttributeTest ()
        {
            // Check the default values
            Assert.AreEqual(0, Body.MinValue);
            Assert.AreEqual(999, Body.MaxValue);
            Assert.AreEqual(10, Body.Value);

            // Set some values
            Body.Value = 12;
            Assert.AreEqual(Body.Value, 12);
            Body.Value = float.MaxValue;
            Assert.AreEqual(Body.Value, 999);
            Body.Value = float.MinValue;
            Assert.AreEqual(Body.Value, 0);

            // Add a modifier
            Body.AddModifier(new TimeBasedModifier("StrengthBuff", 10, 10));
            Assert.AreEqual(10, Body.Value);

            // Advance in time
            for (int i = 0; i < 10; i++)
            {
                GameTime.time = i;
                Assert.AreEqual(10, Body.Value);
            }
            // Until the modifier's life cycle is over
            GameTime.time += 1;
            Assert.AreEqual(0, Body.Value);

            // Add some more modifiers and advance time
            GameTime.time = 0;
            Body.AddModifier(new TimeBasedModifier("StrengthBuff", 10, 10));
            Body.AddModifier(new TimeBasedModifier("AnotherStrengthBuff", 20, 5));
            Assert.AreEqual(30, Body.Value);

            GameTime.time = 5;
            Assert.AreEqual(10, Body.Value);
            Assert.AreEqual(1, Body.Modifiers.Count);
            Assert.IsTrue(Body.IsModified);

            GameTime.time = 10;
            Assert.AreEqual(0, Body.Value);
            Assert.AreEqual(0, Body.Modifiers.Count);
            Assert.IsFalse(Body.IsModified);
        }

        [Test]
        public void SecondaryAttributeTest ()
        {
            // Check the default values
            Assert.AreEqual(0, Level.MinValue);
            Assert.AreEqual(99, Level.MaxValue);
            Assert.AreEqual(0, Level.Value);

            // Try directly setting it
            Level.Value = float.MaxValue;
            Assert.AreEqual(0, Level.Value);
            Level.Value = float.MinValue;
            Assert.AreEqual(0, Level.Value);

            // Change the contributing attributes
            for (int i = 0; i < 100; i++)
            {
                float xp = i * i * 100;
                Experience.Value = xp;
                Assert.AreEqual(i, Level.Value);
            }
        }

        [Test]
        public void LifeTest ()
        {
            // Check the default values
            Assert.AreEqual(0, Life.MinValue);
            Assert.AreEqual(23, Life.MaxValue);
            Assert.AreEqual(23, Life.Value);

            // Set the value
            Life.Value = 10;
            Assert.AreEqual(10, Life.Value);
            Life.Value -= 10;
            Assert.AreEqual(0, Life.Value);

            // Dropping below the minimum
            Life.Value = float.MinValue;
            Assert.AreEqual(0, Life.Value);

            // Exceeding the maximum
            Life.Value = float.MaxValue;
            Assert.AreEqual(23, Life.Value);

            // Change corresponding attributes
            Experience.Value = 100;
            Assert.AreEqual(28, Life.MaxValue);
       
            // Apply modifier and advance in time
            Body.AddModifier(new TimeBasedModifier("StrengthBuff", 11, 10));
            Assert.AreEqual(32, Life.MaxValue);
            GameTime.time = 11f;
            Assert.AreEqual(28, Life.MaxValue);
        }

        [Test]
        public void PrettyRepresentationTest ()
        {
            Body.AddModifier(new TimeBasedModifier("StrengthBuff", 10, 10));
            string repr = "Body      : 20  (0 - 999)\n" +
                          "            + [StrengthBuff]:  10, 10/10 sec";
            Console.WriteLine(Body.ToString());
            Assert.AreEqual(repr, Body.ToString());
        }
    }
}
