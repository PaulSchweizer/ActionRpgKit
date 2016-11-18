using UnityEngine;
using System.Collections.Generic;
using ActionRpgKit.Item;
using ActionRpgKit.Character;

namespace SlotSystem
{
    /// <summary>
    /// View for the SlotSystem.</summary>
    public class SlotView : MonoBehaviour, ISlotChanged
    {
        /// <summary>
        /// Number of available Slots</summary>
        public int NumberOfSlots;

        /// <summary>
        /// Slot Prefab</summary>
        public bool InfiniteSlots;

        /// <summary>
        /// Slot Prefab</summary>
        public Slot Slot;

        /// <summary>
        /// Instantiated Slots</summary>
        private readonly List<Slot> _slots = new List<Slot>();

        /// <summary>
        /// The Parent for the Slots.</summary>
        public Transform SlotParent;

        /// <summary>
        /// Prefab for the Item.</summary>
        public SlottableItem ViewItem;

        /// <summary>
        /// A slot for the equipped weapon.</summary>
        public Slot WeaponSlot;

        /// <summary>
        /// To keep track of the available Items in the View.</summary>
        public Dictionary<int, SlottableItem> _items = new Dictionary<int, SlottableItem>() { };

        /// <summary>
        /// Instantiate the Slots</summary>
        void Start()
        {
            for (int i = 0; i < NumberOfSlots; i++)
            {
                AddSlot();
            }

            InitFromInventory(GamePlayer.Instance.Character.Inventory);
        }
        
        /// <summary>
        /// Reset and initialize the entire view based on the given Inventory.</summary>
        private void InitFromInventory(BaseInventory inventory)
        {
            // Reset the inventory
            foreach (KeyValuePair<int, SlottableItem> entry in _items)
            {
                Destroy(entry.Value.gameObject);
            }
            _items.Clear();
            NumberOfSlots = inventory.ItemCount;
            ResetSlots();

            // Add the Items from the Inventory
            var items = inventory.Items.GetEnumerator();
            var quantities = inventory.Quantities.GetEnumerator();
            while (items.MoveNext() && quantities.MoveNext())
            {
                var invItem = AddItem(items.Current, quantities.Current);
            }



            //int currentActionSlot = 0;
            //for (int i = 0; i < inventory._items.Count; i++)
            //{
                //// Update the weapon Slot
                //if (inventory._items[i] == Player._player.Stats.EquippedWeapon)
                //{
                //    _weaponSlot.Swap(invItem);
                //}
                //if (Player._player.Stats._equippedItems.Contains(inventory._items[i]))
                //{
                //    PlayerMenu._playerMenu._actionSlots[currentActionSlot]._allowsDrag = true;
                //    PlayerMenu._playerMenu._actionSlots[currentActionSlot].Swap(invItem);
                //    PlayerMenu._playerMenu._actionSlots[currentActionSlot]._allowsDrag = false;
                //    currentActionSlot += 1;
                //}
            //}
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
            for (int i = 0; i < NumberOfSlots; i++)
            {
                AddSlot();
            }
        }

        /// <summary>
        /// Add a Slot to the View.</summary>
        protected Slot AddSlot()
        {
            Slot slot = Instantiate(Slot);
            _slots.Add(slot);
            slot.transform.SetParent(SlotParent);
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
            if (_items.Count > NumberOfSlots)
            {
                return InfiniteSlots ? AddSlot() : null;
            }
            foreach (Slot slot in _slots)
            {
                if (slot.Item == null)
                {
                    return slot;
                }
            }
            return InfiniteSlots ? AddSlot() : null;
        }

        /// <summary>
        /// Add an Item to the next available Slot.</summary>
        public void AddItem(SlottableItem item)
        {
            Slot slot = NextAvailableSlot();
            if (slot != null)
            {
                slot.Drop(item);
            }
        }


        /// <summary>
        /// Add an Item to the View.
        /// If it already exists, update the quantity.</summary>
        public SlottableItem AddItem(int itemId, int quantity)
        {
            SlottableItem viewItem;
            if (_items.TryGetValue(itemId, out viewItem))
            {
                viewItem.Quantity = quantity;
                viewItem.UpdateDisplay();
            }
            else
            {
                var item = ActionRpgKitController.Instance.ItemDatabase.Items[itemId];
                viewItem = Instantiate(ViewItem);
                viewItem.Init(item, quantity);
                _items[itemId] = viewItem;
                AddItem(viewItem);
            }
            return viewItem;
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
            if (slot == WeaponSlot)
            {
                if (currentItem == null)
                {
                    GamePlayer.Instance.Character.EquippedWeapon = -1;
                    return;
                }
                if (currentItem.Item is WeaponItemData)
                {
                    var itemData = (WeaponItemData)currentItem.Item;
                    var item = itemData.Item;
                    GamePlayer.Instance.Character.EquippedWeapon = item.Id;
                }
            }
        }
    }
}
















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


