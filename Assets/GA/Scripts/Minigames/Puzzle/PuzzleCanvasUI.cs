using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PuzzleCanvasUI : MonoBehaviour
{
    #region Fields
    [SerializeField] private ImageSlicer _imageSlicerDragItems;
    [SerializeField] private ImageSlicer _imageSlicerSlots;
    [SerializeField] private PieceDragger _pieceDragger;
    [SerializeField] private GameObject _congratsMsg;
    [SerializeField] private Image _imageItemsDraggable;
    [SerializeField] private Image _imageSlot;
    private readonly List<UIDragAndDrop> _registeredPieces = new();
    private readonly HashSet<GameObject> _remainingPieces = new();
    private int _correctCount;
    private int _totalPieces;
    private string _currentRunningId;
    #endregion

    #region public properties
    public UnityEvent<string> PuzzleCompletedUnityEvent;
    #endregion

    #region Unity Callbacks
    private void OnDestroy()
    {
        RemovePieceListeners();
    }
    #endregion

    #region Public methods
    public void SetCurrentRunningId(string currentRunningId)
    {
        _currentRunningId = currentRunningId;
    }

    public void Setup(Sprite imageToSlice, int nSlices)
    {
        RemovePieceListeners();
        _remainingPieces.Clear();
        _correctCount = 0;
        _totalPieces = 0;

        if (_imageSlicerDragItems == null || _imageSlicerSlots == null ||
            _pieceDragger == null || _imageItemsDraggable == null || _imageSlot == null ||
            _imageSlicerDragItems == _imageSlicerSlots)
        {
            Debug.LogWarning("Puzzle: assign source Images, separate drag and slot slicers, and a PieceDragger.", this);
            return;
        }

        if (imageToSlice == null || imageToSlice.packed || nSlices <= 0 ||
            (long)nSlices * nSlices > int.MaxValue ||
            nSlices > imageToSlice.rect.width || nSlices > imageToSlice.rect.height)
        {
            Debug.LogWarning("Puzzle: supply an unpacked sprite and a positive grid size with at least one pixel per piece.", this);
            return;
        }

        int expectedPieces = nSlices * nSlices;
        _imageItemsDraggable.sprite = imageToSlice;
        _imageSlot.sprite = imageToSlice;
        if (_congratsMsg != null)
            _congratsMsg.SetActive(false);

        _imageSlicerDragItems.SliceImage(nSlices, nSlices);
        _imageSlicerSlots.SliceImage(nSlices, nSlices);
        // Smaller grids leave previously generated surplus images inactive.
        List<Image> generatedDragImages = _imageSlicerDragItems.GetGeneratedImages()
            .FindAll(image => image != null && image.gameObject.activeSelf);
        List<Image> generatedSlotImages = _imageSlicerSlots.GetGeneratedImages()
            .FindAll(image => image != null && image.gameObject.activeSelf);

        if (generatedDragImages.Count != expectedPieces || generatedSlotImages.Count != expectedPieces)
        {
            Debug.LogWarning($"Puzzle: both slicers must generate {expectedPieces} active pieces for a {nSlices} x {nSlices} grid.", this);
            return;
        }

        _pieceDragger.SetPiecesAndSlots(generatedDragImages.ToArray(), generatedSlotImages.ToArray());
        foreach (Image piece in generatedDragImages)
        {
            UIDragAndDrop drag = piece != null ? piece.GetComponent<UIDragAndDrop>() : null;
            if (drag == null || !drag.enabled)
            {
                RemovePieceListeners();
                _remainingPieces.Clear();
                Debug.LogWarning("Puzzle: could not configure all draggable pieces.", this);
                return;
            }

            _registeredPieces.Add(drag);
            _remainingPieces.Add(piece.gameObject);
            drag.CorrectDrop.AddListener(OnCorrectDrop);
        }
        _totalPieces = _remainingPieces.Count;
        RandomizeDraggableItemsPositions();
    }
    #endregion

    #region Private methods
    private void RandomizeDraggableItemsPositions()
    {
        if (_imageSlicerDragItems == null)
            return;

        var pieces = new List<RectTransform>();
        foreach (Image image in _imageSlicerDragItems.GetGeneratedImages())
        {
            if (image != null && _remainingPieces.Contains(image.gameObject) &&
                !pieces.Contains(image.rectTransform))
                pieces.Add(image.rectTransform);
        }

        // Resolve UI layout before swapping the pieces' world positions.
        Canvas.ForceUpdateCanvases();
        for (int i = pieces.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector3 position = pieces[i].position;
            pieces[i].position = pieces[j].position;
            pieces[j].position = position;
        }
    }

    private void OnCorrectDrop(GameObject piece, GameObject slot)
    {
        if (piece == null || slot == null || !slot.TryGetComponent<Image>(out Image slotImage) ||
            !_remainingPieces.Remove(piece))
            return;

        Color color = slotImage.color;
        color.a = 1f;
        slotImage.color = color;
        piece.SetActive(false);

        _correctCount++;
        if (_correctCount == _totalPieces)
            PuzzleCompletedUnityEvent?.Invoke(_currentRunningId);
    }

    private void RemovePieceListeners()
    {
        foreach (UIDragAndDrop drag in _registeredPieces)
        {
            if (drag != null)
                drag.CorrectDrop.RemoveListener(OnCorrectDrop);
        }
        _registeredPieces.Clear();
    }
    #endregion
}
