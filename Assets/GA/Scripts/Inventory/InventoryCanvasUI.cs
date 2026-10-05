using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryCanvasUI : MonoBehaviour
{
    #region Fields
    [SerializeField] private InventoryService _inventoryService;
    [SerializeField] private InventorySlotUI[] _inventorySlotsUI;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _prevButton;
    [SerializeField] private Image[] _elementsToDropOn;
    private int _currentPage;
    #endregion

    #region public properties
    public UnityEvent<GameObject,GameObject> CorrectDrop = new UnityEvent<GameObject,GameObject>();
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        if (_nextButton != null)
            _nextButton.onClick.AddListener(NextPage);
        if (_prevButton != null)
            _prevButton.onClick.AddListener(PrevPage);

        SetElementsToDropOn();

        RefreshSlots();
    }

    private void OnDisable()
    {
        if (_nextButton != null)
            _nextButton.onClick.RemoveListener(NextPage);
        if (_prevButton != null)
            _prevButton.onClick.RemoveListener(PrevPage);
    }
    #endregion

    #region Public methods
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
        Debug.Log("FLAG " + id);
        RefreshSlots();
    }

    public void NotifyCorrectDrop(GameObject inventoryItemGO, GameObject elementGO)
    {
        CorrectDrop?.Invoke(inventoryItemGO,elementGO);
    }
    #endregion

    #region Private methods
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

    private void SetElementsToDropOn()
    {
        if (_inventorySlotsUI == null)
            return;

        foreach (InventorySlotUI slot in _inventorySlotsUI)
        {
            if (slot == null)
                continue;

            if (slot.TryGetComponent<UIDragAndDropV2>(out var dragAndDrop))
                dragAndDrop.SetSlots(_elementsToDropOn);
        }
    }
    #endregion
}
