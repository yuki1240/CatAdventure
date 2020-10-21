using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropObject : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;
    private Sprite nowSprite;

    // public AudioClip sound;
    // AudioSource audioSource;

    void Start()
    {
        nowSprite = null;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerDrag == null) return;
        Image droppedImage = pointerEventData.pointerDrag.GetComponent<Image>();
        if (droppedImage.sprite == null)
        {
            return;
        }

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
        // 効果音の再生
        // audioSource.PlayOneShot(sound);

        Image droppedImage = pointerEventData.pointerDrag.GetComponent<Image>();
        if (droppedImage.sprite == null)
        {
            return;
        }
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