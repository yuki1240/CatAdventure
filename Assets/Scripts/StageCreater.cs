using System;
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
    public GameObject grid;
    public float cellSize = 128f;

    public Vector3 startPosition = new Vector3(-2.31f, 0.0f, 0.0f);

    public int mapWidth = 9;
    public int mapHeight = 9;

    // 外部から参照できるが値は変更不可
    public GameObject PlayerObj { get; private set; }

    public CellType[,] cells = null;

    // 初期のマップ情報
    private CellType[,] initCells = null;

    private void Start()
    {
        // 初回だけってどうやって判定するの？
        // そもそもここでマップの情報作ってるから、ここでマップの情報を
        // 生成したあとに、最後にコピーしたcells[]を保持しとけばいい

        initCells = new CellType[mapHeight, mapWidth];
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

        // ここらへんで ok 
        // まずコピーする用のcells[]をメンバ変数に作らないとだめ
        // セルを保存するというよりは、cells（マップ）そのものをコピーする
        // ぇ、じゃあこういう感じでやるってこと？
        // こんなかんじ
        // なるほどー！
        // CreateMapObjのとこでもっかい
        // これを呼び出すの？
        // いや、あくまでCreateMapObjects()はマップ情報からオブジェクトを生成するだけだから
        // CreateMapObjects()内ではやらない
        // CreateMapObjects()を呼び出す前に、下の逆をやる
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                initCells[y, x] = cells[y, x];
            }
        }

        DebugMapCell();

        // そしたらここってこと？
        // いや、やりなおし処理の前
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
                }
                else if (cellType == CellType.JuweryBox)
                {
                    var juweryBoxObj = Instantiate(juweryBox, transform);
                    juweryBoxObj.transform.localPosition = GetSpawnPosition(y, x);
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