using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIDragAndDrop : MonoBehaviour
{
    
    [SerializeField] private Image _imageToDrag;
    [SerializeField] private Image _slot;
    public UnityEvent<GameObject> CorrectDropUnityEvent = new UnityEvent<GameObject>();
    private RectTransform _imageRect;
    private RectTransform _dragParent;
    private EventTrigger _eventTrigger;
    private readonly List<EventTrigger.Entry> _dragEvents = new();
    private Vector3 _originalPosition;
    private Vector2 _pointerOffset;
    private bool _isDragging;
    private int _pointerId;
    private int _originalSiblingIndex;

    public void SetImageToDrag(Image imageToDrag)
    {
        Configure(imageToDrag, _slot);
    }
    public void SetSlot(Image slot)
    {
        Configure(_imageToDrag, slot);
    }

    public void Configure(Image imageToDrag, Image slot)
    {
        OnDisable();
        RemoveDragEvents();
        _imageToDrag = imageToDrag;
        _slot = slot;
        _imageRect = null;
        _dragParent = null;
        if (_imageToDrag != null && _slot != null)
            Initialize();
    }

    private void Awake()
    {
        // Runtime-added components are configured after AddComponent invokes Awake.
        if (_imageToDrag != null && _slot != null)
            Initialize();
    }

    private void Start()
    {
        if (_imageRect == null)
            Initialize();
    }

    private void Initialize()
    {
        if (_imageToDrag == null || _slot == null)
        {
            Debug.LogWarning("Assign the image to drag and its correct slot in the Inspector.", this);
            enabled = false;
            return;
        }

        _imageRect = _imageToDrag.rectTransform;
        _dragParent = _imageRect.parent as RectTransform;
        if (_dragParent == null)
        {
            Debug.LogWarning("The draggable image must have a RectTransform parent.", this);
            enabled = false;
            return;
        }

        _eventTrigger = _imageToDrag.GetComponent<EventTrigger>();
        if (_eventTrigger == null)
            _eventTrigger = _imageToDrag.gameObject.AddComponent<EventTrigger>();
        _imageToDrag.raycastTarget = true;

        AddDragEvent(EventTriggerType.BeginDrag, OnBeginDrag);
        AddDragEvent(EventTriggerType.Drag, OnDrag);
        AddDragEvent(EventTriggerType.EndDrag, OnEndDrag);
        enabled = true;
    }

    private void AddDragEvent(EventTriggerType type, UnityAction<PointerEventData> callback)
    {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(data => callback((PointerEventData)data));
        _eventTrigger.triggers.Add(entry);
        _dragEvents.Add(entry);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isActiveAndEnabled || _imageRect == null || _isDragging ||
            eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _dragParent, eventData.position, eventData.pressEventCamera, out Vector2 pointerPosition))
            return;

        _originalPosition = _imageRect.localPosition;
        _originalSiblingIndex = _imageRect.GetSiblingIndex();
        _pointerOffset = (Vector2)_originalPosition - pointerPosition;
        _pointerId = eventData.pointerId;
        _isDragging = true;
        _imageRect.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging || eventData.pointerId != _pointerId)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _dragParent, eventData.position, eventData.pressEventCamera, out Vector2 pointerPosition))
        {
            Vector2 position = pointerPosition + _pointerOffset;
            _imageRect.localPosition = new Vector3(position.x, position.y, _originalPosition.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDragging || eventData.pointerId != _pointerId)
            return;

        Canvas slotCanvas = _slot != null ? _slot.canvas : null;
        Camera slotCamera = slotCanvas != null &&
            slotCanvas.rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay
            ? slotCanvas.rootCanvas.worldCamera : null;
        bool correctDrop = _slot != null && _slot.isActiveAndEnabled &&
            RectTransformUtility.RectangleContainsScreenPoint(
                _slot.rectTransform, eventData.position, slotCamera);

        _isDragging = false;
        _imageRect.SetSiblingIndex(_originalSiblingIndex);
        if (correctDrop)
        {
            // Align the visual centers, including images with non-centered pivots.
            Vector3 slotCenter = _slot.rectTransform.TransformPoint(_slot.rectTransform.rect.center);
            Vector3 imageCenter = _imageRect.TransformPoint(_imageRect.rect.center);
            _imageRect.position += slotCenter - imageCenter;

            CorrectDropUnityEvent?.Invoke(_imageToDrag.gameObject);

        }
        else
        {         
            _imageRect.localPosition = _originalPosition;
        }
    }

    private void OnDisable()
    {
        if (!_isDragging || _imageRect == null)
            return;

        _isDragging = false;
        _imageRect.SetSiblingIndex(_originalSiblingIndex);
        _imageRect.localPosition = _originalPosition;
    }

    private void OnDestroy()
    {
        RemoveDragEvents();
    }

    private void RemoveDragEvents()
    {
        if (_eventTrigger != null)
        {
            foreach (EventTrigger.Entry entry in _dragEvents)
                _eventTrigger.triggers.Remove(entry);
        }
        _dragEvents.Clear();
        _eventTrigger = null;
    }
    
    
}
