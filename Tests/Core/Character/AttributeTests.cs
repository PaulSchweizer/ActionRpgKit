
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
            primaryAttribute = new Character.PrimaryAttribute();
            GameTime.time = 0f;
        }
        
        [Test]
        public void PrimaryAttributeTest()
        {
        }
    }
}
