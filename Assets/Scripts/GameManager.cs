using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Content;
    public GameObject runButton;
    public GameObject reTryPanel;
    public GameObject clearPanel;
    public GameObject almostPanel;

    public StageCreater StageCreater;

    private PlayerManager playerSclipt;

    // セットされたコマンド画像を入れておく配列
    Image[] dropImageChild = new Image[20];

    // コマンドの情報を保持する配列
    [System.NonSerialized]
    public List<string> cmdList = new List<string>();

    public int CommandNumber = 20;

    private void Start()
    {
        // データのセーブ
        // SaveData.Instance.Save();      
    }

    void Update()
    {

    }

    public void RunButtonClick()
    {
        playerSclipt = StageCreater.PlayerObj.GetComponent<PlayerManager>();

        // コマンドリストの初期化
        cmdList.Clear();

        Image dropImage;

        // Framesの子のFremeImage1～Nまでを取得
        for (var i = 0; i < CommandNumber; i++)
        {
            // FremeImage1～20まで子のDropImageを取得
            var frameImageName = $"FrameImage_{i + 1}";

            try
            {
                dropImage = Content.transform.Find(frameImageName).GetChild(0).gameObject.GetComponent<Image>();
            }
            catch
            {
                break;
            }

            // 枠の中に画像がセットされているとき、その画像名を取得
            if (dropImage.sprite != null)
            {
                cmdList.Add(dropImage.sprite.name);
            }

        }
        // 一連のコマンド情報を猫に渡す
        playerSclipt.ReceaveCmd(cmdList, reTryPanel, clearPanel, almostPanel);
    }

    public void ReTry()
    {
        print("ReTry");
        reTryPanel.SetActive(false);
     
        // 初期化処理
        StageCreater.CreateMapObjects();
    }

    public void Reload()
    {
        SceneManager.LoadScene("Main");
    }

}
