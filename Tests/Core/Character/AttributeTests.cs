
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
        SecondaryAttribute Level;
        VolumeAttribute Life;
        
        [SetUp]
        public void SetUp ()
        {
            GameTime.time = 0f;
            Body = new PrimaryAttribute("Body", 0, 999, 10);
            Experience = new PrimaryAttribute("Experience");
            Level = new SecondaryAttribute("Level",
                        x => (int)(Math.Sqrt(x[0].Value / 100)) * 1f, 
                        new IAttribute[] { Experience }, 0, 99);
            Life = new SecondaryAttribute("Life", 
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
            Body.AddModifier(new Modifier("StrengthBuff", 10, 10));
            Assert.AreEqual(10, Body.Value);
            
            // Advance in time
            for(int i=0; i<10; i++)
            {
                GameTime.time = i;
                Assert.AreEqual(10, Body.Value);
            }
            // Until the modifier's life cycle is over
            GameTime.time += 1;
            Assert.AreEqual(0, Body.Value);
            
            // Add some more modifiers and advance time
            GameTime.time = 0;
            Body.AddModifier(new Modifier("StrengthBuff", 10, 10));
            Body.AddModifier(new Modifier("AnotherStrengthBuff", 20, 5));
            Assert.AreEqual(30, Body.Value);

            GameTime.time = 5;
            Assert.AreEqual(10, Body.Value);
            Assert.AreEqual(1, Body.Modifiers.Count);
            Assert.IsTrue(Body.IsModified);
            
            GameTime.time = 10;
            Assert.AreEqual(0, Body.Value);
            Assert.AreEqual(0, Body.Modifiers.Count);  
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
            for (int i=0; i < 100; i++)
            {
                float xp = i * i * 100;
                Experience.Value = xp;
                Assert.AreEqual(i, Level.Value);
            }
        }
        
        [Test]
        public void VolumeAttributeTest()
        {
            // Check the default values
            Assert.AreEqual(0, Life.MinValue);
            Assert.AreEqual(23, Life.MaxValue);
            Assert.AreEqual(23, Life.Value);
            
            // Set the value
            // Life.Value = 10;
            // Assert.AreEqual(10, Life.Value);
            // Life.Value -= 10;
            // Assert.AreEqual(0, Life.Value);
            
            // Dropping below the minimum
            // Life.Value -= 10;
            // Assert.AreEqual(0, Life.Value);
    
            // Exceeding the maximum
            // Life.Value = float.MaxValue;
            // Assert.AreEqual(23, Life.Value);

            // charStats.Experience.Value = 100;
            // Assert.AreEqual(28, (int)(charStats.Life.MaxValue));

            // charStats.Body.AddModifier(new Character.Modifier("StrengthBuff", 11, 10));
            // Assert.AreEqual(32, (int)(charStats.Life.MaxValue));

            // GameTime.time = 11f;
            // Assert.AreEqual(28, (int)(charStats.Life.MaxValue));
        }
    }
}
