using NUnit.Framework;
using ActionRpgKit.Character;
using ActionRpgKit.Item;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;


namespace ActionRpgKit.Tests.Character
{
    [TestFixture]
    [Category("Character.Inventory")]
    public class InventoryTests
    {
        
        SimpleInventory simpleInventory;
        PlayerInventory playerInventory;

        [SetUp]
        public void SetUp()
        {
            BaseItem herb = new UsableItem();
            herb.Id = 0;
            herb.Name = "Herb";
            herb.Description = "A common herb";
            BaseItem coin = new UsableItem();
            coin.Id = 1;
            coin.Name = "Coin";
            coin.Description = "A gold coin";
            ItemDatabase.Items = new BaseItem[] { herb, coin };
        }

        [Test]
        public void SimpleInventoryTest()
        {
            BaseItem herb = ItemDatabase.GetItemByName("Herb");
            BaseItem coin = ItemDatabase.GetItemByName("Coin");
            simpleInventory = new SimpleInventory(new int[] { herb.Id },
                                                  new int[] { 1 });
            Assert.AreEqual(1, simpleInventory.ItemCount);
            Assert.AreEqual(1, simpleInventory.GetQuantity(herb.Id));
            Assert.AreEqual(0, simpleInventory.GetQuantity(coin.Id));

            // Adding and removing to and from a simple Inventory have no effect.
            simpleInventory.AddItem(coin.Id);
            Assert.AreEqual(0, simpleInventory.GetQuantity(coin.Id));
            simpleInventory.RemoveItem(herb.Id);
            Assert.AreEqual(1, simpleInventory.GetQuantity(herb.Id));

            // Test the pretty representation
            simpleInventory.ToString();
        }

        [Test]
        public void PlayerInventoryTest()
        {
            playerInventory = new PlayerInventory();
            Assert.AreEqual(0, playerInventory.ItemCount);
            BaseItem herb = ItemDatabase.GetItemByName("Herb");
            BaseItem coin = ItemDatabase.GetItemByName("Coin");
            playerInventory.AddItem(herb.Id);
            Assert.AreEqual(1, playerInventory.ItemCount);
            playerInventory.AddItem(herb.Id, 9);
            Assert.AreEqual(1, playerInventory.ItemCount);
            Assert.AreEqual(10, playerInventory.GetQuantity(herb.Id));
            playerInventory.RemoveItem(herb.Id, 9);
            Assert.AreEqual(1, playerInventory.ItemCount);
            playerInventory.RemoveItem(herb.Id, 100);
            Assert.AreEqual(0, playerInventory.ItemCount);
            Assert.AreEqual(0, playerInventory.GetQuantity(herb.Id));

            // Set and check Items and Quantities
            playerInventory.Items = new List<int>() { herb.Id, coin.Id };
            playerInventory.Quantities = new List<int>() { 1, 2 };
            Assert.AreEqual(2, playerInventory.Quantities.ToList<int>().Count);
            Assert.AreEqual(2, playerInventory.Items.ToList<int>().Count);

            // Test the pretty representation
            playerInventory.ToString();
        }

        [Test]
        public void MergeInventoriesTest()
        {
            playerInventory = new PlayerInventory();
            Assert.AreEqual(0, playerInventory.ItemCount);
            BaseItem herb = ItemDatabase.GetItemByName("Herb");
            simpleInventory = new SimpleInventory(new int[] { herb.Id },
                                                  new int[] { 10 });
            playerInventory.AddInventory(simpleInventory);
            Assert.AreEqual(1, playerInventory.ItemCount);
            Assert.AreEqual(10, playerInventory.GetQuantity(herb.Id));
        }

        [Test]
        public void ResetInventoryTest()
        {
            BaseItem herb = ItemDatabase.GetItemByName("Herb");
            BaseItem coin = ItemDatabase.GetItemByName("Coin");
            simpleInventory = new SimpleInventory(new int[] { herb.Id },
                                                  new int[] { 1 });
            playerInventory = new PlayerInventory(new int[] { herb.Id },
                                                  new int[] { 1 });
            Assert.AreEqual(1, playerInventory.ItemCount);
            Assert.AreEqual(1, simpleInventory.ItemCount);
            simpleInventory.Items = new int[] { };
            simpleInventory.Quantities = new int[] { };
            playerInventory.Items = new List<int>();
            playerInventory.Quantities = new List<int>();
            Assert.AreEqual(0, playerInventory.ItemCount);
            Assert.AreEqual(0, simpleInventory.ItemCount);
        }

        [Test]
        public void SerializeInventoryTest()
        {
            BaseItem herb = ItemDatabase.GetItemByName("Herb");
            BaseItem coin = ItemDatabase.GetItemByName("Coin");
            simpleInventory = new SimpleInventory(new int[] { herb.Id, coin.Id },
                                                  new int[] { 1, 2 });
            playerInventory = new PlayerInventory(new int[] { herb.Id, coin.Id },
                                                  new int[] { 1, 2});

            // Deserialize
            BinarySerialize(simpleInventory);
            var deserializedSimpleInventory = (SimpleInventory)BinaryDeserialize(simpleInventory);
            TestInventory(simpleInventory, deserializedSimpleInventory);
            BinarySerialize(playerInventory);
            var deserializedPlayerInventory = (PlayerInventory)BinaryDeserialize(playerInventory);
            TestInventory(playerInventory, deserializedPlayerInventory);
        }

        private void TestInventory(IInventory original, IInventory serialized)
        { 
            var items = original.Items.GetEnumerator();
            var quantities = original.Quantities.GetEnumerator();
            var serializedItems = serialized.Items.GetEnumerator();
            var serializedQuantities = serialized.Quantities.GetEnumerator();
            while (items.MoveNext() && quantities.MoveNext() && 
                   serializedItems.MoveNext() && serializedQuantities.MoveNext())
            {
                Assert.AreEqual(items.Current, serializedItems.Current);
                Assert.AreEqual(quantities.Current, serializedQuantities.Current);
            }
        }

        private void BinarySerialize(IInventory inventory)
        {
            var serializedFile = Path.GetTempPath() + string.Format("/__InventoryTest__.bin");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(serializedFile,
                                           FileMode.Create,
                                           FileAccess.Write,
                                           FileShare.None);
            formatter.Serialize(stream, inventory);
            stream.Close();
        }

        private IInventory BinaryDeserialize(IInventory stats)
        {
            var serializedFile = Path.GetTempPath() + string.Format("/__InventoryTest__.bin");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(serializedFile,
                                    FileMode.Open,
                                    FileAccess.Read,
                                    FileShare.Read);
            IInventory serializedStats = (IInventory)formatter.Deserialize(stream);
            stream.Close();
            File.Delete(serializedFile);
            return serializedStats;
        }
    }
}
