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
        IItem herb;
        IInventory SimpleInventory;
        IInventory PlayerInventory;

        [SetUp]
        public void SetUp()
        {
            herb = new UsableItem();
            herb.Id = 0;
            herb.Name = "Herb";
            herb.Description = "A common herb";

            SimpleInventory = new SimpleInventory();
            PlayerInventory = new PlayerInventory();
        }

        [Test]
        public void SimpleInventoryTest()
        {
            SimpleInventory.Items = new IItem[] { herb };
        }

        [Test]
        public void PlayerInventoryTest()
        {
            PlayerInventory.Items = new IItem[] { herb };
        }
    }
}
