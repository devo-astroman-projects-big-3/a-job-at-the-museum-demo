using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPairConnectMinigameDatabase", menuName = "Game/ConnectPairs/PairConnectMinigameDatabase")]
public class PairConnectMinigameDatabase : ScriptableObject
{
    [SerializeField]
    private List<PairConnectMinigameData> _items = new();

    public IReadOnlyList<PairConnectMinigameData> Items => _items;

    public PairConnectMinigameData GetById(string id)
    {
        foreach (PairConnectMinigameData item in _items)
        {
            if (item != null && item.Id == id)
                return item;
        }

        Debug.LogWarning($"Pair info not found: {id}");
        return null;
    }


    
}
