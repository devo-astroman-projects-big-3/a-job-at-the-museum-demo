using UnityEngine;

[CreateAssetMenu(fileName = "NewScreenInfoPage", menuName = "Game/ScreenInfo/Page")]
public class ScreenInfoPageData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _middleImage;

    [SerializeField] private string _descriptionTitle;
    [SerializeField, TextArea]
    private string _description;


    public string Id => _id;    
    public Sprite MiddleImage => _middleImage;
    public string DescriptionTitle => _descriptionTitle;
    public string Description => _description;
}
