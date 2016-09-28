using System;
using NUnit.Framework;
using ActionRpgKit.Item;

namespace ActionRpgKit.Tests.Item
{
    [TestFixture]
    [Category("Item")]
    public class ItemTests
    {
        IItem herb;

        [SetUp]
        public void SetUp()
        {
            herb = new UsableItem();
            herb.Id = 0;
            herb.Name = "Herb";
            herb.Description = "A common herb";
        }

        [Test]
        public void ItemDatabaseTest()
        {
            ItemDatabase.Items = new IItem[] { herb };
            Assert.AreEqual(1, ItemDatabase.Items.Length);
            var itemByName = ItemDatabase.GetItemByName("Herb");
            Assert.AreSame(itemByName, herb);
            var itemById = ItemDatabase.GetItemById(0);
            Assert.AreSame(itemById, herb);
            var nullItem = ItemDatabase.GetItemByName("NotExisting");
            Assert.AreSame(nullItem, null);
            var nullItem2 = ItemDatabase.GetItemById(-1);
            Assert.AreSame(nullItem2, null);
        }
    }
}
