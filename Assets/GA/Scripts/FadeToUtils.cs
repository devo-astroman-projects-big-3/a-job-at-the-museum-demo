using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeToUtils : MonoBehaviour
{
    public UnityEvent OnFadeCompleted = new UnityEvent();

    [SerializeField] private Image _image;
    [SerializeField, Min(0f)] private float _duration = 1f;

    private Coroutine _fadeCoroutine;

    public void FadeToOpacity()
    {
        StartFade(0f, 1f);
    }

    public void FadeFromOpacity()
    {
        StartFade(1f, 0f);
    }

    private void StartFade(float startAlpha, float targetAlpha)
    {
        if (_image == null)
        {
            Debug.LogWarning("Fade requires an assigned UI Image.", this);
            return;
        }

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        SetAlpha(startAlpha);

        if (_duration <= 0f)
        {
            SetAlpha(targetAlpha);
            OnFadeCompleted?.Invoke();
            return;
        }

        if (isActiveAndEnabled)
            _fadeCoroutine = StartCoroutine(Fade(targetAlpha, startAlpha, _duration));
    }

    private IEnumerator Fade(float targetAlpha, float startAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return null;
            if (_image == null)
                break;

            elapsed += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration));
        }

        _fadeCoroutine = null;

        if (_image != null)
        {
            SetAlpha(targetAlpha);
            OnFadeCompleted?.Invoke();
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = _image.color;
        color.a = alpha;
        _image.color = color;
    }

    private void OnDisable()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }
    }
}
