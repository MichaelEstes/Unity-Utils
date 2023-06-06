using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoldingText : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI foldingText;

    private int final;
    private bool isFolding = false;
    private const float foldTweenTime = 0.6f;
    private const float foldDelayTime = 2f;

    protected void FoldText(string symbol, int toFold, int final, System.Action onFinished = null)
    {
        this.final = final;
        foldingText.SetText(string.Concat(symbol, toFold));

        if (isFolding) return;

        isFolding = true;
        FadeText(foldingText, 1f, foldTweenTime, LeanTweenType.linear, () =>
        {
            LeanTween.moveY(foldingText.rectTransform, 14f, foldTweenTime).setDelay(foldDelayTime).setEaseInOutQuint();
            FadeText(foldingText, 0f, foldTweenTime, LeanTweenType.linear, () =>
            {
                isFolding = false;
                LeanTween.moveY(foldingText.rectTransform, 0f, 0);
                valueText.SetText(this.final.ToString());
                if (!(onFinished is null)) onFinished();
            }, foldDelayTime);
        });
    }

    void FadeText(TextMeshProUGUI text, float targetAlpha, float targetTime, LeanTweenType ease = LeanTweenType.linear)
    {
        LeanTween.value(text.gameObject, a => text.alpha = a, text.alpha, targetAlpha, targetTime).setEase(ease);
    }

    void FadeText(TextMeshProUGUI text, float targetAlpha, float targetTime, LeanTweenType ease, System.Action onComplete, float delay = 0)
    {
        LeanTween.value(text.gameObject, a => text.alpha = a, text.alpha, targetAlpha, targetTime).setEase(ease).setDelay(delay).setOnComplete(onComplete);
    }
}
