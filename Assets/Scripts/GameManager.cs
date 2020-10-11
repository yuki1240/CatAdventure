using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform frames;
    public GameObject RunButton;

    public void RunButtonClick()
    {
        Image dropImage;
        // Framesの子のFremeImage1～Nまでを取得
        foreach (Transform childTrans in frames)
        {
            // FremeImage1～Nまで子のDropImageを取得
            dropImage = childTrans.transform.Find("DropImage").transform.gameObject.GetComponent<Image>();
            
            // 枠の中に画像がセットされているとき、その画像名を取得
            if (dropImage.sprite != null)
            {
                print(dropImage.sprite.name);
            }

        }
    }

    

}
