using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class StageCreater : MonoBehaviour
{
    public enum CellType {
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
    public float cellSize = 0.63f;

    public Vector3 startPosition = new Vector3(-2.31f, 0.0f, 0.0f);

    public int mapWidth = 9;
    public int mapHeight = 9;

    public GameObject PlayerObj { get; private set; }

    CellType[,] cells = null;

    private void Awake()
    {
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
            int[] rand = Enumerable.Range(0, mapWidth - 1).OrderBy(n => Guid.NewGuid()).Take(blockCount).ToArray();
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


        DebugMapCell();
        CreateMapObjects();
    }

    // マップに各オブジェクトを配置
    public void CreateMapObjects()
    {
        DestroyMapObjects();

        for(int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                CellType cellType = cells[y, x];
                if(cellType == CellType.Block) 
                {
                    var blockObj = Instantiate(block, transform);
                    blockObj.transform.position = GetSpawnPosition(y, x);
                }
                else if (cellType == CellType.Enemy)
                {
                    var enemyObj = Instantiate(enemy, transform);
                    enemyObj.transform.position = GetSpawnPosition(y, x);
                }
                else if (cellType == CellType.JuweryBox)
                {
                    var juweryBoxObj = Instantiate(juweryBox, transform);
                    juweryBoxObj.transform.position = GetSpawnPosition(y, x);
                }
                if (cellType == CellType.Player)
                {
                    PlayerObj = Instantiate(player, transform);
                    PlayerObj.transform.position = GetSpawnPosition(y, x);
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

        // Debug.Log(builder.ToString());
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