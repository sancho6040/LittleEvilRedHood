using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadingPanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Tween _fadeTween;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () =>
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        });
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration, () =>
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        });
    }

    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if(_fadeTween != null)
        {
            _fadeTween.Kill(false);
        }

        _fadeTween = _canvasGroup.DOFade(endValue, duration);
        _fadeTween.onComplete += onEnd;
    }
}
