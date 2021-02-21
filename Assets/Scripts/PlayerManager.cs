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
    Image playerImage;

    // プレイヤーの位置情報
    Vector3 playerPosition = Vector3.zero;

    int playerCellX = 0;
    int playerCellY = 0;

    private readonly float CharacterMoveUnit = 120f;

    void Start()
    {
        playerPosition = this.transform.localPosition;
        StageCreater = GameObject.FindWithTag("Wall").GetComponent<StageCreater>();
        playerImage = StageCreater.PlayerObj.GetComponent<Image>();
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
                PlayerWalk();
            }

            // 2歩前に進む
            else if (cmdList[i] == "Walk2")
            {
                PlayerWalk();
                yield return new WaitForSeconds(0.5f);
                PlayerWalk();
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


            // ゲームクリアにならなかったとき
            if (gameManager.gameStopFlag)
            {
                yield break;
            }

            // 最後のコマンドのとき
            if (i == cmdList.Count - 1 && !gameManager.gameStopFlag)
            {
                // 宝箱まであと1マスのとき
                if (gameManager.JuweryBoxCheck(transform.position, currentDirection))
                {
                    StartCoroutine(gameManager.ShowAlmostPanel());
                    yield return new WaitForSeconds(1.0f);
                }

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

        // RaycastHit2D hitInfo = Physics2D.Raycast(currentPos, direction, RayDistance);

        if (CollisionCheck("attack"))
        {
            attackFlag = true;
            // StartCoroutine(gameManager.DestoryEnemy(hitInfo));
        }
    }

    void PlayerWalk()
    {
        gameManager.CallSound("actionSE");

        if (!CollisionCheck("Walk"))
        {
            UpdataPlayerPosition();

            if (GetCurrentDirection() == "front")
            {
                this.transform.localPosition += new Vector3(0f, CharacterMoveUnit, 0f);
            }
            else if (GetCurrentDirection() == "back")
            {
                this.transform.localPosition += new Vector3(0f, -CharacterMoveUnit, 0f);
            }
            else if (GetCurrentDirection() == "right")
            {
                this.transform.localPosition += new Vector3(CharacterMoveUnit, 0f, 0f);
            }
            else if (GetCurrentDirection() == "left")
            {
                this.transform.localPosition += new Vector3(-CharacterMoveUnit, 0f, 0f);
            }
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

    // プレイヤーの前方に障害物がないかをチェック
    public bool CollisionCheck(string _command)
    {
        // 今のプレイヤーの位置を探す
        for (int y = 0; y < StageCreater.mapHeight; y++)
        {
            for (int x = 0; x < StageCreater.mapWidth; x++)
            {
                if (StageCreater.cells[y, x] == StageCreater.CellType.Player)
                {
                    playerCellX = x;
                    playerCellY = y;
                    Debug.Log($"[Player位置] X:{playerCellX}, Y:{playerCellY}");
                    break;
                    
                }
            }
        }

        print("今の向き：" + GetCurrentDirection());

        StageCreater.CellType cellType = StageCreater.CellType.Empty;

        // 探したプレイヤーの位置から、進もうとしているマスの情報を取得
        switch (GetCurrentDirection())
        {
            case "front":
                cellType = StageCreater.cells[playerCellY + 1, playerCellX];
                break;
            case "back":
                cellType = StageCreater.cells[playerCellY - 1, playerCellX];
                break;
            case "right":
                cellType = StageCreater.cells[playerCellY, playerCellX + 1];
                break;
            case "left":
                cellType = StageCreater.cells[playerCellY, playerCellX - 1];
                break;
        }

        print("前方のマス：" + cellType);

        if (cellType != StageCreater.CellType.Empty)
        {
            print("前方になにかしらのオブジェクトあり");
            return true;
        }    

        print("前方になにもない");
        return false;
    }

    void UpdataPlayerPosition()
    {
        switch (GetCurrentDirection())
        {
            case "front":
                StageCreater.cells[playerCellY, playerCellX] = StageCreater.CellType.Empty;
                StageCreater.cells[playerCellY + 1, playerCellX] = StageCreater.CellType.Player;
                break;
            case "back":
                StageCreater.cells[playerCellY, playerCellX] = StageCreater.CellType.Empty;
                StageCreater.cells[playerCellY - 1, playerCellX] = StageCreater.CellType.Player;
                break;
            case "right":
                StageCreater.cells[playerCellY, playerCellX] = StageCreater.CellType.Empty;
                StageCreater.cells[playerCellY, playerCellX + 1] = StageCreater.CellType.Player;
                break;
            case "left":
                StageCreater.cells[playerCellY, playerCellX] = StageCreater.CellType.Empty;
                StageCreater.cells[playerCellY, playerCellX - 1] = StageCreater.CellType.Player;
                break;
        }
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

        if (_collision.transform.tag == "Wall")
        {
            gameManager.gameStopFlag = true;
            gameManager.CallSound("mistakeSE");
            StartCoroutine(gameManager.ShowReTryPanel(attackFlag));
        }
    }


}