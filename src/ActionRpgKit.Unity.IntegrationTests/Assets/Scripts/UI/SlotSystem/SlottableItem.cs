using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SlotSystem
{
    /// <summary>
    /// UI Item that fits into Slots.</summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class SlottableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// Reference to the Item.</summary>
        public ItemData Item;

        /// <summary>
        /// The ModelItem holding the quantity.</summary>
        public int Quantity;

        /// <summary>
        /// Text Field to display the name.</summary>
        public Text Name;

        /// <summary>
        /// Text Field to display the quantity of the Item.</summary>
        public Text QuantityText;

        /// <summary>
        /// The sprite image.</summary>
        public Image Image;

        /// <summary>
        /// The currently dragged Item.</summary>
        public static SlottableItem DraggedItem;

        /// <summary>
        /// The Slot this Item resides in.</summary>
        public Slot Slot;

        /// <summary>
        /// Slot at the beginning of a drag.</summary>
        private Slot _startSlot;

        /// <summary>
        /// The UI RectTransform of the Item.</summary>
        private RectTransform _rect;

        /// <summary>
        /// The Canvas Group.</summary>
        private CanvasGroup _canvasGroup;

        /// <summary>
        /// The Canvas of the Slot.</summary>
        private Canvas _canvas;

        /// <summary>
        /// Prepare the RectTransform to fit the Slot properly.</summary>
        void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            FitIntoSlot();
        }

        /// <summary>
        /// Set the RectTransform so it fits perfectly into the Slot.</summary>
        public void FitIntoSlot()
        {
            _rect.pivot = new Vector2(0, 1);
            _rect.anchorMin = new Vector2(0, 0);
            _rect.anchorMax = new Vector2(1, 1);
            transform.localScale = new Vector3(1, 1, 1);
            _rect.anchoredPosition3D = new Vector3(0, 0, 0);
            _rect.offsetMin = new Vector2(0, 0);
            _rect.offsetMax = new Vector2(0, 0);
        }

        /// <summary>
        /// Catch the current Slot.
        /// Parent the Item under the Canvas in order to have it in
        /// front of everything while dragging.</summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!Slot.AllowsDrag)
            {
                return;
            }
            _canvas = GetComponentInParent<Canvas>();
            DraggedItem = this;
            _startSlot = Slot;
            _canvasGroup.blocksRaycasts = false;
            transform.SetParent(GetComponentInParent<Canvas>().transform);
        }

        /// <summary>
        /// Update the current position to the MousePosition.</summary>
        public void OnDrag(PointerEventData eventData)
        {
            if (!Slot.AllowsDrag)
            {
                return;
            }
            var width = (_rect.rect.width * _canvas.scaleFactor) * 0.5f;
            var height = (_rect.rect.height * _canvas.scaleFactor) * 0.5f;
            transform.position = new Vector3(Input.mousePosition.x - width,
                                             Input.mousePosition.y + height,
                                             1);
        }

        /// <summary>
        /// If no new Slot was found, drop the Item back into the current Slot.</summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!Slot.AllowsDrag)
            {
                return;
            }
            DraggedItem = null;
            _canvasGroup.blocksRaycasts = true;
            if (Slot == _startSlot)
            {
                Slot.Drop(this);
            }
        }

        /// <summary>
        /// Weather the Item accepts the given Slot.</summary>
        public bool AcceptsSlot(Slot slot)
        {
            return true;
        }

        /// <summary>
        /// Initialize the visual UI elements with the information from the
        /// Item</summary>
        public void Init(ItemData item, int quantity)
        {
            Item = item;
            Quantity = quantity;
            UpdateDisplay();
        }

        /// <summary>
        /// Refresh the display of the Item. Called when the quantity of
        /// the Item has changed.</summary>
        public void UpdateDisplay()
        {
            if (Item is UsableItemData)
            {
                var item = (UsableItemData)Item;
                Name.text = item.Item.Name;
            }
            else if (Item is WeaponItemData)
            {
                var item = (WeaponItemData)Item;
                Name.text = item.Item.Name;
            }
            QuantityText.text = Quantity.ToString();
            Image.sprite = Item.Sprite;
        }
    }
}
