using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Content;
    public GameObject runButton;

    public PlayerManager playerSclipt;

    Image[] dropImageChild = new Image[20];

    [System.NonSerialized]
    public List<string> cmdList = new List<string>();

    GameObject clickedGameObject;

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
            string frameImageName = $"FrameImage_{i + 1}";
            dropImage = Content.transform.Find(frameImageName).GetChild(0).gameObject.GetComponent<Image>();
            
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
    }

}
