using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    #region Fields
    [SerializeField] private string _id;
    [SerializeField] private Sprite _slotIcon;
    [SerializeField] private Image _iconImage;
    #endregion

    #region public properties
    public string Id => _id;

    public Sprite SlotIcon => _slotIcon;
    #endregion

    #region Public methods
    public void SetItem(InventoryItemData item)
    {
        _id = item != null ? item.Id : null;
        _slotIcon = item != null ? item.InventoryIcon : null;

        if (_iconImage == null)
            _iconImage = GetComponent<Image>();

        if (_iconImage != null)
        {
            _iconImage.sprite = _slotIcon;
            _iconImage.enabled = _slotIcon != null;
        }
    }
    #endregion
}
