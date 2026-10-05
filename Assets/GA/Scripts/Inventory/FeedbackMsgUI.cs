using System.Collections;
using TMPro;
using UnityEngine;

public class FeedbackMsgUI : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _feedbackMsgGO;
    [SerializeField] private TMP_Text _feedbackMsgText;
    private Coroutine _hideMessageCoroutine;
    #endregion

    #region Unity Callbacks
    private void OnDisable()
    {
        StopMessageTimer();
        if (_feedbackMsgGO != null)
            _feedbackMsgGO.SetActive(false);
    }
    #endregion

    #region Public methods
    public void ShowMsgDuringTime(string msg, int secs)
    {
        if (_feedbackMsgGO == null)
        {
            Debug.LogWarning("Feedback message GameObject is not assigned.", this);
            return;
        }

        StopMessageTimer();

        if (secs <= 0)
        {
            _feedbackMsgGO.SetActive(false);
            return;
        }

        if (_feedbackMsgText == null)
            _feedbackMsgText = _feedbackMsgGO.GetComponentInChildren<TMP_Text>(true);

        if (_feedbackMsgText == null)
        {
            Debug.LogWarning("Feedback message text is not assigned or found.", this);
            _feedbackMsgGO.SetActive(false);
            return;
        }

        _feedbackMsgText.text = msg ?? string.Empty;
        _feedbackMsgGO.SetActive(true);
        _hideMessageCoroutine = StartCoroutine(HideMessageAfter(secs));
    }

    public void ShowMsg(string msg)
    {
        ShowMsgDuringTime(msg,2);
    }

    public void ShowCorrectDefaultMsg()
    {
        ShowMsgDuringTime("Nice",2);
    }

    public void ShowWrongDefaultMsg()
    {
        ShowMsgDuringTime("It doesn't work",2);
    }
    #endregion

    #region Private methods
    private IEnumerator HideMessageAfter(int secs)
    {
        yield return new WaitForSecondsRealtime(secs);

        _hideMessageCoroutine = null;
        if (_feedbackMsgGO != null)
            _feedbackMsgGO.SetActive(false);
    }

    private void StopMessageTimer()
    {
        if (_hideMessageCoroutine == null)
            return;

        StopCoroutine(_hideMessageCoroutine);
        _hideMessageCoroutine = null;
    }
    #endregion
}
