﻿using System;
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
    
    // StageCreaterで生成された、プレイヤーオブジェクト
    GameObject playerObj;

    Vector3 playerStartPos;

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

        // Playerオブジェクトを取得
        playerObj = GameObject.FindWithTag("Player");
    }

    void Update()
    {

    }

    public void RunButtonClick()
    {
        // プレイヤーの初期位置を記録
        playerStartPos = playerObj.transform.position;

        playerSclipt = playerObj.GetComponent<PlayerManager>();

        // コマンドリストの初期化
        cmdList.Clear();

        // print("cmdList : " + cmdList.Count);

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
        reTryPanel.SetActive(false);
        playerObj.transform.position = playerStartPos;
        playerSclipt.setInitPlayerPos();
    }

    public void Reload()
    {
        SceneManager.LoadScene("Main");
    }

}
