using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropObject : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;
    private Sprite nowSprite;

    void Start()
    {
        nowSprite = null;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerDrag == null) return;
        Image droppedImage = pointerEventData.pointerDrag.GetComponent<Image>();
        iconImage.sprite = droppedImage.sprite;

        iconImage.color = Vector4.one * 0.6f;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerDrag == null) return;
        iconImage.sprite = nowSprite;
        if (nowSprite == null)
            iconImage.color = Vector4.zero;
        else
            iconImage.color = Vector4.one;
    }
    public void OnDrop(PointerEventData pointerEventData)
    {
        Image droppedImage = pointerEventData.pointerDrag.GetComponent<Image>();
        iconImage.sprite = droppedImage.sprite;

        // ドロップする予定の画像サイズを取得
        float dropImageSizeX = iconImage.transform.localScale.x;
        float dropImageSizeY = iconImage.transform.localScale.y;

        // ドロップする予定の画像の大きさを半分にして表示
        // iconImage.transform.localScale = new Vector2(dropImageSizeX * 0.5f, dropImageSizeY * 0.5f);

        // ドロップ画像として設定
        nowSprite = droppedImage.sprite;
        iconImage.color = Vector4.one;
    }
}