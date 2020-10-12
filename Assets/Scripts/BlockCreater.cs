using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreater : MonoBehaviour
{
    public GameObject block;
    public Vector3 startPosition = Vector3.zero;
    public int blockCountX = 7; // 必ず5以上の奇数
    public int blockCountY = 7; // 必ず5以上の奇数

    bool[,] mapInfo = null;

    void Start()
    {
        MapGenerator();
        blockGenerator();
        print("ok");
    }

    void MapGenerator()
    {
        mapInfo = new bool[blockCountX, blockCountY];
        for (int y = 0; y < blockCountY; y++)
        {
            for (int x = 0; x < blockCountX; x++)
            {
                bool blockFlag = false; // 通路
                if (x == 0 || y == 0 || x == blockCountX - 1 || y == blockCountY - 1)
                {
                    blockFlag = true; // 壁
                }
                mapInfo[x, y] = blockFlag;
                // mapInfo[x, y] = Random.Range(0, 2) == 1;
            }
        }

        return;

        // 棒を立て、倒す
        var rnd = new Random();

        for (int x = 2; x < blockCountX - 1; x += 2)
        {
            for (int y = 2; y < blockCountY - 1; y += 2)
            {
                // true → 壁
                mapInfo[x, y] = true; // 棒を立てる

                // 倒せるまで繰り返す
                while (true)
                {
                    // 1行目のみ上に倒せる
                    int direction;
                    if (y == 2)
                        // 0～3まで
                        direction = UnityEngine.Random.Range(0, 4);
                    else
                        // 0～2まで
                        direction = UnityEngine.Random.Range(0, 3);

                    // 棒を倒す方向を決める
                    int wallX = x;
                    int wallY = y;

                    switch (direction)
                    {
                        case 0: // 右
                            wallX++;
                            break;
                        case 1: // 下
                            wallY++;
                            break;
                        case 2: // 左
                            wallX--;
                            break;
                        case 3: // 上
                            wallY--;
                            break;
                    }

                    // 壁じゃない場合のみ倒して終了
                    if (mapInfo[wallX, wallY] != true)
                    {
                        mapInfo[wallX, wallY] = true;
                        break;
                    }
                }
            }
        }
    }

    void blockGenerator()
    {
        float blockSizeX = block.transform.localScale.x;
        float blockSizeY = block.transform.localScale.y;

        for (int y = 0; y < blockCountY; y++)
        {
            for (int x = 0; x < blockCountX; x++)
            {
                // Mapの情報を見てブロック作成
                if (mapInfo[x, y])
                {
                    // X軸方向のブロックを作成
                    float posX = blockSizeX * x;
                    float posY = blockSizeY * y;
                    Vector3 blockPos = new Vector3(posX, posY, 0.0f) + startPosition;
                    GameObject obj = Instantiate(block, blockPos, Quaternion.identity);
                    obj.transform.parent = transform;
                }
            }
        }        
    }

    // デバッグ用メソッド
    public static void DebugPrint(int[,] mapInfo)
    {
        print("Width:" + mapInfo.GetLength(0));
        print("Height:" +  mapInfo.GetLength(1));

        for (int y = 0; y < mapInfo.GetLength(1); y++)
        {
            for (int x = 0; x < mapInfo.GetLength(0); x++)
            {
                /*
                if (mapInfo[x, y])
                {
                    print("　");
                }
                else
                {
                    print("■");
                }
                */
            }
            print("");
        }
    }
}
