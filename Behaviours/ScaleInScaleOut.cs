using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleInScaleOut : MonoBehaviour
{

    public float scaleInTime;
    public LeanTweenType scaleInEase;
    public float scaleOutTime;
    public LeanTweenType scaleOutEase;

    public Vector3 scaleFrom;
    public Vector3 scaleTo;

    private LTDescr currentTween;

    void OnEnable()
    {
        if(currentTween == null)
        {
            transform.localScale = scaleFrom;
        } else
        {
            LeanTween.cancel(currentTween.id);
        }

        currentTween = LeanTween.scale(gameObject, scaleTo, scaleInTime).setEase(scaleInEase).setOnComplete(ClearTween);
    }

    void OnDisable()
    {
        if(currentTween != null)
        {
            LeanTween.cancel(currentTween.id);
        }

        currentTween = LeanTween.scale(gameObject, scaleFrom, scaleOutTime).setEase(scaleOutEase).setOnComplete(ClearTween);
    }

    private void ClearTween()
    {
        currentTween = null;
    }
}
