using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PairButton : MonoBehaviour
{
    #region Fields
    [SerializeField] private TMP_Text _textLabel;
    [SerializeField] private Image _imageDisplay;
    private string _pairId;
    private string _text;
    private Sprite _image;
    #endregion

    #region public properties
    public string PairId
    {
        get => _pairId;
        set => _pairId = value;
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            if (_textLabel == null)
                _textLabel = GetComponentInChildren<TMP_Text>(true);
            if (_textLabel != null)
                _textLabel.text = value;
        }
    }

    public Sprite Image
    {
        get => _image;
        set
        {
            _image = value;
            if (_imageDisplay != null)
                _imageDisplay.sprite = value;
        }
    }
    #endregion
}
