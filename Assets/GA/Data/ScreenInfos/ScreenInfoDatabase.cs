using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScreenInfoDatabase", menuName = "Game/ScreenInfo/Database")]
public class ScreenInfoDatabase : ScriptableObject
{
    [SerializeField]
    private List<ScreenInfoPresentationData> _items = new();

    public IReadOnlyList<ScreenInfoPresentationData> Items => _items;

    public ScreenInfoPresentationData GetById(string id)
    {
        foreach (ScreenInfoPresentationData item in _items)
        {
            if (item != null && item.Id == id)
                return item;
        }

        Debug.LogWarning($"Screen info not found: {id}");
        return null;
    }
}
