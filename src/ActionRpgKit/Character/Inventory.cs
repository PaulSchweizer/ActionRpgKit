using System;
using System.Collections.Generic;
using System.Linq;
using ActionRpgKit.Item;
using System.Runtime.Serialization;

namespace ActionRpgKit.Character
{

    #region Interfaces

    /// <summary>
    /// An inventory holds Items.</summary>
    [Serializable]
    public abstract class IInventory
    {
        public abstract IEnumerable<int> Items { get; set; }
        public abstract IEnumerable<int> Quantities { get; set; }
        public abstract void AddItem (int itemId, int quantity = 1);
        public abstract void RemoveItem (int itemId, int quantity = 1);
        public abstract int ItemCount { get; }
        public abstract int GetQuantity (int itemId);
    }

    #endregion

    #region Implementations

    /// <summary>
    /// This inventory holds afixed size array of items.</summary>
    [Serializable]
    public class SimpleInventory : IInventory
    {
        private int[] _items;
        private int[] _quantities;

        public SimpleInventory() { }

        public SimpleInventory(int[] items, int[] quantities)
        {
            Items = items;
            Quantities = quantities;
        }

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

        public override int ItemCount
        {
            get
            {
                return _items.Length;
            }
        }

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

        public override void AddItem(int itemId, int quantity = 1) { }

        public override void RemoveItem(int itemId, int quantity = 1) { }

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
    public class PlayerInventory : IInventory
    {
        public List<int> _items = new List<int>();
        public List<int> _quantities = new List<int>();

        public PlayerInventory() { }

        public PlayerInventory(int[] items, int[] quantities)
        {
            Items = items;
            Quantities = quantities;
        }

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

        public override int ItemCount
        {
            get
            {
                return _items.Count;
            }
        }

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
        public void AddInventory(IInventory inventory)
        {
            for (int i = 0; i < inventory.ItemCount; i++)
            {
                AddItem(inventory.Items.ElementAt(i), inventory.Quantities.ElementAt(i));
            }
        }

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
