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
        public abstract IEnumerable<BaseItem> Items { get; set; }
        public abstract IEnumerable<int> Quantities { get; set; }
        public abstract void AddItem (BaseItem item, int quantity = 1);
        public abstract void RemoveItem (BaseItem item, int quantity = 1);
        public abstract int ItemCount { get; }
        public abstract int GetQuantity (BaseItem item);
    }

    #endregion

    #region Implementations

    /// <summary>
    /// This inventory holds afixed size array of items.</summary>
    [Serializable]
    public class SimpleInventory : IInventory
    {
        private BaseItem[] _items;
        private int[] _quantities;

        public SimpleInventory() { }

        public SimpleInventory(BaseItem[] items, int[] quantities)
        {
            Items = items;
            Quantities = quantities;
        }

        public override IEnumerable<BaseItem> Items
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

        public override int GetQuantity(BaseItem item)
        {
            int index = Array.IndexOf(_items, item);
            if (index > -1)
            {
                return _quantities[index];
            }
            else
            {
                return 0; 
            }
        }

        public override void AddItem(BaseItem item, int quantity = 1) { }

        public override void RemoveItem(BaseItem item, int quantity = 1) { }

        public override string ToString()
        {
            string repr = "";
            repr += string.Format("_SimpleInventory______\n");
            for (int i = 0; i < ItemCount; i++)
            {
                repr += string.Format("|{0, -15}|{1, 4:D4}|\n",
                                      _items[i].Name, _quantities[i]);
                repr += string.Format("+---------------+----+\n");
            }
            return repr;
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            int i = 0;
            foreach(var item in _items)
            {
                _items[i] = ItemDatabase.GetItemById(item.Id);
                i++;
            }
        }
    }

    /// <summary>
    /// Inventory allows to add and remove items.</summary>
    [Serializable]
    public class PlayerInventory : IInventory
    {
        public List<BaseItem> _items = new List<BaseItem>();
        public List<int> _quantities = new List<int>();

        public PlayerInventory() { }

        public PlayerInventory(BaseItem[] items, int[] quantities)
        {
            Items = items;
            Quantities = quantities;
        }

        public override IEnumerable<BaseItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value.ToList<BaseItem>();
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

        public override int GetQuantity(BaseItem item)
        {
            if (_items.Contains(item))
            {
                return _quantities[_items.IndexOf(item)];
            }
            else
            {
                return 0; 
            }
        }

        public override void AddItem(BaseItem item, int quantity = 1)
        {
            if (_items.Contains(item))
            {
                _quantities[_items.IndexOf(item)] += quantity;
            }
            else
            {
                _items.Add(item);
                _quantities.Add(quantity);
            }
        }

        public override void RemoveItem(BaseItem item, int quantity = 1)
        {
            if (_items.Contains(item))
            {
                _quantities[_items.IndexOf(item)] -= quantity;
                if (_quantities[_items.IndexOf(item)] < 1)
                {
                    _quantities.RemoveAt(_items.IndexOf(item));
                    _items.Remove(item);
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
                                      _items[i].Name, _quantities[i]);
                repr += string.Format("+---------------+----+\n");
            }
            return repr;
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            int i = 0;
            foreach (var item in _items.ToList())
            {
                _items[i] = ItemDatabase.GetItemById(item.Id);
                i++;
            }
        }
    }

    #endregion
}
