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
        public void ItemTest()
        {
        }
    }
}
