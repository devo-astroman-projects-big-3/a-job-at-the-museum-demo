using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzleMinigameData", menuName = "Game/Puzzle/PuzzleMinigameDatabase")]
public class PuzzleMinigameDatabase : ScriptableObject
{
    [SerializeField]
    private List<PuzzleMinigameData> _items = new();

    public IReadOnlyList<PuzzleMinigameData> Items => _items;

    public PuzzleMinigameData GetById(string id)
    {
        foreach (PuzzleMinigameData item in _items)
        {
            if (item != null && item.Id == id)
                return item;
        }

        Debug.LogWarning($"PuzzleMinigameData info not found: {id}");
        return null;
    }


    
}
