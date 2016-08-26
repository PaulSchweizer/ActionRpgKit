
using System;
using NUnit.Framework;
namespace CharacterTests
{
    [TestFixture]
    [Category("Character.Attribute")]
    public class CharacterAttributeTests
    {
        Character.PrimaryAttribute primaryAttribute;
        
        [SetUp]
        public void SetUp ()
        {
            primaryAttribute = new Character.PrimaryAttribute("PrimaryTestAttr", 0, 999, 10);
            GameTime.time = 0f;
        }
        
        [Test]
        public void PrimaryAttributeTest()
        {
            // Set some values
            primaryAttribute.Value = 12;
            Assert.AreEqual(primaryAttribute.Value, 12);     
            primaryAttribute.Value = 10000;
            Assert.AreEqual(primaryAttribute.Value, 999);
            primaryAttribute.Value = -10000;
            Assert.AreEqual(primaryAttribute.Value, 0);
        }
    }
}
