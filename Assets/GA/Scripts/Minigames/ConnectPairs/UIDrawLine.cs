using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(RectTransform))]
public class UIDrawLine : MonoBehaviour
{
    #region Fields
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField, Min(1f)] private float thickness = 5f;
    [SerializeField] private Button testButtonA;
    [SerializeField] private Button testButtonB;
    [SerializeField] private bool showTestLine = true;
    private RectTransform container;
    private RectTransform lineRect;
    private Image lineImage;
    private Canvas canvas;
    private bool isDrawing;
    private bool hasDrawnLine;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private readonly List<GameObject> keptLines = new();
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        container = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        GameObject lineObject = new GameObject(
            "Connection Line",
            typeof(RectTransform),
            typeof(Image),
            typeof(LayoutElement)
        );

        lineRect = lineObject.GetComponent<RectTransform>();
        lineRect.SetParent(container, false);

        lineRect.anchorMin = container.pivot;
        lineRect.anchorMax = container.pivot;
        lineRect.pivot = new Vector2(0f, 0.5f);

        lineImage = lineObject.GetComponent<Image>();
        lineImage.raycastTarget = false;
        lineImage.enabled = false;
        lineObject.GetComponent<LayoutElement>().ignoreLayout = true;
    }

    private void LateUpdate()
    {
        if (showTestLine && testButtonA != null && testButtonB != null)
        {
            DrawTestLine();
            return;
        }

        if (hasDrawnLine)
        {
            if (isDrawing && TryGetMousePoint(out Vector2 mousePoint))
                endPoint = mousePoint;
            DrawLine(startPoint, endPoint);
            return;
        }

        lineImage.enabled = false;
    }

    private void OnEnable()
    {
        if (lineImage != null)
            lineImage.enabled = hasDrawnLine && (endPoint - startPoint).sqrMagnitude > 0f;
    }

    private void OnDisable()
    {
        isDrawing = false;
        if (lineImage != null)
            lineImage.enabled = false;
    }

    private void OnDestroy()
    {
        ClearAllLines();
        if (lineRect != null)
            Destroy(lineRect.gameObject);
    }
    #endregion

    #region Public methods
    public void DrawTestLine()
    {
        if (container == null || lineImage == null || testButtonA == null || testButtonB == null)
            return;

        RectTransform rectA = testButtonA.GetComponent<RectTransform>();
        RectTransform rectB = testButtonB.GetComponent<RectTransform>();
        Vector2 start = container.InverseTransformPoint(rectA.TransformPoint(rectA.rect.center));
        Vector2 end = container.InverseTransformPoint(rectB.TransformPoint(rectB.rect.center));
        DrawLine(start, end);
    }

    public void OnMousePointDown()
    {
        if (!TryGetMousePoint(out Vector2 mousePoint))
            return;

        startPoint = mousePoint;
        endPoint = mousePoint;
        hasDrawnLine = true;
        isDrawing = true;
        DrawLine(startPoint, endPoint);
    }

    public void OnMousePointUp()
    {
        if (!isDrawing)
            return;

        if (TryGetMousePoint(out Vector2 mousePoint))
            endPoint = mousePoint;
        isDrawing = false;
        DrawLine(startPoint, endPoint);
    }

    public void KeepLine()
    {
        if (!hasDrawnLine || lineImage == null)
            return;

        OnMousePointUp();
        if (lineImage.enabled)
        {
            GameObject savedLine = Instantiate(lineRect.gameObject, container, false);
            savedLine.name = "Kept Connection Line";
            keptLines.Add(savedLine);
        }

        ClearLine();
    }

    public void ClearAllLines()
    {
        ClearLine();
        foreach (GameObject savedLine in keptLines)
        {
            if (savedLine == null)
                continue;
            savedLine.SetActive(false);
            Destroy(savedLine);
        }
        keptLines.Clear();
    }

    public void ClearLine()
    {
        isDrawing = false;
        hasDrawnLine = false;
        startPoint = Vector2.zero;
        endPoint = Vector2.zero;

        if (lineImage != null)
            lineImage.enabled = false;
    }
    #endregion

    #region Private methods
    private void DrawLine(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        lineImage.enabled = direction.sqrMagnitude > 0f;

        lineRect.localPosition = new Vector3(start.x, start.y, 0f);
        lineRect.sizeDelta = new Vector2(direction.magnitude, Mathf.Max(1f, thickness));

        float angle = Mathf.Atan2(direction.y, direction.x)
                      * Mathf.Rad2Deg;

        lineRect.localRotation = Quaternion.Euler(0f, 0f, angle);
        lineImage.color = lineColor;
    }

    private bool TryGetMousePoint(out Vector2 localPoint)
    {
        localPoint = Vector2.zero;
        if (canvas == null)
        {
            Debug.LogWarning("UIButtonLine must be inside a Canvas.", this);
            return false;
        }

#if ENABLE_INPUT_SYSTEM
        if (Mouse.current == null)
            return false;
        Vector2 screenPoint = Mouse.current.position.ReadValue();
#else
        Vector2 screenPoint = Input.mousePosition;
#endif

        Canvas rootCanvas = canvas.rootCanvas;
        Camera uiCamera = rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null : rootCanvas.worldCamera;
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            container, screenPoint, uiCamera, out localPoint);
    }
    #endregion
}
