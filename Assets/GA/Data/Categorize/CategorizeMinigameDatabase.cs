using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCategorizeMinigameDatabase", menuName = "Game/Category/NewCategorizeMinigameDatabase")]
public class CategorizeMinigameDatabase : ScriptableObject
{
    [SerializeField]
    private List<CategorizeMinigameData> _items = new();

    public IReadOnlyList<CategorizeMinigameData> Items => _items;

    public CategorizeMinigameData GetById(string id)
    {
        foreach (CategorizeMinigameData item in _items)
        {
            if (item != null && item.Id == id)
                return item;
        }

        Debug.LogWarning($"Category info not found: {id}");
        return null;
    }


    
}
