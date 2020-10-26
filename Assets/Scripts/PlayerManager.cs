using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // 猫の移動速度
    public float speed = 1.0f;

    // ステージがクリアされたらtrue
    bool clearFlag = false;

    // GameManagerから渡される各パターンのパネル
    GameObject reTryPanel;
    GameObject clearPanel;
    GameObject almostPanel;

    public Vector3 startPosition;

    // 宝箱まであと1マスだったら、true
    bool isAlmostCollision = false;

    // 今の状態のプレイヤー画像
    SpriteRenderer playerImage;

    // それぞれの向きの猫画像
    public Sprite frontImage;
    public Sprite backImage;
    public Sprite rightImage;
    public Sprite leftImage;

    // 音源の準備
    public AudioSource audioSource;
    public AudioClip mistake;
    public AudioClip enemyDeath;
    public AudioClip ActionSE;

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
        for (int i = 0; i < cmdList.Count; i++)
        {
            string nowCmd = cmdList[i];
            string plaeyInfo = GetPlayerInfo();
            Vector3 nowPos = transform.position;

            // 攻撃
            if (nowCmd == "Attack" && !clearFlag)
            {
                
                switch (plaeyInfo)
                {
                    case "front":
                        RaycastHit2D hitObj = Physics2D.Raycast(nowPos, Vector2.up, 0.6f);
                        DestoryEnemy(hitObj);
                        break;

                    case "back":
                        hitObj = Physics2D.Raycast(nowPos, Vector2.down, 0.6f);

                        DestoryEnemy(hitObj);
                        break;

                    case "right":
                        hitObj = Physics2D.Raycast(nowPos, Vector2.right, 0.6f);

                        DestoryEnemy(hitObj);
                        break;

                    case "left":
                        hitObj = Physics2D.Raycast(nowPos, Vector2.left, 0.6f);

                        DestoryEnemy(hitObj);
                        break;

                }
            }

            // 1歩前に進む
            else if (nowCmd == "Walk1" && !clearFlag)
            {
                Vector3 currentPosition = transform.position;
                switch (plaeyInfo)
                {
                    case "front":
                        RaycastHit2D hitObj = Physics2D.Raycast(nowPos, Vector2.up, 0.6f);
                        if (!WallCheck(hitObj))
                        {
                            rb.MovePosition(currentPosition + new Vector3(0.0f, 0.63f, 0.0f));
                        }
                        break;

                    case "back":
                        hitObj = Physics2D.Raycast(nowPos, Vector2.down, 0.6f);
                        if (!WallCheck(hitObj))
                        {
                            rb.MovePosition(currentPosition + new Vector3(0.0f, -0.63f, 0.0f));
                        }
                        break;

                    case "right":
                        hitObj = Physics2D.Raycast(nowPos, Vector2.right, 0.6f);
                        if (!WallCheck(hitObj))
                        {
                            rb.MovePosition(currentPosition + new Vector3(0.63f, 0.0f, 0.0f));
                        }
                        break;

                    case "left":
                        hitObj = Physics2D.Raycast(nowPos, Vector2.left, 0.6f);
                        if (!WallCheck(hitObj))
                        {
                            rb.MovePosition(currentPosition + new Vector3(-0.63f, 0.0f, 0.0f));
                        }
                        break;
                }
                audioSource.PlayOneShot(ActionSE);
            }

            // 2歩前に進む
            else if (nowCmd == "Walk2" && !clearFlag)
            {
                Vector3 currentPosition = transform.position;
                switch (plaeyInfo)
                {
                    case "front":
                        RaycastHit2D hitObj = Physics2D.Raycast(nowPos, Vector2.up, 1.2f);
                        if (!WallCheck(hitObj))
                        {
                            rb.MovePosition(currentPosition + new Vector3(0.0f, 1.26f, 0.0f));
                        }
                        break;

                    case "back":
                        hitObj = Physics2D.Raycast(nowPos, Vector2.down, 1.2f);
                        if (!WallCheck(hitObj))
                        {
                            rb.MovePosition(currentPosition + new Vector3(0.0f, -1.26f, 0.0f));
                        }
                        break;

                    case "right":
                        hitObj = Physics2D.Raycast(nowPos, Vector2.right, 1.2f);
                        if (!WallCheck(hitObj))
                        {
                            rb.MovePosition(currentPosition + new Vector3(1.26f, 0.0f, 0.0f));
                        }
                        break;

                    case "left":
                        hitObj = Physics2D.Raycast(nowPos, Vector2.left, 1.2f);
                        if (!WallCheck(hitObj))
                        {
                            rb.MovePosition(currentPosition + new Vector3(-1.26f, 0.0f, 0.0f));
                        }
                        break;
                }
                audioSource.PlayOneShot(ActionSE);
            }

            // 右回転
            else if (nowCmd == "TrunRight" && !clearFlag)
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
                audioSource.PlayOneShot(ActionSE);
            }

            // 左回転
            else if (nowCmd == "TrunLeft" && !clearFlag)
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
                audioSource.PlayOneShot(ActionSE);
            }

            // 1秒間のスリープ処理
            if (i != cmdList.Count - 1)
            {
                yield return new WaitForSeconds(1.0f);
            }
            // 最後のコマンドを実行時
            else
            {
                DisplayAlmostPanel1(plaeyInfo);
                yield return new WaitForSeconds(0.5f);
            }
        }
        if (!clearFlag && !isAlmostCollision)
        {
            yield return new WaitForSeconds(0.5f);
            audioSource.PlayOneShot(mistake);
            reTryPanel.SetActive(true);
        }
    }

    public void setInitPlayerPos()
    {
        playerImage.sprite = frontImage;
        this.transform.position = startPosition;
    }

    // プレイヤーの前にオブジェクトがあればtrueを返す
    bool WallCheck(RaycastHit2D hitInfo)
    {
        if (hitInfo.collider == null)
        {
            return false;
        }
        if (hitInfo.transform.tag != "JuweryBox")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 敵の削除（待機する時間を返す）
    void DestoryEnemy(RaycastHit2D hitInfo)
    {
        if (hitInfo.collider == null)
        {
            return;
        }
        // 敵だったとき1秒後に削除して、さらに1秒間待機する
        if (hitInfo.transform.tag == "Enemy")
        {
            audioSource.PlayOneShot(enemyDeath);
            hitInfo.transform.gameObject.GetComponent<Animator>().Play("EnemyRotate");

            // 失敗時にまた表示したいので、Destroyしないで、非表示にして1秒間待ちたい//////////////////////////////////////////////////////////////////////////
            Destroy(hitInfo.transform.gameObject, 1);
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

    // コマンド情報をGameManagerから受り、PlayerMove()へ渡して動かす
    public void ReceaveCmd(List<string> _cmdList, GameObject _retryObj, GameObject _clearObj, GameObject _almostObj)
    {
        reTryPanel = _retryObj;
        clearPanel = _clearObj;
        almostPanel = _almostObj;

        // プレイヤーの初期位置の保存
        startPosition = this.transform.position;

        cmdList = _cmdList;

        // 受け取ったコマンド情報を元に猫を動かす
        StartCoroutine(PlayerMove());
    }

    // 宝箱までたどり着いたとき
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.tag == "JuweryBox")
        {
            clearFlag = true;
            clearPanel.SetActive(true);
        }

        if (collider.transform.tag == "Wall")
        {
            clearFlag = true;
            reTryPanel.SetActive(true);
        }
    }


    // Almostパネルを表示するかのチェック（各方向の確認）
    void DisplayAlmostPanel1(string plaeyInfo)
    {
        Vector3 nowPos = transform.position;

        switch (plaeyInfo)
        {
            case "front":
                RaycastHit2D hitObj = Physics2D.Raycast(nowPos, Vector2.up, 1.2f);
                DisplayAlmostPanel2(hitObj);
                break;

            case "right":
                hitObj = Physics2D.Raycast(nowPos, Vector2.right, 1.2f);
                DisplayAlmostPanel2(hitObj);
                break;

            case "left":
                hitObj = Physics2D.Raycast(nowPos, Vector2.left, 1.2f);
                DisplayAlmostPanel2(hitObj);
                break;
        }
    }


    // Almostパネルを表示するかのチェック（宝箱だったら、2秒間表示する）
    void DisplayAlmostPanel2(RaycastHit2D hitInfo)
    {
        if (hitInfo.collider == null)
        {
            return;
        }
        if (hitInfo.transform.tag == "JuweryBox")
        {
            isAlmostCollision = true;
            almostPanel.SetActive(true);
            StartCoroutine(Sleep());
            almostPanel.SetActive(false);
        }
    }

    // 2秒間待機する
    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(2.0f);
    }
}





//string GetDirectionObj(string plaeyInfo)
//{
//    Vector3 nowPos = transform.position;

//    // 宝箱との衝突判定(false = 当たっていない)
//    string obj = null;

//    switch (plaeyInfo)
//    {
//        case "front":
//            RaycastHit2D hitObj = Physics2D.Raycast(nowPos, Vector2.up, 1.2f);
//            obj = GetCollisionObj(hitObj);
//            break;

//        case "right":
//            hitObj = Physics2D.Raycast(nowPos, Vector2.right, 1.2f);
//            obj = GetCollisionObj(hitObj);
//            break;

//        case "left":
//            hitObj = Physics2D.Raycast(nowPos, Vector2.left, 1.2f);
//            obj = GetCollisionObj(hitObj);
//            break;
//    }

//    return obj;
//}

//string GetCollisionObj(RaycastHit2D hitInfo)
//{
//    if (hitInfo.collider == null)
//    {
//        return null;
//    }
//    else
//    {
//        return hitInfo.collider.tag;
//    }
//}