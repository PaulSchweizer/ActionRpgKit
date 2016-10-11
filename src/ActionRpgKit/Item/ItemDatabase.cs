using System;

namespace ActionRpgKit.Item
{
    /// <summary>
    /// Holds all the Items available in the Game.</summary>
    public static class ItemDatabase
    {
        /// <summary>
        /// The index of the array corresponds to the id of the IItem.</summary>
        public static IItem[] Items { get; set; }

        /// <summary>
        /// Retrieve the IItem by Id.</summary>
        public static IItem GetItemById(int id)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].Id == id)
                {
                    return Items[i];
                }
            }
            return null;
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

        /// <summary>
        /// Pretty representation of the ItemDatabase.</summary>
        public static new string ToString()
        {
            string repr = "*ItemDatabase:*\n";
            repr += "=============================\n";
            repr += string.Format("|{0, -4} | {1, -20}|\n", "*Id*", "*Name*");
            repr += "=============================\n";
            for (int i = 0; i < Items.Length; i++)
            {
                repr += string.Format("|{0, -4} | {1, -20}|\n", Items[i].Id, Items[i].Name);
                repr += "-----------------------------\n";
            }
            return repr;
        }
    }
}
