using System;

namespace ActionRpgKit.Item
{

    #region Abstracts

    /// <summary>
    /// Basic implementation of the IItem.</summary>
    [Serializable]
    public abstract class BaseItem
    {
        public int Id;

        public string Name;

        public string Description;
    }

    #endregion

    #region Implementations

    /// <summary>
    /// This item is usable for Skills.</summary>
    [Serializable]
    public class UsableItem : BaseItem
    {

    }

    /// <summary>
    /// Weapons add to the fight statistics.</summary>
    [Serializable]
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
