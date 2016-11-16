using UnityEngine;
using System.Collections.Generic;
using ActionRpgKit.Item;

namespace SlotSystem
{
    /// <summary>
    /// View for the SlotSystem.</summary>
    public class SlotView : MonoBehaviour, ISlotChanged
    {
        /// <summary>
        /// Number of available Slots</summary>
        public int _numberOfSlots;

        /// <summary>
        /// Slot Prefab</summary>
        public bool _infiniteSlots;

        /// <summary>
        /// Slot Prefab</summary>
        public Slot _slot;

        /// <summary>
        /// Instantiated Slots</summary>
        protected readonly List<Slot> _slots = new List<Slot>();

        /// <summary>
        /// TParent for the Slots.</summary>
        public Transform _slotParent;

        /// <summary>
        /// Prefab for the Item.</summary>
        public SlottableItem _viewItem;

        /// <summary>
        /// A slot for the equipped weapon.</summary>
        public Slot _weaponSlot;

        /// <summary>
        /// To keep track of the available Items in the View.</summary>
        public Dictionary<ItemData, SlottableItem> _items = new Dictionary<ItemData, SlottableItem>() { };

        /// <summary>
        /// Instantiate the Slots</summary>
        void Awake()
        {
            for (int i = 0; i < _numberOfSlots; i++)
            {
                AddSlot();
            }
        }

        /// <summary>
        /// Reset all the slots.</summary>
        public void ResetSlots()
        {
            foreach (Slot slot in _slots)
            {
                Destroy(slot.Item);
                Destroy(slot.gameObject);
            }
            _slots.Clear();
            for (int i = 0; i < _numberOfSlots; i++)
            {
                AddSlot();
            }
        }

        /// <summary>
        /// Add a Slot to the View.</summary>
        protected Slot AddSlot()
        {
            Slot slot = Instantiate(_slot);
            _slots.Add(slot);
            slot.transform.SetParent(_slotParent);
            slot.transform.localScale = new Vector3(1, 1, 1);
            slot.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            return slot;
        }

        /// <summary>
        /// Return the next available Slot or null if all Slots are taken.
        /// Overridden to always add a new Slot if the number of items is
        /// bigger than the number of Slots in the view.</summary>
        public Slot NextAvailableSlot()
        {
            if (_items.Count > _numberOfSlots)
            {
                return _infiniteSlots ? AddSlot() : null;
            }
            foreach (Slot slot in _slots)
            {
                if (slot.Item == null)
                {
                    return slot;
                }
            }
            return _infiniteSlots ? AddSlot() : null;
        }

        /// <summary>
        /// Add an Item to the next available Slot.</summary>
        public virtual void AddItem(SlottableItem item)
        {
            Slot slot = NextAvailableSlot();
            if (slot != null)
            {
                slot.Drop(item);
            }
        }

        /// <summary>
        /// Clear the entire View by destroying all slot objects.</summary>
        public void ClearView()
        {
            foreach (Slot slot in _slots)
            {
                if (!slot.IsFree)
                {
                    GameObject.Destroy(slot.Item.gameObject);
                    slot.Clear();
                }
            }
        }

        /// <summary>
        /// Update the Character in case the Item is placed in a special
        /// Slot, like the Weapon Slot.</summary>
        public void SlotChanged(Slot slot, SlottableItem currentItem, SlottableItem previousItem)
        {
            if (slot == _weaponSlot)
            {
                var item = (WeaponItem)currentItem.Data;
                GamePlayer.Instance.Character.EquippedWeapon = item.Id;
            }
        }
    }
}














    ///// <summary>
    ///// Add an Item to the View.
    ///// If it already exists, update the quantity.</summary>
    //public InventoryViewItem AddItem(InventoryItem item, int quantity)
    //{
    //    InventoryViewItem viewItem;
    //    if (_items.TryGetValue(item, out viewItem))
    //    {
    //        viewItem._quantity = quantity;
    //        viewItem.UpdateDisplay();
    //    }
    //    else
    //    {
    //        viewItem = Instantiate(_viewItem);
    //        viewItem.Init(item, quantity);
    //        _items[item] = viewItem;
    //        AddItem(viewItem);
    //    }
    //    return viewItem;
    //}

    ///// <summary>
    ///// Delete the given Item.</summary>
    //public void RemoveItem(InventoryItem item)
    //{
    //    var invItem = _items[item];
    //    _items.Remove(item);
    //    Destroy(invItem.gameObject);
    //}

    ///// <summary>
    ///// Change and updates the Display data on the Item.</summary>
    //public void ChangeItemData(InventoryItem item, int quantity)
    //{
    //    var invItem = _items[item];
    //    invItem._quantity = quantity;
    //    invItem.UpdateDisplay();
    //}

    ///// <summary>
    ///// Reset and initialize the entire view based on the given Inventory.</summary>
    //public void InitFromInventory(Inventory inventory)
    //{
    //    foreach (KeyValuePair<InventoryItem, InventoryViewItem> entry in _items)
    //    {
    //        Destroy(entry.Value.gameObject);
    //    }
    //    _items.Clear();
    //    _numberOfSlots = inventory._items.Count;
    //    ResetSlots();
    //    int currentActionSlot = 0;
    //    for (int i = 0; i < inventory._items.Count; i++)
    //    {
    //        var invItem = AddItem(inventory._items[i], inventory._quantities[i]);
    //        // Update the weapon Slot
    //        if (inventory._items[i] == Player._player.Stats.EquippedWeapon)
    //        {
    //            _weaponSlot.Swap(invItem);
    //        }
    //        if (Player._player.Stats._equippedItems.Contains(inventory._items[i]))
    //        {
    //            PlayerMenu._playerMenu._actionSlots[currentActionSlot]._allowsDrag = true;
    //            PlayerMenu._playerMenu._actionSlots[currentActionSlot].Swap(invItem);
    //            PlayerMenu._playerMenu._actionSlots[currentActionSlot]._allowsDrag = false;
    //            currentActionSlot += 1;
    //        }
    //    }
    //}
