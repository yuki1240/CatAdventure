using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // 猫の移動速度
    public float speed = 1.0f;

    // 今の状態の猫画像
    SpriteRenderer playerImage;

    // それぞれの向きの猫画像
    public Sprite frontImage;
    public Sprite backImage;
    public Sprite rightImage;
    public Sprite leftImage;

    public Sprite attackImage;

    public GameObject enemy;

    // 一連のコマンド情報が入ったリスト
    List<string> cmdList = new List<string>();

    private Animator animator = null;
    private Rigidbody2D rb = null;

    void Start()
    {
        animator = this.transform.GetComponent<Animator>();
        rb = this.transform.GetComponent<Rigidbody2D>();
        playerImage = this.transform.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.position + new Vector3(0.0f, 10f, 0.0f), Color.blue, 10, false);
    }

    IEnumerator PlayerMove()
    {

        for (int i = 0; i < 4; i++)
        {
            string nowCmd = cmdList[i];
            string plaeyInfo = GetPlayerInfo();

            // 攻撃
            if (nowCmd == "Attack")
            { 
                Vector3 nowPos = transform.position;

                switch (plaeyInfo)
                {
                    

                    case "front":
                        print("今の向き : " + "front");
                        
                        RaycastHit2D HitObject = Physics2D.Raycast(nowPos, -Vector2.up);

                        print(HitObject.transform.name);

                        if (HitObject.transform.tag == "Enemy")
                        {
                            enemy.SetActive(false);
                        }
                        break;

                    case "back":
                        
                        break;

                    case "right":
                        
                        break;

                    case "left":
                        
                        break;
                }
            }
            else if (nowCmd == "Walk")
            {
                Vector3 currentPosition = transform.position;

                switch (plaeyInfo)
                {
                    case "front":
                        print("front");
                        // animator.Play("Walk_front");
                        rb.MovePosition(currentPosition + new Vector3(0.0f, 0.65f, 0.0f));
                        break;

                    case "back":
                        print("back");
                        // animator.Play("Walk_back");
                        rb.MovePosition(currentPosition + new Vector3(0.0f, -0.65f, 0.0f));
                        break;

                    case "right":
                        print("right");
                        // animator.Play("Walk_right");
                        rb.MovePosition(currentPosition + new Vector3(-0.65f, 0.0f, 0.0f));
                        break;

                    case "left":
                        print("left");
                        // animator.Play("Walk_left");
                        rb.MovePosition(currentPosition + new Vector3(0.65f, 0.0f, 0.0f));
                        break;
                }
            }

            // 右回転
            else if (nowCmd == "TrunRight")
            {
                Vector3 currentPosition = transform.position;

                switch (plaeyInfo)
                {
                    case "front":
                        playerImage.sprite = rightImage;
                        break;

                    case "back":
                        playerImage.sprite = leftImage;
                        break;

                    case "right":
                        playerImage.sprite = backImage;
                        break;

                    case "left":
                        playerImage.sprite = frontImage;
                        break;
                }
            }

            // 左回転
            else if (nowCmd == "TrunLeft")
            {
                Vector3 currentPosition = transform.position;

                switch (plaeyInfo)
                {
                    case "front":
                        playerImage.sprite = leftImage;
                        break;

                    case "back":
                        playerImage.sprite = rightImage;
                        break;

                    case "right":
                        playerImage.sprite = frontImage;
                        break;

                    case "left":
                        playerImage.sprite = backImage;
                        break;
                }
            }
            // 1秒間のスリープ処理
            yield return new WaitForSeconds(1.0f);
        }
    }

    // 今の猫の向きを取得
    string GetPlayerInfo()
    {
        // 前を向いているとき
        if (playerImage.sprite.name == frontImage.name)
        {
            return "front";
        }
        // 後ろを向いているとき
        else if (playerImage.sprite.name == backImage.name)
        {
            return "back";
        }
        // 右を向いているとき
        else if (playerImage.sprite.name == rightImage.name)
        {
            return "right";
        }
        // 左を向いているとき
        else if (playerImage.sprite.name == leftImage.name)
        {
            return "left";
        }

        return "";
    }

    // コマンド情報をGameManagerから受け取るための処理
    public void ReceaveCmd(List<string> _cmdList)
    {
        cmdList = _cmdList;

        for (int i = 0; i < cmdList.Count; i++)
        {
            // print(i+1 + "番目：" + cmdList[i]);
        }

        // 受け取ったコマンド情報を元に猫を動かす
        StartCoroutine(PlayerMove());
    }
}
