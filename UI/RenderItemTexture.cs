using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class RenderItemUITexture
{
    public void RenderItem(string addressableLoc, Camera itemCamera, RenderTexture itemRenderTexture, RawImage itemImage, System.Action<GameObject> SetItemObj, System.Action OnRendered = null, bool fadeIn = false)
    {
        Addressables.InstantiateAsync(addressableLoc, itemCamera.transform, false).Completed += async (op) =>
        {
            if (op.Result == null)
            {
                Debug.LogError("Error creating crafting ui inventory item image");
                return;
            }

            GameObject itemObj = op.Result;

            if (!(SetItemObj is null)) SetItemObj(itemObj);

            await Task.Yield();

            itemCamera.targetTexture = itemRenderTexture;
            itemCamera.Render();
            itemImage.texture = itemRenderTexture;
            itemImage.enabled = true;
            itemObj.SetActive(false);
            Object.Destroy(itemObj);

            if (fadeIn)
            {
                LeanTween.alpha(itemImage.rectTransform, 1f, 1f);
            }
            else
            {
                LeanTween.alpha(itemImage.rectTransform, 1f, 0f);
            }

            if (!(OnRendered is null)) OnRendered();
        };
    }
}
