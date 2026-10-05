using UnityEngine;

public class ScreenInfo : MonoBehaviour
{
    
    [SerializeField] private GameObject _screenInfoGO;
    [SerializeField] private ScreenInfoDatabase _screenInfoDatabase;
    [SerializeField] private ScreenInfoCanvasUI _screenInfoCanvasUI;
    

    
    private GameObject _canvasGO;

    public void PlayPresentation(string idInfo)
    {
        ScreenInfoPresentationData screenInfoPresentationData = _screenInfoDatabase.GetById(idInfo);
        if (screenInfoPresentationData == null)
            return;

        _screenInfoCanvasUI.SetCurrentRunningId(idInfo);
        _screenInfoCanvasUI.SetInfoScreenInfo(screenInfoPresentationData);
        _screenInfoGO.SetActive(true);
    }

    public void ClosePresentation()
    {
        _screenInfoGO.SetActive(false);
    }

}
