using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class Sample : MonoBehaviour
{
    List<int> NumList = new List<int>();

    // 左上の出現座標
    Vector3 upperLeft = new Vector3(-1.89f, 4.15f, 0.0f);
    // 右上の出現座標
    Vector3 upperRight = new Vector3(1.89f, 4.15f, 0.0f);
    // 左下の出現座標
    Vector3 lowerLeft = new Vector3(-1.89f, 0.35f, 0.0f);
    // 右下の出現座標
    Vector3 lowerRight = new Vector3(1.89f, 0.35f, 0.0f);

    public GameObject jewelryBox;
    public GameObject player;
    public GameObject enemy;

    public GameObject block;
    public BlockCreater blockCreater;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        Vector3 boxPos = jewelryBox.transform.position;
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
        while (true)
        {

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
}
