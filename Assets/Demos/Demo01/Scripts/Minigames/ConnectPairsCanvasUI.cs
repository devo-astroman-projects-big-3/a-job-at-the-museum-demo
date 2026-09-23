using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ConnectPairsCanvasUI : MonoBehaviour
{
    
    [SerializeField] private PairButton[] _leftGroup;
    [SerializeField] private PairButton[] _rightGroup;

    private PairButton _leftSelection;
    private PairButton _rightSelection;

    private bool _leftDragging = false;
    private bool _rightDragging = false;

    public void SetValues(PairData[] pairs)
    {
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


    //click left to right
    public void OnLeftPointerDown(PairButton pButton)
    {
        Debug.Log("OnLeftPointerDown " + pButton.PairId);
        _leftSelection = pButton;
        _leftDragging = true;
        
    }

    public void OnLeftPointerUp(PairButton pButton)
    {
        Debug.Log("OnLeftPointerUp " + pButton.PairId);
        _leftSelection = pButton;
        _leftDragging = false;
        //CheckPairSelection();
    }



    public void OnRightPointerDown(PairButton pButton)
    {
        Debug.Log("OnRightPointerDown " + pButton.PairId);
         _rightSelection = pButton;
         _leftDragging = true;
    }

    public void OnRightPointerUp(PairButton pButton)
    {
        Debug.Log("OnRightPointerUp " + pButton.PairId);
        _rightSelection = pButton;
        _leftDragging = false;
        CheckPairSelection();
    }
    //click right to left




    

    public void OnLeftPointerEnter(PairButton pButton)
    {
        Debug.Log("OnLeftPointerEnter");
        
    }


    public void OnLeftPointerExit(PairButton pButton)
    {
        Debug.Log("OnLeftPointerExit");
    }




    public void OnRightPointerEnter(PairButton pButton)
    {
        Debug.Log("OnRightPointerEnter");
    }

    public void OnRightPointerExit(PairButton pButton)
    {
        Debug.Log("OnRightPointerExit");
    }

    // Retained for PairButton's existing pointer-event forwarding.
    public void BeginPointerConnection(PairButton button, PointerEventData eventData)
    {
    }

    public void EndPointerConnection(PointerEventData eventData)
    {
    }

    private void CheckPairSelection()
    {
        if(_leftSelection.PairId == _rightSelection.PairId)
        {
            Debug.Log("correct!");
        }
        else
        {
            Debug.Log("incorrect!");
        }
    }
}
