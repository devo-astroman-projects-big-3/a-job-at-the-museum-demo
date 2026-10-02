using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionsDatabase", menuName = "Game/Exam/QuestionsDatabase")]
public class QuestionsDatabase : ScriptableObject
{
    [SerializeField]
    private List<Question> _items = new();

    public IReadOnlyList<Question> Items => _items;

    public Question GetById(string id)
    {
        foreach (Question item in _items)
        {
            if (item != null && item.Id == id)
                return item;
        }

        Debug.LogWarning($"Question info not found: {id}");
        return null;
    }
    
}
