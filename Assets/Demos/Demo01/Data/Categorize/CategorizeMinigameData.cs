using UnityEngine;

[CreateAssetMenu(fileName = "NewCategorizeMinigameData", menuName = "Game/Category/CategorizeMinigameData")]
public class CategorizeMinigameData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private CategoryData _categoryOne;
    [SerializeField] private CategoryData _categoryTwo;


    public string Id => _id;
    public CategoryData CategoryOne => _categoryOne;
    public CategoryData CategoryTwo => _categoryTwo;
    

}
