using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Selector : MonoBehaviour
{
    public Vector2 size {
        get {
            return image.rectTransform.sizeDelta;
        }
    }

    Vector2 initPos;
    Vector2 initSize;
    Color initColor;

    Image image;
    Tweener activePosTween;
    Tweener activeSizeTween;
    Tweener activeColorTween;

    private void Awake()
    {
        image = GetComponent<Image>();

        initPos = image.rectTransform.anchoredPosition;
        initSize = image.rectTransform.sizeDelta;
        initColor = image.color;
    }

    public void ResetSettings()
    {
        image.rectTransform.anchoredPosition = initPos;
        image.rectTransform.sizeDelta = initSize;
        image.color = initColor;

        if( activePosTween != null ) { activePosTween.Kill(); }
        if( activeSizeTween != null ) { activeSizeTween.Kill(); }
        if( activeColorTween != null ) { activeColorTween.Kill(); }
    }
    public void MoveTo(Vector2 pos, float duration)
    {
        if( activePosTween != null ) { activePosTween.Kill(); }

        activePosTween = image.rectTransform.DOMove(pos, duration).SetEase(Ease.InOutCirc);
    }
    public void SizeTo(Vector2 size, float duration)
    {
        if( activeSizeTween != null ) { activeSizeTween.Kill(); }

        activeSizeTween = image.rectTransform.DOSizeDelta(size, duration).SetEase(Ease.InOutCirc);
    }
    public void ColorTo( Color color, float duration )
    {
        if( activeColorTween != null ) { activeColorTween.Kill(); }

        activeColorTween = image.DOColor(color, duration).SetEase(Ease.InOutCirc);
    }
}

