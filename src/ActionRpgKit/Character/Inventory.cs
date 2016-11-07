using System;
using System.Collections.Generic;
using System.Linq;
using ActionRpgKit.Item;
using System.Runtime.Serialization;

namespace ActionRpgKit.Character
{

    #region Interfaces

    /// <summary>
    /// An Inventory holds Items and their quantity.
    /// It can be thought of as a table with two columns, like this:
    /// +=======+============+
    /// | Items | Quantities |
    /// +=======+============+
    /// | Sword | 1          |
    /// +-------+------------+
    /// | Gold  | 12         |
    /// +-------+------------+
    /// </summary>
    [Serializable]
    public abstract class BaseInventory
    {
        /// <summary>
        /// The ids of the Items in the Inventory</summary>
        public abstract IEnumerable<int> Items { get; set; }

        /// <summary>
        /// The quantities of the Items in the Inventory</summary>
        public abstract IEnumerable<int> Quantities { get; set; }

        /// <summary>
        /// Add a new Item</summary>
        /// <param name="itemId">The id of the item</param>
        /// <param name="quantity">The amount of this item</param>
        public abstract void AddItem (int itemId, int quantity = 1);

        /// <summary>
        /// Remove a new Item</summary>
        /// <param name="itemId">The id of the item</param>
        /// <param name="quantity">The amount of this item</param>
        public abstract void RemoveItem (int itemId, int quantity = 1);

        /// <summary>
        /// The count of the different Items. Not the total count of items.</summary>
        public abstract int ItemCount { get; }

        /// <summary>
        /// The quantity of the given Item.</summary>
        /// <param name="itemId">The id of the Item</param>
        /// <returns>The quantity of this Item.</returns>
        public abstract int GetQuantity (int itemId);
    }

    #endregion

    #region Implementations

    /// <summary>
    /// This inventory holds a fixed size array of items.</summary>
    [Serializable]
    public class SimpleInventory : BaseInventory
    {
        /// <summary>
        /// Items are stored in an array.</summary>
        private int[] _items = new int[] { };

        /// <summary>
        /// Quantities are stored in an array.</summary>
        private int[] _quantities = new int[] { };

        public SimpleInventory() { }

        public SimpleInventory(int[] items, int[] quantities)
        {
            Items = items;
            Quantities = quantities;
        }

