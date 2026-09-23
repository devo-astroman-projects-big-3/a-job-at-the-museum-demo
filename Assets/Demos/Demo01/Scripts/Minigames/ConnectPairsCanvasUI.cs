using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ConnectPairsCanvasUI : MonoBehaviour
{
    
    [SerializeField] private PairButton[] _leftGroup;
    [SerializeField] private PairButton[] _rightGroup;

    [SerializeField] private Color _lineColor = Color.white;
    [SerializeField, Min(1f)] private float _lineWidth = 4f;
    private readonly HashSet<PairButton> _matched = new();
    private readonly List<Connection> _connections = new();
    private readonly List<RaycastResult> _hits = new();
    private PairButton _start;
    private PairButton _hovered;
    private RectTransform _lineLayer;
    private RectTransform _dragLine;
    private PointerEventData _pointer;

    private class Connection
    {
        public PairButton Start;
        public PairButton End;
        public RectTransform Line;
    }

    public void SetValues(PairData[] pairs)
    {
        CancelDrag();
        foreach (Connection connection in _connections)
            if (connection.Line != null)
                Destroy(connection.Line.gameObject);
        _connections.Clear();
        _matched.Clear();

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
            _leftGroup[i].Bind(this);
            _rightGroup[i].Bind(this);
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
    

    public void OnLeftPointerEnter(PairButton pButton)
    {
        _hovered = pButton;
    }

    public void OnLeftPointerDown(PairButton pButton)
    {
        BeginConnection(pButton);
    }

    public void OnLeftPointerUp(PairButton pButton)
    {
        if (_pointer == null)
            FinishConnection(_hovered);
    }
    public void OnLeftPointerExit(PairButton pButton)
    {
        if (_hovered == pButton)
            _hovered = null;
    }

    public void OnRightPointerEnter(PairButton pButton)
    {
        OnLeftPointerEnter(pButton);
    }

    public void OnRightPointerDown(PairButton pButton)
    {
        BeginConnection(pButton);
    }

    public void OnRightPointerUp(PairButton pButton)
    {
        OnLeftPointerUp(pButton);
    }
    public void OnRightPointerExit(PairButton pButton)
    {
        OnLeftPointerExit(pButton);
    }

    private bool IsLeft(PairButton button) =>
        _leftGroup != null && System.Array.IndexOf(_leftGroup, button) >= 0;

    private bool IsRight(PairButton button) =>
        _rightGroup != null && System.Array.IndexOf(_rightGroup, button) >= 0;

    private void BeginConnection(PairButton button)
    {
        if (!isActiveAndEnabled || button == null || _start != null ||
            _matched.Contains(button) || string.IsNullOrEmpty(button.PairId) ||
            (!IsLeft(button) && !IsRight(button)))
            return;

        if (_lineLayer == null)
        {
            var layer = new GameObject("Pair Lines", typeof(RectTransform), typeof(LayoutElement));
            _lineLayer = layer.GetComponent<RectTransform>();
            _lineLayer.SetParent(transform, false);
            _lineLayer.anchorMin = Vector2.zero;
            _lineLayer.anchorMax = Vector2.one;
            _lineLayer.offsetMin = _lineLayer.offsetMax = Vector2.zero;
            layer.GetComponent<LayoutElement>().ignoreLayout = true;
        }
        var line = new GameObject("Pair Line", typeof(RectTransform), typeof(Image));
        _dragLine = line.GetComponent<RectTransform>();
        _dragLine.SetParent(_lineLayer, false);
        _dragLine.pivot = new Vector2(0f, 0.5f);
        Image image = line.GetComponent<Image>();
        image.color = _lineColor;
        image.raycastTarget = false;
        _start = button;
        DrawLine(_dragLine, ButtonPosition(button), ButtonPosition(button));
    }

    public void BeginPointerConnection(PairButton button, PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || _pointer != null)
            return;
        BeginConnection(button);
        if (_start == button)
            _pointer = eventData;
    }

    public void EndPointerConnection(PointerEventData eventData)
    {
        if (_pointer == null || eventData.pointerId != _pointer.pointerId)
            return;
        _hits.Clear();
        EventSystem.current.RaycastAll(eventData, _hits);
        PairButton target = _hits.Count > 0 ? _hits[0].gameObject.GetComponentInParent<PairButton>() : null;
        FinishConnection(target);
    }

    private void FinishConnection(PairButton target)
    {
        if (_start == null)
            return;
        bool oppositeGroups = target != null &&
            ((IsLeft(_start) && IsRight(target)) || (IsRight(_start) && IsLeft(target)));
        if (oppositeGroups && target.isActiveAndEnabled && !_matched.Contains(target) &&
            _start.PairId == target.PairId)
        {
            DrawLine(_dragLine, ButtonPosition(_start), ButtonPosition(target));
            _connections.Add(new Connection { Start = _start, End = target, Line = _dragLine });
            _matched.Add(_start);
            _matched.Add(target);
            _dragLine = null;
        }
        CancelDrag();
    }

    private Vector2 ButtonPosition(PairButton button)
    {
        RectTransform rect = button.transform as RectTransform;
        Vector3 center = rect != null ? rect.TransformPoint(rect.rect.center) : button.transform.position;
        return _lineLayer.InverseTransformPoint(center);
    }

    private void DrawLine(RectTransform line, Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        line.localPosition = new Vector3(start.x, start.y, 0f);
        line.sizeDelta = new Vector2(direction.magnitude, _lineWidth);
        line.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private void LateUpdate()
    {
        foreach (Connection connection in _connections)
            if (connection.Start != null && connection.End != null && connection.Line != null)
                DrawLine(connection.Line, ButtonPosition(connection.Start), ButtonPosition(connection.End));
        if (_start == null || _dragLine == null || _pointer == null)
            return;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _lineLayer, _pointer.position, _pointer.pressEventCamera, out Vector2 end))
            DrawLine(_dragLine, ButtonPosition(_start), end);
    }

    private void CancelDrag()
    {
        if (_dragLine != null)
            Destroy(_dragLine.gameObject);
        _dragLine = null;
        _start = null;
        _hovered = null;
        _pointer = null;
    }

    private void OnDisable() => CancelDrag();

}
