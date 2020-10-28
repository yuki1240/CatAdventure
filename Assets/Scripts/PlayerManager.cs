using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // 猫の移動速度
    public float speed = 1.0f;

    // 各方向の猫画像
    public Sprite frontImage;
    public Sprite backImage;
    public Sprite rightImage;
    public Sprite leftImage;

    // 音源
    public AudioSource audioSource;
    public AudioClip mistake;
    public AudioClip enemyDeath;
    public AudioClip actionSE;
    public AudioClip clearSE;

    // 宝箱まであと1マスだったら、true
    public bool isAlmostCollision = false;

    // trueのとき、ゲームを中断（範囲外や敵に衝突したとき）
    public bool gameStopFlag = false;


    // GameManagerから渡される各パターンのパネル
    GameObject reTryPanel;
    GameObject clearPanel;
    GameObject almostPanel;

    // 一連のコマンド情報が入ったリスト
    List<string> cmdList = new List<string>();


    StageCreater StageCreater;


    // 今の状態のプレイヤー画像
    SpriteRenderer playerImage;

    
    private Rigidbody2D rb = null;
    private readonly float RayDistance = 0.6f;
    private readonly float CharacterMoveUnit = 0.63f;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        StageCreater = GameObject.FindWithTag("Wall").GetComponent<StageCreater>();
        playerImage = StageCreater.PlayerObj.GetComponent<SpriteRenderer>();

        //////////////////////////////////////////////////// 以下の3行がエラーになる理由
        // GameManager.StageCreater.PlayerObj

        // playerImage = StageCreater.PlayerObj.GetComponent<SpriteRenderer>();

        // playerImage = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
    }

    void PlayerWalk(string currentDirection, float rayDistance, float characterMoveUnit)
    {
        var currentPos = transform.position;
        audioSource.PlayOneShot(actionSE);
        switch (currentDirection)
        {
            case "front":
                RaycastHit2D hitObj = Physics2D.Raycast(currentPos, Vector2.up, rayDistance);
                //if (!ObjCheck(hitObj))
                //{
                    rb.MovePosition(currentPos + Vector3.up * characterMoveUnit);
                //}
                break;

            case "back":
                hitObj = Physics2D.Raycast(currentPos, Vector2.down, rayDistance);
                //if (!ObjCheck(hitObj))
                //{
                   rb.MovePosition(currentPos + Vector3.down * characterMoveUnit);
                //}
                break;

            case "right":
                hitObj = Physics2D.Raycast(currentPos, Vector2.right, rayDistance);
                //if (!ObjCheck(hitObj))
                //{
                    rb.MovePosition(currentPos + Vector3.right * characterMoveUnit);
                //}
                break;

            case "left":
                hitObj = Physics2D.Raycast(currentPos, Vector2.left, rayDistance);
                //if (!ObjCheck(hitObj))
                //{
                    rb.MovePosition(currentPos + Vector3.left * characterMoveUnit);
                //}
                break;
        }
    }

    void PlayerTrun()
    {

    }

    IEnumerator PlayerMove()
    {

        // 今の位置
        Vector3 currentPos = transform.position;

        // 今の向き
        string currentDirection = "front";

        for (int i = 0; i < cmdList.Count; i++)
        {
            // 攻撃コマンド
            if (cmdList[i] == "Attack")
            {

                switch (currentDirection)
                {
                    case "front":
                        RaycastHit2D hitObj = Physics2D.Raycast(currentPos, Vector2.up, 0.6f);
                        DestoryEnemy(hitObj);
                        break;

                    case "back":
                        hitObj = Physics2D.Raycast(currentPos, Vector2.down, 0.6f);

                        DestoryEnemy(hitObj);
                        break;

                    case "right":
                        hitObj = Physics2D.Raycast(currentPos, Vector2.right, 0.6f);

                        DestoryEnemy(hitObj);
                        break;

                    case "left":
                        hitObj = Physics2D.Raycast(currentPos, Vector2.left, 0.6f);

                        DestoryEnemy(hitObj);
                        break;

                }
            }

            // 1歩前に進む
            else if (cmdList[i] == "Walk1")
            {
                float rayDistance = RayDistance * 1;
                float characterMoveUnit = CharacterMoveUnit * 1;
                PlayerWalk(currentDirection, rayDistance, characterMoveUnit);
            }

            // 2歩前に進む
            else if (cmdList[i] == "Walk2")
            {
                float rayDistance = RayDistance * 2;
                float characterMoveUnit = CharacterMoveUnit * 2;
                PlayerWalk(currentDirection, rayDistance, characterMoveUnit);
            }

            // 右回転
            else if (cmdList[i] == "TrunRight")
            {
                Vector3 currentPosition = transform.position;

                switch (currentDirection)
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
                audioSource.PlayOneShot(actionSE);

                // 回転が終わったら今の向きを代入する
                currentDirection = GetcurrentDirection();
            }

            // 左回転
            else if (cmdList[i] == "TrunLeft")
            {
                Vector3 currentPosition = transform.position;

                switch (currentDirection)
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
                audioSource.PlayOneShot(actionSE);

                // 回転が終わったら今の向きを代入する
                currentDirection = GetcurrentDirection();
            }

            // 1秒間のスリープ処理
            if (i != cmdList.Count - 1)
            {
                yield return new WaitForSeconds(1.0f);
            }
            // 最後のコマンドを実行時
            else
            {
                DisplayAlmostPanel1(currentDirection);
                yield return new WaitForSeconds(0.5f);
            }

        }
        if (!isAlmostCollision)
        {
            yield return new WaitForSeconds(0.5f);
            audioSource.PlayOneShot(mistake);
            reTryPanel.SetActive(true);
        }

        // ゲームの中断フラグが立っていないかをチェック
        if (gameStopFlag)
        {
            yield break;
        }
    }

    // 敵の削除（待機する時間を返す）
    void DestoryEnemy(RaycastHit2D _hitInfo)
    {
        if (_hitInfo.collider == null)
        {
            return;
        }
        // 敵だったとき1秒後に削除して、さらに1秒間待機する
        if (_hitInfo.transform.tag == "Enemy")
        {
            audioSource.PlayOneShot(enemyDeath);
            _hitInfo.transform.gameObject.GetComponent<Animator>().Play("EnemyRotate");

            // 失敗時にまた表示したいので、Destroyしないで、非表示にして1秒間待ちたい//////////////////////////////////////////////////////////////////////////
            Destroy(_hitInfo.transform.gameObject, 1);
        }
    }

    // 今の猫の向きを取得
    string GetcurrentDirection()
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

        cmdList = _cmdList;

        // 受け取ったコマンド情報を元に猫を動かす
        StartCoroutine(PlayerMove());
    }

    // 宝箱までたどり着いたとき
    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.transform.tag == "JuweryBox")
        {
            gameStopFlag = true;
            clearPanel.SetActive(true);
            audioSource.PlayOneShot(clearSE);
        }
    }

    // 壁にぶつかったとき（範囲外のとき）
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.transform.tag == "Wall")
        {
            gameStopFlag = true;
            reTryPanel.SetActive(true);
            audioSource.PlayOneShot(mistake);
        }
    }


    // Almostパネルを表示するかのチェック（各方向の確認）
    void DisplayAlmostPanel1(string _currentDirection)
    {
        Vector3 nowPos = transform.position;
        // nowPos = Vector3(nowPos.x, nowPos.y, nowPos.z);

        switch (_currentDirection)
        {
            case "front":
                RaycastHit2D hitObj = Physics2D.Raycast(nowPos, Vector2.up, 0.6f);
                DisplayAlmostPanel2(hitObj);
                break;

            case "right":
                hitObj = Physics2D.Raycast(nowPos, Vector2.right, 0.6f);
                DisplayAlmostPanel2(hitObj);
                break;

            case "left":
                hitObj = Physics2D.Raycast(nowPos, Vector2.left, 0.6f);
                DisplayAlmostPanel2(hitObj);
                break;
        }
    }


    // Almostパネルを表示するかのチェック（1マス先が宝箱だったら、2秒間表示する）
    void DisplayAlmostPanel2(RaycastHit2D _hitInfo)
    {
        if (_hitInfo.collider == null)
        {
            return;
        }
        if (_hitInfo.transform.tag == "JuweryBox")
        {
            isAlmostCollision = true;
            almostPanel.SetActive(true);

            // これだと、並列処理されてしまい意味がない//////////////////////////////////////////////////////////////////////////////////////////////////////
            // StartCoroutine(Sleep());

            // almostPanel.SetActive(false);
        }
    }

}