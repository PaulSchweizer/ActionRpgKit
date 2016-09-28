using System;
using NUnit.Framework;
using ActionRpgKit.Core;
using ActionRpgKit.Character;
using ActionRpgKit.Item;
using System.Collections.Generic;
using System.Linq;

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
            IItem coin = new UsableItem();
            coin.Id = 1;
            coin.Name = "Coin";
            coin.Description = "A gold coin";
            ItemDatabase.Items = new IItem[] { herb, coin };
        }

        [Test]
        public void SimpleInventoryTest()
        {
            IItem herb = ItemDatabase.GetItemByName("Herb");
            IItem coin = ItemDatabase.GetItemByName("Coin");
            simpleInventory = new SimpleInventory(new IItem[] { herb },
                                                  new int[] { 1 });
            Assert.AreEqual(1, simpleInventory.ItemCount);
            Assert.AreEqual(1, simpleInventory.GetQuantity(herb));
            Assert.AreEqual(0, simpleInventory.GetQuantity(coin));

            // Adding and removing to and from a simple Inventory have no effect.
            simpleInventory.AddItem(coin);
            Assert.AreEqual(0, simpleInventory.GetQuantity(coin));
            simpleInventory.RemoveItem(herb);
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
            Assert.AreEqual(1, playerInventory.ItemCount);
            Assert.AreEqual(10, playerInventory.GetQuantity(herb));
            playerInventory.RemoveItem(herb, 9);
            Assert.AreEqual(1, playerInventory.ItemCount);
            playerInventory.RemoveItem(herb, 100);
            Assert.AreEqual(0, playerInventory.ItemCount);
            Assert.AreEqual(0, playerInventory.GetQuantity(herb));

            // Iterate over Items
            Assert.AreEqual(0, playerInventory.Quantities.ToList<int>().Count);
            Assert.AreEqual(0, playerInventory.Items.ToList<IItem>().Count);
        }

        [Test]
        public void MergeInventoriesTest()
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
