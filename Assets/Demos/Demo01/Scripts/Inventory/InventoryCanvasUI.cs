using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvasUI : MonoBehaviour
{
    
    [SerializeField] private InventoryService _inventoryService;
    [SerializeField] private InventorySlotUI[] _inventorySlotsUI;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _prevButton;

    private int _currentPage;


    private void OnEnable()
    {
        if (_nextButton != null)
            _nextButton.onClick.AddListener(NextPage);
        if (_prevButton != null)
            _prevButton.onClick.AddListener(PrevPage);

        RefreshSlots();
    }

    private void OnDisable()
    {
        if (_nextButton != null)
            _nextButton.onClick.RemoveListener(NextPage);
        if (_prevButton != null)
            _prevButton.onClick.RemoveListener(PrevPage);
    }

    public void NextPage()
    {
        _currentPage++;
        RefreshSlots();
    }

    public void PrevPage()
    {
        _currentPage--;
        RefreshSlots();
    }

    public void AddItem(string id)
    {
        if (_inventoryService == null)
            return;

        _inventoryService.AddItem(id);
        RefreshSlots();
    }

    public void DropItem(string id)
    {
        if (_inventoryService == null)
            return;

        _inventoryService.RemoveItem(id);
        RefreshSlots();
    }

    private void RefreshSlots()
    {
        int slotCount = _inventorySlotsUI == null ? 0 : _inventorySlotsUI.Length;
        int itemCount = _inventoryService == null ? 0 : _inventoryService.Items.Count;
        int lastPage = slotCount > 0 && itemCount > 0 ? (itemCount - 1) / slotCount : 0;
        _currentPage = Mathf.Clamp(_currentPage, 0, lastPage);

        int startIndex = _currentPage * slotCount;
        for (int i = 0; i < slotCount; i++)
        {
            InventorySlotUI slot = _inventorySlotsUI[i];
            if (slot == null)
                continue;

            int itemIndex = startIndex + i;
            slot.SetItem(itemIndex < itemCount ? _inventoryService.Items[itemIndex] : null);
        }

        if (_nextButton != null)
            _nextButton.interactable = _currentPage < lastPage;
        if (_prevButton != null)
            _prevButton.interactable = _currentPage > 0;
    }

}
