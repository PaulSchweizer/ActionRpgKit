using System;

namespace ActionRpgKit.Item
{
    /// <summary>
    /// An Item is something that is usable by Characters.</summary>
    public interface IItem
    {
        /// <summary>
        /// The id is used to identify the Item and possibly link it to 
        /// a database of any kind.</summary>
        int Id { get; set; }
        
        /// <summary>
        /// A preferrably unique name.</summary>
        string Name { get; set; }
        
        /// <summary>
        /// The description can be shown in the Game to provide further information.</summary>
        string Description{ get; set; }
    }

    /// <summary>
    /// Basic implementation of the IItem.</summary>
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

    /// <summary>
    /// This item is usable for Skills.</summary>
    public class UsableItem : BaseItem
    {

    }
}
