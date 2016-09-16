using System;

namespace ActionRpgKit.Item
{
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
