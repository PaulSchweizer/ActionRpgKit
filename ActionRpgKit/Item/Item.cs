using System;

namespace ActionRpgKit.Item
{
    public interface IItem
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description{ get; set; }
    }

    public abstract class BaseItem : IItem
    {
        public int Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }
    }

    public class UsableItem : BaseItem
    {

    }

    /// <summary>
    /// Holds all the Items available in the Game.</summary>
    public static class ItemDatabase
    {
        /// <summary>
        /// The index of the array corresponds to the id of the IItem.</summary>
        public static IItem[] Items
        {
            get; set;
        }

        /// <summary>
        /// Retrieve the IItem by Name.</summary>
        public static IItem GetItemByName(string name)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].Name == name)
                {
                    return Items[i];
                }
            }
            return null;
        }
    }
}
