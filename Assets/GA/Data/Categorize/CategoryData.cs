using UnityEngine;

[CreateAssetMenu(fileName = "NewCategoryData", menuName = "Game/Category/CategoryData")]
public class CategoryData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _categoryName;
    [SerializeField] private Sprite[] _images;


    public string Id => _id;
    public string CategoryName => _categoryName;
    public Sprite[] Images => _images;

}
