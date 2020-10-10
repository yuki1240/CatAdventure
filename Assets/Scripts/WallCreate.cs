using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCreate : MonoBehaviour
{
    public GameObject block;

    // Start is called before the first frame update
    void Start()
    {
        // block.SetActive(true);

        // 下の壁を作成
        float posX = -2.61f;
        float posY = -0.6f;
        for (int i = 0; i < 14; i++)
        {
            GameObject objX = Instantiate(block, new Vector3(posX, posY, 0.0f), Quaternion.identity);
            posX += 0.405f;
            // 作成したオブジェクトをWallの子として登録
            objX.transform.parent = transform;
        }

        // 上の壁を作成
        posX = -2.61f;
        posY = 4.82f;
        for (int i = 0; i < 14; i++)
        {
            GameObject objX = Instantiate(block, new Vector3(posX, posY, 0.0f), Quaternion.identity);
            posX += 0.405f;
            // 作成したオブジェクトをWallの子として登録
            objX.transform.parent = transform;
        }

        // 右の壁を作成
        posX = 2.655f;
        posY = -0.6f;
        for (int i = 0; i < 14; i++)
        {
            GameObject objX = Instantiate(block, new Vector3(posX, posY, 0.0f), Quaternion.identity);
            posY += 0.405f;
            // 作成したオブジェクトをWallの子として登録
            objX.transform.parent = transform;
        }

        // 左の壁を作成
        posX = -2.61f;
        posY = -0.6f;
        for (int i = 0; i < 14; i++)
        {
            GameObject objX = Instantiate(block, new Vector3(posX, posY, 0.0f), Quaternion.identity);
            posY += 0.405f;
            // 作成したオブジェクトをWallの子として登録
            objX.transform.parent = transform;
        }

        block.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
