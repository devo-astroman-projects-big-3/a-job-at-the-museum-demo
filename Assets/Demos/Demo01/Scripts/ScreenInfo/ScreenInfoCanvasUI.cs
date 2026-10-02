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

    public UnityEvent<string> OnAllPagesView; //when invoked should send _currentRunningId

    private ScreenInfoPresentationData _presentation;
    private int _currentPageIndex;

    private string _currentRunningId;
    private bool[] _viewedPages;
    private int _viewedPageCount;
    private bool _allPagesViewNotified;

    private void OnEnable()
    {
        MarkCurrentPageViewed();
    }

    public void SetCurrentRunningId(string currentRunningId)
    {
        _currentRunningId = currentRunningId;
    }

    public void SetInfoScreenInfo(ScreenInfoPresentationData screenInfoPresentationData)
    {
        ScreenInfoPageData[] pages = screenInfoPresentationData.ScreenInfoPageData;

        _presentation = screenInfoPresentationData;
        _currentPageIndex = 0;
        _viewedPages = pages != null ? new bool[pages.Length] : null;
        _viewedPageCount = 0;
        _allPagesViewNotified = false;
        if (pages == null || pages.Length == 0 || pages[0] == null)
        {
            SetPageButtonStates(false, false);
            Debug.LogWarning("Screen info presentation has no valid first page.", this);
            return;
        }
        _topTitleLabel.text = _presentation.TopTitle;
        _mainTitleLabel.text = _presentation.MainTitle;
        _subTitleLabel.text = _presentation.SubTitle;
        ShowPage(pages[_currentPageIndex]);
        SetPageButtonStates(false, _currentPageIndex < pages.Length - 1);
    }

    public void NextPage()
    {
        if (_presentation == null)
            return;
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
        if (_presentation == null)
            return;

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
        MarkCurrentPageViewed();
    }

    private void MarkCurrentPageViewed()
    {
        if (!isActiveAndEnabled || _viewedPages == null || _viewedPages.Length == 0
            || _allPagesViewNotified || _viewedPages[_currentPageIndex]
            || _presentation.ScreenInfoPageData[_currentPageIndex] == null)
            return;

        _viewedPages[_currentPageIndex] = true;
        _viewedPageCount++;
        if (_viewedPageCount == _viewedPages.Length)
        {
            _allPagesViewNotified = true;
            OnAllPagesView?.Invoke(_currentRunningId);
        }
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
