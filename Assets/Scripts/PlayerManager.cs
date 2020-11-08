using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // 各方向の猫画像
    public Sprite frontImage;
    public Sprite backImage;
    public Sprite rightImage;
    public Sprite leftImage;

    // 今のコマンドが攻撃ならtrue
    bool attackFlag = false;

    // 一連のコマンド情報が入ったリスト
    List<string> cmdList = new List<string>();

    // StageCreaterへの参照
    StageCreater StageCreater;

    // GamaManagerへの参照
    GameManager gameManager;

    // プレイヤーの方向
    string currentDirection = "front";

    // 今の状態のプレイヤー画像
    SpriteRenderer playerImage;

    private Rigidbody2D rb = null;
    private readonly float RayDistance = 0.6f;
    private readonly float CharacterMoveUnit = 0.63f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StageCreater = GameObject.FindWithTag("Wall").GetComponent<StageCreater>();
        playerImage = StageCreater.PlayerObj.GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // コマンド情報をGameManagerから受け取り、PlayerMove()へ渡して動かす
    public void ReceaveCmd(List<string> _cmdList)
    {
        cmdList = _cmdList;

        // 受け取ったコマンド情報を元に猫を動かす
        StartCoroutine(PlayerMove());
    }

    IEnumerator PlayerMove()
    {
        for (int i = 0; i < cmdList.Count; i++)
        {
            attackFlag = false;

            // 攻撃コマンド
            if (cmdList[i] == "Attack")
            {
                PlayerAttack();
            }

            // 1歩前に進む
            else if (cmdList[i] == "Walk1")
            {
                PlayerWalk(RayDistance, CharacterMoveUnit);
            }

            // 2歩前に進む
            else if (cmdList[i] == "Walk2")
            {
                PlayerWalk(RayDistance, CharacterMoveUnit);
                yield return new WaitForSeconds(0.5f);
                PlayerWalk(RayDistance, CharacterMoveUnit);
            }

            // 右回転
            else if (cmdList[i] == "TrunRight")
            {
                PlayerTrun(cmdList[i]);
            }

            // 左回転
            else if (cmdList[i] == "TrunLeft")
            {
                PlayerTrun(cmdList[i]);
            }

            // 他の処理が終わるのを待つ
            yield return new WaitForSeconds(0.1f);

            // 宝箱まであと1マスのとき
            // gameManager.ShowDisplayCheck(currentDirection);

            // ゲームクリアにならなかったとき
            if (gameManager.gameStopFlag)
            {
                
                yield break;
            }

            // 最後のコマンドのとき
            if (i == cmdList.Count -1 && !gameManager.gameStopFlag)
            {
                StartCoroutine(gameManager.ShowReTryPanel(attackFlag));
                yield break;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    void PlayerAttack()
    {
        gameManager.CallSound("attackSE");
        Vector3 currentPos = transform.position;
        Vector3 direction = Vector3.zero;

        switch (currentDirection)
        {
            case "front":
                direction = Vector3.up;
                break;
            case "back":
                direction = Vector3.down;
                break;
            case "right":
                direction = Vector3.right;
                break;
            case "left":
                direction = Vector3.left;
                break;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(currentPos, direction, RayDistance);

        if (gameManager.CollisionCheck(hitInfo, "attack"))
        {
            attackFlag = true;
            StartCoroutine(gameManager.DestoryEnemy(hitInfo));
        }
    }

    void PlayerWalk(float _rayDistance, float _characterMoveUnit)
    {
        gameManager.CallSound("actionSE");

        Vector3 currentPos = transform.position;
        Vector3 direction = Vector3.zero;

        switch (currentDirection)
        {
            case "front":
                direction = Vector3.up;
                break;
            case "back":
                direction = Vector3.down;
                break;
            case "right":
                direction = Vector3.right;
                break;
            case "left":
                direction = Vector3.left;
                break;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(currentPos, direction, _rayDistance);

        if (!gameManager.CollisionCheck(hitInfo, "Walk"))
        {
            rb.MovePosition(currentPos + direction * _characterMoveUnit);
        }
    }

    void PlayerTrun(string _commandDirection)
    {
        gameManager.gameStopFlag = false;
        gameManager.CallSound("actionSE");

        if (_commandDirection == "TrunRight")
        {
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
        }
        if (_commandDirection == "TrunLeft")
        {
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
        }
        currentDirection = GetCurrentDirection();
    }

    // 今の猫の向きを取得
    string GetCurrentDirection()
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

    // 宝箱までたどり着いたとき
    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.transform.tag == "JuweryBox")
        {
            gameManager.gameStopFlag = true;
            gameManager.CallSound("clearSE");
            StartCoroutine(gameManager.ShowClearPanel());
        }
    }

    // 壁にぶつかったとき（範囲外のとき）
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        print("呼ばれた");
        if (_collision.transform.tag == "Wall")
        {
            gameManager.gameStopFlag = true;
            gameManager.CallSound("mistakeSE");
            StartCoroutine(gameManager.ShowReTryPanel(attackFlag));
        }
    }

    //// 一歩先が宝箱かどうかを返す関数
    //public bool JuweryBoxCheck()
    //{
    //    print("currentDirection : " + currentDirection);
    //    Vector3 currentPos = transform.position;
    //    Vector2 direction = Vector2.zero;
    //    float distance = 0.6f;

    //    switch (currentDirection)
    //    {
    //        case "front":
    //            direction = Vector2.up;
    //            break;
    //        case "right":
    //            direction = Vector2.right;
    //            break;
    //        case "left":
    //            direction = Vector2.left;
    //            break;
    //    }

    //    RaycastHit2D hitInfo = Physics2D.Raycast(currentPos, direction, distance);

    //    if (hitInfo.collider == null)
    //    {
    //        print("null");
    //        return false;
    //    }
    //    print(hitInfo.transform.tag);
    //    return hitInfo.transform.tag == "JuweryBox";
    //}
}