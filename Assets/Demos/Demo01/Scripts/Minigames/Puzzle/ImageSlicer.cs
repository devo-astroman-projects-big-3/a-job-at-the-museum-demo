using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlicer : MonoBehaviour
{
    [SerializeField] private Image _imageToSlice;
    [SerializeField] private Image[] _pieces;
    private readonly List<Sprite> _generatedSprites = new();
    private readonly List<Image> _generatedImages = new();

    private void Start()
    {
        SliceImage(2,2);
    }
    public void SliceImage(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0 || (long)rows * columns > int.MaxValue)
        {
            Debug.LogWarning("ImageSlicer: rows and columns must be positive and form a valid grid.", this);
            return;
        }

        if (_imageToSlice == null || _imageToSlice.sprite == null)
        {
            Debug.LogWarning("ImageSlicer: assign a source Image with a sprite.", this);
            return;
        }

        Sprite source = _imageToSlice.sprite;
        // Packed sprites can be rotated or tightly cropped by an atlas.
        if (source.packed)
        {
            Debug.LogWarning("ImageSlicer: use a source sprite that is not packed in a Sprite Atlas.", this);
            return;
        }

        Rect sourceRect = source.rect;
        if (columns > sourceRect.width || rows > sourceRect.height)
        {
            Debug.LogWarning("ImageSlicer: each piece must contain at least one pixel.", this);
            return;
        }

        int count = rows * columns;
        if (_pieces == null || _pieces.Length < count)
            System.Array.Resize(ref _pieces, count);

        var targets = new HashSet<Image>();
        for (int i = 0; i < count; i++)
        {
            if (_pieces[i] == null)
                continue;

            if (_pieces[i] == _imageToSlice || !targets.Add(_pieces[i]))
            {
                Debug.LogWarning($"ImageSlicer: _pieces Element {i} must reference a unique Image other than the source.", this);
                return;
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (_pieces[i] != null)
                continue;

            var pieceObject = new GameObject($"Piece_{i}", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
            pieceObject.layer = _imageToSlice.gameObject.layer;
            pieceObject.transform.SetParent(_imageToSlice.transform, false);
            pieceObject.GetComponent<LayoutElement>().ignoreLayout = true;
            Image image = pieceObject.GetComponent<Image>();
            _pieces[i] = image;
            _generatedImages.Add(image);
            targets.Add(image);
        }

        ClearGeneratedSprites();
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                // Integer boundaries keep all pixels, including any remainder at the edges.
                float xMin = Mathf.Floor(column * sourceRect.width / columns);
                float xMax = Mathf.Floor((column + 1) * sourceRect.width / columns);
                float yMin = Mathf.Floor((rows - row - 1) * sourceRect.height / rows);
                float yMax = Mathf.Floor((rows - row) * sourceRect.height / rows);
                var rect = new Rect(sourceRect.x + xMin, sourceRect.y + yMin, xMax - xMin, yMax - yMin);
                Sprite piece = Sprite.Create(source.texture, rect, new Vector2(0.5f, 0.5f),
                    source.pixelsPerUnit, 0, SpriteMeshType.FullRect);
                piece.name = $"{source.name}_Row{row}_Column{column}";
                _generatedSprites.Add(piece);

                Image target = _pieces[row * columns + column];
                target.overrideSprite = null;
                target.sprite = piece;
                target.type = Image.Type.Simple;
                if (_generatedImages.Contains(target))
                {
                    // Fit generated UI pieces over their corresponding source regions.
                    RectTransform pieceRect = target.rectTransform;
                    pieceRect.anchorMin = new Vector2(xMin / sourceRect.width, yMin / sourceRect.height);
                    pieceRect.anchorMax = new Vector2(xMax / sourceRect.width, yMax / sourceRect.height);
                    pieceRect.offsetMin = Vector2.zero;
                    pieceRect.offsetMax = Vector2.zero;
                }
                target.gameObject.SetActive(true);
            }
        }

        for (int i = count; i < _pieces.Length; i++)
        {
            if (_pieces[i] != null && _pieces[i] != _imageToSlice && !targets.Contains(_pieces[i]))
                _pieces[i].gameObject.SetActive(false);
        }
    }

    private void ClearGeneratedSprites()
    {
        foreach (Sprite sprite in _generatedSprites)
        {
            if (sprite != null)
                Destroy(sprite);
        }
        _generatedSprites.Clear();
    }

    private void OnDestroy()
    {
        ClearGeneratedSprites();
        foreach (Image image in _generatedImages)
        {
            if (image != null)
                Destroy(image.gameObject);
        }
        _generatedImages.Clear();
    }


}
