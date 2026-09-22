using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScreenInfoCanvasUI : MonoBehaviour
{
    
    [SerializeField] private TMP_Text _topTitleLabel;
    [SerializeField] private TMP_Text _mainTitleLabel;
    [SerializeField] private TMP_Text _subTitleLabel;
    [SerializeField] private Image _middleImage;
    [SerializeField] private TMP_Text _descriptionTitleLabel;
    [SerializeField] private TMP_Text _descriptionLabel;

    [SerializeField] private Button _leftPageButton;
    [SerializeField] private Button _rightPageButton;

    public UnityEvent OnClose;

    private ScreenInfoPresentationData _presentation;
    private int _currentPageIndex;

    public void SetInfoScreenInfo(ScreenInfoPresentationData screenInfoPresentationData)
    {
        ScreenInfoPageData[] pages = screenInfoPresentationData.ScreenInfoPageData;

        _presentation = screenInfoPresentationData;
        _currentPageIndex = 0;
        _topTitleLabel.text = _presentation.TopTitle;
        _mainTitleLabel.text = _presentation.MainTitle;
        _subTitleLabel.text = _presentation.SubTitle;
        ShowPage(pages[_currentPageIndex]);
        SetPageButtonStates(false, _currentPageIndex < pages.Length - 1);
    }

    public void NextPage()
    {
        ScreenInfoPageData[] pages = _presentation.ScreenInfoPageData;
        int nextPageIndex = _currentPageIndex + 1;
        if (pages == null || nextPageIndex >= pages.Length)
            return;

        if (pages[nextPageIndex] == null)
        {
            Debug.LogWarning($"Screen info page at index {nextPageIndex} is missing.", this);
            return;
        }

        _currentPageIndex = nextPageIndex;
        ShowPage(pages[_currentPageIndex]);
        SetPageButtonStates(_currentPageIndex > 0, _currentPageIndex < pages.Length - 1);
    }
    public void PrevPage()
    {

        ScreenInfoPageData[] pages = _presentation.ScreenInfoPageData;
        int previousPageIndex = _currentPageIndex - 1;
        if (pages == null || previousPageIndex < 0 || previousPageIndex >= pages.Length)
            return;

        if (pages[previousPageIndex] == null)
        {
            Debug.LogWarning($"Screen info page at index {previousPageIndex} is missing.", this);
            return;
        }

        _currentPageIndex = previousPageIndex;
        ShowPage(pages[_currentPageIndex]);
        SetPageButtonStates(_currentPageIndex > 0, _currentPageIndex < pages.Length - 1);
    }

    private void ShowPage(ScreenInfoPageData page)
    {
        _middleImage.sprite = page.MiddleImage;
        _middleImage.enabled = page.MiddleImage != null;
        _descriptionTitleLabel.text = page.DescriptionTitle;
        _descriptionLabel.text = page.Description;
    }

    public void Close()
    {
        OnClose?.Invoke();
    }

    public void SetPageButtonStates(bool leftInteractable, bool rightInteractable)
    {
        _leftPageButton.interactable = leftInteractable;

        _rightPageButton.interactable = rightInteractable;
    }
}
