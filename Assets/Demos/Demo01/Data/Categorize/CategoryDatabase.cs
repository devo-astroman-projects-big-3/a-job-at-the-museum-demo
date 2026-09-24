using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCategoryDatabase", menuName = "Game/Category/NewCategoryDatabase")]
public class CategoryDatabase : ScriptableObject
{
    [SerializeField]
    private List<CategoryData> _items = new();

    public IReadOnlyList<CategoryData> Items => _items;

    public CategoryData GetById(string id)
    {
        foreach (CategoryData item in _items)
        {
            if (item != null && item.Id == id)
                return item;
        }

        Debug.LogWarning($"Category info not found: {id}");
        return null;
    }


    
}
