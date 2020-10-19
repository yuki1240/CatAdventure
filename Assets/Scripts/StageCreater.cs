using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class StageCreater : MonoBehaviour
{
    public GameObject block;
    public GameObject enemy;
    public GameObject player;

    public GameObject canvas;

    public Vector3 startPosition = Vector3.zero;
    // List<int> randPosX = new List<int>();

    public int mapWidth = 9;
    public int mapHeight = 9;

    // マップ上の障害物（敵やブロック）の情報
    bool[,] mapInfo = null;

    Vector3[,] mapPos;

    // マップの座標情報
    Vector3 coordinates = Vector3.zero;

    void Start()
    {
        // 配列の初期化
        mapInfo = new bool[mapWidth, mapHeight];

        Vector3[,] pos = new Vector3[mapWidth, mapHeight];
        Vector3 startPos = Vector3.zero;
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                mapPos[x, y] = new Vector3(startPos.x, startPos.y, 0.0f);
                startPos.x += block.transform.localScale.x;
            }
            startPos.y += block.transform.localScale.y;
        }

        blockCreater();
    }

    void Update()
    {

    }

    void blockCreater()
    {
        // var _randPosX = randPosXCreater();


        // マップ情報を見て、ブロックを作る
        float blockSizeX = block.transform.localScale.x;
        float blockSizeY = block.transform.localScale.y;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int n = 0; n < 3; n++)
                {
                    // randPosXに含まれるランダムな数の座標にきたら
                    if (_randPosX.Contains(x))
                    {
                        // Blockを出現させる処理
                        float posX = blockSizeX * x;
                        float posY = blockSizeY * y;
                        // -426, 13
                        // -470, -640
                        Vector3 blockPos = new Vector3(0.0f, 451.0f, 0.0f);
                        // blockPos = blockPos - canvas.transform.position;
                        GameObject obj = Instantiate(block, blockPos, Quaternion.identity);
                        obj.transform.parent = transform;
                    }
                }
            }
        }

     
    }

    void enemyCreater()
    {

    }

    void playerCreater()
    {

    }

    // マップに敵やブロックがあるかどうかのチェック
    void cheakObject()
    {
        mapInfo = new bool[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

            }
        }
    }

    Vector3 getCoordinates(int n)
    {

        var _randPosX = new List<int>();

        for (int i = 0; i < mapWidth; i++)
        {
            _randPosX.Add(i);
        }

        // randPosXのシャッフル
        _randPosX = _randPosX.OrderBy(a => Guid.NewGuid()).ToList();

        // RemoveAt(0)を行い、3つの値(初期値)を持ったrandPosXを作成する
        for (int i = 0; i < mapWidth - n; i++)
        {
            _randPosX.RemoveAt(0);
        }

        for (int i = 0; i < n; i++)
        {
            // 障害物がなかったら、その場所の座標を返す
            if (!mapInfo[_randPosX[i], n])
            {
                return mapPos[i, n];
            }
        {

        return Vector3.zero;
    }

    List<int> randPosXCreater()
    {
        var _randPosX = new List<int>();

        for (int i = 0; i < mapWidth; i++)
        {
            _randPosX.Add(i);
        }

        // randPosXのシャッフル
        _randPosX = _randPosX.OrderBy(a => Guid.NewGuid()).ToList();

        // RemoveAt(0)を行い、3つの値(初期値)を持ったrandPosXを作成する
        for (int i = 0; i < mapWidth - 3; i++)
        {
            _randPosX.RemoveAt(0);
        }

        return _randPosX;
    }
}
