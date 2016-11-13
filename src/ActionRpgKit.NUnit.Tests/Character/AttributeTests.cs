using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character.Attribute;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace ActionRpgKit.NUnit.Tests.Character
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
        SimpleVolumeAttribute Energy;

        int _valueChangedEventTriggered;
        int _maxReachedEventTriggered;
        int _minReachedEventTriggered;

        [SetUp]
        public void SetUp()
        {
            GameTime.Reset();
            Body = new PrimaryAttribute("Body", 0, 999, 10);
            Experience = new PrimaryAttribute("Experience", 0);
            MagicRegenerationRate = new PrimaryAttribute("MagicRegenerationRate", 0, 99, 1);
            Level = new SecondaryAttribute("Level",
                        x => (int)(Math.Sqrt(x[0].Value / 100)) * 1f,
                        new BaseAttribute[] { Experience }, 0, 99);
            Life = new VolumeAttribute("Life",
                         x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f,
                         new BaseAttribute[] { Level, Body }, 0, 999);
            Magic = new VolumeAttribute("Magic",
                    x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f,
                    new BaseAttribute[] { Level, Body }, 0, 999);
            Energy = new SimpleVolumeAttribute("Energy", 0, 999, 0);
        }

        [Test]
        public void PrimaryAttributeTest()
        {
            PrimaryAttributeTest(Body);
        }

        private void PrimaryAttributeTest(PrimaryAttribute attr)
        {
            GameTime.time = 0;
            // Set some values
            attr.Value = attr.MinValue;
            Assert.AreEqual(attr.Value, attr.MinValue);
            attr.Value = float.MaxValue;
            Assert.AreEqual(attr.Value, attr.MaxValue);
            attr.Value = float.MinValue;
            Assert.AreEqual(attr.Value, attr.MinValue);

            // Add a modifier
            attr.AddModifier(new TimeBasedModifier("StrengthBuff", 10, 10));
            Assert.AreEqual(attr.MinValue + 10, attr.Value);

            // Advance in time
            for (int i = 0; i < 10; i++)
            {
                GameTime.time = i;
                Assert.AreEqual(attr.MinValue + 10, attr.Value);
            }
            // Until the modifier's life cycle is over
            GameTime.time += 1;
            Assert.AreEqual(attr.MinValue, attr.Value);

            // Add some more modifiers and advance time
            GameTime.time = 0;
            attr.AddModifier(new TimeBasedModifier("StrengthBuff", 10, 10));
            attr.AddModifier(new TimeBasedModifier("AnotherStrengthBuff", 20, 5));
            Assert.AreEqual(attr.MinValue + 30, attr.Value);

            GameTime.time = 5;
            Assert.AreEqual(attr.MinValue + 10, attr.Value);
            Assert.AreEqual(1, attr.Modifiers.Count);
            Assert.IsTrue(attr.IsModified);

            GameTime.time = 10;
            Assert.AreEqual(attr.MinValue, attr.Value);
            Assert.AreEqual(0, attr.Modifiers.Count);
            Assert.IsFalse(attr.IsModified);

            attr.Value = attr.MaxValue;
            attr.Reset();
            Assert.AreEqual(attr.MinValue, attr.Value);
        }

        [Test]
        public void SecondaryAttributeTest()
        {
            SecondaryAttributeTest(Level);
        }
        
        private void SecondaryAttributeTest(SecondaryAttribute attr)
        {
            // Check the default values
            Assert.AreEqual(0, attr.MinValue);
            Assert.AreEqual(99, attr.MaxValue);
            Assert.AreEqual(0, attr.Value);

            // Try directly setting it
            attr.Value = float.MaxValue;
            Assert.AreEqual(0, attr.Value);
            attr.Value = float.MinValue;
            Assert.AreEqual(0, attr.Value);

            // Change the contributing attributes
            for (int i = 0; i < 100; i++)
            {
                float xp = i * i * 100;
                Experience.Value = xp;
                Assert.AreEqual(i, attr.Value);
            }
            Experience.Reset();
            Assert.AreEqual(0, Experience.Value);
            Assert.AreEqual(0, attr.Value);
        }

        [Test]
        public void VolumeTest()
        {
            VolumeTest(Life);
        }

        private void VolumeTest(VolumeAttribute volumeAttr)
        {
            GameTime.time = 0;
            // Check the default values
            Body.Value = 0;
            Assert.AreEqual(0, volumeAttr.MinValue);
            Assert.AreEqual(20, volumeAttr.MaxValue);
            Assert.AreEqual(20, volumeAttr.Value);
            Body.Value = 10;
            Assert.AreEqual(23, volumeAttr.MaxValue);
            Assert.AreEqual(20, volumeAttr.Value);
            volumeAttr.Value = 23;
            Body.Value = 0;
            Assert.AreEqual(0, volumeAttr.MinValue);
            Assert.AreEqual(20, volumeAttr.MaxValue);
            Assert.AreEqual(20, volumeAttr.Value);
            Body.Value = 10;
            // Set the value
            volumeAttr.Value = 10;
            Assert.AreEqual(10, volumeAttr.Value);
            volumeAttr.Value -= 10;
            Assert.AreEqual(0, volumeAttr.Value);

            // Dropping below the minimum
            volumeAttr.Value = float.MinValue;
            Assert.AreEqual(0, volumeAttr.Value);

            // Exceeding the maximum
            volumeAttr.Value = float.MaxValue;
            Assert.AreEqual(23, volumeAttr.Value);

            // Change corresponding attributes
            Experience.Value = 100;
            Assert.AreEqual(28, volumeAttr.MaxValue);
       
            // Apply modifier and advance in time
            Body.AddModifier(new TimeBasedModifier("StrengthBuff", 11, 10));
            Assert.AreEqual(32, volumeAttr.MaxValue);
            GameTime.time = 11f;
            Assert.AreEqual(28, volumeAttr.MaxValue);

            // Reset
            volumeAttr.Value = 0;
            volumeAttr.Reset();
            Assert.AreEqual(28, volumeAttr.Value);
        }

        [Test]
        public void PrettyRepresentationTest()
        {
            Body.AddModifier(new TimeBasedModifier("StrengthBuff", 10, 10));
            string repr = "Body      : 20  (0 - 999)\n" +
                          "            + [StrengthBuff]:  10, 10/10 sec";
            Assert.AreEqual(repr, Body.ToString());
        }

        [Test]
        public void SimpleVolumeTest()
        {
            // Check the default values
            Assert.AreEqual(0, Energy.MinValue);
            Assert.AreEqual(999, Energy.MaxValue);
            Assert.AreEqual(0, Energy.Value);

            // Try directly setting it
            Energy.Value = float.MaxValue;
            Assert.AreEqual(999, Energy.Value);

            // Reset
            Energy.Value = float.MinValue;
            Assert.AreEqual(0, Energy.Value);
            Energy.Reset();
            Assert.AreEqual(999, Energy.Value);
        }

        [Test]
        public void SignalTest()
        {
            // Change the value and test the received signal
            Body.OnValueChanged += new ValueChangedHandler(ValueChangedDemo);
            Body.Value = 100;
            Assert.AreEqual(1, _valueChangedEventTriggered);
            
            // Adding a modifier triggers the signal
            var modifier = new TimeBasedModifier("StrengthBuff", 10, 10);
            Body.AddModifier(modifier);
            Assert.AreEqual(2, _valueChangedEventTriggered);
            
            // Removing a modifier triggers the signal
            Body.RemoveModifier(modifier);
            Assert.AreEqual(3, _valueChangedEventTriggered);
            
            // Secondary Attributes work the same
            Level.OnValueChanged += new ValueChangedHandler(ValueChangedDemo);
            Experience.Value = 100;
            Assert.AreEqual(4, _valueChangedEventTriggered);    
            
            // Adding a modifier triggers the signal
            Experience.AddModifier(modifier);
            Assert.AreEqual(5, _valueChangedEventTriggered);
            
            // Removing a modifier triggers the signal
            Experience.RemoveModifier(modifier);
            Assert.AreEqual(6, _valueChangedEventTriggered);
            
            // Maximum and minimum signals are emitted too.
            Body.OnMaxReached += new MaxReachedHandler(MaxReachedDemo);
            Body.Value = 999;
            Assert.AreEqual(1, _maxReachedEventTriggered);

            Body.OnMinReached += new MinReachedHandler(MinReachedDemo);
            Body.Value = -100;
            Assert.AreEqual(1, _minReachedEventTriggered);
        }

        [Test]
        public void SerializationTest()
        {
            Assert.IsTrue(typeof(PrimaryAttribute).IsSerializable);
            Assert.IsTrue(typeof(SecondaryAttribute).IsSerializable);
            Assert.IsTrue(typeof(VolumeAttribute).IsSerializable);

            var random = new Random();

            // Serialize a PrimaryAttribute
            Body.Value = random.Next(10, 500);
            Body.MinValue = random.Next(0, 9);
            Body.MaxValue = random.Next(501, 999);
            Body.AddModifier(new TimeBasedModifier("Test", 10, 10));
            BinarySerialize(Body);

            // Binary deserialization of PrimaryAttribute
            PrimaryAttribute binarySerializedBody = (PrimaryAttribute)BinaryDeserialize(Body);
            Assert.AreEqual(Body.Value, binarySerializedBody.Value);
            Assert.AreEqual(Body.MinValue, binarySerializedBody.MinValue);
            Assert.AreEqual(Body.MaxValue, binarySerializedBody.MaxValue);
            PrimaryAttributeTest(binarySerializedBody);

            // Serialize a SecondaryAttribute
            Experience.Value = 100;
            BinarySerialize(Level);

            // Binary deserialization of SecondaryAttribute
            SecondaryAttribute binarySerializedLevel = (SecondaryAttribute)BinaryDeserialize(Level);
            Assert.AreEqual(Level.Value, binarySerializedLevel.Value);
            Assert.AreEqual(1, binarySerializedLevel.Attributes.Length);
            Experience.Value = 0;
            binarySerializedLevel.Attributes = new BaseAttribute[] { Experience };
            binarySerializedLevel.Value = binarySerializedLevel.MaxValue;
            SecondaryAttributeTest(binarySerializedLevel);

            // Serialize a VolumeAttribute
            Body.MinValue = 0;
            Body.MaxValue = 999;
            Body.Value = 0;
            Life.Value = random.Next(0, 23);
            BinarySerialize(Life);

            // Binary deserialization of VolumeAttribute
            VolumeAttribute binarySerializedLife = (VolumeAttribute)BinaryDeserialize(Life);
            Assert.AreEqual(Life.Value, binarySerializedLife.Value);
            Assert.AreEqual(2, binarySerializedLife.Attributes.Length);
            Body.Value = 0;
            binarySerializedLife.Attributes = new BaseAttribute[] { Level, Body };
            binarySerializedLife.Value = binarySerializedLife.MaxValue;
            VolumeTest(binarySerializedLife);
        }

        private void BinarySerialize(BaseAttribute attr)
        {
            var serializedFile = Path.GetTempPath() + string.Format("/__AttributeTests__{0}.bin", attr.Name);
            Console.WriteLine(serializedFile);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(serializedFile,
                                           FileMode.Create,
                                           FileAccess.Write,
                                           FileShare.None);
            formatter.Serialize(stream, attr);
            stream.Close();
        }

        private BaseAttribute BinaryDeserialize(BaseAttribute attr)
        {
            var serializedFile = Path.GetTempPath() + string.Format("/__AttributeTests__{0}.bin", attr.Name);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(serializedFile,
                                    FileMode.Open,
                                    FileAccess.Read,
                                    FileShare.Read);
            BaseAttribute serializedAttr = (BaseAttribute)formatter.Deserialize(stream);
            stream.Close();
            File.Delete(serializedFile);
            return serializedAttr;
        }

        public void ValueChangedDemo(BaseAttribute sender, float value)
        {
            _valueChangedEventTriggered += 1;
        }

        public void MaxReachedDemo(BaseAttribute sender)
        {
            _maxReachedEventTriggered += 1;
        }

        public void MinReachedDemo(BaseAttribute sender)
        {
            _minReachedEventTriggered += 1;
        }
    }
}
