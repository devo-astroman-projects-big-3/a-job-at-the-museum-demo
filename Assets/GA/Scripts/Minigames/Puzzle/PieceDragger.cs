using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceDragger : MonoBehaviour
{
    #region Fields
    [SerializeField] private Image[] _draggablePieces;
    [SerializeField] private Image[] _slotPieces;
    #endregion

    #region Public methods
    public void SetPiecesAndSlots(Image[] draggablePieces, Image[] slotPieces)
    {
        _draggablePieces = draggablePieces;
        _slotPieces = slotPieces;
        SetOpacitySlotImages(.5f);
        SetPiecesToDrag();
    }

    public void SetOpacitySlotImages(float opacity)
    {
        if (_slotPieces == null)
            return;

        float alpha = Mathf.Clamp01(opacity);
        foreach (Image slot in _slotPieces)
        {
            if (slot == null)
                continue;

            Color color = slot.color;
            color.a = alpha;
            slot.color = color;
        }
    }

    public void SetPiecesToDrag()
    {
        if (_draggablePieces == null || _slotPieces == null ||
            _draggablePieces.Length != _slotPieces.Length)
        {
            Debug.LogWarning("PieceDragger: assign equally sized draggable and slot arrays.", this);
            return;
        }

        var pieces = new HashSet<Image>();
        var slots = new HashSet<Image>();
        for (int i = 0; i < _draggablePieces.Length; i++)
        {
            Image piece = _draggablePieces[i];
            Image slot = _slotPieces[i];
            if (piece == null || slot == null || !pieces.Add(piece) || !slots.Add(slot) ||
                !(piece.transform.parent is RectTransform))
            {
                Debug.LogWarning($"PieceDragger: Element {i} requires unique piece and slot Images, " +
                    "and the piece must have a RectTransform parent.", this);
                return;
            }
        }
        if (pieces.Overlaps(slots))
        {
            Debug.LogWarning("PieceDragger: draggable pieces and target slots must be separate Images.", this);
            return;
        }

        for (int i = 0; i < _draggablePieces.Length; i++)
        {
            Image piece = _draggablePieces[i];
            UIDragAndDrop drag = piece.GetComponent<UIDragAndDrop>();
            if (drag == null)
                drag = piece.gameObject.AddComponent<UIDragAndDrop>();
            drag.Configure(piece, _slotPieces[i]);
        }
    }
    #endregion
}
