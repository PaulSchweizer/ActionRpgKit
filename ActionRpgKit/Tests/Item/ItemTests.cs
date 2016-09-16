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
            var item = ItemDatabase.GetItemByName("Herb");
            Assert.AreEqual(item, herb);
        }
    }
}
