using System;

namespace ActionRpgKit.Item
{

    #region Interfaces

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

    #endregion

    #region Abstracts

    /// <summary>
    /// Basic implementation of the IItem.</summary>
    public abstract class BaseItem : IItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    #endregion

    #region Implementations

    /// <summary>
    /// This item is usable for Skills.</summary>
    public class UsableItem : BaseItem
    {

    }

    /// <summary>
    /// Weapons add to the fight statistics.</summary>
    public class WeaponItem : BaseItem
    {
        /// <summary>
        /// The Damage dealt by the Weapon.</summary>
        public float Damage;

        /// <summary>
        /// The range modifier for the total attack range.</summary>
        public float Range;

        /// <summary>
        /// The speed modifier for the total speed.</summary>
        public float Speed;
    }

    #endregion
}
