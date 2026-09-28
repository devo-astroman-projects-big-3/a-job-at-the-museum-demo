using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIDragAndDropV2 : MonoBehaviour
{
    
    [SerializeField] private Image _imageToDrag;
    [SerializeField] private Image _slot;
    [SerializeField] private Image[] _wrongSlots;
    [SerializeField] private bool _allowPassMouseEvents;
    public UnityEvent<GameObject,GameObject> CorrectDrop = new UnityEvent<GameObject,GameObject>();
    public UnityEvent<GameObject,GameObject> WrongDrop = new UnityEvent<GameObject,GameObject>(); //when dropped on some of the _wrongSlots
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

        bool correctDrop = ContainsDropPosition(_slot, eventData.position);
        Image wrongSlot = null;
        // Prefer the correct slot and notify only once if wrong slots overlap.
        if (!correctDrop && _wrongSlots != null)
        {
            foreach (Image candidate in _wrongSlots)
            {
                if (ContainsDropPosition(candidate, eventData.position))
                {
                    wrongSlot = candidate;
                    break;
                }
            }
        }

        _isDragging = false;
        RestoreRaycastTargets();
        _imageRect.SetSiblingIndex(_originalSiblingIndex);
        if (correctDrop)
        {
            // Align the visual centers, including images with non-centered pivots.
            Vector3 slotCenter = _slot.rectTransform.TransformPoint(_slot.rectTransform.rect.center);
            Vector3 imageCenter = _imageRect.TransformPoint(_imageRect.rect.center);
            _imageRect.position += slotCenter - imageCenter;

            Debug.Log("CORRECT DROP");
            CorrectDrop?.Invoke(_imageToDrag.gameObject, _slot.gameObject);

        }
        else
        {         
            _imageRect.localPosition = _originalPosition;
            if (wrongSlot != null)
                WrongDrop?.Invoke(_imageToDrag.gameObject, wrongSlot.gameObject);
        }
    }

    private static bool ContainsDropPosition(Image slot, Vector2 position)
    {
        if (slot == null || !slot.isActiveAndEnabled)
            return false;

        Canvas slotCanvas = slot.canvas;
        Camera slotCamera = slotCanvas != null &&
            slotCanvas.rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay
            ? slotCanvas.rootCanvas.worldCamera : null;

        return RectTransformUtility.RectangleContainsScreenPoint(
            slot.rectTransform, position, slotCamera);
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
