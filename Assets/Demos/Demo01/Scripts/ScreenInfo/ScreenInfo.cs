using UnityEngine;

public class ScreenInfo : MonoBehaviour
{
    
    [SerializeField] private GameObject _screenInfoGO;
    [SerializeField] private ScreenInfoDatabase _screenInfoDatabase;
    [SerializeField] private ScreenInfoCanvasUI _screenInfoCanvasUI;

    
    private GameObject _canvasGO;

    public void PlayPresentation(string idInfo)
    {
        _screenInfoGO.SetActive(true);
        ScreenInfoPresentationData screenInfoPresentationData = _screenInfoDatabase.GetById(idInfo);

        _screenInfoCanvasUI.SetInfoScreenInfo(screenInfoPresentationData);
    }

    public void ClosePresentation()
    {
        _screenInfoGO.SetActive(false);
    }

}