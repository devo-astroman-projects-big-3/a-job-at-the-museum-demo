using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CategorizeCanvasUI : MonoBehaviour
{

    [SerializeField] private SlotGroups _slotGroupLeft;
    [SerializeField] private SlotGroups _slotGroupRight;
    [SerializeField] private DraggableItems _draggableItems;

    public UnityEvent OnComplete = new UnityEvent();

    private readonly HashSet<int> _placedLeft = new();
    private readonly HashSet<int> _placedRight = new();
    private int _leftCount;
    private int _rightCount;
    private bool _completed;

    public void SetValues(CategoryData categoryOne,CategoryData categoryTwo)
    {
        SetValues(categoryOne != null ? categoryOne.Images : null,
            categoryTwo != null ? categoryTwo.Images : null);
    }

    public void SetValues(Sprite[] leftImages, Sprite[] rightImages)
    {

        _placedLeft.Clear();
        _placedRight.Clear();
        _leftCount = 0;
        _rightCount = 0;
        _completed = false;

        if (leftImages == null || rightImages == null ||
            _slotGroupLeft == null || _slotGroupRight == null || _draggableItems == null)
        {
            Debug.LogWarning("Assign both image subsets, slot groups and draggable items.", this);
            return;
        }

        // Evaluate every group so the Console reports all configuration problems.
        bool leftValid = _slotGroupLeft.CanSetImages(leftImages, "slotGroupLeft (selected images)");
        bool rightValid = _slotGroupRight.CanSetImages(rightImages, "slotGroupRight (selected images)");
        bool draggableValid = _draggableItems.CanSetImages(leftImages, rightImages);
        if (!leftValid || !rightValid || !draggableValid)
        {
            return;
        }

        _draggableItems.SetLeftItemImages(leftImages);
        _draggableItems.SetLeftRightImages(rightImages);
        _slotGroupLeft.SetImages(leftImages);
        _slotGroupRight.SetImages(rightImages);
        _leftCount = leftImages.Length;
        _rightCount = rightImages.Length;
    }

    public void OnItemLeftPlacedCorrectly(int index)
    {
        if (_completed || index < 0 || index >= _leftCount || !_placedLeft.Add(index))
            return;

        _slotGroupLeft.CorrectDrop(index);
        _draggableItems.HideLeftItem(index);
        CheckAllCorrects();
    }

    public void OnItemRightPlacedCorrectly(int index)
    {
        if (_completed || index < 0 || index >= _rightCount || !_placedRight.Add(index))
            return;

        _slotGroupRight.CorrectDrop(index);
        _draggableItems.HideRightItem(index);
        CheckAllCorrects();
    }

    private void CheckAllCorrects()
    {
        if (_completed || _leftCount + _rightCount == 0 ||
            _placedLeft.Count != _leftCount || _placedRight.Count != _rightCount)
            return;

        _completed = true;
        OnComplete?.Invoke();
    }
    
    
}
