using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionRpgKit.Item;

namespace ActionRpgKit.Character
{
    /// <summary>
    /// An inventory holds Items.</summary>
    public interface IInventory
    {
        IEnumerable<IItem> Items { get; set; }
        IEnumerable<int> Quantities { get; set; }
        void AddItem (IItem item, int quantity);
        void RemoveItem (IItem item, int quantity);
        int ItemCount { get; }
        int GetQuantity (IItem item);
    }

    /// <summary>
    /// This inventory holds afixed size array of items.</summary>
    public class SimpleInventory : IInventory
    {
        private IItem[] _items;
        private int[] _quantities;

        public SimpleInventory () { }

        public SimpleInventory (IItem[] items, int[] quantities)
        {
            Items = items;
            Quantities = quantities;
        }

        public IEnumerable<IItem> Items
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

        public IEnumerable<int> Quantities
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

        public int ItemCount
        {
            get
            {
                return _items.Length;
            }
        }

        public int GetQuantity (IItem item)
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

        public void AddItem (IItem item, int quantity = 1)
        {
            
        }

        public void RemoveItem (IItem item, int quantity = 1)
        {
            
        }

        public override string ToString()
        {
            string repr = "";
            repr += string.Format("_SimpleInventory______\n");
            for (int i = 0; i < ItemCount; i++)
            {
                repr += string.Format("|{0, -15}|{1, 4}|\n",
                                      _items[i].Name, _quantities[i]);
                repr += string.Format("---------------+------\n");
            }
            return repr;
        }
    }

    /// <summary>
    /// Inventory allows to add and remove items.</summary>
    public class PlayerInventory : IInventory
    {
        private List<IItem> _items = new List<IItem>();
        private List<int> _quantities = new List<int>();

        public IEnumerable<IItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value.ToList<IItem>();
            }
        }

        public IEnumerable<int> Quantities
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

        public int ItemCount
        {
            get
            {
                return _items.Count;
            }
        }

        public int GetQuantity (IItem item)
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

        public void AddItem (IItem item, int quantity = 1)
        {
            if (_items.Contains(item))
            {
                _quantities[_items.IndexOf(item)] += 1;
            }
            else
            {
                _items.Add(item);
                _quantities.Add(quantity);
            }
        }

        public void RemoveItem (IItem item, int quantity = 1)
        {
            if (_items.Contains(item))
            {
                _quantities[_items.IndexOf(item)] -= 1;
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
                repr += string.Format("|{0, -15}|{1, 4}|\n", 
                                      _items[i].Name, _quantities[i]);
                repr += string.Format("----------------------\n");
            }
            return repr;
        }
    }
}
