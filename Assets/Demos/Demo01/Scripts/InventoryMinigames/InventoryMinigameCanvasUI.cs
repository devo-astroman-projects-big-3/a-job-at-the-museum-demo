using UnityEngine;


public class InventoryMinigameCanvasUI : MonoBehaviour
{

    [SerializeField] private ElementInventoryItemCombination[] _combinations;

    private void ProcessCombination(string inventoryItemId, string elementMinigameId)
    {
        if (!string.IsNullOrEmpty(inventoryItemId) &&
            !string.IsNullOrEmpty(elementMinigameId) && _combinations != null)
        {
            foreach (ElementInventoryItemCombination combination in _combinations)
            {
                if (combination == null || combination.InventoryItem == null ||
                    combination.ElementMinigame == null)
                    continue;

                if (combination.InventoryItem.Id == inventoryItemId &&
                    combination.ElementMinigame.Id == elementMinigameId)
                {
                    combination.NotifyCombinationMade();
                    combination.NotifyInventoryItemCombinated();
                    
                    return;
                }
            }
        }
    }

    public void ProcessDrop(GameObject inventoryItem, GameObject elementDroppedOn)
    {
        Debug.Log("InventoryMinigameCanvasUI_ProcessDrop_ ");

        string inventoryItemId = inventoryItem.GetComponent<InventorySlotUI>().Id;
        string elementDroppedOnId = elementDroppedOn.GetComponent<ElementMinigame>().Id;

        Debug.Log(inventoryItemId + " "  +elementDroppedOnId);

        ProcessCombination(inventoryItemId,elementDroppedOnId);
        
    }
}
