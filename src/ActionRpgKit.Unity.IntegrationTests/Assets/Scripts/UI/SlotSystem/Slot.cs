using UnityEngine;
using UnityEngine.EventSystems;

namespace SlotSystem
{
    /// <summary>
    /// Slot to hold a SlottableItem.</summary>
    public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        /// <summary>
        /// The current Item in this slot</summary>
        public SlottableItem Item;

        /// <summary>
        /// Whether the slot allows manipulation of the item it holds.</summary>
        public bool AllowsDrag = true;

        /// <summary>
        /// Accepted Item types for this slot.
        /// If left empty, all Item types are accepted.
        public string AcceptedItemType;

        /// <summary>
        /// An item is dropped onto this Slot.
        /// </summary>
        public void OnDrop(PointerEventData eventData)
        {
            Swap(SlottableItem._draggedItem);
        }

        /// <summary>
        /// The click handler.</summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsFree)
            {
                OnClicked();
            }
        }

        /// <summary>
        /// Virtual method called on click.</summary>
        public virtual void OnClicked() { }

        /// <summary>
        /// Weather the Slot accepts the given Item.</summary>
        public bool AcceptsItem(SlottableItem item)
        {
            if (AcceptedItemType == "" || AcceptedItemType == item.Data.GetType().Name)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Weather the Slot is free.</summary>
        public bool IsFree
        {
            get
            {
                return Item == null;
            }
        }

        /// <summary>
        /// Swap the given Item with the Item in this Slot if possible.</summary>
        public void Swap(SlottableItem item)
        {
            var currentItem = Item;
            var draggedItem = item;
            if (draggedItem == null || currentItem == draggedItem)
            {
                return;
            }
            var oldSlot = draggedItem._slot;

            if (IsFree && AcceptsItem(draggedItem) && draggedItem.AcceptsSlot(this))
            {
                Drop(draggedItem);
                oldSlot.Clear();
                return;
            }

            // Items need to be swapped
            if (AcceptsItem(draggedItem) && oldSlot.AcceptsItem(currentItem) &&
                draggedItem.AcceptsSlot(this) && currentItem.AcceptsSlot(oldSlot))
            {
                Drop(draggedItem);
                oldSlot.Drop(currentItem);
            }
        }

        /// <summary>
        /// Drop an Item onto this Slot.</summary>
        public void Drop(SlottableItem item)
        {
            var oldItem = Item;
            var newItem = item;
            item.transform.SetParent(transform);
            item.FitIntoSlot();
            ExecuteEvents.ExecuteHierarchy<ISlotChanged>(gameObject, null, (x, y) => x.SlotChanged(this, newItem, oldItem));
            Item = item;
            Item._slot = this;
        }

        /// <summary>
        /// Set the Slot free.</summary>
        public void Clear()
        {
            var oldItem = Item;
            Item = null;
            ExecuteEvents.ExecuteHierarchy<ISlotChanged>(gameObject, null, (x, y) => x.SlotChanged(this, null, oldItem));
        }
    }

    /// <summary>
    /// Slots send a change signal when their Item has been changed.</summary>
    public interface ISlotChanged : IEventSystemHandler
    {
        void SlotChanged(Slot slot,
                          SlottableItem newItem,
                          SlottableItem oldItem);
    }
}