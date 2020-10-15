using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Content;
    public GameObject runButton;

    public PlayerManager playerSclipt;

    // セットされたコマンド画像を入れておく配列
    Image[] dropImageChild = new Image[20];

    // コマンドの情報を保持する配列
    [System.NonSerialized]
    public List<string> cmdList = new List<string>();

    void Update()
    {

    }

    public void RunButtonClick()
    {
        // コマンドリストの初期化
        cmdList.Clear();

        Image dropImage;
        // Framesの子のFremeImage1～Nまでを取得
        for (int i = 0; i < 4; i++)
        {
            // FremeImage1～20まで子のDropImageを取得
            string frameImageName = $"FrameImage_{i + 1}";
            dropImage = Content.transform.Find(frameImageName).GetChild(0).gameObject.GetComponent<Image>();
            
            // print(frames.transform.GetChild(i).transform.GetChild(0).name);

            // 枠の中に画像がセットされているとき、その画像名を取得
            if (dropImage.sprite != null)
            {
                cmdList.Add(dropImage.sprite.name);
                // print(dropImage.sprite.name);
            }
            else
            {
                cmdList.Add(null);
            }

        }
        // 一連のコマンド情報を猫に渡す
        playerSclipt.ReceaveCmd(cmdList);
    }

}
