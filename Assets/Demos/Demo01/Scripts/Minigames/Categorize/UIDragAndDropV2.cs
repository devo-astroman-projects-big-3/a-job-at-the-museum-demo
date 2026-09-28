using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIDragAndDropV2 : MonoBehaviour
{
    
    [SerializeField] private Image _imageToDrag;
    [SerializeField] private Image[] _slots;
    [SerializeField] private bool _allowPassMouseEvents;

    public UnityEvent<GameObject,GameObject> CorrectDrop = new UnityEvent<GameObject,GameObject>();
    // Reports the dragged item and the matched slot.

    private RectTransform _imageRect;
    private RectTransform _dragParent;
    private EventTrigger _eventTrigger;
    private readonly List<EventTrigger.Entry> _dragEvents = new();
    private readonly List<Graphic> _disabledRaycastTargets = new();
    private Vector3 _originalPosition;
    private Vector2 _pointerOffset;
    private bool _isDragging;
    private int _pointerId;
    private int _originalSiblingIndex;

    public void SetImageToDrag(Image imageToDrag)
    {
        Configure(imageToDrag, _slots);
    }
    public void SetSlot(Image slot)
    {
        Configure(_imageToDrag, slot);
    }

    public void SetSlots(Image[] slots)
    {
        Configure(_imageToDrag, slots);
    }

    public void Configure(Image imageToDrag, params Image[] slots)
    {
        OnDisable();
        RemoveDragEvents();
        _imageToDrag = imageToDrag;
        _slots = slots;
        _imageRect = null;
        _dragParent = null;
        if (_imageToDrag != null)
            Initialize();
    }

    private void Awake()
    {
        // Runtime-added components are configured after AddComponent invokes Awake.
        if (_imageToDrag != null)
            Initialize();
    }

    private void Start()
    {
        if (_imageRect == null)
            Initialize();
    }

    private void Initialize()
    {
        if (_imageToDrag == null)
        {
            Debug.LogWarning("Assign the image to drag in the Inspector.", this);
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
        if (_allowPassMouseEvents)
        {
            // Include child graphics so labels and decorations do not block hover events.
            foreach (Graphic graphic in _imageToDrag.GetComponentsInChildren<Graphic>(true))
            {
                if (!graphic.raycastTarget)
                    continue;

                _disabledRaycastTargets.Add(graphic);
                graphic.raycastTarget = false;
            }
        }
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

        Image droppedSlot = FindDropSlot(eventData.position);

        _isDragging = false;
        RestoreRaycastTargets();
        _imageRect.SetSiblingIndex(_originalSiblingIndex);
        if (droppedSlot != null)
        {
            // Align the visual centers, including images with non-centered pivots.
            Vector3 slotCenter = droppedSlot.rectTransform.TransformPoint(droppedSlot.rectTransform.rect.center);
            Vector3 imageCenter = _imageRect.TransformPoint(_imageRect.rect.center);
            _imageRect.position += slotCenter - imageCenter;

            Debug.Log("DROP IN ONE OF THE SLOTS");
            CorrectDrop?.Invoke(_imageToDrag.gameObject, droppedSlot.gameObject);

        }
        else
        {         
            _imageRect.localPosition = _originalPosition;
        }
    }

    private Image FindDropSlot(Vector2 position)
    {
        if (_slots == null)
            return null;

        // If slots overlap, use the first active match in the configured array.
        foreach (Image slot in _slots)
        {
            if (slot == null || !slot.isActiveAndEnabled)
                continue;

            Canvas slotCanvas = slot.canvas;
            Camera slotCamera = slotCanvas != null &&
                slotCanvas.rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay
                ? slotCanvas.rootCanvas.worldCamera : null;

            if (RectTransformUtility.RectangleContainsScreenPoint(
                slot.rectTransform, position, slotCamera))
                return slot;
        }

        return null;
    }

    private void OnDisable()
    {
        RestoreRaycastTargets();
        if (!_isDragging || _imageRect == null)
            return;

        _isDragging = false;
        _imageRect.SetSiblingIndex(_originalSiblingIndex);
        _imageRect.localPosition = _originalPosition;
    }

    private void OnDestroy()
    {
        RestoreRaycastTargets();
        RemoveDragEvents();
    }

    private void RestoreRaycastTargets()
    {
        foreach (Graphic graphic in _disabledRaycastTargets)
        {
            if (graphic != null)
                graphic.raycastTarget = true;
        }
        _disabledRaycastTargets.Clear();
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
