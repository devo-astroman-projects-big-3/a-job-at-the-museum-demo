using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PairButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
    IInitializePotentialDragHandler, IDragHandler
{    
    [SerializeField] private TMP_Text _textLabel;
    [SerializeField] private Image _imageDisplay;
    
    private ConnectPairsCanvasUI _owner;
    private string _pairId;
    private string _text;
    private Sprite _image;

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

    public void Bind(ConnectPairsCanvasUI owner) => _owner = owner;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_owner == null)
            _owner = GetComponentInParent<ConnectPairsCanvasUI>();
        if (_owner != null)
            _owner.BeginPointerConnection(this, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_owner != null)
            _owner.EndPointerConnection(eventData);
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Keep the drag target on this button until PointerUp.
    }

}