        /// <summary>
        /// Set the Items.</summary>
        public override IEnumerable<int> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value.ToArray();
            }
        }

        /// <summary>
        /// Set the quantities.</summary>
        public override IEnumerable<int> Quantities
        {
            get
            {
                return _quantities;
            }

            set
            {
                _quantities = value.ToArray();
            }
        }

        /// <summary>
        /// The length of the item array.</summary>
        public override int ItemCount
        {
            get
            {
                return _items.Length;
            }
        }

        /// <summary>
        /// Quantity of the given Item.</summary>
        /// <param name="itemId">The id of the Item.</param>
        /// <returns>The count of the Item.</returns>
        public override int GetQuantity(int itemId)
        {
            int index = Array.IndexOf(_items, itemId);
            if (index > -1)
            {
                return _quantities[index];
            }
            else
            {
                return 0; 
            }
        }

        /// <summary>
        /// Add the item to the array by creating a new array of the 
        /// respective length if necessary.</summary>
        /// <param name="itemId">The id of the Item to add</param>
        /// <param name="quantity">The count of this item.</param>
        public override void AddItem(int itemId, int quantity = 1)
        {
            if (_items.Contains(itemId))
            {
                _quantities[Array.IndexOf(_items, itemId)] += quantity;
            }
            else
            {
                Array.Resize(ref _items, _items.Length + 1);
                Array.Resize(ref _quantities, _quantities.Length + 1);
                _items[_items.Length - 1] = itemId;
                _quantities[_quantities.Length - 1] = quantity;
            }

        }

        /// <summary>
        /// Remove the item from the array. Recreating the new array with the 
        /// respective length if necessary.</summary>
        /// <param name="itemId">The id of the Item to remove</param>
        /// <param name="quantity">The count of this item.</param>
        public override void RemoveItem(int itemId, int quantity = 1)
        {
            if (_items.Contains(itemId))
            {
                var index = Array.IndexOf(_items, itemId);
                _quantities[index] -= quantity;
                if (_quantities[index] < 1)
                {
                    var newItems = new int[_items.Length - 1];
                    var newQuantities = new int[_quantities.Length - 1];
                    int newIndex = 0;
                    for(int i = 0; i < _items.Length; i++)
                    {
                        if (index != i)
                        {
                            newItems[newIndex] = _items[i];
                            newQuantities[newIndex] = _quantities[i];
                            newIndex++;
                        }
                    }
                    _items = newItems;
                    _quantities = newQuantities;
                }
            }

        }

        /// <summary>
        /// Sort the items and quantities into a text based table.</summary>
        /// <returns>The pretty representation of the inventory.</returns>
        public override string ToString()
        {
            string repr = "";
            repr += string.Format("_SimpleInventory______\n");
            for (int i = 0; i < ItemCount; i++)
            {
                repr += string.Format("|{0, -15}|{1, 4:D4}|\n",
                                      _items[i], _quantities[i]);
                repr += string.Format("+---------------+----+\n");
            }
            return repr;
        }
    }

    /// <summary>
    /// Inventory allows to add and remove items.</summary>
    [Serializable]
    public class PlayerInventory : BaseInventory
    {
        /// <summary>
        /// Items are stored in a list.</summary>
        public List<int> _items = new List<int>();

        /// <summary>
        /// Quantities are stored in a list.</summary>
        public List<int> _quantities = new List<int>();

        public PlayerInventory() { }

        public PlayerInventory(int[] items, int[] quantities)
        {
            Items = items;
            Quantities = quantities;
        }

        /// <summary>
        /// Items are stored in a list.</summary>
        public override IEnumerable<int> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value.ToList<int>();
            }
        }

        /// <summary>
        /// Quantities are stored in a list.</summary>
        public override IEnumerable<int> Quantities
        {
            get
            {
                return _quantities;
            }

            set
            {
                _quantities = value.ToList<int>();
            }
        }

        /// <summary>
        /// Count of the item list.</summary>
        public override int ItemCount
        {
            get
            {
                return _items.Count;
            }
        }

        /// <summary>
        /// Amount of the given Item in the Inventory.</summary>
        /// <param name="itemId">The Item id</param>
        /// <returns>The amount of the Item in the Inventory.</returns>
        public override int GetQuantity(int itemId)
        {
            if (_items.Contains(itemId))
            {
                return _quantities[_items.IndexOf(itemId)];
            }
            else
            {
                return 0; 
            }
        }

        /// <summary>
        /// Add the item to the list.</summary>
        /// <param name="itemId">The id of the Item to add</param>
        /// <param name="quantity">The count of this item.</param>
        public override void AddItem(int itemId, int quantity = 1)
        {
            if (_items.Contains(itemId))
            {
                _quantities[_items.IndexOf(itemId)] += quantity;
            }
            else
            {
                _items.Add(itemId);
                _quantities.Add(quantity);
            }
        }

        /// <summary>
        /// Remove the item from the list.</summary>
        /// <param name="itemId">The id of the Item to remove</param>
        /// <param name="quantity">The count of this item.</param>
        public override void RemoveItem(int itemId, int quantity = 1)
        {
            if (_items.Contains(itemId))
            {
                _quantities[_items.IndexOf(itemId)] -= quantity;
                if (_quantities[_items.IndexOf(itemId)] < 1)
                {
                    _quantities.RemoveAt(_items.IndexOf(itemId));
                    _items.Remove(itemId);
                }
            }
        }

        /// <summary>
        /// Transfer all the Items from the given Inventory.</summary>
        public void AddInventory(BaseInventory inventory)
        {
            for (int i = 0; i < inventory.ItemCount; i++)
            {
                AddItem(inventory.Items.ElementAt(i), inventory.Quantities.ElementAt(i));
            }
        }

        /// <summary>
        /// Sort the items and quantities into a text based table.</summary>
        /// <returns>The pretty representation of the inventory.</returns>
        public override string ToString()
        {
            string repr = "";
            repr += string.Format("_PlayerInventory______\n");
            for (int i=0; i < ItemCount; i++)
            {
                repr += string.Format("|{0, -15}|{1, 4:D4}|\n", 
                                      _items[i], _quantities[i]);
                repr += string.Format("+---------------+----+\n");
            }
            return repr;
        }
    }

    #endregion
}
