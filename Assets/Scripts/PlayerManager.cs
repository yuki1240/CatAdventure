using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // 猫の移動速度
    public float speed = 1.0f;

    // ステージがクリアされたらtrue
    [System.NonSerialized] static bool clearFlag = false;

    // 今の状態のプレイヤー画像
    SpriteRenderer playerImage;

    GameObject playerPrefab;

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
        playerImage = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Debug.DrawRay(transform.position, transform.position + new Vector3(0.0f, 10f, 0.0f), Color.blue, 10, false);

    }

    IEnumerator PlayerMove()
    {

        for (int i = 0; i < cmdList.Count; i++)
        {
            string nowCmd = cmdList[i];
            string plaeyInfo = GetPlayerInfo();

            // 攻撃
            if (nowCmd == "Attack")
            { 
                Vector3 nowPos = transform.position;
                print("Attack");
                switch (plaeyInfo)
                {
                    

                    case "front":
                        print("今の向き : " + "front");
                        
                        RaycastHit2D HitObject = Physics2D.Raycast(nowPos, Vector2.up);

                        print(HitObject.transform.name);

                        if (HitObject.transform.tag == "Enemy")
                        {
                            print("enemy!");
                            enemy.SetActive(false);
                        }
                        break;

                    case "back":
                        print("今の向き : " + "front");

                        HitObject = Physics2D.Raycast(nowPos, -Vector2.up);

                        print(HitObject.transform.name);

                        if (HitObject.transform.tag == "Enemy")
                        {
                            print("enemy!");
                            enemy.SetActive(false);
                        }
                        break;

                    case "right":
                        print("今の向き : " + "front");

                        HitObject = Physics2D.Raycast(nowPos, Vector2.right);

                        print(HitObject.transform.name);

                        if (HitObject.transform.tag == "Enemy")
                        {
                            print("enemy!");
                            enemy.SetActive(false);
                        }
                        break;

                    case "left":
                        print("今の向き : " + "front");

                        HitObject = Physics2D.Raycast(nowPos, Vector2.left);

                        print(HitObject.transform.name);

                        if (HitObject.transform.tag == "Enemy")
                        {
                            print("enemy!");
                            enemy.SetActive(false);
                        }
                        break;
                }
            }

            // 1歩前に進む
            else if (nowCmd == "Walk1")
            {
                Vector3 currentPosition = transform.position;

                switch (plaeyInfo)
                {
                    case "front":
                        // animator.Play("Walk_front");
                        rb.MovePosition(currentPosition + new Vector3(0.0f, 0.63f, 0.0f));
                        break;

                    case "back":
                        // animator.Play("Walk_back");
                        rb.MovePosition(currentPosition + new Vector3(0.0f, -0.63f, 0.0f));
                        break;

                    case "right":
                        // animator.Play("Walk_right");
                        rb.MovePosition(currentPosition + new Vector3(0.63f, 0.0f, 0.0f));
                        break;

                    case "left":
                        // animator.Play("Walk_left");
                        rb.MovePosition(currentPosition + new Vector3(-0.63f, 0.0f, 0.0f));
                        break;
                }
            }

            // 2歩前に進む
            else if (nowCmd == "Walk2")
            {
                Vector3 currentPosition = transform.position;

                switch (plaeyInfo)
                {
                    case "front":
                        // animator.Play("Walk_front");
                        rb.MovePosition(currentPosition + new Vector3(0.0f, 1.3f, 0.0f));
                        break;

                    case "back":
                        // animator.Play("Walk_back");
                        rb.MovePosition(currentPosition + new Vector3(0.0f, -1.3f, 0.0f));
                        break;

                    case "right":
                        // animator.Play("Walk_right");
                        rb.MovePosition(currentPosition + new Vector3(1.3f, 0.0f, 0.0f));
                        break;

                    case "left":
                        // animator.Play("Walk_left");
                        rb.MovePosition(currentPosition + new Vector3(-1.3f, 0.0f, 0.0f));
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
        
        Vector3 _startPos = this.transform.position;

        cmdList = _cmdList;

        for (int i = 0; i < cmdList.Count; i++)
        {
            // print(i+1 + "番目：" + cmdList[i]);
        }

        // 受け取ったコマンド情報を元に猫を動かす
        StartCoroutine(PlayerMove());

        // すべての処理が終わって、宝箱にたどり着いたいなかったら、初期座標に戻す
        if (!clearFlag)
        {
            print("aa");
            this.transform.position = _startPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "JuweryBox")
        {
            clearFlag = true;
            print("Clear!");
        }
    }
}
