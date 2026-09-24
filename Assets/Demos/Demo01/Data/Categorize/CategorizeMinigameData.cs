using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewCategorizeMinigameData", menuName = "Game/Category/CategorizeMinigameData")]
public class CategorizeMinigameData : ScriptableObject
{
    [SerializeField] private string _id;
    [FormerlySerializedAs("_categoryOne")]
    [SerializeField] private CategoryData _categoryLeft;
    [SerializeField] private int[] _categoryOneSelectedIndexes;

    [FormerlySerializedAs("_categoryTwo")]
    [SerializeField] private CategoryData _categoryRight;
    [FormerlySerializedAs("_categoryTwoSelectedIndexes")]
    [SerializeField] private int[] _categoryRightSelectedIndexes;


    public string Id => _id;
    public CategoryData CategoryLeft => _categoryLeft;
    public int[] CategoryLeftSelectedIndexes => _categoryOneSelectedIndexes;


    public CategoryData CategoryRight => _categoryRight;
    public int[] CategoryRightSelectedIndexes => _categoryRightSelectedIndexes;

}
