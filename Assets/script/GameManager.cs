using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform frames;
    public GameObject RunButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void RunButtonClick()
    {
        Image dropImage;
        // Framesの子のFremeImage1～Nまでを取得
        foreach (Transform childTrans in frames)
        {
            // FremeImage1～Nまで子のDropImageを取得
            dropImage = childTrans.transform.Find("DropImage").transform.gameObject.GetComponent<Image>();

            // ImageコンポーネントがNoneの時の処理をしたい
            /*
            if (dropImage.sprite.name != "None")
            {
                print(dropImage.sprite.name);
            }
            */
        }
    }

    

}
