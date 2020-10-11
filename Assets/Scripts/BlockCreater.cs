using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreater : MonoBehaviour
{
    public GameObject Block;
    public Vector3 StartPosition = Vector3.zero;
    public int BlockCountX = 6; // 必ず5以上
    public int BlockCountY = 6; // 必ず5以上

    bool[,] MapInfo = null;

    void Start()
    {
        MapGenerator();
        BlockGenerator();
    }

    void MapGenerator()
    {
        MapInfo = new bool[BlockCountX, BlockCountY];
        for (int y = 0; y < BlockCountY; y++)
        {
            for (int x = 0; x < BlockCountX; x++)
            {
                bool BlockFlag = false; // 壁
                if (Random.Range(0, 2) == 1)
                {
                    BlockFlag = true; // 通路
                }
                MapInfo[x, y] = BlockFlag;
                // MapInfo[x, y] = Random.Range(0, 2) == 1;
            }
        }

        // 棒を立て、倒す
        var rnd = new Random();

        for (int x = 2; x < BlockCountX - 1; x += 2)
        {
            for (int y = 2; y < BlockCountY - 1; y += 2)
            {
                // false → 壁
                MapInfo[x, y] = false; // 棒を立てる

                // 倒せるまで繰り返す
                while (true)
                {
                    // 1行目のみ上に倒せる
                    int direction;
                    if (y == 2)
                        // 0～4まで
                        direction = UnityEngine.Random.Range(0, 5);
                    else
                        // 0～3まで
                        direction = UnityEngine.Random.Range(0, 4);

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
                    if (MapInfo[wallX, wallY] != false)
                    {
                        MapInfo[wallX, wallY] = false;
                        break;
                    }
                }
            }
        }
    }

    void BlockGenerator()
    {
        float BlockSizeX = Block.transform.localScale.x;
        float BlockSizeY = Block.transform.localScale.y;

        for (int y = 0; y < BlockCountY; y++)
        {
            for (int x = 0; x < BlockCountX; x++)
            {
                // Mapの情報を見てブロック作成
                if (MapInfo[x, y])
                {
                    // X軸方向のブロックを作成
                    float posX = BlockSizeX * x;
                    float posY = BlockSizeY * y;
                    Vector3 blockPos = new Vector3(posX, posY, 0.0f) + StartPosition;
                    GameObject obj = Instantiate(Block, blockPos, Quaternion.identity);
                    obj.transform.parent = transform;
                }
            }
        }        
    }

    // デバッグ用メソッド
    public static void DebugPrint(int[,] MapInfo)
    {
        print($"Width: {MapInfo.GetLength(0)}");
        print($"Height: {MapInfo.GetLength(1)}");
        for (int y = 0; y < MapInfo.GetLength(1); y++)
        {
            for (int x = 0; x < MapInfo.GetLength(0); x++)
            {
                /*
                if (MapInfo[x, y])
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
