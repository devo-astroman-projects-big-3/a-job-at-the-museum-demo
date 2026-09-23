using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectPairsCanvasUI : MonoBehaviour
{
    
    [SerializeField] private UIButtonLine _uIButtonLine;
    
    [SerializeField] private PairButton[] _leftGroup;
    [SerializeField] private PairButton[] _rightGroup;

    private PairButton _leftSelection;
    private PairButton _rightSelection;

    private bool _leftDragging = false;
    private bool _rightDragging = false;

    public void SetValues(PairData[] pairs)
    {
        ResetSelection();
        if (_uIButtonLine != null)
            _uIButtonLine.ClearAllLines();
        if (pairs == null || _leftGroup == null || _rightGroup == null ||
            pairs.Length > _leftGroup.Length || pairs.Length > _rightGroup.Length)
        {
            Debug.LogWarning("Assign enough left and right buttons for all pairs.", this);
            return;
        }
        for (int index = 0; index < pairs.Length; index++)
        {
            if (pairs[index] == null || string.IsNullOrEmpty(pairs[index].Id) ||
                _leftGroup[index] == null || _rightGroup[index] == null)
            {
                Debug.LogWarning($"Missing pair, ID or button at index {index}.", this);
                return;
            }
        }
        for (int index = 0; index < _leftGroup.Length; index++)
            if (_leftGroup[index] != null)
                _leftGroup[index].gameObject.SetActive(index < pairs.Length);
        for (int index = 0; index < _rightGroup.Length; index++)
            if (_rightGroup[index] != null)
                _rightGroup[index].gameObject.SetActive(index < pairs.Length);

        int i = 0;
        foreach(PairData pair in pairs)
        {
            _leftGroup[i].PairId = pair.Id;
            _leftGroup[i].Image = pair.Image;

            _rightGroup[i].PairId = pair.Id;
            _rightGroup[i].Text = pair.Text;
            i++;
        }

        ChangePositionsRandomly(_leftGroup);
        ChangePositionsRandomly(_rightGroup);

    }

    private void ChangePositionsRandomly(PairButton[] group)
    {
        var buttons = new List<PairButton>();
        foreach (PairButton button in group)
            if (button != null && button.gameObject.activeSelf)
                buttons.Add(button);

        for (int i = buttons.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Transform a = buttons[i].transform;
            Transform b = buttons[j].transform;
            if (a == b)
                continue;
            if (a.parent == b.parent && a.parent != null &&
                a.parent.TryGetComponent<LayoutGroup>(out var layout) && layout.isActiveAndEnabled)
            {
                int aIndex = a.GetSiblingIndex();
                int bIndex = b.GetSiblingIndex();
                a.SetSiblingIndex(bIndex);
                b.SetSiblingIndex(aIndex);
            }
            else
            {
                Vector3 position = a.position;
                a.position = b.position;
                b.position = position;
            }
        }
    }


    public void OnLeftPointerDown(PairButton pButton)
    {
        ResetSelection();
        if (!IsInGroup(pButton, _leftGroup))
            return;

        _leftSelection = pButton;
        _leftDragging = true;
        StartSelectionLine();
    }

    public void OnRightPointerDown(PairButton pButton)
    {
        ResetSelection();
        if (!IsInGroup(pButton, _rightGroup))
            return;

        _rightSelection = pButton;
        _rightDragging = true;
        StartSelectionLine();
    }

    public void OnLeftPointerUp(PairButton pButton)
    {
        FinishSelection();
    }

    public void OnRightPointerUp(PairButton pButton)
    {
        FinishSelection();
    }

    public void OnLeftPointerEnter(PairButton pButton)
    {
        if (_rightDragging && IsInGroup(pButton, _leftGroup))
            _leftSelection = pButton;
    }

    public void OnLeftPointerExit(PairButton pButton)
    {
        if (_rightDragging && _leftSelection == pButton)
            _leftSelection = null;
    }

    public void OnRightPointerEnter(PairButton pButton)
    {
        if (_leftDragging && IsInGroup(pButton, _rightGroup))
            _rightSelection = pButton;
    }

    public void OnRightPointerExit(PairButton pButton)
    {
        if (_leftDragging && _rightSelection == pButton)
            _rightSelection = null;
    }

    private static bool IsInGroup(PairButton button, PairButton[] group)
    {
        return button != null && button.isActiveAndEnabled && group != null &&
            System.Array.IndexOf(group, button) >= 0;
    }

    private void FinishSelection()
    {
        if (!_leftDragging && !_rightDragging)
            return;

        // PointerUp normally arrives on the starting button, so use the hovered target.
        if ((_leftDragging || _rightDragging) &&
            IsInGroup(_leftSelection, _leftGroup) &&
            IsInGroup(_rightSelection, _rightGroup))
        {
            CheckPairSelection();
        }
        else if (_uIButtonLine != null)
        {
            _uIButtonLine.ClearLine();
        }

        ResetSelection();
    }

    private void CheckPairSelection()
    {
        if (_leftSelection == null || _rightSelection == null)
            return;

        if (!string.IsNullOrEmpty(_leftSelection.PairId) &&
            _leftSelection.PairId == _rightSelection.PairId)
        {
            Debug.Log("correct!", this);
            if (_uIButtonLine != null)
                _uIButtonLine.KeepLine();
        }
        else
        {
            Debug.Log("incorrect!", this);
            if (_uIButtonLine != null)
                _uIButtonLine.ClearLine();
        }
    }

    private void StartSelectionLine()
    {
        if (_uIButtonLine == null)
        {
            Debug.LogWarning("Assign UIButtonLine in the Inspector to draw pair connections.", this);
            return;
        }

        _uIButtonLine.OnMousePointDown();
    }

    private void ResetSelection()
    {
        _leftSelection = null;
        _rightSelection = null;
        _leftDragging = false;
        _rightDragging = false;
    }

    private void OnDisable()
    {
        if ((_leftDragging || _rightDragging) && _uIButtonLine != null)
            _uIButtonLine.ClearLine();
        ResetSelection();
    }
}
