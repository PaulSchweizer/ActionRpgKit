using System;
using NUnit.Framework;
using ActionRpgKit.Item;

namespace ActionRpgKit.Tests.Item
{
    [TestFixture]
    [Category("Item")]
    public class ItemTests
    {
        BaseItem herb;
        BaseItem sword;

        [SetUp]
        public void SetUp()
        {
            herb = new UsableItem();
            herb.Id = 0;
            herb.Name = "Herb";
            herb.Description = "A common herb";

            sword = new WeaponItem();
            sword.Id = 1;
            sword.Name = "Sword";
            sword.Description = "A sharp sword";
        }

        [Test]
        public void ItemDatabaseTest()
        {
            ItemDatabase.Items = new BaseItem[] { herb };
            Assert.AreEqual(1, ItemDatabase.Items.Length);
            var itemByName = ItemDatabase.GetItemByName("Herb");
            Assert.AreSame(itemByName, herb);
            var itemById = ItemDatabase.GetItemById(0);
            Assert.AreSame(itemById, herb);
            var nullItem = ItemDatabase.GetItemByName("NotExisting");
            Assert.AreSame(nullItem, null);
            var nullItem2 = ItemDatabase.GetItemById(-1);
            Assert.AreSame(nullItem2, null);

            ItemDatabase.Items = new BaseItem[] { herb, sword };
            Console.WriteLine(ItemDatabase.ToString());
        }
    }
}
