using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private ImageSlicer _imageSlicerDragItems;
    [SerializeField] private ImageSlicer _imageSlicerSlots;
    [SerializeField] private PieceDragger _pieceDragger;
    public UnityEvent PuzzleCompletedUnityEvent = new UnityEvent();
    private readonly List<UIDragAndDrop> _registeredPieces = new();
    private readonly HashSet<GameObject> _remainingPieces = new();
    private int _correctCount;
    private int _totalPieces;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        RemovePieceListeners();
        _remainingPieces.Clear();
        _correctCount = 0;
        _totalPieces = 0;

        if (_imageSlicerDragItems == null || _imageSlicerSlots == null ||
            _pieceDragger == null || _imageSlicerDragItems == _imageSlicerSlots)
        {
            Debug.LogWarning("Puzzle: assign separate drag and slot slicers and a PieceDragger.", this);
            return;
        }

        _imageSlicerDragItems.SliceImage(2, 2);
        _imageSlicerSlots.SliceImage(2, 2);
        List<Image> generatedDragImages = _imageSlicerDragItems.GetGeneratedImages();
        List<Image> generatedSlotImages = _imageSlicerSlots.GetGeneratedImages();

        if (generatedDragImages.Count != 4 || generatedSlotImages.Count != 4)
        {
            Debug.LogWarning("Puzzle: both slicers must generate four pieces.", this);
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
            drag.CorrectDropUnityEvent.AddListener(OnCorrectDrop);
        }
        _totalPieces = _remainingPieces.Count;
    }

    private void OnCorrectDrop(GameObject piece)
    {
        //OnCorrectDrop should make the slot visi

        if (piece == null || !_remainingPieces.Remove(piece))
            return;

        piece.SetActive(false);
        _correctCount++;
        if (_correctCount == _totalPieces)
            PuzzleCompletedUnityEvent?.Invoke();
    }

    private void RemovePieceListeners()
    {
        foreach (UIDragAndDrop drag in _registeredPieces)
        {
            if (drag != null)
                drag.CorrectDropUnityEvent.RemoveListener(OnCorrectDrop);
        }
        _registeredPieces.Clear();
    }

    private void OnDestroy()
    {
        RemovePieceListeners();
    }
}
