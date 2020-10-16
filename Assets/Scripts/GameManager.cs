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

    public PlayerManager playerSclipt;

    // セットされたコマンド画像を入れておく配列
    Image[] dropImageChild = new Image[20];

    // コマンドの情報を保持する配列
    [System.NonSerialized]
    public List<string> cmdList = new List<string>();

    List<int> NumList = new List<int>();

    private void Start()
    {
        NumList.Add(0);
        NumList.Add(1);
        NumList.Add(2);

        box.transform.position = setStartPosition();
        player.transform.position = setStartPosition();
        enemy.transform.position = enemyPositon();
        print("最終的なEnemyのposition : " + enemy.transform.position);
    }

    void Update()
    {

    }

    Vector3 setStartPosition()
    {
        Vector3 pos = Vector3.zero;

        // NumListのシャッフル
        // NumList = NumList.OrderBy(a => Guid.NewGuid()).ToList();

        switch (NumList[0])
        {
            case 0:
                // 左上の出現座標を返す
                pos = new Vector3(-1.88f, 4.05f, 0.0f);
                NumList[0] = UnityEngine.Random.Range(0, 3);
                NumList.RemoveAt(0);
                return pos;

            case 1:
                // 右上の出現座標を返す
                pos = new Vector3(1.88f, 4.05f, 0.0f);
                NumList[0] = UnityEngine.Random.Range(0, 3);
                NumList.RemoveAt(0);
                return pos;
            case 2:
                // 左下の出現座標を返す
                pos = new Vector3(-1.88f, 0.25f, 0.0f);
                NumList[0] = UnityEngine.Random.Range(0, 3);
                NumList.RemoveAt(0);
                return pos;
        }
        return Vector3.zero;
    }

    Vector3 enemyPositon()
    {
        Vector3 boxPos = box.transform.position;
        Vector3 playerPos = player.transform.position;

        int randX = UnityEngine.Random.Range(0, BlockCreater.blockCountX);
        int randY = UnityEngine.Random.Range(0, BlockCreater.blockCountY);

        Vector3[,] pos = new Vector3[BlockCreater.blockCountX, BlockCreater.blockCountY];

        for (int y = 0; y < BlockCreater.blockCountY; y++)
        {
            float posX = -1.88f;
            float posY = 4.05f;
            for (int x = 0; x < BlockCreater.blockCountX; x++)
            {
                pos[x, y] = new Vector3(posX, posY, 0.0f);
                posX += 0.43f;
            }
            posY += 0.54f;
        }

        while (true){
            print("位置探し");
            if (BlockCreater.mapInfo[randX, randY] == false)
            {
                print("randX : " + randX);
                print("randY : " + randY);
                print("pos[" + randX + ", " + randY + "] : " + pos[randX, randY]);
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
        for (var i = 0; i < 4; i++)
        {
            // FremeImage1～20まで子のDropImageを取得
            var frameImageName = $"FrameImage_{i + 1}";
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
