using UnityEngine;

public class InventoryMinigame : MonoBehaviour
{    
    
    [SerializeField] private InventoryMinigameCanvasUI[] _inventoryMinigameCanvasUI;

    public void InitInventoryMinigame(string id)
    {
        if (_inventoryMinigameCanvasUI == null || string.IsNullOrEmpty(id))
            return;

        foreach (InventoryMinigameCanvasUI canvasUI in _inventoryMinigameCanvasUI)
        {
            if (canvasUI != null && canvasUI.Id == id)
                canvasUI.gameObject.SetActive(true);
        }

    }

    public void CloseMinigame()
    {
        if (_inventoryMinigameCanvasUI == null)
            return;

        foreach (InventoryMinigameCanvasUI canvasUI in _inventoryMinigameCanvasUI)
        {
            if (canvasUI != null)
                canvasUI.gameObject.SetActive(false);
        }
    } 



}
