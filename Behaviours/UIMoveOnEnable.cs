using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveOnEnable : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float moveTime;
    public LeanTweenType ease;

    private RectTransform rectTransform;

    void OnEnable()
    {
        if (!rectTransform)
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
        }

        transform.position = startPos;
        LeanTween.move(rectTransform, endPos, moveTime).setEase(ease);
    }
}
