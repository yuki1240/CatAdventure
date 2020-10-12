using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject frames;
    public GameObject runButton;
    public GameObject clearButtons;

    public GameObject plaer;
    PlayerManager playerSclipt;

    GameObject[] buttonChild = new GameObject[20];

    [System.NonSerialized]
    public List<string> cmdList = new List<string>();

    GameObject clickedGameObject;


    private void Start()
    {
        playerSclipt = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            clickedGameObject = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
            }

            // print(clickedGameObject);
        }
    }

    public void RunButtonClick()
    {
        // コマンドリストの初期化
        cmdList.Clear();

        Image dropImage;
        // Framesの子のFremeImage1～Nまでを取得
        for (int i = 0; i < 4; i++)
        {
            // FremeImage1～20まで子のDropImageを取得
            dropImage = frames.transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Image>();
            
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
        // 一連のコマンド情報をプレイヤーに渡す
        playerSclipt.ReceaveCmd(cmdList);

        // プレイヤーをコマンド入力に沿って動かす
        playerSclipt.PlayerMove();
    }

    public void ClearButtonClick(int num)
    {
        
        switch (num)
        {
            case 1:
                buttonChild[0] = null;
                print("button 1");                break;            case 2:
                buttonChild[1] = null;
                print("button 2");
                break;
        }
    }



}
