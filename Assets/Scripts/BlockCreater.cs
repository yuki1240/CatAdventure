using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreater : MonoBehaviour
{
    public GameObject Block;
    public Vector3 StartPosition = Vector3.zero;
    public int BlockCountX = 6;
    public int BlockCountY = 6;

    bool[,] MapInfo = null;

    // Start is called before the first frame update
    void Start()
    {
        MapGenerator();
        BlockGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MapGenerator()
    {
        MapInfo = new bool[BlockCountX, BlockCountY];
        for (int y = 0; y < BlockCountY; y++)
        {
            for (int x = 0; x < BlockCountX; x++)
            {
                bool BlockFlag = false;
                if (Random.Range(0, 2) == 1)
                {
                    BlockFlag = true;
                }
                MapInfo[x, y] = BlockFlag;
                // MapInfo[x, y] = Random.Range(0, 2) == 1;
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
}
