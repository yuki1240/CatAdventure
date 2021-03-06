﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class StageCreater : MonoBehaviour
{
    public enum CellType
    {
        Empty,
        Block,
        JuweryBox,
        Enemy,
        Player
    }

    public GameObject block;
    public GameObject enemy;
    public GameObject juweryBox;
    public GameObject player;
    GameObject grid;
    public float cellSize = 128f;

    public Vector3 startPosition = new Vector3(-2.31f, 0.0f, 0.0f);

    public int mapWidth = 9;
    public int mapHeight = 9;

    // 外部から参照できるが値は変更不可
    public GameObject PlayerObj { get; private set; }

    public CellType[,] cells = null;

    // 初期のマップ情報
    private CellType[,] initCells = null;

    public GameObject[,] enemyList = null;

    // 更にいうとどうせ座標整数だから、Vector2Int型っていうのがある
    // へー！知らんかった
    // どんどん型をしぼって言ったほうがいいのね
    // というより、扱うデータによって型を変える
    // 二次元座標だったら基本Vector2,三次元座標だったら基本Vector3とか
    //　了解！
    public Vector2Int juweryBoxPos = new Vector2Int();

    private void Start()
    {
        initCells = new CellType[mapHeight, mapWidth];
        enemyList = new GameObject[mapHeight, mapWidth];
        cells = new CellType[mapHeight, mapWidth];

        // 空のセルで埋める
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapHeight; x++)
            {
                cells[y, x] = CellType.Empty;
            }
        }

        // 宝箱配置
        int randNum = UnityEngine.Random.Range(0, mapWidth);
        cells[mapHeight - 1, randNum] = CellType.JuweryBox;

        // プレイヤー配置
        randNum = UnityEngine.Random.Range(0, mapWidth);
        cells[0, randNum] = CellType.Player;

        // ブロックの配置
        int blockCount = UnityEngine.Random.Range(1, 4);
        for (int y = 1; y < mapHeight - 1; y++)
        {
            int[] rand = Enumerable.Range(0, mapWidth).OrderBy(n => Guid.NewGuid()).Take(blockCount).ToArray();
            for (int i = 0; i < rand.Length; i++)
            {
                cells[y, rand[i]] = CellType.Block;
            }
        }


        // 敵の配置
        for (int y = 1; y < mapHeight - 1; y++)
        {
            randNum = UnityEngine.Random.Range(0, mapWidth);
            cells[y, randNum] = CellType.Enemy;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                initCells[y, x] = cells[y, x];
            }
        }

        DebugMapCell();

        CreateMapObjects();
    }

    public void Retry()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                cells[y, x] = initCells[y, x];
            }
        }

        CreateMapObjects();
    }

    // マップに各オブジェクトを配置
    public void CreateMapObjects()
    {
        DestroyMapObjects();

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                CellType cellType = cells[y, x];
                if (cellType == CellType.Block)
                {
                    var blockObj = Instantiate(block, transform);
                    blockObj.transform.localPosition = GetSpawnPosition(y, x);
                }
                else if (cellType == CellType.Enemy)
                {
                    var enemyObj = Instantiate(enemy, transform);
                    enemyObj.transform.localPosition = GetSpawnPosition(y, x);

                    // 敵の出現先情報を保持
                    enemyList[y, x] = enemyObj;
                }
                else if (cellType == CellType.JuweryBox)
                {
                    var juweryBoxObj = Instantiate(juweryBox, transform);
                    juweryBoxObj.transform.localPosition = GetSpawnPosition(y, x);
                    juweryBoxPos = new Vector2Int(x, y);    //　逆
                    // ぇ、↑の同じにするんじゃないの？
                    // 違う
                    // ↑は二次元配列だから、[y,x]ってなってるけど、Vector2型は
                    // Vector2(x,y)
                    // いまだと、
                    // juweryBoxPos.x = y, juweryBoxPos.y = x
                    // になっちゃってる
                    // コンストラクタを見てみ
                    // Vector2Int(int x, int y);    こう書いてあるからx,yの順番にわたす
                    // あね
                }
                if (cellType == CellType.Player)
                {
                    PlayerObj = Instantiate(player, transform);
                    PlayerObj.transform.localPosition = GetSpawnPosition(y, x);
                }
            }
        }
    }

    void DestroyMapObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    Vector3 GetSpawnPosition(int y, int x)
    {
        Vector3 pos = startPosition;
        pos.x += x * cellSize;
        pos.y += y * cellSize;
        return pos;
    }

    void DebugMapCell()
    {
        StringBuilder builder = new StringBuilder();
        for (int y = mapHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                builder.Append(CellTypeToString(cells[y, x]));
            }
            builder.AppendLine();
        }

        Debug.Log(builder.ToString());
    }

    string CellTypeToString(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Empty:
                return "□";
            case CellType.Block:
                return "■";
            case CellType.JuweryBox:
                return "▽";
            case CellType.Enemy:
                return "×";
            case CellType.Player:
                return "P";
            default:
                return "";
        }
    }

}