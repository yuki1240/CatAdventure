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
    bool clearFlag = false;

    // 今の状態のプレイヤー画像
    SpriteRenderer playerImage;

    GameObject playerPrefab;

    // それぞれの向きの猫画像
    public Sprite frontImage;
    public Sprite backImage;
    public Sprite rightImage;
    public Sprite leftImage;

    public Sprite attackImage;

    public GameObject tryAgainText;

    // 音源の準備
    public AudioSource audioSource;
    public AudioClip mistake;

    // 一連のコマンド情報が入ったリスト
    List<string> cmdList = new List<string>();

    // private Animator animator = null;
    private Rigidbody2D rb = null;

    void Start()
    {
        // animator = this.transform.GetComponent<Animator>();
        rb = this.transform.GetComponent<Rigidbody2D>();
        playerImage = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
       
    }

    IEnumerator PlayerMove()
    {
        // 実行の時間調整用
        float sleepTime = 0.5f;

        for (int i = 0; i < cmdList.Count; i++)
        {
            string nowCmd = cmdList[i];
            string plaeyInfo = GetPlayerInfo();

            // 攻撃
            if (nowCmd == "Attack")
            {
                Vector3 _nowPos = transform.position;
                // print("Attack");
                switch (plaeyInfo)
                {
                    case "front":
                        RaycastHit2D _hitObj = Physics2D.Raycast(_nowPos, new Vector2(0.0f, 0.4f));
                        sleepTime += DestoryEnemy(_hitObj, sleepTime);
                        break;

                    case "back":
                        _hitObj = Physics2D.Raycast(_nowPos, new Vector2(0.0f, -0.4f));

                        sleepTime += DestoryEnemy(_hitObj, sleepTime);
                        break;

                    case "right":
                        _hitObj = Physics2D.Raycast(_nowPos, new Vector2(0.4f, 0.0f));

                        sleepTime += DestoryEnemy(_hitObj, sleepTime);
                        break;

                    case "left":
                        _hitObj = Physics2D.Raycast(_nowPos, new Vector2(-0.4f, 0.0f));

                        sleepTime += DestoryEnemy(_hitObj, sleepTime);
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
                        rb.MovePosition(currentPosition + new Vector3(0.0f, 1.26f, 0.0f));
                        break;

                    case "back":
                        // animator.Play("Walk_back");
                        rb.MovePosition(currentPosition + new Vector3(0.0f, -1.26f, 0.0f));
                        break;

                    case "right":
                        // animator.Play("Walk_right");
                        rb.MovePosition(currentPosition + new Vector3(1.26f, 0.0f, 0.0f));
                        break;

                    case "left":
                        // animator.Play("Walk_left");
                        rb.MovePosition(currentPosition + new Vector3(-1.26f, 0.0f, 0.0f));
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
            if (i != cmdList.Count - 1)
            {
                yield return new WaitForSeconds(1.0f);
            }
            // 最後のコマンドを実行時
            else
            {
                yield return new WaitForSeconds(sleepTime);
            }
        }
        if (!clearFlag)
        {
            audioSource.PlayOneShot(mistake); 

            // tryAgainText.SetActive(true);
        }

    }

    // 敵の削除
    float DestoryEnemy(RaycastHit2D obj, float time)
    {
        if (obj.transform.tag == "Enemy")
        {
            obj.transform.gameObject.GetComponent<Animator>().Play("EnemyRotate");
            Destroy(obj.transform.gameObject, 1);
            return 1.0f;
        }
        else
        {
            return 0.0f;
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

        // print("cmdList.Count : " + cmdList.Count);

        for (int i = 0; i < cmdList.Count; i++)
        {
            // print(i+1 + "番目：" + cmdList[i]);
        }

        // 受け取ったコマンド情報を元に猫を動かす
        StartCoroutine(PlayerMove());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "JuweryBox")
        {
            clearFlag = true;
            print("Clear!");
        }
    }

    public void TryAgainButtonClick()
    {
        tryAgainText.SetActive(false);
    }
}
