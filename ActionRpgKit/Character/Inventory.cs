using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionRpgKit.Item;

namespace ActionRpgKit.Character
{
    /// <summary>
    /// An inventory holds Items.
    /// </summary>
    public interface IInventory
    {
        IEnumerable<IItem> Items { get; set; } 
    }

    /// <summary>
    /// This inventory holds afixed size array of items.
    /// </summary>
    public class SimpleInventory : IInventory
    {
        private IItem[] _items;

        IEnumerable<IItem> IInventory.Items
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
    }

    /// <summary>
    /// Inventory allows to add and remove items.
    /// </summary>
    public class PlayerInventory : IInventory
    {
        private List<IItem> _items;

        IEnumerable<IItem> IInventory.Items
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
    }
}
