using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPairDatabase", menuName = "Game/ConnectPairs/PairDatabase")]
public class PairDatabase : ScriptableObject
{
    [SerializeField]
    private List<PairData> _items = new();

    public IReadOnlyList<PairData> Items => _items;

    public PairData GetById(string id)
    {
        foreach (PairData item in _items)
        {
            if (item != null && item.Id == id)
                return item;
        }

        Debug.LogWarning($"Pair info not found: {id}");
        return null;
    }
}
