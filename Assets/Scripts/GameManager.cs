using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject Content;
    public GameObject runButton;
    public GameObject box;
    public GameObject player;
    public GameObject enemy;

    public GameObject block;

    public BlockCreater blockCreater;

    public PlayerManager playerSclipt;

    // 左上の出現座標
    Vector3 upperLeft = new Vector3(-1.89f, 4.15f, 0.0f);
    // 右上の出現座標
    Vector3 upperRight = new Vector3(1.89f, 4.15f, 0.0f);
    // 左下の出現座標
    Vector3 lowerLeft = new Vector3(-1.89f, 0.35f, 0.0f);
    // 右下の出現座標
    Vector3 lowerRight = new Vector3(1.89f, 0.35f, 0.0f);

    // セットされたコマンド画像を入れておく配列
    Image[] dropImageChild = new Image[20];

    // コマンドの情報を保持する配列
    [System.NonSerialized]
    public List<string> cmdList = new List<string>();

    List<int> NumList = new List<int>();

    public static int commandNumber = 20;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            NumList.Add(i);
        }

        // 宝箱、プレイヤー、敵の初期位置を決める
        box.transform.position = setStartPosition();
        print(box.transform.position);

        player.transform.position = setStartPosition();
        print(player.transform.position);

        enemy.transform.position = enemyPositon();

        // データのセーブ
        // SaveData.Instance.Save();
    }

    void Update()
    {

    }

    Vector3 setStartPosition()
    {
        Vector3 pos = Vector3.zero;

        // NumListのシャッフル
        NumList = NumList.OrderBy(a => Guid.NewGuid()).ToList();

        foreach (int n in NumList)
        {
            print(n);
        }

        print("NumList[0] : " + NumList[0]);

        switch (NumList[0])
        {
            case 0:
                // 左上の出現座標を返す
                pos = upperLeft;
                NumList.RemoveAt(0);
                return pos;

            case 1:
                // 右上の出現座標を返す
                pos = upperRight;
                NumList.RemoveAt(0);
                return pos;
            case 2:
                // 左下の出現座標を返す
                pos = lowerLeft;
                NumList.RemoveAt(0);
                return pos;
            case 3:
                // 右下の出現座標を返す
                pos = lowerRight;
                NumList.RemoveAt(0);
                return pos;
        }
        return Vector3.zero;
    }

    Vector3 enemyPositon()
    {
        Vector3 boxPos = box.transform.position;
        Vector3 playerPos = player.transform.position;

        // ランダムなX, Yを宣言
        int randX = UnityEngine.Random.Range(0, BlockCreater.blockCountX);
        int randY = UnityEngine.Random.Range(0, BlockCreater.blockCountY);

        Vector3[,] pos = new Vector3[BlockCreater.blockCountX, BlockCreater.blockCountY];

        Vector3 startPos = blockCreater.startPosition;

        for (int y = 0; y < BlockCreater.blockCountY; y++)
        {
            // 左端の座標の初期化
            startPos.x = blockCreater.startPosition.x;

            for (int x = 0; x < BlockCreater.blockCountX; x++)
            {
                pos[x, y] = new Vector3(startPos.x, startPos.y, 0.0f);
                // 左下の座標に、ブロックの幅だけ足していく
                startPos.x += block.transform.localScale.x;
            }
            // 左下の座標に、ブロックの高さ分だけ足していく
            startPos.y += block.transform.localScale.y;
        }

        // 通路の場所をランダムに探す
        while (true){

            // 座標の位置が、通路かつ端っこじゃなければ、その座標を返す
            if (BlockCreater.mapInfo[randX, randY] == false && !(randX == 1 || randY == 1 || randX == 8 || randY == 8))
            {
                
                return pos[randX, randY];
            }
            else
            {
                randX = UnityEngine.Random.Range(0, BlockCreater.blockCountX);
                randY = UnityEngine.Random.Range(0, BlockCreater.blockCountY);
            }
        }
    }

    public void RunButtonClick()
    {
        // コマンドリストの初期化
        cmdList.Clear();

        Image dropImage;

        // Framesの子のFremeImage1～Nまでを取得
        for (var i = 0; i < commandNumber; i++)
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
            else
            {
                cmdList.Add(null);
            }

        }
        // 一連のコマンド情報を猫に渡す
        playerSclipt.ReceaveCmd(cmdList);
    }

}
