using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DrawLineCanvasUI : MonoBehaviour
{
    #region Fields
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField, Min(1f)] private float thickness = 5f;
    private RectTransform container;
    private RectTransform line;
    private Canvas canvas;
    private Vector2 startPoint;
    private bool hasStartPoint;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        container = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }
    #endregion

    #region Public methods
    // startPosition must be in screen coordinates.

    public void StartDrawLine(Vector2 startPosition)
    {
        hasStartPoint = false;

        if (!TryGetLocalPoint(startPosition, out startPoint))
            return;

        if (line == null)
        {
            GameObject lineObject = new GameObject(
                "Drawn Line",
                typeof(RectTransform),
                typeof(Image)
            );

            line = lineObject.GetComponent<RectTransform>();
            line.SetParent(container, false);

            // Match anchors to the parent's local origin.
            line.anchorMin = container.pivot;
            line.anchorMax = container.pivot;
            line.pivot = new Vector2(0f, 0.5f);

            Image image = lineObject.GetComponent<Image>();
            image.raycastTarget = false;
        }

        line.GetComponent<Image>().color = lineColor;
        line.anchoredPosition = startPoint;
        line.localRotation = Quaternion.identity;
        line.sizeDelta = new Vector2(0f, thickness);

        hasStartPoint = true;
    }

    // endPosition must be in screen coordinates.
    // You can call this repeatedly to preview the line while dragging.

    public void EndDrawLine(Vector2 endPosition)
    {
        if (!hasStartPoint || line == null)
            return;

        if (!TryGetLocalPoint(endPosition, out Vector2 endPoint))
            return;

        Vector2 direction = endPoint - startPoint;

        line.anchoredPosition = startPoint;
        line.sizeDelta = new Vector2(direction.magnitude, thickness);

        float angle = Mathf.Atan2(direction.y, direction.x)
                      * Mathf.Rad2Deg;

        line.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void ClearLine()
    {
        hasStartPoint = false;

        if (line != null)
        {
            Destroy(line.gameObject);
            line = null;
        }
    }
    #endregion

    #region Private methods
    private bool TryGetLocalPoint(
        Vector2 screenPosition,
        out Vector2 localPosition)
    {
        localPosition = Vector2.zero;

        if (canvas == null)
        {
            Debug.LogError(
                "UILineDrawer must be inside a Canvas.", this);
            return false;
        }

        Canvas rootCanvas = canvas.rootCanvas;
        Camera uiCamera = rootCanvas.renderMode ==
                          RenderMode.ScreenSpaceOverlay
            ? null
            : rootCanvas.worldCamera;

        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            container,
            screenPosition,
            uiCamera,
            out localPosition
        );
    }
    #endregion
}
