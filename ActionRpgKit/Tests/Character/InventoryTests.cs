using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Item;

namespace ActionRpgKit.Tests.Character
{
    [TestFixture]
    [Category("Character.Inventory")]
    public class InventoryTests
    {
        
        IInventory simpleInventory;
        PlayerInventory playerInventory;

        [SetUp]
        public void SetUp()
        {
            IItem herb = new UsableItem();
            herb.Id = 0;
            herb.Name = "Herb";
            herb.Description = "A common herb";
            ItemDatabase.Items = new IItem[] { herb };
        }

        [Test]
        public void SimpleInventoryTest()
        {
            IItem herb = ItemDatabase.GetItemByName("Herb");
            simpleInventory = new SimpleInventory(new IItem[] { herb },
                                                  new int[] { 1 });
            Assert.AreEqual(1, simpleInventory.ItemCount);
            Console.WriteLine(simpleInventory.ToString());
            Assert.AreEqual(1, simpleInventory.GetQuantity(herb));
        }

        [Test]
        public void PlayerInventoryTest()
        {
            playerInventory = new PlayerInventory();
            Assert.AreEqual(0, playerInventory.ItemCount);
            IItem herb = ItemDatabase.GetItemByName("Herb");
            playerInventory.AddItem(herb);
            Assert.AreEqual(1, playerInventory.ItemCount);
            playerInventory.AddItem(herb, 9);
            Console.WriteLine(playerInventory.ToString());
            Assert.AreEqual(1, playerInventory.ItemCount);
            Assert.AreEqual(10, playerInventory.GetQuantity(herb));
            playerInventory.RemoveItem(herb, 9);
            Assert.AreEqual(1, playerInventory.ItemCount);
            playerInventory.RemoveItem(herb, 1);
            Assert.AreEqual(0, playerInventory.ItemCount);
            Assert.AreEqual(0, playerInventory.GetQuantity(herb));
        }

        [Test]
        public void NergeInventoriesTest()
        {
            playerInventory = new PlayerInventory();
            Assert.AreEqual(0, playerInventory.ItemCount);
            IItem herb = ItemDatabase.GetItemByName("Herb");
            simpleInventory = new SimpleInventory(new IItem[] { herb },
                                                  new int[] { 10 });
            playerInventory.AddInventory(simpleInventory);
            Assert.AreEqual(1, playerInventory.ItemCount);
            Assert.AreEqual(10, playerInventory.GetQuantity(herb));
        }
    }
}
