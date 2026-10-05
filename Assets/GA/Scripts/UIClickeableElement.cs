using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIClickeableElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent ClickUnityEvent = new UnityEvent();
    private Outline outline;

    void Awake() {
        outline = GetComponent<Outline>();
        if (outline) outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (outline) outline.enabled = true;
        // optional: scale effect
        transform.localScale = Vector3.one * 1.05f;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (isActiveAndEnabled && eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Mouse click!");
            ClickUnityEvent?.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (outline) outline.enabled = false;
        transform.localScale = Vector3.one;
    }
}
