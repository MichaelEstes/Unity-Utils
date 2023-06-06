using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaManager : MonoBehaviour
{
    private Rect lastSafeArea;
    private RectTransform parentRectTransform;

    private void Start()
    {
        parentRectTransform = this.GetComponentInParent<RectTransform>();
    }

    private void Update()
    {
        if (lastSafeArea != Screen.safeArea)
        {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        Rect safeAreaRect = Screen.safeArea;

        float scaleRatio = parentRectTransform.rect.width / Screen.width;

        float left = safeAreaRect.xMin * scaleRatio;
        float right = -(Screen.width - safeAreaRect.xMax) * scaleRatio;
        float top = 0;
        float bottom = 0;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(right, top);

        lastSafeArea = Screen.safeArea;
    }
}
