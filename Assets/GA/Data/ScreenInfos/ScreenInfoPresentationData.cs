using UnityEngine;

[CreateAssetMenu(fileName = "NewScreenInfoPresentation", menuName = "Game/ScreenInfo/Presentation")]
public class ScreenInfoPresentationData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _topTitle;
    [SerializeField] private string _mainTitle;
    [SerializeField] private string _subTitle;
    [SerializeField] private ScreenInfoPageData[] _screenInfoPageData;


    public string Id => _id;
    public string TopTitle => _topTitle;
    public string MainTitle => _mainTitle;
    public string SubTitle => _subTitle;
    public ScreenInfoPageData[] ScreenInfoPageData => _screenInfoPageData;    
}
